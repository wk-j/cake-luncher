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
            var keyName = ".cake";

            using (RegistryKey key = Registry.LocalMachine.OpenSubKey("Software\\Classes", true))
            {
                if (key == null)
                {
                }
                else
                {
                    try
                    {
                        key.DeleteValue(keyName);
                    } catch { }
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
