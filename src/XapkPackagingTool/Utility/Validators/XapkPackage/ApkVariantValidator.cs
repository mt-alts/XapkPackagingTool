/* 
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/


/* 
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using XapkPackagingTool.Common.Data.Model.Xapk;

namespace XapkPackagingTool.Utility.Validators.XapkPackage
{
    internal static class ApkVariantValidator
    {
        public static bool IsValid(XapkConfig xapkConfig)
        {
            if (!string.IsNullOrWhiteSpace(xapkConfig.BaseApk) && xapkConfig.Manifest.SplitApks?.Count == 0)
            {
                return true;
            }
            else if (string.IsNullOrWhiteSpace(xapkConfig.BaseApk) && xapkConfig.Manifest.SplitConfigs?.Count > 0)
            {
                if (xapkConfig.Manifest.SplitConfigs.Contains("base"))
                    return true;
                throw new Exceptions.BaseApkNotFoundException("StrBaseSplitApkNotFound".Localize());
            }
            else
            {
                throw new Exceptions.BaseApkNotFoundException("StrBaseApkNotFound".Localize());
            }
        }
    }
}
