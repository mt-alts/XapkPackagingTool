/* 
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using System.IO;
using XapkPackagingTool.Common.Utility.ObjectSerialization;

namespace XapkPackagingTool.Utility.RecentItems
{
    public class FileSystemRecentFileRepository : IRecentFileRepository
    {
        public const int MaxRecentFiles = 15;
        private readonly string _filePath;

        public FileSystemRecentFileRepository(string filePath)
        {
            _filePath = filePath;
        }

        public IEnumerable<RecentFile> GetRecentFiles()
        {
            if (File.Exists(_filePath))
            {
                string json = File.ReadAllText(_filePath);
                return JsonSerializer.Deserialize<List<RecentFile>>(json) ?? new List<RecentFile>();
            }
            return Enumerable.Empty<RecentFile>();
        }

        public void SaveRecentFiles(IEnumerable<RecentFile> files)
        {
            lock (_filePath)
            {
                string json = JsonSerializer.Serialize(files);
                File.WriteAllText(_filePath, json);
            }
        }
    }
}
