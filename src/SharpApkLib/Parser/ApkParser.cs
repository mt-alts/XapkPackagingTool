/*
   Copyright (c) 2024 Metin Altýkardeþ
   Licensed under the MIT License. See the LICENSE.
*/

using System.IO;
using AaptCommandApi;
using XapkPackagingTool.Common.Data.Model.Apk;

namespace SharpApkLib.Parser
{
    public class ApkParser
    {
        private ApkMetadata ParseApk(string apkFile)
        {
            IAapt2 aapt2 = new Aapt2();
            var apkMetadata = aapt2.DumpBadging(apkFile);
            return apkMetadata;
        }

        public ApkMetadata Parse(string apkFile)
        {
            if (!File.Exists(apkFile))
                throw new FileNotFoundException(apkFile);
            var apkMetadata = ParseApk(apkFile);
            if (apkMetadata == null)
                throw new NullReferenceException(nameof(apkMetadata));
            return apkMetadata;
        }
    }
}
