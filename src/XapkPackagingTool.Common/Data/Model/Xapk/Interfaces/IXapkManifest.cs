/* 
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/
namespace XapkPackagingTool.Common.Data.Model.Xapk.Interfaces
{
    public interface IXapkManifest : IMetadata
    {
        public List<StringWrapper>? Permissions { get; set; }
        public List<Locale>? Locales { get; set; }
        public List<SplitApk>? SplitApks { get; set; }
        public List<string> SplitConfigs { get; set; }
        public List<Expansion>? Expansions { get; set; }
        public long? TotalSize { get; set; }
    }
}
