/*
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using System.IO;
using Microsoft.Extensions.DependencyInjection;
using XapkPackagingTool.Common.Data.Model.Xapk;
using XapkPackagingTool.Exceptions;
using XapkPackagingTool.Service.Interfaces;
using XapkPackagingTool.Utility.RecentItems;

namespace XapkPackagingTool.Helper
{
    internal static class RecentConfigLoader
    {
        public static (XapkConfig, string) LoadRecentConfig(RecentFile recentFile)
        {
            var filePath = GetRecentFilePath(recentFile);

            if (!File.Exists(filePath))
                throw new FileNotFoundException(
                    string.Format("StrFileNotFoundMessage".Localize(), filePath)
                );

            var config = LoadXapkConfig(filePath);

            return (config, filePath);
        }

        private static string GetRecentFilePath(RecentFile recentFile)
        {
            return Path.Combine(recentFile.FilePath, recentFile.FileName);
        }

        private static XapkConfig LoadXapkConfig(string filePath)
        {
            var extension = Path.GetExtension(filePath).ToLower();
            XapkConfig config;
            bool isPackage = IsXapkOrApkFile(extension);
            config = isPackage ? ImportFromPackage(filePath) : LoadXapkConfigFromFile(filePath);

            if (config != null)
            {
                return config;
            }
            else
            {
                if (isPackage)
                    throw new PackageReadException(filePath);
                else
                    throw new UnableToReadConfigurationException(filePath);
            }
        }

        private static bool IsXapkOrApkFile(string extension) =>
            extension.Equals("StrXapkExt".Localize()) || extension.Equals("StrApkExt".Localize());

        private static XapkConfig ImportFromPackage(string packagePath)
        {
            var configService = App.ServiceProvider.GetRequiredService<IConfigService>();
            var config = configService.ImportFromPackage(packagePath);
            configService.ConfigPath = string.Empty;
            return config;
        }

        private static XapkConfig LoadXapkConfigFromFile(string filePath)
        {
            var configService = App.ServiceProvider.GetRequiredService<IConfigService>();
            var config = configService.LoadFromDisk(filePath);
            configService.ConfigPath = filePath;
            return config;
        }
    }
}
