/*
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using System.IO;

namespace XapkPackagingTool.Helper
{
    internal static class PathHelper
    {
        private static string GetBasePath()
        {
            return AppDomain.CurrentDomain.BaseDirectory;
        }

        public static string GetFullPath(string relativePath)
        {
            string basePath = GetBasePath();

            return Path.Combine(basePath, relativePath);
        }
    }
}
