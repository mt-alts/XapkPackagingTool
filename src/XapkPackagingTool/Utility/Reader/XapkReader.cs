/* 
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using System.IO;
using XapkPackagingTool.Common.Data.Model.Xapk;
using XapkPackagingTool.Common.Helpers.FileHelpers;
using XapkPackagingTool.Constants;
using XapkPackagingTool.Exceptions;

namespace XapkPackagingTool.Utility.Reader
{
    internal class XapkReader : IMetadataReader
    {
        public XapkConfig Read(string xapkPath)
        {
            if (string.IsNullOrWhiteSpace(xapkPath) || !File.Exists(xapkPath))
                throw new FileNotFoundException(
                    string.Format("StrFileNotFoundMessage".Localize(), xapkPath ?? string.Empty)
                );

            var pathExtension = Path.GetExtension(xapkPath);
            if (!pathExtension.Equals(FileExtensions.XAPK, StringComparison.OrdinalIgnoreCase))
                throw new UnsupportedFileFormatException(pathExtension);

            SharpXapkLib.Reader.XapkReader reader = new SharpXapkLib.Reader.XapkReader(xapkPath);
            var manifest = reader.ReadManifest();

            var config = SetupConfiguration(manifest, xapkPath);
            return config;
        }

        private static XapkConfig SetupConfiguration(XapkManifest manifest, string apkPath)
        {
            var config = new XapkConfig();
            config.Manifest = manifest;
            config.BuildPath = FileNameHelper.GetUniqueFileName(
                Path.Combine(Path.GetDirectoryName(apkPath), $"{manifest.PackageName}{FileExtensions.XAPK}")
            );
            return config;
        }
    }
}
