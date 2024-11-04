/* 
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/
using XapkPackagingTool.Common.Data.Enums;

namespace XapkPackagingTool.Common.Data.Model.Xapk
{
    public class ApkVariant
    {
        public ApkVariantSpecies Species { get; set; }
        public object Data { get; set; }

        public ApkVariant() { }

        public ApkVariant(ApkVariantSpecies species, object data)
        {
            Species = species;
            Data = data;
        }
    }
}
