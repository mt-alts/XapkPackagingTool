/*
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using System.IO;

namespace XapkPackagingTool.Helper
{
    internal static class EnvironmentPaths
    {
        public static string AppData
        {
            get => Path.Combine(LocalAppData, Properties.Path.Default.AppDataDirectoryName);
        }

        public static string LocalAppData
        {
            get =>
                Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    Properties.Path.Default.LocalAppDataDirectoryName
                );
        }

        public static string StoredXapkConfigFiles
        {
            get => Path.Combine(LocalAppData, Properties.Path.Default.XapkConfigFilesDirectoryName);
        }

        public static string StoredXapkPackages
        {
            get => Path.Combine(LocalAppData, Properties.Path.Default.XapkPackagesDirectoryName);
        }

        public static string BasePath
        {
            get => GetBasePath();
        }

        private static string GetBasePath()
        {
            return AppDomain.CurrentDomain.BaseDirectory;
        }

        public static string GetBaseDirectoryFilePath(string relativePath)
        {
            string basePath = GetBasePath();

            return Path.Combine(basePath, relativePath);
        }
    }
}
