/*
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using System.IO;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using XapkPackagingTool.Common.Data.Model.Xapk;
using XapkPackagingTool.Helper;
using XapkPackagingTool.Service;
using XapkPackagingTool.Service.SystemServices;
using XapkPackagingTool.Utility.AssetUtility;

namespace XapkPackagingTool.ViewModel.InputVM
{
    internal class ExpansionInputViewModel : InputViewModelBase
    {
        private static readonly string ASSET_PATH = AssetPath.Xapk.PredefinedExpansionInstallLocations;

        private string _file;
        private string _installPath;

        public string File
        {
            get => _file;
            set
            {
                _file = value;
                OnPropertyChanged(nameof(File));
            }
        }

        public string InstallPath
        {
            get => _installPath;
            set
            {
                _installPath = value;
                OnPropertyChanged(nameof(InstallPath));
            }
        }

        public string InstallLocation { get; set; }
        public List<string> InstallLocations { get; set; }
        public ICommand SelectFileCommand { get; private set; }
        public ICommand SelectInstallPathCommand { get; private set; }

        public override object Result
        {
            get
            {
                return new Expansion
                {
                    File = this.File,
                    InstallLocation = this.InstallLocation,
                    InstallPath = $"{InstallPath}/{System.IO.Path.GetFileName(File)}",
                };
            }
        }

        public ExpansionInputViewModel()
        {
            CreateCommands();
            LoadData();
        }

        public ExpansionInputViewModel(Expansion expansion)
        {
            if (expansion == null)
                throw new ArgumentNullException(nameof(expansion));
            CreateCommands();
            LoadData();
            this.InstallLocation = expansion.InstallLocation ?? string.Empty;
            this.File = expansion.File ?? string.Empty;
            this.InstallPath = !string.IsNullOrWhiteSpace(expansion.InstallPath)
                ? Path.GetDirectoryName(expansion.InstallPath).Replace('\\', '/')
                : string.Empty;
        }

        private void LoadData()
        {
            try
            {
                if (!System.IO.File.Exists(ASSET_PATH))
                    throw new Exceptions.AssetLoadException(ASSET_PATH);
                InstallLocations = AssetLoader<List<string>>
                    .LoadData(ASSET_PATH)
                    .ConvertAll(item => item.ToString());
            }
            catch (Exception exc)
            {
                ExceptionHandler.HandleException(exc);
                IsRequestClose = true;
            }
        }

        private void CreateCommands()
        {
            SelectFileCommand = new RelayCommand(SelectFileExecute);
            SelectInstallPathCommand = new RelayCommand(SelectInstallPathExecute);
        }

        private void SelectFileExecute()
        {
            string file = new OpenFileService().OpenDialog("", "");
            if (file != null)
                this.File = file;
        }

        private void SelectInstallPathExecute()
        {
            var dialogService = App.ServiceProvider.GetRequiredService<IDialogService>();
            var (isResult, result) = string.IsNullOrWhiteSpace(InstallPath)
                ? dialogService.ShowDialog<AndroidFileSystemSimulationViewModel>()
                : dialogService.ShowDialog<AndroidFileSystemSimulationViewModel>(InstallPath);
            if (isResult && result is string _result)
                this.InstallPath = _result;
        }
    }
}
