/*
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using System.IO;
using XapkPackagingTool.Common.Utility.ObjectSerialization;
using XapkPackagingTool.Utility.Storage;

namespace XapkPackagingTool.Utility.RecentItems
{
    public class FileSystemRecentFileRepository : IRecentFileRepository
    {
        public const int MaxRecentFiles = 15;
        private readonly string _recentsFilePath;

        public FileSystemRecentFileRepository(string filePath)
        {
            _recentsFilePath = filePath;
        }

        public IEnumerable<RecentFile> GetRecentFiles()
        {
            if (File.Exists(_recentsFilePath))
            {
                using (
                    var fileStream = new FileStream(
                        _recentsFilePath,
                        FileMode.Open,
                        FileAccess.Read,
                        FileShare.None
                    )
                )
                using (var reader = new StreamReader(fileStream))
                {
                    string json = reader.ReadToEnd();
                    return JsonSerializer.Deserialize<List<RecentFile>>(json)
                        ?? new List<RecentFile>();
                }
            }

            return Enumerable.Empty<RecentFile>();
        }

        public void SaveRecentFiles(IEnumerable<RecentFile> files)
        {
            string json = JsonSerializer.Serialize(files);

            FileCreator.CreateFileIfNotExists(_recentsFilePath);

            using (
                var fileStream = new FileStream(
                    _recentsFilePath,
                    FileMode.Truncate,
                    FileAccess.Write,
                    FileShare.None
                )
            )
            using (var writer = new StreamWriter(fileStream))
            {
                writer.Write(json);
            }
        }
    }
}
