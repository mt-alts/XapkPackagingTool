/* 
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using XapkPackagingTool.Common.Data.Model.Xapk;
using XapkPackagingTool.Exceptions;

namespace XapkPackagingTool.Utility.Reader
{
    internal class PackageReader : IPackageReader
    {
        public XapkConfig Read(string packagePath)
        {
            var reader = GetManifestReader(packagePath);
            var config = reader.Read(packagePath);
            return config;
        }

        private static IMetadataReader GetManifestReader(string packagePath)
        {
            var packageType = PackageTypeIdentifier.IdentifyPackageType(packagePath);
            switch (packageType)
            {
                case PackageType.AndroidPackage:
                    return new ApkReader();
                case PackageType.ExtendedAndroidPackage:
                    return new XapkReader();
            }
            var pathExt = System.IO.Path.GetExtension(packagePath);
            throw new UnsupportedFileFormatException(pathExt);
        }
    }
}
