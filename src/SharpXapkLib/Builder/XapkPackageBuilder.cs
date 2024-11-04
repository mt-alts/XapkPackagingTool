/*
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using SharpXapkLib.Exceptions;
using SharpXapkLib.Inserter;
using SharpXapkLib.Utility;
using XapkPackagingTool.Common.Data.Model.Xapk;

namespace SharpXapkLib.Builder
{
    /// <summary>
    /// Builds an Xapk package.
    /// </summary>
    public class XapkPackageBuilder
    {
        private readonly XapkConfig _configuration;
        private readonly IXapkInserterFactory _xapkInserterFactory;
        private readonly IFileMapper _fileMapper;

        /// <summary>
        /// Occurs when the build process starts.
        /// </summary>
        public event EventHandler BuildStarted;

        /// <summary>
        /// Occurs when the insert process starts.
        /// </summary>
        public event EventHandler InsertStarted;

        /// <summary>
        /// Occurs when the insert process completes.
        /// </summary>
        public event EventHandler InsertCompleted;

        /// <summary>
        /// Occurs when the build process finishes.
        /// </summary>
        public event EventHandler<string> BuildCompleted;

        /// <summary>
        /// Occurs when the build process fails.
        /// </summary>
        public event EventHandler<string> BuildFailed;

        /// <summary>
        /// Occurs when the insert progress changes.
        /// </summary>
        public event EventHandler<int> InsertPercentChanged;

        private XapkInserter _xapkInserter;

        public XapkPackageBuilder(XapkConfig config)
        {
            _configuration = config;
            _xapkInserterFactory = new XapkInserterFactory();
            _fileMapper = new FileMapper();
        }

        public void Build()
        {
            try
            {
                OnEvent(BuildStarted);
                var xapkMap = CreateXapkFileMap();
                _xapkInserter = CreateInserter(xapkMap);

                OnEvent(InsertStarted);
                _xapkInserter.Insert();
                OnEvent(InsertCompleted);

                _xapkInserter.InsertMetadata(_configuration.Manifest);
                _xapkInserter.Apply();

                OnBuildCompleted(_configuration.BuildPath);
            }
            catch (Exception ex)
            {
                OnBuildFailed(ExceptionHandler.GetExceptionMessage(ex));
            }
        }

        public void CancelBuild()
        {
            _xapkInserter.CancelInsert();
        }

        private static string ManifestBuild(XapkManifest xapkManifest)
        {
            try
            {
                return ManifestHandler.SerializeManifest(xapkManifest.DeepClone());
            }
            catch (Exception exc)
            {
                throw new MetadataConvertException(
                    string.Format("MetadataCreateError".Localize(), exc.Message)
                );
            }
        }

        private XapkInserter CreateInserter(XapkFileMap xapkFileMap)
        {
            var inserter = _xapkInserterFactory.CreateInserter(
                xapkFileMap,
                _configuration.BuildPath
            );
            inserter.ProgressChanged += OnInsertProgressChanged;
            inserter.InsertStarted += OnInsertStarted;
            inserter.InsertCompleted += OnInsertCompleted;
            return inserter;
        }

        private XapkFileMap CreateXapkFileMap()
        {
            var files = FileGenerator.GenerateFiles(_configuration);
            _fileMapper.AddFiles(files);
            var xapkMap = new XapkFileMap(
                _fileMapper.GetUncompressedFiles(),
                _fileMapper.GroupItemsByCompressedFile()
            );
            return xapkMap;
        }

        private void OnBuildCompleted(string packagePath)
        {
            BuildCompleted?.Invoke(this, packagePath);
        }

        private void OnInsertCompleted(object? sender, EventArgs e)
        {
            OnEvent(InsertCompleted);
        }

        private void OnInsertStarted(object? sender, EventArgs e)
        {
            InsertStarted(this, EventArgs.Empty);
        }

        private void OnInsertProgressChanged(object? sender, int percent)
        {
            InsertPercentChanged?.Invoke(this, percent);
        }

        private void OnEvent(EventHandler eventHandler)
        {
            eventHandler?.Invoke(this, EventArgs.Empty);
        }

        private void OnBuildFailed(string message)
        {
            BuildFailed?.Invoke(this, message);
        }
    }
}
