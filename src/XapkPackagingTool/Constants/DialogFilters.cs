/*
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

namespace XapkPackagingTool.Constants
{
    internal static class DialogFilters
    {
        public static readonly string PackageFiles =
            $"{"StrPackageFiles".Localize()} (*{FileExtensions.APK}, *{FileExtensions.XAPK})|*{FileExtensions.APK};*{FileExtensions.XAPK}";

        public static readonly string ApkFile =
            $"{"StrApkExtDescription".Localize()} (*{FileExtensions.APK})|*{FileExtensions.APK}";

        public static readonly string XapkConfigFiles =
            $"{"StrXapkConfigFileFilterDescription".Localize()} (*{FileExtensions.XAPK_CONFIG})|*{FileExtensions.XAPK_CONFIG}";

        public static readonly string PngImage =
            $"{"PngFileFilterDescription".Localize()} (*{FileExtensions.PNG})|*{FileExtensions.PNG}";
    }
}
