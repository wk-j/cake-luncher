#tool "nuget:?package=xunit.runner.console&version=2.1.0"
#tool "nuget:?package=gitreleasemanager&version=0.4.0"

var projectName = "CakeLauncher";
var user = EnvironmentVariable("ghu");
var pass = EnvironmentVariable("ghp");
var apiKey = EnvironmentVariable("chp");
var version = ParseAssemblyInfo($"./{projectName}/Properties/AssemblyInfo.cs").AssemblyVersion;
var solution = $"{projectName}.sln";

var releasePath = $"./{projectName}.Register/bin/Release";

Action  clearZip = () => {
    //DeleteFiles($"{projectName}.Installer/*.msi");
    DeleteFiles("zip/*.zip");
};

Func<string> getZip = () => {
    return new System.IO.DirectoryInfo("zip").GetFiles("*.zip").LastOrDefault().FullName;
};

Task("Create-Zip")
    .Does(() => {
           clearZip();
           var dest = $"zip/{projectName}-{version}.zip";
           Zip(releasePath, dest);
});

Task("Create-Choco-Package")
    .IsDependentOn("Build-Release")
    .Does(() => {
        var spec = $"CakeLauncher.Chocoletey\\cake-launcher\\cake-launcher.nuspec";
        var text = System.IO.File.ReadAllText(spec)
            .Replace("$version", version);
        System.IO.File.WriteAllText(spec, text);

        StartProcess("choco", new ProcessSettings {
            Arguments = $"pack {spec}"
        });

        var package = new System.IO.DirectoryInfo("./").GetFiles("*.nupkg").FirstOrDefault().FullName;

        ChocolateyPush(package, new ChocolateyPushSettings {
            Source                = "https://chocolatey.org",
            ApiKey                = apiKey,
            //Timeout               = 300,
            Debug                 = false,
            Verbose               = false,
            Force                 = false,
            Noop                  = false,
            LimitOutput           = false,
            ExecutionTimeout      = 13,
            CacheLocation         = @"C:\temp",
            AllowUnofficial        = false
        });
    });

Task("Install")
    .IsDependentOn("Build-Release")
    .Does(() => {
        StartProcess($"{releasePath}/install.cmd", new ProcessSettings {
            WorkingDirectory = $"{releasePath}"
        });
    });

Task("Uninstall").Does(() => {
    StartProcess($"./{releasePath}/uninstall.cmd", new ProcessSettings {
        WorkingDirectory = $"{releasePath}"
    });
});

Task("Build-Release")
    .Does(() => {
        clearZip();
        var settings = new MSBuildSettings {
            ToolVersion = MSBuildToolVersion.VS2015,
            Configuration = "Release"
        }.WithTarget("Rebuild");
        MSBuild(solution, settings);
    });

Task("Create-Github-Release")
    .IsDependentOn("Build-Release")
    .IsDependentOn("Create-Zip")
    .Does(() => {
        var zip = getZip();
        var tag = $"v{version}";
        var args = $"tag -a {tag} -m \"{projectName} {tag}\"";
        var owner = "wk-j";
        var repo = "cake-launcher";

        StartProcess("git", new ProcessSettings {
            Arguments = args
        });

        StartProcess("git", new ProcessSettings {
            Arguments = $"push https://{user}:{pass}@github.com/{owner}/{repo}.git {tag}"
        });

        GitReleaseManagerCreate(user, pass, owner , repo, new GitReleaseManagerCreateSettings {
            Name              = tag,
            InputFilePath = "RELEASE.md",
            Prerelease        = false,
            TargetCommitish   = "master",
        });
        GitReleaseManagerAddAssets(user, pass, owner, repo, tag, zip);
        GitReleaseManagerPublish(user, pass, owner , repo, tag);
    });

Task("Build").Does(() => {
    DotNetBuild(solution);
});

var target = Argument("target", "Default");
RunTarget(target);