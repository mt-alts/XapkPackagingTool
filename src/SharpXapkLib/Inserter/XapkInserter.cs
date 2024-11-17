/*
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using SharpXapkLib.Builder;
using SharpXapkLib.Exceptions;
using SharpXapkLib.Utility;
using System.IO;
using System.Text;
using XapkPackagingTool.Common.Data.Model.Xapk;
using XapkPackagingTool.Common.Utility.ZipUtility;

namespace SharpXapkLib.Inserter
{
    internal class XapkInserter
    {
        private const string MANIFEST_FILE_NAME = "manifest.json";

        private readonly ZipInserter _zipInserter;
        private readonly string _outputFile;
        private long totalSize;
        private long _totalTransferredBytes;
        private readonly XapkFileMap _fileMap;

        public event EventHandler<int> ProgressChanged;
        public event EventHandler InsertStarted;
        public event EventHandler InsertCompleted;

        private volatile bool isCancelled = false;
        public bool IsCancelled
        {
            get => isCancelled;
            set => isCancelled = value;
        }

        internal XapkInserter(XapkFileMap xapkFileMap, string outputFilePath)
        {
            _outputFile = outputFilePath;
            _fileMap =
                xapkFileMap
                ?? throw new PackagingResourcesNotSpecifiedException(
                    "PackagingResourcesNotSpecified".Localize()
                );
            if (string.IsNullOrWhiteSpace(outputFilePath))
                throw new PackagingDirectoryNotSpecifiedException(
                    "PackagingDirectoryNotSpecified".Localize()
                );
            _zipInserter = new ZipInserter(outputFilePath);
             _zipInserter.TransferredBytes += InserterTransferredBytes;
        }

        private void InserterTransferredBytes(object? sender, int transferredBytes)
        {
            _totalTransferredBytes = _totalTransferredBytes + transferredBytes;
            int percent = (int)((double)_totalTransferredBytes / totalSize * 100);
            OnProgressChanged(percent);
        }

        private static long CalculateBuildSize(XapkFileMap map)
        {
            IBuildSizeCalculator buildSizeCalculator = new BuildSizeCalculator(map);
            return buildSizeCalculator.GetTotalSize();
        }

        internal void Insert()
        {
            try
            {
                totalSize = CalculateTotalBuildSize();
                OnInsertStarted();

                InsertUncompressedFiles();
                InsertCompressedFiles();

                OnInsertCompleted();
            }
            catch (Exception exc) when (IsCancelled)
            {
                throw new OperationCanceledException();
            }
            catch
            {
                throw;
            }
        }

        private long CalculateTotalBuildSize()
        {
            return CalculateBuildSize(_fileMap);
        }

        private void InsertUncompressedFiles()
        {
            var uncompressedFiles = _fileMap.Uncompressed;
            if (uncompressedFiles.Any())
                InsertFiles(uncompressedFiles);
        }

        private void InsertCompressedFiles()
        {
            var compressedFiles = _fileMap.Compressed;
            if (compressedFiles.Any())
                InsertCompressedFiles(compressedFiles);
        }


        internal void CancelInsert()
        {
            IsCancelled = true;
            _zipInserter.IsCancelled = true;
        }

        internal void InsertFiles(List<XapkInsertMap> xapkMaps)
        {
            foreach (var file in xapkMaps)
            {
                if (IsCancelled)
                    break;

                InsertFile(file);
            }
        }

        private void InsertFile(XapkInsertMap file)
        {
            _zipInserter.AddFile(file.Source, file.Target);
        }

        internal void InsertCompressedFiles(List<CompressedFileGroup> compressedFiles)
        {
            foreach (var file in compressedFiles)
            {
                if (IsCancelled)
                    break;

                var copyInfos = CreateFileCopyInfoList(file);
                InsertCompressedFile(file.CompressedFileName, copyInfos);
            }
        }

        private static List<FileCopyInfo> CreateFileCopyInfoList(CompressedFileGroup fileGroup)
        {
            return fileGroup
                .Entries.Select(entry => new FileCopyInfo(entry.Source, entry.Target))
                .ToList();
        }

        private void InsertCompressedFile(string compressedFileName, List<FileCopyInfo> copyInfos)
        {
            _zipInserter.AddFilesFromZip(compressedFileName, copyInfos);
        }

        public void InsertMetadata(XapkManifest manifest)
        {
            manifest.TotalSize = totalSize;
            var metadata = MetadataCreator.CreateMetadata(manifest);
            using (Stream contentStream = new MemoryStream(Encoding.UTF8.GetBytes(metadata)))
                _zipInserter.AppendToZip(MANIFEST_FILE_NAME, contentStream);
        }

        public void Apply()
        {
            _zipInserter.Save();
        }

        private void OnInsertCompleted()
        {
            InsertCompleted?.Invoke(this, new EventArgs());
        }

        private void OnInsertStarted()
        {
            InsertStarted?.Invoke(this, new EventArgs());
        }

        private void OnProgressChanged(int percent)
        {
            ProgressChanged?.Invoke(this, percent);
        }
    }
}
