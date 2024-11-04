/* 
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using XapkPackagingTool.Common.Data.Model.Xapk;

namespace SharpXapkLib.Builder
{
    public class BuildConfiguration
    {
        public string Icon { get; set; }
        public XapkManifest Manifest { get; set; }
        public string BuildPath { get; set; }
    }
}
