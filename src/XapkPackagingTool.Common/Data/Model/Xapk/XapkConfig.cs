/* 
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/
using XapkPackagingTool.Common.Data.Enums;

namespace XapkPackagingTool.Common.Data.Model.Xapk
{
    public class XapkConfig
    {
        public string ConfigName { get; set; }
        public string BuildPath { get; set; }
        public XapkManifest Manifest { get; set; }
        public ApkVariantSpecies VariantSpecies { get; set; }
        public string BaseApk { get; set; }
    }
}
