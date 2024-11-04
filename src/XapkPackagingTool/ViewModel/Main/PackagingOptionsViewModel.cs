/*
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using System.Windows.Input;
using XapkPackagingTool.Service;
using XapkPackagingTool.Service.Interfaces;
using XapkPackagingTool.Service.Interfaces.DataService;

namespace XapkPackagingTool.ViewModel.Main
{
    internal class PackagingOptionsViewModel : ViewModelBase
    {
        private readonly IPackagingOptionsService _packagingOptionsService;
        private readonly ISaveFileService _saveFileService;

        public string BuildPath
        {
            get { return _packagingOptionsService.BuildPath; }
            set
            {
                _packagingOptionsService.BuildPath = value;
                OnPropertyChanged(nameof(BuildPath));
            }
        }

        public ICommand BrowseApkCommand { get; init; }

        public PackagingOptionsViewModel(
            IXapkConfigService xapkConfigService,
            ISaveFileService saveFileService
        )
        {
            _packagingOptionsService = xapkConfigService;
            xapkConfigService.DataChanged += (sender, args) =>
            {
                BuildPath = xapkConfigService.BuildPath;
            };

            _saveFileService = saveFileService;
            BrowseApkCommand = new RelayCommand(BrowseApkExecute);
        }

        private void BrowseApkExecute()
        {
            var buildPath = _saveFileService.OpenDialog(
                "Select for build XAPK package file",
                "XAPK File (*.xapk)|*.xapk"
            );

            if (string.IsNullOrWhiteSpace(buildPath))
                return;

            BuildPath = buildPath;
        }
    }
}
