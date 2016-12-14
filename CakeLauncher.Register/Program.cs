using Microsoft.Win32;
using System;
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

            Console.WriteLine("working dir | {0}", path);

            var dllPath = Path.Combine(path, "CakeLauncher.dll");
            var srm = Path.Combine(path, "srm.exe");

            Process.Start(new ProcessStartInfo()
            {
                FileName = "srm.exe", 
                Verb = "runas",
                WorkingDirectory = path,
                UseShellExecute = true,
                Arguments = $"install CakeLauncher.dll -codebase"
            });
        }
    }
}
