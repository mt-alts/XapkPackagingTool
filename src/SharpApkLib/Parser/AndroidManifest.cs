/*
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using XapkPackagingTool.Common.Data.Model.Xapk.Interfaces;

namespace SharpApkLib.Parser
{
    public class AndroidManifest : IMetadata
    {
        public string? Name { get; set; }
        public string? PackageName { get; set; }
        public string? VersionCode { get; set; }
        public string? VersionName { get; set; }
        public string? Icon { get; set; }
        public string? MinSdkVersion { get; set; }
        public string? TargetSdkVersion { get; set; }

        [SkipTransfer]
        public List<string>? Permissions { get; set; }

        [SkipTransfer]
        public Dictionary<string, string>? Locales { get; set; }
    }
}
