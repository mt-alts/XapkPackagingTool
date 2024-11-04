/*
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

namespace XapkPackagingTool.Common.Utility.ZipUtility
{
    public interface IZipInserter
    {
        void AddFileFromLocal(string source, string target);
        void AddFilesFromZip(string sourceZipPath, IEnumerable<FileCopyInfo> fileCopyInfos);
        void AddFile(string fileName, string content, string folderPath = null);
        void Save();
    }
}
