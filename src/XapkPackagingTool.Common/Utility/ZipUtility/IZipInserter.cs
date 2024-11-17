/*
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using System.IO;

namespace XapkPackagingTool.Common.Utility.ZipUtility
{
    public interface IZipInserter
    {
        void AddFile(string source, string target);
        void AddFilesFromZip(string sourceZipPath, IEnumerable<FileCopyInfo> fileCopyInfos);
        void AppendToZip(string fileName, Stream contentStream, string folderPath = null);
        void Save();
    }
}
