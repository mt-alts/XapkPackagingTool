/*
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using System.IO;
using SharpXapkLib.Utility;
using XapkPackagingTool.Common.Utility.ZipUtility;

namespace SharpXapkLib.Builder
{
    internal class BuildSizeCalculator : IBuildSizeCalculator
    {
        private XapkFileMap _map;

        public BuildSizeCalculator(XapkFileMap xapkFileMap)
        {
            _map = xapkFileMap;
        }

        private long GetFileSize(string file)
        {
            if (File.Exists(file))
            {
                var fileInfo = new FileInfo(file);
                return fileInfo.Length;
            }
            throw new FileNotFoundException(file);
        }

        private long GetCompressedFilesSize(string compressedFile, List<string> entry)
        {
            if (File.Exists(compressedFile))
                return ZipEntrySizeCalculator.CalculateEntriesTotalSize(compressedFile, entry);
            else
                throw new FileNotFoundException(compressedFile);
        }

        private long CalculateUncompressedFiles()
        {
            long size = 0;
            var uncompressedFiles = _map.Uncompressed;
            if (uncompressedFiles != null && uncompressedFiles.Count > 0)
                foreach (var uncompressedFile in uncompressedFiles)
                    size += GetFileSize(uncompressedFile.Source);
            return size;
        }

        private long CalculateCompressedFiles()
        {
            long size = 0;
            var compressedFiles = _map.Compressed;
            if (compressedFiles != null && compressedFiles.Count > 0)
            {
                foreach (var compressedFile in compressedFiles)
                {
                    string f = compressedFile.CompressedFileName;
                    size += GetCompressedFilesSize(
                        compressedFile.CompressedFileName,
                        compressedFile.Entries.Select(f => f.Source).ToList()
                    );
                }
            }
            return size;
        }

        public long GetTotalSize()
        {
            return CalculateUncompressedFiles() + CalculateCompressedFiles();
        }
    }
}
