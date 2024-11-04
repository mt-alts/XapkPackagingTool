/* 
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/
using Newtonsoft.Json;
using XapkPackagingTool.Common.Data.Model.Xapk.Interfaces;
using XapkPackagingTool.Common.Utility.ObjectSerialization.Converter;

namespace XapkPackagingTool.Common.Data.Model.Xapk
{
    public class XapkManifest : IXapkManifest, ICloneable
    {
        [JsonProperty("xapk_version")]
        public int XapkVersion { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("package_name")]
        public string? PackageName { get; set; }

        [JsonProperty("version_code")]
        public string? VersionCode { get; set; }

        [JsonProperty("version_name")]
        public string? VersionName { get; set; }

        [JsonProperty("icon", NullValueHandling = NullValueHandling.Ignore)]
        public string? Icon { get; set; }

        [JsonProperty("min_sdk_version")]
        public string? MinSdkVersion { get; set; }

        [JsonProperty("target_sdk_version")]
        public string? TargetSdkVersion { get; set; }

        [JsonConverter(typeof(StringWrapperStringConverter))]
        [JsonProperty("permissions", NullValueHandling = NullValueHandling.Ignore)]
        public List<StringWrapper>? Permissions { get; set; }

        [JsonConverter(typeof(LocalesDictionaryConverter))]
        [JsonProperty("locales_name", NullValueHandling = NullValueHandling.Ignore)]
        public List<Locale>? Locales { get; set; }

        [JsonProperty("split_apks", NullValueHandling = NullValueHandling.Ignore)]
        public List<SplitApk>? SplitApks { get; set; }

        [JsonProperty("split_configs", NullValueHandling = NullValueHandling.Ignore)]
        public List<string>? SplitConfigs { get; set; }

        [JsonProperty("expansions", NullValueHandling = NullValueHandling.Ignore)]
        public List<Expansion>? Expansions { get; set; }

        [JsonProperty("total_size", NullValueHandling = NullValueHandling.Ignore)]
        public long? TotalSize { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public XapkManifest DeepClone()
        {
            var clone = (XapkManifest)Clone();
            if (this.SplitApks != null && SplitApks.Count > 0)
            {
                clone.SplitApks = new List<SplitApk>(this.SplitApks.Count);
                foreach (SplitApk splitApk in this.SplitApks)
                    clone.SplitApks.Add((SplitApk)splitApk.Clone());
            }

            if (this.Expansions != null && Expansions.Count > 0)
            {
                clone.Expansions = new List<Expansion>(Expansions.Count);
                foreach (var expansion in Expansions)
                    clone.Expansions.Add((Expansion)expansion.Clone());
            }

            return clone;
        }
    }
}
