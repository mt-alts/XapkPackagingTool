/* 
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

//using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Zip;
using System.IO;

namespace XapkPackagingTool.Common.Utility.ZipUtility
{
    //public class ZipInserter : IZipInserter, IDisposable
    //{
    //    private readonly string _targetZipPath;
    //    private FileStream _fileStream;
    //    private ZipArchive _zipArchive;
    //    private bool _disposed = false;
    //    private const int BufferSize = 16384; // 16KB Buffer

    //    public event EventHandler<int> TransferredBytes;

    //    public ZipInserter(string targetZipPath)
    //    {
    //        _targetZipPath = targetZipPath;
    //        InitializeZipArchive();
    //    }

    //    private void InitializeZipArchive()
    //    {
    //        // ZIP dosyasını aç veya oluştur
    //        _fileStream = new FileStream(_targetZipPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
    //        _zipArchive = new ZipArchive(_fileStream, ZipArchiveMode.Update, leaveOpen: true);
    //    }

    //    public void AddFileFromLocal(string source, string target)
    //    {
    //        try
    //        {
    //            // Hedef dosya yolunu oluştur
    //            string entryPath = BuildEntryPath(target);

    //            // Yeni bir ZIP girişi oluştur
    //            ZipArchiveEntry entry = _zipArchive.CreateEntry(entryPath, CompressionLevel.Optimal);

    //            using (Stream entryStream = entry.Open())
    //            using (FileStream sourceStream = new FileStream(source, FileMode.Open, FileAccess.Read, FileShare.Read))
    //            using (BufferedStream bufferedSourceStream = new BufferedStream(sourceStream, 8192)) // 8KB buffer
    //            {
    //                CopyStreamInChunks(bufferedSourceStream, entryStream);
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            throw new IOException($"Yerel dosya eklenirken hata oluştu: {source}", ex);
    //        }
    //    }

    //    private void CopyStreamInChunks(Stream sourceStream, Stream targetStream)
    //    {
    //        byte[] buffer = new byte[8192];  // 8 KB buffer boyutu
    //        int bytesRead;

    //        while ((bytesRead = sourceStream.Read(buffer, 0, buffer.Length)) > 0)
    //        {
    //            targetStream.WriteAsync(buffer, 0, bytesRead).GetAwaiter().GetResult();
    //        }
    //        Debug.Write("");
    //    }



    //    public void AddFilesFromZip(string sourceZipPath, IEnumerable<FileCopyInfo> fileCopyInfos)
    //    {
    //        try
    //        {
    //            using (FileStream sourceZipStream = new FileStream(sourceZipPath, FileMode.Open, FileAccess.Read, FileShare.Read))
    //            using (ZipArchive sourceZip = new ZipArchive(sourceZipStream, ZipArchiveMode.Read, leaveOpen: false))
    //            {
    //                foreach (var fileCopyInfo in fileCopyInfos)
    //                {
    //                    ZipArchiveEntry sourceEntry = sourceZip.GetEntry(fileCopyInfo.Source);
    //                    if (sourceEntry != null)
    //                    {
    //                        string entryPath = BuildEntryPath(fileCopyInfo.Target);
    //                        ZipArchiveEntry targetEntry = _zipArchive.CreateEntry(entryPath, CompressionLevel.Optimal);

    //                        using (Stream sourceEntryStream = sourceEntry.Open())
    //                        using (Stream targetEntryStream = targetEntry.Open())
    //                        {
    //                            CopyStream(sourceEntryStream, targetEntryStream);
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            throw new IOException($"ZIP dosyasından dosya eklenirken hata oluştu: {sourceZipPath}", ex);
    //        }
    //    }

    //    public void AddFile(string fileName, string content, string folderPath = null)
    //    {
    //        try
    //        {
    //            string entryPath = BuildEntryPath(fileName, folderPath);
    //            ZipArchiveEntry entry = _zipArchive.CreateEntry(entryPath, CompressionLevel.Optimal);

    //            using (Stream entryStream = entry.Open())
    //            using (StreamWriter writer = new StreamWriter(entryStream, Encoding.UTF8))
    //            {
    //                byte[] contentBytes = Encoding.UTF8.GetBytes(content);
    //                int bytesToTransfer = contentBytes.Length;
    //                int offset = 0;

    //                while (bytesToTransfer > 0)
    //                {
    //                    int chunkSize = Math.Min(BufferSize, bytesToTransfer);
    //                    writer.BaseStream.Write(contentBytes, offset, chunkSize);
    //                    offset += chunkSize;
    //                    bytesToTransfer -= chunkSize;
    //                    OnTransferredBytes(chunkSize);
    //                }

    //                writer.Flush();
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            throw new IOException($"İçerik eklenirken hata oluştu: {fileName}", ex);
    //        }
    //    }

    //    private byte[] buffer;

    //    private void CopyStream(Stream source, Stream destination)
    //    {
    //        buffer = new byte[BufferSize];
    //        int bytesRead;
    //        while ((bytesRead = source.Read(buffer, 0, buffer.Length)) > 0)
    //        {
    //            destination.Write(buffer, 0, bytesRead);
    //            OnTransferredBytes(bytesRead);
    //        }
    //    }

    //    private string BuildEntryPath(string fileName, string folderPath = null)
    //    {
    //        string path = folderPath == null ? string.Empty : EnsureTrailingSlash(folderPath);
    //        // ZIP standardı için '/' kullanımı
    //        return Path.Combine(path, fileName).Replace(Path.DirectorySeparatorChar, '/');
    //    }

    //    private string EnsureTrailingSlash(string folderPath)
    //    {
    //        return !string.IsNullOrWhiteSpace(folderPath) && !folderPath.EndsWith("/", StringComparison.Ordinal)
    //            ? folderPath.TrimEnd(Path.DirectorySeparatorChar) + "/"
    //            : folderPath;
    //    }

    //    public void Save()
    //    {
    //        Dispose();
    //    }

    //    protected virtual void Dispose(bool disposing)
    //    {
    //        if (!_disposed)
    //        {
    //            if (disposing)
    //            {
    //                _zipArchive?.Dispose();
    //                _fileStream?.Dispose();
    //            }

    //            _disposed = true;
    //        }
    //    }

    //    public void Dispose()
    //    {
    //        Dispose(true);
    //        GC.SuppressFinalize(this);
    //    }

    //    ~ZipInserter()
    //    {
    //        Dispose(false);
    //    }

    //    protected virtual void OnTransferredBytes(int bytes)
    //    {
    //        TransferredBytes?.Invoke(this, bytes);
    //    }
    //}




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

        private void InitializeZipOutputStream()
        {
            FileStream fs = new FileStream(_targetZipPath, FileMode.OpenOrCreate, FileAccess.Write);
            _zipOutputStream = new ZipOutputStream(fs)
            {
                IsStreamOwner = true // FileStream'in bizim kontrol etmemizi sağlamak için true
            };
        }

        public void AddFileFromLocal(string source, string target)
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

        public void AddFile(string fileName, string content, string folderPath = null)
        {
            string entryPath = BuildEntryPath(fileName, folderPath);
            byte[] contentBytes = System.Text.Encoding.UTF8.GetBytes(content);
            ZipEntry entry = new ZipEntry(entryPath)
            {
                DateTime = DateTime.Now,
                Size = contentBytes.Length
            };
            _zipOutputStream.PutNextEntry(entry);

            int bytesToTransfer = contentBytes.Length;
            int bufferSize = 4096;
            int offset = 0;

            while (bytesToTransfer > 0)
            {
                int chunkSize = Math.Min(bufferSize, bytesToTransfer);
                _zipOutputStream.Write(contentBytes, offset, chunkSize);
                offset += chunkSize;
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
