/* 
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

namespace XapkPackagingTool.Utility.RecentItems
{
    public interface IRecentFileRepository
    {
        IEnumerable<RecentFile> GetRecentFiles();
        void SaveRecentFiles(IEnumerable<RecentFile> files);
    }
}
