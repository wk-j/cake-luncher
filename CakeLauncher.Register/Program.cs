using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CakeLauncher.Register
{
    public class Program
    {
        public static void Main(string[] args)
        {
            RegisterExtension();
            Register();

        }

        private static void RegisterExtension()
        {
            // substitute "HKEY_LOCAL_MACHINE" if needed...
            Registry.SetValue("HKEY_LOCAL_MACHINE\\Software\\Classes\\.cake", "", "Cake Build", RegistryValueKind.String);
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
