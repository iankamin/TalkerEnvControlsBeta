using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TalkerEnvControlsBETA
{
    class ConsoleControls
    {
        public int[] relayValues = { 0, 0, 0 };

        public void relayControl(int index)
        {
            Process cmd = new Process();
            cmd.StartInfo.FileName = @"C:\Program Files\PuTTY\plink.exe";
            cmd.StartInfo.UseShellExecute = false;
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.Arguments = "-ssh ubnt@192.168.1.84 -pw ubnt ";
            cmd.StartInfo.CreateNoWindow = true;
            string arg = "echo " + relayValues[index - 1] + " > relay" + (index);

            cmd.Start();
            Thread.Sleep(200);
            cmd.StandardInput.WriteLine('y');
            Thread.Sleep(200);
            cmd.StandardInput.WriteLine("cd /proc/power");
            Thread.Sleep(200);
            cmd.StandardInput.WriteLine(arg);
            Thread.Sleep(200);
            cmd.StandardInput.WriteLine("exit");
            string output = cmd.StandardOutput.ReadToEnd();

            relayValues[index-1] = (relayValues[index-1] == 0 ? 1 : 0);
        }
    }
}
