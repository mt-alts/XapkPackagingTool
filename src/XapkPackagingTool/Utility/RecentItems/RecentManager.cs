/* 
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;

namespace XapkPackagingTool.Utility.RecentItems
{
    public class RecentManager : IRecentManager
    {
        private readonly IRecentFileRepository _repository;
        private ObservableCollection<RecentFile> _recentFiles;
        private readonly ReaderWriterLockSlim _lock = new();

        public RecentManager()
        {
            _repository = new FileSystemRecentFileRepository(
                Path.Combine(
                    Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                        Properties.Path.Default.AppDataDir
                    ),
                    Properties.Path.Default.RecentsFile
                )
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
            get
            {
                _lock.EnterReadLock();
                try
                {
                    return new ObservableCollection<RecentFile>(_recentFiles);
                }
                finally
                {
                    _lock.ExitReadLock();
                }
            }
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
            _lock.EnterWriteLock();
            try
            {
                if (!string.IsNullOrWhiteSpace(fileName) && !string.IsNullOrWhiteSpace(filePath))
                    AddRecentFileInternal(fileName, filePath);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        private void AddRecentFileInternal(string fileName, string filePath)
        {
            var existingFile = _recentFiles.FirstOrDefault(f =>
                f.FileName == fileName && f.FilePath == filePath
            );

            if (existingFile != null)
                _recentFiles.Remove(existingFile);

            var newFile = new RecentFile
            {
                FileName = fileName,
                FilePath = filePath,
                LastAccessTime = DateTime.Now
            };
            _recentFiles.Insert(0, newFile);

            if (_recentFiles.Count > FileSystemRecentFileRepository.MaxRecentFiles)
                _recentFiles.RemoveAt(_recentFiles.Count - 1);

            _repository.SaveRecentFiles(_recentFiles);
        }

        public void DeleteRecentFile(string filePath)
        {
            _lock.EnterWriteLock();
            try
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
            finally
            {
                _lock.ExitWriteLock();
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
