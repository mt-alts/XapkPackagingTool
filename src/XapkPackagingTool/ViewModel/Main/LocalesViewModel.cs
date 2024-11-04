/*
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using XapkPackagingTool.Common.Collection.Generic;
using XapkPackagingTool.Common.Data.Model.Xapk;
using XapkPackagingTool.Service;
using XapkPackagingTool.Service.Interfaces;
using XapkPackagingTool.Service.Interfaces.DataService;
using XapkPackagingTool.Utility.Reader;
using XapkPackagingTool.ViewModel.InputVM;

namespace XapkPackagingTool.ViewModel.Main
{
    internal class LocalesViewModel : CollectionViewModel<Locale>
    {
        private readonly IDialogService _dialogService;
        private readonly ILocaleService _localeService;
        private readonly IPackageReader _packageReader;
        private readonly IMessageDialogService _messageDialogService;

        private bool _selectedItemIsEditing;

        public bool SelectedItemIsEditing
        {
            get { return _selectedItemIsEditing; }
            set
            {
                if (value)
                {
                    if (SelectedItems.Count == 1)
                        _selectedItemIsEditing = true;
                }
                else
                {
                    _selectedItemIsEditing = false;
                }

                OnPropertyChanged(nameof(SelectedItemIsEditing));
            }
        }

        public string PackageGlobalName
        {
            get => _localeService.PackageGlobalName;
        }

        public LocalesViewModel(
            IOpenFileService openFileService,
            IMessageDialogService messageDialogService,
            IDialogService dialogService,
            IPackageReader packageReader,
            IXapkConfigService xapkConfigService
        )
        {
            _messageDialogService = messageDialogService;
            _dialogService = dialogService;
            _packageReader = packageReader;

            _localeService = xapkConfigService;
            xapkConfigService.DataChanged += (sender, args) =>
            {
                Items = new SortableBindingList<Locale>(_localeService.Locales);
            };

            Items = new SortableBindingList<Locale>(_localeService.Locales);
        }

        protected override void AddExecute()
        {
            var (isSuccess, result) = _dialogService.ShowDialog<LocaleInputViewModel>(
                Items.Select(locale => locale.LanguageCode).ToList()
            );

            if (isSuccess && result is List<KeyValuePair<string, string>> kvpLocales)
            {
                var locales = kvpLocales
                    .Select(kvp => new Locale { LanguageCode = kvp.Key, Name = PackageGlobalName })
                    .ToList();

                Items.AddRange(locales);
            }
        }

        protected override void EditExecute()
        {
            SelectedItemIsEditing = true;
        }

        public void RemoveMatchingItems(List<Locale> locales)
        {
            for (int i = 0; i < Items.Count; i++)
            {
                var item = Items[i];
                if (locales.Any(locale => item.LanguageCode.Equals(locale.LanguageCode)))
                    Items.RemoveAt(i);
            }
        }

        protected override List<Locale> LoadItemsFromPackage(string path)
        {
            var config = _packageReader.Read(path);
            var locales = config.Manifest.Locales;
            if (locales == null || !locales.Any())
            {
                _messageDialogService.ShowWarning(
                    "StrLocalizedAppNamesNotDefined".Localize(),
                    "StrAppName".Localize()
                );
                return new();
            }
            return locales;
        }

        protected override (bool isResult, object result) ShowDialogForItem(object item = null)
        {
            return _dialogService.ShowDialog<ExpansionInputViewModel>();
        }
    }
}
