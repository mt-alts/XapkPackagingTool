/*
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using System.IO;
using System.IO.Compression;

namespace XapkPackagingTool.Common.Utility.ZipUtility
{
    public class ZipReader : IDisposable
    {
        private readonly ZipArchive _zipArchive;
        private readonly FileStream _fileStream;

        public ZipReader(string zipFilePath)
        {
            if (string.IsNullOrWhiteSpace(zipFilePath))
                throw new ArgumentNullException(nameof(zipFilePath));

            _fileStream = new FileStream(zipFilePath, FileMode.Open, FileAccess.Read);
            _zipArchive = new ZipArchive(_fileStream, ZipArchiveMode.Read, false);
        }

        public string ReadAsString(string entryName)
        {
            var entry = GetZipArchiveEntry(entryName);
            using (var stream = entry.Open())
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        public Stream ReadAsStream(string entryName)
        {
            var entry = GetZipArchiveEntry(entryName);
            return entry.Open(); 
        }

        private ZipArchiveEntry GetZipArchiveEntry(string entryName)
        {
            var entry = _zipArchive.GetEntry(entryName);
            if (entry != null)
                return entry;

            throw new FileNotFoundException($"{entryName} not found in {_fileStream.Name}!");
        }

        public bool EntryExist(string entryName)
        {
            var entry = _zipArchive.GetEntry(entryName);
            return entry != null;
        }

        public void Dispose()
        {
            _zipArchive?.Dispose();
            _fileStream?.Dispose();
        }
    }
}
