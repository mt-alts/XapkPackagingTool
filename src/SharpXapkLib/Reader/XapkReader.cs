/*
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using System.Windows.Media.Imaging;
using SharpXapkLib.Exceptions;
using SharpXapkLib.Utility;
using XapkPackagingTool.Common.Data.Model.Xapk;
using XapkPackagingTool.Common.Utility.ZipUtility;

namespace SharpXapkLib.Reader
{
    public class XapkReader : IXapkReader
    {
        private readonly ZipReader _zipReader;

        public string XapkFilePath { get; }

        public XapkReader(string xapkPath)
        {
            _zipReader = new ZipReader(xapkPath);
            XapkFilePath = xapkPath;
        }

        public string ReadAsString(string fileInXapk)
        {
            return _zipReader.ReadAsString(fileInXapk);
        }

        public string? GetMonolithicApkPath(string packageName)
        {
            if (_zipReader.EntryExist($"{packageName}.apk"))
                return $"{XapkFilePath}>{packageName}.apk";
            return null;
        }

        public bool EntryExist(string filePath)
        {
            return _zipReader.EntryExist(filePath);
        }

        public BitmapImage GetIcon(string iconPath)
        {
            var streamedIcon = _zipReader.ReadAsStream(iconPath);
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.StreamSource = streamedIcon;
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.EndInit();
            bitmap.Freeze();
            return bitmap;
        }

        public XapkManifest ReadManifest()
        {
            try
            {
                string xapkManifestJson = ReadAsString("manifest.json");
                return ManifestHandler.DeserializeManifest(xapkManifestJson, XapkFilePath);
            }
            catch (Exception)
            {
                throw new MetadataReadException(
                    string.Format("MetadataCannotRead".Localize(), XapkFilePath),
                    XapkFilePath
                );
            }
        }
    }
}
