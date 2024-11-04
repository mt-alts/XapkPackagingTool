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
            long totalSize = 0;

            using (ZipArchive zipArchive = ZipFile.OpenRead(zipFilePath))
            {
                foreach (string entryName in entryNames)
                {
                    ZipArchiveEntry entry = zipArchive.GetEntry(entryName);
                    if (entry != null)
                        totalSize += entry.Length;
                    else
                        throw new ZipEntryNotFoundException(zipFilePath, entryName);
                }
            }

            return totalSize;
        }
    }
}
