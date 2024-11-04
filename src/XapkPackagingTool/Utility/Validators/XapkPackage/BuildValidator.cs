/* 
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using XapkPackagingTool.Common.Data.Model.Xapk;

namespace XapkPackagingTool.Utility.Validators.XapkPackage
{
    internal static class BuildValidator
    {
        public static bool IsValid(XapkConfig xapkConfig)
        {
            return ApkVariantValidator.IsValid(xapkConfig);
        }
    }
}
