using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace CakeLauncher.Register
{
    public class Program
    {
        public static void Main(string[] args)
        {
            UpdateRegistry();
            Register();
        }

        private static void UpdateRegistry()
        {
            var key = "HKEY_LOCAL_MACHINE\\Software\\Classes\\.cake";
            Registry.SetValue(key, "", "Cake Build", RegistryValueKind.String);
        }

        private static void Register()
        {
            var path = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName;

            Process.Start(new ProcessStartInfo()
            {
                FileName = "srm.exe",
                Verb = "runas",
                WorkingDirectory = path,
                Arguments = $"install CakeLauncher.dll -codebase"
            });
        }
    }
}
