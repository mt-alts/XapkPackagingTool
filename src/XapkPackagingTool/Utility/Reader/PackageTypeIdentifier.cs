/* 
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/
using System.IO;

namespace XapkPackagingTool.Utility.Reader
{
    public sealed class PackageTypeIdentifier
    {
        private static readonly string[] SupportedPackageExtensions = { ".apk", ".xapk" };

        public static PackageType IdentifyPackageType(string packageFilePath)
        {
            if (!File.Exists(packageFilePath))
                throw new FileNotFoundException($"Package file not found: {packageFilePath}", packageFilePath);
            if (string.IsNullOrWhiteSpace(packageFilePath))
                throw new ArgumentNullException(nameof(packageFilePath));

            string extension = Path.GetExtension(packageFilePath).ToLower();

            return extension switch
            {
                ".apk" => PackageType.AndroidPackage,
                ".xapk" => PackageType.ExtendedAndroidPackage,
                _ => SupportedPackageExtensions.Contains(extension) ? PackageType.Unknown : throw new ArgumentException($"Unsupported file extension: {extension}", nameof(packageFilePath))
            };
        }
    }
}
