/*
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using SharpXapkLib.Builder;
using XapkPackagingTool.Common.Data.Model.Xapk;
using XapkPackagingTool.Constants;
using XapkPackagingTool.Enums;
using XapkPackagingTool.Helper;
using XapkPackagingTool.Service;
using XapkPackagingTool.Service.Interfaces;
using XapkPackagingTool.Service.Interfaces.DataService;
using XapkPackagingTool.Utility.RecentItems;
using XapkPackagingTool.Utility.Validators.XapkPackage;
using XapkPackagingTool.ViewModel.Startup;

namespace XapkPackagingTool.ViewModel.Main
{
    internal class MainViewModel : ViewModelBase
    {
        private readonly ISaveFileService _saveFileService;
        private readonly IOpenFileService _openFileService;
        private readonly IConfirmDialogService _confirmDialogService;
        private readonly IWindowService _windowService;
        private readonly IConfigService _configService;
        private readonly IXapkConfigService _xapkConfigService;
        private readonly IProgressService _progressService;
        private readonly IRecentManager _recentManager;
        private readonly IDialogService _dialogService;

        private readonly string _initialXapkConfigHash;

        public ObservableCollection<string> RecentFiles
        {
            get =>
                new ObservableCollection<string>(
                    _recentManager
                        .RecentFiles?.Take(10)
                        .Select(recent => Path.Combine(recent.FilePath, recent.FileName))
                );
        }

        public ICommand NewPackageCommand { get; private set; }
        public ICommand OpenConfigCommand { get; private set; }
        public ICommand OpenPackageCommand { get; private set; }
        public ICommand CloseCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public ICommand SaveAsCommand { get; private set; }
        public ICommand ExitAppCommand { get; private set; }
        public ICommand OpenAboutCommand { get; private set; }
        public ICommand BuildCommand { get; private set; }
        public ICommand OpenRecentCommand { get; private set; }

        public IViewModelHost VMHost { get; init; }

        public MainViewModel(
            IViewModelHost vmHost,
            IConfigService configService,
            IProgressService progressService,
            IWindowService windowService,
            IOpenFileService openFileService,
            ISaveFileService saveFileService,
            IConfirmDialogService confirmDialogService,
            IRecentManager recentManager,
            IXapkConfigService xapkConfigService,
            IDialogService dialogService
        )
        {
            _saveFileService = saveFileService;
            _confirmDialogService = confirmDialogService;
            _recentManager = recentManager;
            _configService = configService;
            _openFileService = openFileService;
            _windowService = windowService;
            _progressService = progressService;
            _xapkConfigService = xapkConfigService;
            _dialogService = dialogService;
            VMHost = vmHost;
            RegisterCommands();
            CreateViewModels();
            VMHost.SwitchViewModelCommand.Execute("metadata");

            _initialXapkConfigHash = _xapkConfigService.GetConfigHashCode();
        }

        private void RegisterCommands()
        {
            BuildCommand = new RelayCommand(BuildCommandExecute);
            NewPackageCommand = new RelayCommand(NewPackageExecute);
            OpenConfigCommand = new RelayCommand(OpenConfigExecute);
            OpenPackageCommand = new RelayCommand(OpenPackageExecute);
            SaveCommand = new RelayCommand(SaveExecute);
            SaveAsCommand = new RelayCommand(SaveAsExecute);
            CloseCommand = new RelayCommand(CloseExecute);
            ExitAppCommand = new RelayCommand(ExitAppExecute);
            OpenAboutCommand = new RelayCommand(OpenAboutExecute);
            OpenRecentCommand = new RelayCommand<int>(OpenRecentExecute);
        }

        private void OpenRecentExecute(int index)
        {
            try
            {
                var (config, filePath) = RecentConfigLoader.LoadRecentConfig(
                    _recentManager.RecentFiles[index]
                );

                OpenConfig(config, filePath);
            }
            catch (Exception exc)
            {
                ExceptionHandler.HandleException(exc);
            }
        }

        private void OpenAboutExecute()
        {
            _dialogService.ShowDialogWithoutResult<AboutViewModel>();
        }

        private void ExitAppExecute()
        {
            if (CheckForUnsavedChanges())
                Application.Current?.Shutdown();
        }

        private void CloseExecute()
        {
            if (CheckForUnsavedChanges())
            {
                _windowService.ShowWindow<StartupViewModel>();
                _windowService.CloseWindow<MainViewModel>();
            }
        }

        private void SaveExecute()
        {
            SaveChanges();
        }

        private void SaveAsExecute()
        {
            SaveAsNewConfig();
        }

        private void OpenPackageExecute()
        {
            var path = _openFileService.OpenDialog(
                "StrOpenPackage".Localize(),
                DialogFilters.PackageFiles
            );
            if (string.IsNullOrWhiteSpace(path))
                return;

            var config = OpenPackage(path);
            OpenConfig(config, path);
        }

        private void OpenConfigExecute()
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

        private void CreateViewModels()
        {
            VMHost.AddViewModel<PackagingOptionsViewModel>(MainViewNavigationKeys.PACKAGE);
            VMHost.AddViewModel<PackageMetadataViewModel>(MainViewNavigationKeys.METADATA);
            VMHost.AddViewModel<PermissionsViewModel>(MainViewNavigationKeys.PERMISSIONS);
            VMHost.AddViewModel<LocalesViewModel>(MainViewNavigationKeys.LOCALES);
            VMHost.AddViewModel<ExpansionsViewModel>(MainViewNavigationKeys.EXPANSIONS);
            VMHost.AddViewModel<ApkVariantsViewModel>(MainViewNavigationKeys.APK_VARIANTS);
        }

        private void BuildCommandExecute()
        {
            try
            {
                if (BuildValidator.IsValid(_xapkConfigService.GetConfig()))
                {
                    var config = _xapkConfigService.GetConfig();
                    var xapkBuilder = new XapkPackageBuilder(config);
                    _progressService.CancelRequired += (sender, e) =>
                    {
                        xapkBuilder.CancelBuild();
                    };

                    InitBuilderEvents(xapkBuilder);

                    var _builderThread = new Thread(() =>
                    {
                        xapkBuilder.Build();
                    });

                    _builderThread.Start();
                    _progressService.ShowProgress();
                }
            }
            catch (Exception exc)
            {
                ExceptionHandler.HandleException(exc);
            }
        }

        private void InitBuilderEvents(XapkPackageBuilder xapkPackageBuilder)
        {
            xapkPackageBuilder.BuildStarted += XapkPackageBuilder_BuildStarted;
            xapkPackageBuilder.BuildFailed += XapkPackageBuilder_BuildFailed;
            xapkPackageBuilder.BuildCompleted += XapkPackageBuilder_BuildCompleted;
            xapkPackageBuilder.InsertStarted += XapkPackageBuilder_InsertStarted;
            xapkPackageBuilder.InsertCompleted += XapkPackageBuilder_InsertCompleted;
            xapkPackageBuilder.InsertPercentChanged += XapkPackageBuilder_InsertPercentChanged;
        }

        private void XapkPackageBuilder_InsertCompleted(object? sender, EventArgs e)
        {
            _progressService.UpdateStatusMessage("LabelFinalizing".Localize());
            _progressService.IndeterminateModeEnable(true);
        }

        private void XapkPackageBuilder_InsertPercentChanged(object? sender, int e)
        {
            _progressService.UpdateProgress(e);
        }

        private void XapkPackageBuilder_InsertStarted(object? sender, EventArgs e)
        {
            _progressService.UpdateStatusMessage("LabelAddingResources".Localize());
            _progressService.IndeterminateModeEnable(false);
        }

        private void XapkPackageBuilder_BuildCompleted(object? sender, string packagePath)
        {
            _progressService.ReportCompletion(
                packagePath ?? _xapkConfigService.BuildPath,
                "LabelPackagingCompleted".Localize()
            );
        }

        private void XapkPackageBuilder_BuildFailed(object? sender, string e)
        {
            _progressService.ReportFailure(e, "LabelPackagingFailed".Localize());
        }

        private void XapkPackageBuilder_BuildStarted(object? sender, EventArgs e)
        {
            _progressService.UpdateStatusMessage(
                "LabelPreparingResources".Localize(),
                "LabelPackaging".Localize()
            );
        }

        private void NewPackageExecute()
        {
            _windowService.ShowWindow<NewPackageViewModel>();
        }

        private bool CheckForUnsavedChanges()
        {
            string latestXapkConfigHash = _xapkConfigService.GetConfigHashCode();

            if (HasUnsavedChanges(latestXapkConfigHash))
                return true;
            var result = string.IsNullOrWhiteSpace(_configService.ConfigPath)
                ? _confirmDialogService.ShowWithCancel(
                    "LabelUnsavedConfigurationPrompt".Localize(),
                    "LabelAskToSaveChanges".Localize()
                )
                : _confirmDialogService.ShowWithCancel(
                    "LabelUnsavedChangesPrompt".Localize(),
                    "LabelAskToSaveChanges".Localize()
                );

            return HandleSaveConfirmResult(result);
        }

        private bool HasUnsavedChanges(string latestXapkConfigHash)
        {
            return _initialXapkConfigHash.Equals(latestXapkConfigHash);
        }

        private bool SaveChanges()
        {
            if (string.IsNullOrWhiteSpace(_configService.ConfigPath))
                return SaveAsNewConfig();
            else
                _configService.SaveChanges(_xapkConfigService.GetConfig());

            return true;
        }

        private bool SaveAsNewConfig()
        {
            string savePath = _saveFileService.OpenDialog("LabelSave".Localize(), Constants.DialogFilters.XapkConfigFiles);

            if (string.IsNullOrWhiteSpace(savePath))
                return false;

            _configService.Save(savePath, _xapkConfigService.GetConfig());
            _configService.ConfigPath = savePath;

            return true;
        }

        private bool HandleSaveConfirmResult(SaveConfirmResult result)
        {
            switch (result)
            {
                case SaveConfirmResult.Yes:
                    return SaveChanges();

                case SaveConfirmResult.No:
                    return true;

                case SaveConfirmResult.Cancel:
                default:
                    return false;
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
            _xapkConfigService.LoadConfig(config);
        }

        private XapkConfig OpenPackage(string packagePath)
        {
            var config = _configService.ImportFromPackage(packagePath);
            _configService.ConfigPath = string.Empty;
            return config;
        }
    }
}
