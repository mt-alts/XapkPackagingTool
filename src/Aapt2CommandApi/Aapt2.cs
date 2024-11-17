/*
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using System.IO;
using XapkPackagingTool.Common.Data.Model.Apk;
using XapkPackagingTool.Common.Utility.TerminalClient;

namespace AaptCommandApi
{
    public class Aapt2 : IAapt2
    {
        private static readonly string CMD_DUMP_BADGING = "dump badging {0}";
        private static readonly string AAPT2EXE =
            $"{Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "aapt2.exe")}";

        private readonly ITerminal terminal;

        public Aapt2()
        {
            if (!File.Exists(AAPT2EXE))
                throw new FileNotFoundException(AAPT2EXE);
            this.terminal = new Terminal(AAPT2EXE);
        }

        public Aapt2(string aapt2exe)
        {
            if (!File.Exists(aapt2exe))
                throw new FileNotFoundException(aapt2exe);
            this.terminal = new Terminal(aapt2exe);
        }

        private string DumpBadge(string apkFile)
        {
            try
            {
                string dumpedData = terminal.Execute(string.Format(CMD_DUMP_BADGING, apkFile));
                return dumpedData;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public ApkMetadata DumpBadging(string apkPath)
        {
            string dumpedData = DumpBadge(apkPath);
            var apkMetaData = ApkMetadataParser.Parse(dumpedData);
            return apkMetaData;
        }
    }
}
