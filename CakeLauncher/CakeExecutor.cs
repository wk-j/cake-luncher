using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CakeLauncher
{
    class CakeExecutor
    {
        public static void ExecuteCmd(string task, string workingDir)
        {
            var process = new Process();
            process.StartInfo.FileName = "powershell";
            process.StartInfo.Arguments = $"-ExecutionPolicy ByPass -File \"{workingDir}\build.ps1\" -target  \"{task}\"";
            process.StartInfo.WorkingDirectory = workingDir;
            process.Start();
        }

    }
}
