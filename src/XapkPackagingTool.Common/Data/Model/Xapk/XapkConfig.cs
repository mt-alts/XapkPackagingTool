/*
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using XapkPackagingTool.Common.Data.Enums;
using XapkPackagingTool.Common.Data.Model.Xapk.Interfaces;

namespace XapkPackagingTool.Common.Data.Model.Xapk
{
    public class XapkConfig : ICloneable
    {
        public string ConfigName { get; set; }
        public string BuildPath { get; set; }
        public XapkManifest Manifest { get; set; }

        public ApkVariantSpecies VariantSpecies
        {
            get
            {
                return Manifest.XapkVersion <= 1
                    ? ApkVariantSpecies.MONOLITHIC
                    : ApkVariantSpecies.SPLIT;
            }
            set
            {
                Manifest.XapkVersion = value == ApkVariantSpecies.MONOLITHIC
                    ? Manifest.XapkVersion = 1
                    : Manifest.XapkVersion = 2;
            }
        }

        public string BaseApk { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public XapkConfig DeepClone()
        {
            var clone = (XapkConfig)Clone();
            clone.Manifest = Manifest.DeepClone();
            return clone;
        }
    }
}
