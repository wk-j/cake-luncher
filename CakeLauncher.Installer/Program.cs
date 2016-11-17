using System;
using System.Collections.Generic;
using System.Linq;
using WixSharp;

namespace CakeLauncher.Installer
{
    class Program
    {
        static Dir GetTopDir(FileFilter filter)
        {
            var config = filter.GetWixFiles("*.config");
            var dll = filter.GetWixFiles("*.dll");
            var exe = filter.GetWixFiles("*.exe");
            config.ForEach(x => x.SetComponentPermanent(true));

            var files = new List<File>();
            files.AddRange(config);
            files.AddRange(dll);
            files.AddRange(exe);

            var dir = new Dir(".", files.ToArray());
            return dir;
        }

        static Dir[] GetStructures(string root)
        {
            var filter = new FileFilter(root);

            var dirs = new Dir[] {
                GetTopDir(filter),
            };

            return dirs.ToArray();
        }

        static string GetProjectDir()
        {
            var current = new System.IO.DirectoryInfo("..\\").FullName;
            var projectDir = System.IO.Path.Combine(current, "CakeLauncher.Register\\bin\\Release");
            return projectDir;
        }

        static string GetVersion()
        {
            var projectDir = GetProjectDir();
            var exePath = System.IO.Path.Combine(projectDir, "CakeLauncher.dll");
            var version = ThisVersion.Version.GetAssemblyVersion(exePath);
            return version;
        }

        static Config CreateConfig()
        {
            var projectDir = GetProjectDir();
            var version = GetVersion();
            var installerName = $"CakeLauncher.{version}";

            var config = new Config
            {
                ProjectDir = projectDir,
                InstallerName = installerName,
                TargetDir = $"%ProgramFiles%\\CakeLauncher",
                ToolLocation = @"C:\Program Files (x86)\WiX Toolset v3.10\bin"
            };
            return config;
        }

        static void Main(string[] args)
        {
            var config = CreateConfig();
            var version = GetVersion();

            Environment.SetEnvironmentVariable("WIXSHARP_WIXDIR", config.ToolLocation, EnvironmentVariableTarget.Process);

            var structure = GetStructures(config.ProjectDir);
            var topDir = new Dir(config.TargetDir, structure);

            var action = new SetPropertyAction("IDIR", "[INSTALLDIR]");
            var project = new Project(config.InstallerName, topDir, action);
            project.UI = WUI.WixUI_InstallDir;
            project.LicenceFile = System.IO.Path.Combine(config.ProjectDir, "LICENSE.rtf");

            project.UpgradeCode = Guid.Parse("FCD4BCB7-3B5D-4A90-8483-6A152CFD8F0F");
            project.ProductId = Guid.NewGuid();
            project.Version = new Version(version);
            project.MajorUpgrade = new MajorUpgrade { AllowSameVersionUpgrades = true, DowngradeErrorMessage = "Higher version already installed" };

            project.Actions.Add(new InstalledFileAction("srm.exe", "install CakeLauncher.dll -codebase", Return.check, When.After, Step.InstallFiles, Condition.Always));


            Compiler.BuildMsi(project);
        }
    }
}
