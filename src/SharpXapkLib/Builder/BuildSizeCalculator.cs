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

        private static long GetFileSize(string file)
        {
            if (!File.Exists(file))
                throw new FileNotFoundException(file);
            var fileInfo = new FileInfo(file);
            return fileInfo.Length;
        }

        private static long GetCompressedFilesSize(string compressedFile, List<string> entry)
        {
            if (File.Exists(compressedFile))
                return ZipEntrySizeCalculator.CalculateEntriesTotalSize(compressedFile, entry);
            throw new FileNotFoundException(compressedFile);
        }

        private static long CalculateUncompressedFiles(
            IEnumerable<Inserter.XapkInsertMap> uncompressedFiles
        )
        {
            return uncompressedFiles?.Sum(file => GetFileSize(file.Source)) ?? 0;
        }

        private static long CalculateCompressedFiles(
            IEnumerable<CompressedFileGroup> compressedFiles
        )
        {
            return compressedFiles?.Sum(compressedFile =>
                    GetCompressedFilesSize(
                        compressedFile.CompressedFileName,
                        compressedFile.Entries.Select(entry => entry.Source).ToList()
                    )
                ) ?? 0;
        }

        public long GetTotalSize()
        {
            return CalculateUncompressedFiles(_map.Uncompressed)
                + CalculateCompressedFiles(_map.Compressed);
        }
    }
}
