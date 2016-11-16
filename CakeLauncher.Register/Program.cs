using System;
using System.Collections.Generic;
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
            try
            {
                var service = new RegistrationServices();
                var path = new FileInfo(Assembly.GetCallingAssembly().Location).Directory.FullName;
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
    }
}
