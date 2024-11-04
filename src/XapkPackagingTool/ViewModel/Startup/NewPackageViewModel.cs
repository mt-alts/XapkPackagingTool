/*
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using System.IO;
using System.Windows.Input;
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
        private string _path;
        private string _packageName;
        private string _appName;

        private readonly ISaveFileService _saveFileService;
        private readonly IRecentManager _recentManager;
        private readonly IConfigService _configService;
        private readonly IXapkConfigService _dataService;
        private readonly IWindowService _windowService;

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

        public string SavePath
        {
            get => _path;
            set
            {
                _path = value;
                OnPropertyChanged(nameof(SavePath));
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
            ISaveFileService saveFileService
        )
        {
            _recentManager = recentManager;
            _configService = configService;
            _dataService = xapkConfigService;
            _windowService = windowService;
            _saveFileService = saveFileService;
            BackCommand = new RelayCommand(BackCommandExecute);
            CreatePackageCommand = new RelayCommand(CreatePackageExecute);
            SelectLocationCommand = new RelayCommand(SelectLocationExecute);
        }

        private void BackCommandExecute()
        {
            SwitchBackRequested?.Invoke(this, new EventArgs());
        }

        private void CreatePackageExecute()
        {
            try
            {
                if (ValidateNewPackage())
                {
                    var config = _configService.CreateNew();
                    config.ConfigName = ConfigName;
                    config.Manifest.PackageName = PackageName;
                    config.Manifest.Name = AppName;
                    config.BuildPath =
                        $"{Path.Combine(Path.GetDirectoryName(SavePath), $"{PackageName}.xapk")}";
                    config.BaseApk = string.Empty;
                    _configService.ConfigPath = SavePath;
                    _configService.Save(SavePath, config);
                    _dataService.LoadConfig(config);

                    _recentManager.AddRecentFile(SavePath);

                    _windowService.ShowWindow<MainViewModel>();
                    _windowService.CloseWindow<StartupViewModel>();
                }
            }
            catch (Exception exc)
            {
                ExceptionHandler.HandleException(exc);
            }
        }

        private void SelectLocationExecute()
        {
            var path = _saveFileService.OpenDialog(
                "LabelConfigurationFileLocation".Localize(),
                DialogFilters.XapkConfigFiles
            );
            if (string.IsNullOrWhiteSpace(path))
                return;
            SavePath = path;
        }

        private bool ValidateNewPackage()
        {
            return !(string.IsNullOrWhiteSpace(SavePath) || string.IsNullOrWhiteSpace(PackageName));
        }
    }
}
