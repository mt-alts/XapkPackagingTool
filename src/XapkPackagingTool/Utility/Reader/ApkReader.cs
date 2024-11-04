/* 
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using SharpApkLib.Parser;
using System.IO;
using XapkPackagingTool.Common.Data;
using XapkPackagingTool.Common.Data.Model.Apk;
using XapkPackagingTool.Common.Data.Model.Xapk;
using XapkPackagingTool.Common.Helpers.FileHelpers;
using XapkPackagingTool.Constants;
using XapkPackagingTool.Exceptions;

namespace XapkPackagingTool.Utility.Reader
{
    internal class ApkReader : IMetadataReader
    {
        public XapkConfig Read(string apkPath)
        {
            if (string.IsNullOrWhiteSpace(apkPath) || !File.Exists(apkPath))
                throw new FileNotFoundException(
                    string.Format("StrFileNotFoundMessage".Localize(), apkPath ?? string.Empty)
                );

            var pathExtension = Path.GetExtension(apkPath);
            if (!pathExtension.Equals(FileExtensions.APK, StringComparison.OrdinalIgnoreCase))
                throw new UnsupportedFileFormatException(pathExtension);

            var apkParser = new ApkParser();
            var apkMetada = apkParser.Parse(apkPath);

            return SetupConfiguration(apkMetada, apkPath);
        }

        private static XapkConfig SetupConfiguration(ApkMetadata apkMetada, string apkPath)
        {
            var config = new XapkConfig();
            config.Manifest = ReadManifest(apkMetada);
            config.Manifest.Icon = ConfigureIconPath(apkPath, config.Manifest.Icon);
            config.VariantSpecies = Common.Data.Enums.ApkVariantSpecies.MONOLITHIC;
            config.BuildPath = FileNameHelper.GetUniqueFileName(
                Path.Combine(Path.GetDirectoryName(apkPath), $"{apkMetada.PackageName}{FileExtensions.XAPK}"));
            config.BaseApk = apkPath;
            return config;
        }

        private static XapkManifest ReadManifest(ApkMetadata apkMetada)
        {
            var xapkManifest = new XapkManifest();

            xapkManifest.Permissions = ReadPermissions(apkMetada);
            xapkManifest.Locales = ReadLocales(apkMetada);
            return (XapkManifest)
                Common.Utility.Reflection.ReflectionHelper.TransferProperties(
                    apkMetada,
                    xapkManifest
                );
        }

        private static List<StringWrapper> ReadPermissions(ApkMetadata apkMetada) =>
            apkMetada
                .Permissions.Select(permission => new StringWrapper(permission))
                .ToList();

        private static List<Locale> ReadLocales(ApkMetadata apkMetada) =>
            apkMetada
                .Locales.Select(kvp => new Locale { LanguageCode = kvp.Key, Name = kvp.Value })
                .ToList();

        private static string ConfigureIconPath(string apkPath, string iconPath)
        {
            if (string.IsNullOrWhiteSpace(apkPath) || !Path.GetExtension(apkPath).Equals(".apk"))
                return string.Empty;
            if (string.IsNullOrWhiteSpace(iconPath) || !Path.GetExtension(iconPath).Equals(".png"))
                return string.Empty;

            return $"{apkPath}>{iconPath}";
        }
    }
}
