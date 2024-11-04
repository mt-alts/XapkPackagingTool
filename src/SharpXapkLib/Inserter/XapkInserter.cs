/*
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using SharpXapkLib.Builder;
using SharpXapkLib.Exceptions;
using SharpXapkLib.Utility;
using XapkPackagingTool.Common.Data.Model.Xapk;
using XapkPackagingTool.Common.Utility.ZipUtility;

namespace SharpXapkLib.Inserter
{
    internal class XapkInserter
    {
        private const string MANIFEST_FILE_NAME = "manifest.json";

        private readonly ZipInserter _zipInserter;
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
            long buildSize = buildSizeCalculator.GetTotalSize();
            return buildSize;
        }

        internal void Insert()
        {
            try
            {
                totalSize = CalculateBuildSize(_fileMap);

                OnInsertStarted();

                var uncompressedFiles = _fileMap.Uncompressed;
                if (uncompressedFiles != null && uncompressedFiles.Count > 0)
                    InsertFiles(uncompressedFiles);

                var compressedFiles = _fileMap.Compressed;
                if (compressedFiles != null && compressedFiles.Count > 0)
                    InsertCompressedFiles(compressedFiles);

                OnInsertCompleted();
            }
            catch (Exception exc)
            {
                if (IsCancelled)
                    throw new OperationCanceledException();
                else
                    throw exc;
            }
        }

        internal void CancelInsert()
        {
            this.IsCancelled = true;
            _zipInserter.IsCancelled = true;
        }

        internal void InsertFiles(List<XapkInsertMap> xapkMaps)
        {
            foreach (var file in xapkMaps)
            {
                if (IsCancelled)
                    break;
                _zipInserter.AddFileFromLocal(file.Source, file.Target);
            }
        }

        internal void InsertCompressedFiles(List<CompressedFileGroup> compressedFiles)
        {
            foreach (var file in compressedFiles)
            {
                if (IsCancelled)
                    break;
                List<FileCopyInfo> copyInfos = file
                    .Entries.Select(x => new FileCopyInfo(x.Source, x.Target))
                    .ToList();
                _zipInserter.AddFilesFromZip(file.CompressedFileName, copyInfos);
            }
        }

        public void InsertMetadata(XapkManifest manifest)
        {
            manifest.TotalSize = totalSize;
            var metadata = MetadataCreator.CreateMetadata(manifest);
            _zipInserter.AddFile(MANIFEST_FILE_NAME, metadata);
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
