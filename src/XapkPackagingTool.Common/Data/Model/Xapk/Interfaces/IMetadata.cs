/* 
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/
namespace XapkPackagingTool.Common.Data.Model.Xapk.Interfaces
{
    public interface IMetadata
    {
        public string? Name { get; set; }
        public string? PackageName { get; set; }
        public string? VersionCode { get; set; }
        public string? VersionName { get; set; }
        public string? Icon { get; set; }
        public string? MinSdkVersion { get; set; }
        public string? TargetSdkVersion { get; set; }
    }
}
