/*
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using System.IO;
using System.Text.RegularExpressions;
using XapkPackagingTool.Common.Data.Model.Apk;

namespace AaptCommandApi
{
    internal class ApkMetadataParser
    {
        internal static ApkMetadata Parse(string dumpedData)
        {
            var metadata = new ApkMetadata
            {
                Icon = GetIconPath(dumpedData),
                PackageName = GetProp(dumpedData, "name="),
                Name = GetProp(dumpedData, "label="),
                VersionName = GetProp(dumpedData, "versionName="),
                VersionCode = GetProp(dumpedData, "versionCode="),
                TargetSdkVersion = GetProp(dumpedData, "targetSdkVersion:"),
                MinSdkVersion = GetProp(dumpedData, "sdkVersion:"),
                Locales = GetLocales(dumpedData),
                Permissions = GetPermissions(dumpedData),
            };
            return metadata;
        }

        private static bool IsPngFile(string filePath)
        {
            var extension = Path.GetExtension(filePath);
            if (!string.IsNullOrWhiteSpace(extension))
                return extension.Equals(".png", StringComparison.OrdinalIgnoreCase);
            return false;
        }

        private static string? GetIconPath(string apkManifest)
        {
            string[] iconDefinations =
            {
                "application-icon-640:",
                "application-icon-480",
                "application-icon-320",
                "application-icon-240",
                "application-icon-120",
                "icon="
            };
            foreach (var iconDefination in iconDefinations)
            {
                var icon = GetProp(apkManifest, iconDefination);
                if (IsPngFile(icon))
                    return icon;
            }

            return null;
        }

        private static string? GetProp(string input, string value)
        {
            var match = Regex.Match(input, value + "'(.*?)'");
            if (match.Success)
                return match.Groups[1].Value;
            return null;
        }

        private static Dictionary<string, string> GetLocales(string apkManifest)
        {
            var lines = apkManifest.Split('\n');
            var languageLabels = new Dictionary<string, string>();

            foreach (var line in lines)
                if (line.StartsWith("application-label-"))
                {
                    var parts = line.Split(":");
                    if (parts.Length == 2)
                    {
                        var languageCode = parts[0].Substring("application-label-".Length);
                        var label = parts[1].Replace("'", string.Empty);
                        languageLabels[languageCode] = label;
                    }
                }

            return languageLabels;
        }

        private static List<string> GetPermissions(string apkManifest)
        {
            var permissionPattern = @"uses-permission: name='(.*?)'";
            var matches = Regex.Matches(apkManifest, permissionPattern);

            var permissions = new List<string>();

            for (var i = 0; i < matches.Count; i++)
                permissions.Add(matches[i].Groups[1].Value);

            return permissions;
        }
    }
}
