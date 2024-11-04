/* 
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using System.Diagnostics;
using System.Text;

namespace XapkPackagingTool.Common.Utility.TerminalClient
{
    public class Terminal : ITerminal
    {
        private readonly string executableFile;

        public Terminal(string executable_file)
        {
            executableFile = executable_file;
        }

        public string Execute(string command)
        {
            Console.OutputEncoding = Encoding.Unicode;
            string? data;
            using (var proc = new Process())
            {
                proc.StartInfo = ExecInfo(command);
                proc.Start();
                data = proc.StandardOutput.ReadToEnd();
                proc.WaitForExit();
            }

            return data;
        }

        private ProcessStartInfo ExecInfo(string arg)
        {
            var psi = new ProcessStartInfo();
            psi.FileName = executableFile;
            psi.Arguments = arg;
            psi.UseShellExecute = false;
            psi.StandardOutputEncoding = Encoding.UTF8;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardInput = true;
            psi.RedirectStandardError = true;
            psi.CreateNoWindow = true;
            return psi;
        }
    }
}
