/*
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using System.Collections.ObjectModel;
using System.IO;
using Newtonsoft.Json;
using XapkPackagingTool.Dto;
using XapkPackagingTool.Helper;

namespace XapkPackagingTool.ViewModel
{
    internal class AboutViewModel : ViewModelBase
    {
        public event Action RequestClose;

        private static readonly string ASSET_PATH = PathHelper.GetFullPath(
            Properties.Path.Default.AssetUsedComponentsJsonDataPath
        );

        public string App { get; }

        private ObservableCollection<LibraryInfo> _usedLibraries;

        public ObservableCollection<LibraryInfo> UsedLibraries
        {
            get { return _usedLibraries; }
            set
            {
                _usedLibraries = value;
                OnPropertyChanged(nameof(UsedLibraries));
            }
        }

        public AboutViewModel()
        {
            try
            {
                App = $"{Helper.AppInfoHelper.GetAppName()} {Helper.AppInfoHelper.GetAppVersion()}";
                LoadComponents();
            }
            catch (Exception exc)
            {
                ExceptionHandler.HandleException(exc);
                RequestClose?.Invoke();
            }
        }

        private void LoadComponents()
        {
            if (!File.Exists(ASSET_PATH))
                throw new Exceptions.AssetLoadException(ASSET_PATH);

            var json = File.ReadAllText(ASSET_PATH);
            var libraries = JsonConvert.DeserializeObject<List<LibraryInfo>>(json);
            UsedLibraries = new ObservableCollection<LibraryInfo>(libraries);
        }
    }
}
