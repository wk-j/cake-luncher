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
            Register();
        }

        private static void _Register()
        {
            try
            {
                var service = new RegistrationServices();
                var path = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName;
                var dll = Path.Combine(path, "CakeLauncher.dll");
                var asm = Assembly.LoadFile(dll);
                service.RegisterAssembly(asm, AssemblyRegistrationFlags.SetCodeBase);

                MessageBox.Show("Register CakeLauncher success");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static void Register()
        {
            var path = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName;

            Process.Start(new ProcessStartInfo()
            {
                FileName = RuntimeEnvironment.GetRuntimeDirectory() + "RegAsm.exe",
                Verb = "runas",
                Arguments = $"{path}\\CakeLauncher.dll"
            });
            Process.Start(new ProcessStartInfo()
            {
                FileName = "srm.exe",
                Verb = "runas",
                Arguments = $"install {path}\\CakeLauncher.dll -codebase"
            });
            Process[] explorers = Process.GetProcessesByName("explorer");
            foreach (Process explorer in explorers)
            {
                try
                {
                    explorer.Kill();
                }
                catch { }
            }
        }
    }
}
