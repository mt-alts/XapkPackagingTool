/*
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using System.Configuration;
using System.Diagnostics;
using System.Windows.Input;
using XapkPackagingTool.Common.Data.Model.Xapk.Interfaces;
using XapkPackagingTool.Common.Utility.Reflection;
using XapkPackagingTool.Constants;
using XapkPackagingTool.Exceptions;
using XapkPackagingTool.Helper;
using XapkPackagingTool.Service;
using XapkPackagingTool.Service.Interfaces;
using XapkPackagingTool.Service.Interfaces.DataService;
using XapkPackagingTool.Service.SystemServices;
using XapkPackagingTool.Utility.Reader;

namespace XapkPackagingTool.ViewModel.Main
{
    internal class PackageMetadataViewModel : ViewModelBase
    {
        private static readonly string MIN_SDK_LEVEL =
            ConfigurationManager.AppSettings["MinSdkVersion"] ?? "9";

        private static readonly string LATEST_SDK_LEVEL =
            ConfigurationManager.AppSettings["LatestSdkVersion"] ?? "35";

        private readonly IOpenFileService _openFileService;
        private readonly IManifestService _manifestService;
        private readonly IPackageReader _packageReader;

        public ICommand ImportFromPackageCommand { get; }
        public ICommand SelectIconCommand { get; }
        public ICommand ClearIconCommand { get; }

        public string Icon
        {
            get { return _manifestService.Manifest.Icon ?? string.Empty; }
            set
            {
                _manifestService.Manifest.Icon = value;
                OnPropertyChanged(nameof(Icon));
                var v = value;
                Debug.Write(v);
            }
        }

        public string Name
        {
            get { return _manifestService.Manifest.Name ?? string.Empty; }
            set
            {
                _manifestService.Manifest.Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public string PackageName
        {
            get { return _manifestService.Manifest.PackageName ?? string.Empty; }
            set
            {
                _manifestService.Manifest.PackageName = value;
                OnPropertyChanged(nameof(PackageName));
            }
        }

        public string VersionCode
        {
            get { return _manifestService.Manifest.VersionCode ?? string.Empty; }
            set
            {
                _manifestService.Manifest.VersionCode = value;
                OnPropertyChanged(nameof(VersionCode));
            }
        }

        public string VersionName
        {
            get { return _manifestService.Manifest.VersionName ?? string.Empty; }
            set
            {
                _manifestService.Manifest.VersionName = value;
                OnPropertyChanged(nameof(VersionName));
            }
        }

        public string MinSdkVersion
        {
            get { return _manifestService.Manifest.MinSdkVersion ?? MIN_SDK_LEVEL; }
            set
            {
                _manifestService.Manifest.MinSdkVersion = value.ToString();
                OnPropertyChanged(nameof(MinSdkVersion));
            }
        }

        public string TargetSdkVersion
        {
            get { return _manifestService.Manifest.TargetSdkVersion ?? LATEST_SDK_LEVEL; }
            set
            {
                _manifestService.Manifest.TargetSdkVersion = value;
                OnPropertyChanged(nameof(TargetSdkVersion));
            }
        }

        public PackageMetadataViewModel(
            IOpenFileService openFileService,
            IXapkConfigService xapkConfigService,
            IPackageReader packageReader
        )
        {
            _openFileService = openFileService;
            _packageReader = packageReader;

            xapkConfigService.DataChanged += ManifestService_DataChanged;
            _manifestService = xapkConfigService;

            ImportFromPackageCommand = new RelayCommand(ImportFromPackageExecute);
            SelectIconCommand = new RelayCommand(SelectIconExecute);
            ClearIconCommand = new RelayCommand(ClearIconExecute);
        }

        private void ManifestService_DataChanged(object? sender, EventArgs e)
        {
            ReflectionHelper.TransferProperties(_manifestService.Manifest, this);
        }

        private void ImportFromPackageExecute()
        {
            try
            {
                var path = new OpenFileService().OpenDialog(
                    "StrOpenPackage".Localize(),
                    DialogFilters.PackageFiles
                );
                if (string.IsNullOrWhiteSpace(path))
                    return;

                var config = _packageReader.Read(path);
                IMetadata metadata = config.Manifest;

                if (metadata == null)
                    throw new PackageImportException(path);

                ReflectionHelper.TransferProperties(metadata, this);
            }
            catch (Exception exc)
            {
                ExceptionHandler.HandleException(exc);
            }
        }

        private void SelectIconExecute()
        {
            try
            {
                var path = new OpenFileService().OpenDialog(
                    "StrSelectPackageIcon".Localize(),
                    DialogFilters.PngImage
                );
                if (string.IsNullOrWhiteSpace(path))
                    return;
                Icon = path;
            }
            catch (Exception exc)
            {
                ExceptionHandler.HandleException(exc);
            }
        }

        private void ClearIconExecute()
        {
            Icon = string.Empty;
        }
    }
}
