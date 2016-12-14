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
            UpdateRegister();
            Deregister();

        }

        private static void UpdateRegister()
        {
            var root = "HKEY_LOCAL_MACHINE\\Software\\Classes";
            var keyName = ".cake";
            Registry.SetValue("", "", "Cake Build", RegistryValueKind.String);

            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(root, true))
            {
                if (key == null)
                {
                }
                else
                {
                    key.DeleteValue(keyName);
                }
            }
        }

        private static void Deregister()
        {
            var path = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName;

            Process.Start(new ProcessStartInfo()
            {
                FileName = "srm.exe",
                Verb = "runas",
                WorkingDirectory = path,
                Arguments = $"uninstall CakeLauncher.dll"
            });
        }
    }
}
