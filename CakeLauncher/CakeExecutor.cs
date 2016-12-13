using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CakeLauncher
{
    public class CakeExecutor
    {
        public static void ExecuteCmd(string task, string workingDir)
        {
            var process = new Process();
            process.StartInfo.FileName = "build.cmd";
            process.StartInfo.Arguments = $"-target {task}";
            process.StartInfo.WorkingDirectory = workingDir;
            process.Start();
        }
    }
}
