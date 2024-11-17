/* 
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

//using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Zip;
using System.IO;

namespace XapkPackagingTool.Common.Utility.ZipUtility
{
    public class ZipInserter : IZipInserter, IDisposable
    {
        private readonly string _targetZipPath;
        private ZipOutputStream _zipOutputStream;

        public event EventHandler<int> TransferredBytes;

        private volatile bool isCancelled = false;
        public bool IsCancelled
        {
            get => isCancelled;
            set => isCancelled = value;
        }

        public ZipInserter(string targetZipPath)
        {
            _targetZipPath = targetZipPath;
            InitializeZipOutputStream();
        }

        public void Truncate()
        {
            using (FileStream fs = new FileStream(_targetZipPath, FileMode.OpenOrCreate))
                fs.SetLength(0);
        }

        private void InitializeZipOutputStream()
        {
            if (System.IO.File.Exists(_targetZipPath))
                this.Truncate();
            else if (!System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(_targetZipPath)))
                System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(_targetZipPath));

            FileStream fs = new FileStream(_targetZipPath, FileMode.OpenOrCreate, FileAccess.Write);
            _zipOutputStream = new ZipOutputStream(fs)
            {
                IsStreamOwner = true
            };
        }

        public void AddFile(string source, string target)
        {
            string directoryPath = Path.GetDirectoryName(target) ?? string.Empty;
            string entryPath = Path.Combine(directoryPath, Path.GetFileName(target));

            using (FileStream fs = File.OpenRead(source))
            {
                ZipEntry entry = new ZipEntry(entryPath)
                {
                    DateTime = DateTime.Now,
                    Size = fs.Length
                };
                _zipOutputStream.PutNextEntry(entry);

                byte[] buffer = new byte[4096];
                int bytesRead;
                while ((bytesRead = fs.Read(buffer, 0, buffer.Length)) > 0)
                {
                    if (IsCancelled)
                        break;
                    _zipOutputStream.Write(buffer, 0, bytesRead);
                    OnTransferredBytes(bytesRead);
                }

                _zipOutputStream.CloseEntry();
            }
        }

        public void AddFilesFromZip(string sourceZipPath, IEnumerable<FileCopyInfo> fileCopyInfos)
        {
            using (ZipFile sourceZip = new ZipFile(sourceZipPath))
            {
                foreach (FileCopyInfo fileCopyInfo in fileCopyInfos)
                {
                    ZipEntry entry = sourceZip.GetEntry(fileCopyInfo.Source);
                    if (entry != null)
                    {
                        using (Stream entryStream = sourceZip.GetInputStream(entry))
                        {
                            ZipEntry newEntry = new ZipEntry(fileCopyInfo.Target)
                            {
                                DateTime = DateTime.Now,
                                Size = entry.Size
                            };
                            _zipOutputStream.PutNextEntry(newEntry);

                            byte[] buffer = new byte[4096];
                            int bytesRead;
                            while ((bytesRead = entryStream.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                if (IsCancelled)
                                    break;
                                _zipOutputStream.Write(buffer, 0, bytesRead);
                                OnTransferredBytes(bytesRead);
                            }

                            _zipOutputStream.CloseEntry();
                        }
                    }
                }
            }
        }

        public void AppendToZip(string fileName, Stream contentStream, string folderPath = null)
        {
            string entryPath = BuildEntryPath(fileName, folderPath);
            ZipEntry entry = new ZipEntry(entryPath)
            {
                DateTime = DateTime.Now,
                Size = contentStream.Length
            };
            _zipOutputStream.PutNextEntry(entry);

            long bytesToTransfer = contentStream.Length;
            int bufferSize = 4096;
            byte[] buffer = new byte[bufferSize];

            while (bytesToTransfer > 0)
            {
                int chunkSize = contentStream.Read(buffer, 0, bufferSize);
                if (chunkSize == 0)
                    break;

                _zipOutputStream.Write(buffer, 0, chunkSize);
                bytesToTransfer -= chunkSize;
                OnTransferredBytes(chunkSize);
            }

            _zipOutputStream.CloseEntry();
        }

        private string BuildEntryPath(string fileName, string folderPath)
        {
            string path = folderPath == null ? string.Empty : EnsureTrailingSlash(folderPath);
            return Path.Combine(path, fileName);
        }

        private string EnsureTrailingSlash(string folderPath)
        {
            return !string.IsNullOrWhiteSpace(folderPath) && !folderPath.EndsWith(Path.DirectorySeparatorChar.ToString())
                ? folderPath + Path.DirectorySeparatorChar
                : folderPath;
        }

        public void Save()
        {
            _zipOutputStream.Finish();
            _zipOutputStream.Close();
        }

        public void Dispose()
        {
            _zipOutputStream?.Dispose();
        }

        protected virtual void OnTransferredBytes(int bytes)
        {
            TransferredBytes?.Invoke(this, bytes);
        }
    }
}
