/*
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Input;
using XapkPackagingTool.Common.Data.Model.Xapk;
using XapkPackagingTool.Constants;
using XapkPackagingTool.Helper;
using XapkPackagingTool.Service;
using XapkPackagingTool.Service.Interfaces;
using XapkPackagingTool.Service.Interfaces.DataService;
using XapkPackagingTool.Utility.AssetUtility;
using XapkPackagingTool.Utility.Reader;
using XapkPackagingTool.ViewModel.InputVM;

namespace XapkPackagingTool.ViewModel.Main.ApkVariants
{
    internal class SplitsApkViewModel : CollectionViewModel<SplitApk>
    {
        private static readonly Regex SPLIT_APK_PATTERN = new Regex(
            @"^config\.(.*?)\.apk$",
            RegexOptions.Compiled
        );

        private readonly ISplitApkService _splitApkService;
        private readonly IMessageDialogService _messageDialogService;
        private readonly IDialogService _dialogService;
        private readonly IOpenFileService _openFileService;
        private readonly IPackageReader _packageReader;

        public ICommand IncludeSplitsApk { get; init; }

        public SplitsApkViewModel(
            IConfirmDialogService confirmDialogService,
            IMessageDialogService messageDialogService,
            IDialogService dialogService,
            IOpenFileService openFileService,
            IXapkConfigService xapkConfigService,
            IPackageReader packageReader
        )
        {
            _dialogService = dialogService;
            _openFileService = openFileService;
            _packageReader = packageReader;
            _messageDialogService = messageDialogService;

            xapkConfigService.DataChanged += SplitApkService_DataChanged;
            _splitApkService = xapkConfigService;

            Items = new Common.Collection.Generic.SortableBindingList<SplitApk>(
                _splitApkService.SplitApks
            );

            IncludeSplitsApk = new RelayCommand(IncludeSplitsApkExecute);
        }

        private void SplitApkService_DataChanged(object? sender, EventArgs e)
        {
            Items = new Common.Collection.Generic.SortableBindingList<SplitApk>(
                _splitApkService.SplitApks
            );
        }

        private void IncludeSplitsApkExecute()
        {
            try
            {
                var splitsApk = _openFileService.OpenDialogWithMultiSelection(
                    "DialogTitle_AddSplitAPKs".Localize(),
                    DialogFilters.ApkFile
                );
                if (splitsApk != null && splitsApk.Length > 0)
                {
                    var compatibleApks = GetCompatibleApks(splitsApk);
                    Items.AddRange(compatibleApks);
                }
            }
            catch (Exception exc)
            {
                ExceptionHandler.HandleException(exc);
            }
        }

        private static List<SplitApk> GetCompatibleApks(string[] splitApks)
        {
            var configs = LoadConfigs();

            return splitApks
                .Select(apk => SPLIT_APK_PATTERN.Match(Path.GetFileName(apk)))
                .Where(match => match.Success && configs.Contains(match.Groups[1].Value))
                .Select(match => new SplitApk(match.Groups[1].Value, match.Value))
                .ToList();
        }

        protected override void EditExecute()
        {
            if (SelectedIndex < 0 || SelectedIndex >= Items.Count)
                return;

            var selectedSplitApk = Items[SelectedIndex];

            var (isResult, result) = _dialogService.ShowDialog<SplitInputViewModel>(
                selectedSplitApk
            );

            if (isResult && result is SplitApk updatedSplitApk)
                Items[SelectedIndex] = updatedSplitApk;
        }

        private static List<string> LoadConfigs()
        {
            var configs = AssetLoader<List<string>>.LoadData(
                Properties.Path.Default.AssetApplicationBinaryInterfaces
            );
            configs.AddRange(
                AssetLoader<Dictionary<string, string>>
                    .LoadData(Properties.Path.Default.AssetLocales)
                    .Select(locale => locale.Key)
                    .ToList()
            );
            configs.AddRange(
                AssetLoader<List<string>>.LoadData(Properties.Path.Default.AssetDensityQualifiers)
            );
            return configs;
        }

        protected override List<SplitApk> LoadItemsFromPackage(string path)
        {
            var config = _packageReader.Read(path);
            List<SplitApk>? splitApks = config.Manifest.SplitApks;
            if (splitApks == null || !splitApks.Any())
            {
                _messageDialogService.ShowWarning(
                    "StrSplitApkNotContained".Localize(),
                    "StrAppName".Localize()
                );
                return new();
            }
            return splitApks;
        }

        protected override (bool isResult, object result) ShowDialogForItem(object item = null)
        {
            if (item == null)
                return _dialogService.ShowDialog<SplitInputViewModel>();
            else
                return _dialogService.ShowDialog<SplitInputViewModel>(item);
        }
    }
}
