/*
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

namespace XapkPackagingTool.Common.Utility.ZipUtility
{
    using System.Collections.Generic;
    using System.IO.Compression;
    using XapkPackagingTool.Common.Exceptions;

    public static class ZipEntrySizeCalculator
    {
        public static long CalculateEntriesTotalSize(string zipFilePath, List<string> entryNames)
        {
            if (string.IsNullOrWhiteSpace(zipFilePath))
                throw new ArgumentException("Zip file path cannot be null or whitespace.", nameof(zipFilePath));

            if (entryNames == null || entryNames.Count == 0)
                throw new ArgumentException("Entry names cannot be null or empty.", nameof(entryNames));

            using (ZipArchive zipArchive = ZipFile.OpenRead(zipFilePath))
            {
                return entryNames.Sum(entryName =>
                {
                    var entry = zipArchive.GetEntry(entryName);
                    if (entry == null)
                        throw new ZipEntryNotFoundException(zipFilePath, entryName);
                    return entry.Length;
                });
            }
        }

    }
}
