/*
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using System.Windows.Input;
using XapkPackagingTool.Service;
using XapkPackagingTool.Service.Interfaces;
using XapkPackagingTool.Service.Interfaces.DataService;

namespace XapkPackagingTool.ViewModel.Main.ApkVariants
{
    internal class MonolithicApkViewModel : ViewModelBase
    {
        private readonly IOpenFileService _fileService;
        private readonly IMonolithicApkService _dataService;

        public ICommand BrowseApkFiles { get; set; }

        public string ApkPath
        {
            get => _dataService.BaseApk;
            set
            {
                _dataService.BaseApk = value;
                OnPropertyChanged(nameof(ApkPath));
            }
        }

        public MonolithicApkViewModel(
            IOpenFileService openFileService,
            IXapkConfigService xapkConfigService
        )
        {
            _fileService = openFileService;
            _dataService = xapkConfigService;

            BrowseApkFiles = new RelayCommand(ExecuteBrowseApkFiles);
        }

        private void ExecuteBrowseApkFiles()
        {
            var path = _fileService.OpenDialog(
                "StrSelectApkFile".Localize(),
                Constants.DialogFilters.ApkFile
            );
            if (!string.IsNullOrWhiteSpace(path))
            {
                ApkPath = path;
            }
        }
    }
}
