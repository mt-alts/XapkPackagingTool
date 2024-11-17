/*
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using System.IO;
using System.Windows.Controls;
using System.Windows.Input;
using XapkPackagingTool.Common.Data.Model.Xapk;
using XapkPackagingTool.Constants;
using XapkPackagingTool.Helper;
using XapkPackagingTool.Service;
using XapkPackagingTool.Service.Interfaces;
using XapkPackagingTool.Service.Interfaces.DataService;
using XapkPackagingTool.Utility.RecentItems;
using XapkPackagingTool.ViewModel.Main;

namespace XapkPackagingTool.ViewModel.Startup
{
    internal class NewPackageViewModel : ViewModelBase
    {
        private string _configName;
        private string _location;
        private string _packageName;
        private string _appName;

        private readonly IFolderSelectionService _folderService;
        private readonly IRecentManager _recentManager;
        private readonly IConfigService _configService;
        private readonly IXapkConfigService _dataService;
        private readonly IWindowService _windowService;
        private readonly IMessageDialogService _messageService;

        public event EventHandler SwitchBackRequested;

        public string AppName
        {
            get { return _appName; }
            set
            {
                _appName = value;
                OnPropertyChanged(nameof(AppName));
            }
        }

        public string ConfigName
        {
            get => _configName;
            set
            {
                _configName = value;
                OnPropertyChanged(nameof(ConfigName));
            }
        }

        public string SaveLocation
        {
            get => _location;
            set
            {
                _location = value;
                OnPropertyChanged(nameof(SaveLocation));
                OnPropertyChanged(nameof(CanCreatePackage));
            }
        }

        public string PackageName
        {
            get => _packageName;
            set
            {
                _packageName = value;
                OnPropertyChanged(nameof(PackageName));
                OnPropertyChanged(nameof(CanCreatePackage));
            }
        }

        public bool CanCreatePackage => ValidateNewPackage();

        public ICommand BackCommand { get; }
        public ICommand CreatePackageCommand { get; }
        public ICommand SelectLocationCommand { get; }

        public NewPackageViewModel(
            IRecentManager recentManager,
            IConfigService configService,
            IXapkConfigService xapkConfigService,
            IWindowService windowService,
            IFolderSelectionService folderSelectionService,
            IMessageDialogService messageDialogService
        )
        {
            _recentManager = recentManager;
            _configService = configService;
            _dataService = xapkConfigService;
            _windowService = windowService;
            _folderService = folderSelectionService;
            _messageService = messageDialogService;

            BackCommand = new RelayCommand(BackCommandExecute);
            CreatePackageCommand = new RelayCommand(CreatePackageExecute);
            SelectLocationCommand = new RelayCommand(SelectLocationExecute);

            var path = EnvironmentPaths.StoredXapkConfigFiles;
            SaveLocation = EnvironmentPaths.StoredXapkConfigFiles;
        }

        private void BackCommandExecute()
        {
            SwitchBackRequested?.Invoke(this, new EventArgs());
        }

        private void CreatePackageExecute()
        {
            try
            {
                var savePath = GenerateConfigSavePath();

                if (!ValidateAndCreatePackage(savePath))
                    return;

                var config = GenerateNewConfig();
                SaveAndLoadConfig(config, savePath);

                FinalizeCreationProcess();
            }
            catch (Exception exc)
            {
                ExceptionHandler.HandleException(exc);
            }
        }

        private string GenerateConfigSavePath()
        {
            return Path.Combine(
                SaveLocation,
                FileNameHelper.CreateAvaliableFileName(ConfigName, FileExtensions.XAPK_CONFIG)
            );
        }

        private bool ValidateAndCreatePackage(string savePath)
        {
            if (!ValidateNewPackage())
                return false;

            if (!IsConfigFileValid())
            {
                ShowWarningMessage(
                    string.Format("StrFileAlreadyExistsWarning".Localize(), savePath),
                    "StrAppName".Localize()
                );
                return false;
            }
            return true;
        }

        private XapkConfig GenerateNewConfig()
        {
            var config = CreateBaseConfig();
            SetConfigPaths(config);
            return config;
        }

        private XapkConfig CreateBaseConfig()
        {
            var config = _configService.CreateNew();
            config.ConfigName = ConfigName;
            config.Manifest.PackageName = PackageName;
            config.Manifest.Name = AppName;
            config.BaseApk = string.Empty;
            return config;
        }

        private void SetConfigPaths(XapkConfig config)
        {
            config.BuildPath = GetUniqueBuildPath();
        }

        private string GetUniqueBuildPath()
        {
            return Common.Helpers.FileHelpers.FileNameHelper.GetUniqueFileName(
                Path.Combine(
                    EnvironmentPaths.StoredXapkPackages,
                    FileNameHelper.CreateAvaliableFileName(ConfigName, FileExtensions.XAPK)
                )
            );
        }

        private void SaveAndLoadConfig(XapkConfig config, string savePath)
        {
            _configService.ConfigPath = savePath;
            _configService.Save(savePath, config);
            _dataService.LoadConfig(config);

            _recentManager.AddRecentFile(savePath);
        }

        private void FinalizeCreationProcess()
        {
            _windowService.ShowWindow<MainViewModel>();
            _windowService.CloseWindow<StartupViewModel>();
        }

        private bool IsConfigFileValid()
        {
            return !File.Exists(
                Path.Combine(
                    SaveLocation,
                    FileNameHelper.CreateAvaliableFileName(ConfigName, FileExtensions.XAPK_CONFIG)
                )
            );
        }

        private void ShowWarningMessage(string message, string title)
        {
            _messageService.ShowWarning(message, title);
        }

        private void SelectLocationExecute()
        {
            var (isResult, path) = _folderService.OpenDialog(
                "LabelConfigurationFileLocation".Localize()
            );
            if (!isResult)
                return;
            SaveLocation = path;
        }

        private bool ValidateNewPackage()
        {
            return !(
                string.IsNullOrWhiteSpace(SaveLocation)
                || string.IsNullOrWhiteSpace(ConfigName)
                || string.IsNullOrWhiteSpace(PackageName)
            );
        }
    }
}
