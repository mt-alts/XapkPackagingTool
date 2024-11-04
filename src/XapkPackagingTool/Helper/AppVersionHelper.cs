/*
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using System.Diagnostics;
using System.Reflection;

namespace XapkPackagingTool.Helper
{
    internal static class AppInfoHelper
    {
        public static string GetAppVersion()
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version;
            return $"{version?.Major}.{version?.Minor}.{version?.Build}";
        }

        public static string GetAppName()
        {
            var versionInfo = FileVersionInfo.GetVersionInfo(
                Assembly.GetExecutingAssembly().Location
            );
            return versionInfo.ProductName ?? "XAPK Packaging Tool";
        }
    }
}
