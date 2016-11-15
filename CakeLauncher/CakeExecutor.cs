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
            //    return `powershell -ExecutionPolicy ByPass -File build.ps1 -target \"${taskName}\"`;
            var process = new Process();
            process.StartInfo.FileName = "powershell";
            process.StartInfo.Arguments = $"-ExecutionPolicy ByPass -File build.ps1 -target  \"{task}\"";
            process.StartInfo.WorkingDirectory = workingDir;
            process.Start();
        }
    }
}
