/*
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using System.Collections.ObjectModel;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Input;
using XapkPackagingTool.Common.Data.Model.Xapk;
using XapkPackagingTool.Exceptions;
using XapkPackagingTool.Helper;
using XapkPackagingTool.Service;
using XapkPackagingTool.Service.SystemServices;
using XapkPackagingTool.Utility.AssetUtility;

namespace XapkPackagingTool.ViewModel.InputVM
{
    internal sealed class SplitInputViewModel : InputViewModelBase
    {
        private List<string> _appBinaryInterfaces = new();
        private List<string> _localeCodes = new();
        private List<string> _densityQualifiers = new();

        private ObservableCollection<string> _filteredData;
        private List<string> _comparedList = new();
        private string _searchText;
        private string _splitApk;
        private bool _isAbiSelected;
        private bool _isLocaleSelected;
        private bool _isDpiSelected;

        public ICommand SplitApkSelectCommand { get; private set; }

        public int SelectedIndex { get; set; }

        public string SplitApkFile
        {
            get => _splitApk;
            set => SetProperty(ref _splitApk, value);
        }

        public bool IsAbiSelected
        {
            get => _isAbiSelected;
            set
            {
                if (SetProperty(ref _isAbiSelected, value))
                {
                    UpdateComparedList(_appBinaryInterfaces, value);
                    FilterData();
                }
            }
        }

        public bool IsLocaleSelected
        {
            get => _isLocaleSelected;
            set
            {
                if (SetProperty(ref _isLocaleSelected, value))
                {
                    UpdateComparedList(_localeCodes, value);
                    FilterData();
                }
            }
        }

        public bool IsDpiSelected
        {
            get => _isDpiSelected;
            set
            {
                if (SetProperty(ref _isDpiSelected, value))
                {
                    UpdateComparedList(_densityQualifiers, value);
                    FilterData();
                }
            }
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                if (SetProperty(ref _searchText, value))
                    FilterData();
            }
        }

        private List<string> ComparedList
        {
            get => _comparedList;
            set => SetProperty(ref _comparedList, value);
        }

        public ObservableCollection<string> FilteredData
        {
            get => _filteredData;
            set => SetProperty(ref _filteredData, value);
        }

        public override object Result
        {
            get
            {
                if (SelectedIndex < 0 || !ConfigIsAvailable(FilteredData[SelectedIndex]))
                    throw new InvalidOperationException("Invalid configuration selected!");

                return new SplitApk { File = SplitApkFile, Id = FilteredData[SelectedIndex] };
            }
        }

        public SplitInputViewModel()
        {
            Initialize();
        }

        public SplitInputViewModel(SplitApk splitApk)
            : this()
        {
            if (splitApk == null)
                throw new ArgumentNullException(nameof(splitApk));

            SplitApkFile = splitApk.File ?? string.Empty;
            ChangeConfigSelectionsByConfig(splitApk.Id ?? string.Empty);
            SearchText = splitApk.Id;
            SelectedIndex = 0;
        }

        private void Initialize()
        {
            LoadData();
            ComparedList.Add("base");

            FilteredData = new ObservableCollection<string>(_comparedList);
            SplitApkSelectCommand = new RelayCommand(SplitApkSelect);

            IsAbiSelected = true;
            IsLocaleSelected = true;
            IsDpiSelected = true;
        }

        private void LoadData()
        {
            try
            {
                _appBinaryInterfaces = LoadAssetData<List<string>>(
                    AssetPath.Xapk.PredefinedAppBinaryInterfaces,
                    "config."
                );
                _localeCodes = LoadAssetData<Dictionary<string, string>>(
                    AssetPath.Xapk.PredefinedLocaleCodes,
                    "config."
                );
                _densityQualifiers = LoadAssetData<List<string>>(
                    AssetPath.Xapk.PredefinedDensityQualifiers,
                    "config."
                );
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
                IsRequestClose = true;
            }
        }

        private static List<string> LoadAssetData<T>(string path, string prefix)
        {
            if (!File.Exists(path))
                return new List<string>();

            var data = AssetLoader<T>.LoadData(path);
            if (data is List<string> list)
                return list.ConvertAll(item => $"{prefix}{item}");
            else if (data is Dictionary<string, string> dict)
                return dict.Select(item => $"{prefix}{item.Key}").ToList();
            throw new AssetLoadException(path);
        }

        private void UpdateComparedList(IEnumerable<string> sourceList, bool add)
        {
            if (add)
                ComparedList.AddRange(sourceList);
            else
                ComparedList.RemoveAll(sourceList.Contains);
        }

        private void FilterData()
        {
            FilteredData = string.IsNullOrWhiteSpace(SearchText)
                ? new ObservableCollection<string>(ComparedList)
                : new ObservableCollection<string>(
                    ComparedList.Where(item =>
                        Regex.IsMatch(item, SearchText, RegexOptions.IgnoreCase)
                    )
                );
        }

        private void SplitApkSelect()
        {
            var selectedFile = new OpenFileService().OpenDialog(
                "StrSelectAPK".Localize(),
                Constants.DialogFilters.ApkFile
            );

            if (!string.IsNullOrEmpty(selectedFile))
                SplitApkFile = selectedFile;
        }

        private void ChangeConfigSelectionsByConfig(string config)
        {
            IsAbiSelected = _appBinaryInterfaces.Contains(config);
            IsLocaleSelected = _localeCodes.Contains(config);
            IsDpiSelected = _densityQualifiers.Contains(config);
        }

        private bool ConfigIsAvailable(string config) => ComparedList.Contains(config);
    }
}
