/*
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using System.Collections.ObjectModel;
using System.Windows.Input;
using XapkPackagingTool.Common.Data.Model.Xapk;
using XapkPackagingTool.Constants;
using XapkPackagingTool.Helper;
using XapkPackagingTool.Service;
using XapkPackagingTool.Service.Interfaces;
using XapkPackagingTool.Service.Interfaces.DataService;
using XapkPackagingTool.Utility.Reader;
using XapkPackagingTool.Utility.RecentItems;
using XapkPackagingTool.ViewModel.Main;

namespace XapkPackagingTool.ViewModel.Startup
{
    internal class GettingStartedViewModel : ViewModelBase
    {
        private readonly IRecentManager _recentManager;
        private readonly IOpenFileService _openFileService;
        private readonly IConfigService _configService;
        private readonly IXapkConfigService _dataService;
        private readonly IPackageReader _packageReader;
        private readonly IWindowService _windowService;

        public event EventHandler<string> SwitchVMRequested;

        public ObservableCollection<RecentFile> RecentFiles
        {
            get => _recentManager.RecentFiles;
        }

        public int RecentFilesSelectedIndex { get; set; } = -1;

        public string ProductName
        {
            get => Helper.AppInfoHelper.GetAppName();
        }
        public string ProductVersion
        {
            get => $"Version {Helper.AppInfoHelper.GetAppVersion()}";
        }

        public ICommand OpenConfigCommand { get; private set; }
        public ICommand CreateNewPackageCommand { get; private set; }
        public ICommand ImportPackageCommand { get; private set; }
        public ICommand OpenRecentCommand { get; private set; }
        public ICommand DeleteRecentFileCommand { get; private set; }

        public GettingStartedViewModel(
            IConfigService configService,
            IXapkConfigService xapkConfigService,
            IWindowService windowService,
            IRecentManager recentManager,
            IPackageReader packageReader,
            IOpenFileService openFileService
        )
        {
            _configService = configService;
            _dataService = xapkConfigService;
            _windowService = windowService;
            _recentManager = recentManager;
            _packageReader = packageReader;
            _openFileService = openFileService;

            OpenConfigCommand = new RelayCommand(OpenConfigExecute);
            CreateNewPackageCommand = new RelayCommand(CreateNewPackageExecute);
            ImportPackageCommand = new RelayCommand(ImportPackageExecute);
            OpenRecentCommand = new RelayCommand(OpenRecentExecute);
            DeleteRecentFileCommand = new RelayCommand<object>(DeleteRecentFileExecute);
        }

        private void CreateNewPackageExecute()
        {
            SwitchVMRequested?.Invoke(this, StartupViewNavigationKeys.CREATE_NEW);
        }

        private void OpenConfigExecute()
        {
            try
            {
                var path = _openFileService.OpenDialog(
                    "StrOpenXapkConfig".Localize(),
                    DialogFilters.XapkConfigFiles
                );
                if (string.IsNullOrWhiteSpace(path))
                    return;
                var config = OpenXapkConfigFile(path);
                OpenConfig(config, path);
            }
            catch (Exception exc)
            {
                ExceptionHandler.HandleException(exc);
            }
        }

        private void OpenRecentExecute()
        {
            try
            {
                if (IsRecentFileNotSelected())
                    return;

                var (config, filePath) = RecentConfigLoader.LoadRecentConfig(
                    RecentFiles[RecentFilesSelectedIndex]
                );

                OpenConfig(config, filePath);
            }
            catch (Exception exc)
            {
                ExceptionHandler.HandleException(exc);
            }
        }

        private bool IsRecentFileNotSelected() => RecentFilesSelectedIndex < 0;

        private void DeleteRecentFileExecute(object recentFile)
        {
            RecentFile rFile = (RecentFile)recentFile;
            if (!RecentFiles.Contains(rFile))
                return;
            var path = System.IO.Path.Combine(rFile.FilePath, rFile.FileName);
            DeleteRecentFile(path);
        }

        private void ImportPackageExecute()
        {
            try
            {
                var path = _openFileService.OpenDialog(
                    "StrImportPackage".Localize(),
                    DialogFilters.PackageFiles
                );
                if (string.IsNullOrWhiteSpace(path))
                    return;
                var config = _packageReader.Read(path);
                OpenConfig(config, path);
            }
            catch (Exception exc)
            {
                ExceptionHandler.HandleException(exc);
            }
        }

        private XapkConfig OpenXapkConfigFile(string filePath)
        {
            var config = _configService.LoadFromDisk(filePath);
            _configService.ConfigPath = filePath;
            return config;
        }

        private void OpenConfig(XapkConfig config, string path)
        {
            _recentManager.AddRecentFile(path);
            OpenConfig(config);
        }

        private void OpenConfig(XapkConfig config)
        {
            _dataService.LoadConfig(config);

            _windowService.ShowWindow<MainViewModel>();
            _windowService.CloseWindow<StartupViewModel>();
        }

        private void DeleteRecentFile(string file)
        {
            if (!string.IsNullOrWhiteSpace(file))
            {
                _recentManager.DeleteRecentFile(file);
                OnPropertyChanged(nameof(RecentFiles));
            }
        }
    }
}
