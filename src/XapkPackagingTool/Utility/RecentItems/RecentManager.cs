/*
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using XapkPackagingTool.Helper;

namespace XapkPackagingTool.Utility.RecentItems
{
    public class RecentManager : IRecentManager
    {
        private readonly IRecentFileRepository _repository;
        private ObservableCollection<RecentFile> _recentFiles;

        public RecentManager()
        {
            _repository = new FileSystemRecentFileRepository(
                Path.Combine(EnvironmentPaths.AppData, Properties.Path.Default.RecentsFile)
            );
            _recentFiles = new ObservableCollection<RecentFile>(_repository.GetRecentFiles());
            ValidRecentFiles(_recentFiles);
            _recentFiles.CollectionChanged += RecentFiles_CollectionChanged;
        }

        private void ValidRecentFiles(ObservableCollection<RecentFile> recentFiles)
        {
            var validRecentFiles = recentFiles
                ?.Where(file =>
                    !string.IsNullOrWhiteSpace(file.FileName)
                    && !string.IsNullOrWhiteSpace(file.FilePath)
                )
                .ToList();

            if (validRecentFiles?.Count != recentFiles?.Count)
            {
                _recentFiles = new ObservableCollection<RecentFile>(validRecentFiles);
                _repository.SaveRecentFiles(_recentFiles);
            }
        }

        public ObservableCollection<RecentFile> RecentFiles
        {
            get { return new ObservableCollection<RecentFile>(_recentFiles); }
        }

        public void AddRecentFile(string file)
        {
            var fileName = Path.GetFileName(file) ?? string.Empty;
            var filePath = Path.GetDirectoryName(file) ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(fileName) && !string.IsNullOrWhiteSpace(filePath))
                AddRecentFile(fileName, filePath);
        }

        public void AddRecentFile(string fileName, string filePath)
        {
            if (!string.IsNullOrWhiteSpace(fileName) && !string.IsNullOrWhiteSpace(filePath))
                AddRecentFileInternal(fileName, filePath);
        }

        private void AddRecentFileInternal(string fileName, string filePath)
        {
            RemoveExistingFile(fileName, filePath);
            AddNewFile(fileName, filePath);
            TrimExcessFiles();
            _repository.SaveRecentFiles(_recentFiles);
        }

        private void RemoveExistingFile(string fileName, string filePath)
        {
            var existingFile = _recentFiles.FirstOrDefault(f =>
                f.FileName.Equals(fileName) && f.FilePath.Equals(filePath)
            );

            if (existingFile != null)
                _recentFiles.Remove(existingFile);
        }

        private void AddNewFile(string fileName, string filePath)
        {
            var newFile = new RecentFile
            {
                FileName = fileName,
                FilePath = filePath,
                LastAccessTime = DateTime.Now
            };
            _recentFiles.Insert(0, newFile);
        }

        private void TrimExcessFiles()
        {
            if (_recentFiles.Count > FileSystemRecentFileRepository.MaxRecentFiles)
                _recentFiles.RemoveAt(_recentFiles.Count - 1);
        }

        public void DeleteRecentFile(string filePath)
        {
            var fileToDelete = _recentFiles.FirstOrDefault(f =>
                System.IO.Path.Combine(f.FilePath, f.FileName) == filePath
            );
            if (fileToDelete != null)
            {
                _recentFiles.Remove(fileToDelete);
                _repository.SaveRecentFiles(_recentFiles);
            }
        }

        private void RecentFiles_CollectionChanged(
            object sender,
            NotifyCollectionChangedEventArgs e
        )
        {
            OnRecentFilesChanged();
        }

        public event EventHandler RecentFilesChanged;

        protected virtual void OnRecentFilesChanged()
        {
            RecentFilesChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
