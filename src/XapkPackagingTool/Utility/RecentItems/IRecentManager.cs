/* 
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using System.Collections.ObjectModel;

namespace XapkPackagingTool.Utility.RecentItems
{
    public interface IRecentManager
    {
        public ObservableCollection<RecentFile> RecentFiles { get; }
        public void AddRecentFile(string file);
        public void AddRecentFile(string fileName, string filePath);
        public void DeleteRecentFile(string filePath);
        public event EventHandler RecentFilesChanged;
    }
}
