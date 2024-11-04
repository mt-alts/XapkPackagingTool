/*
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using XapkPackagingTool.Common.Collection.Generic;
using XapkPackagingTool.Helper;
using XapkPackagingTool.Utility.AssetUtility;

namespace XapkPackagingTool.ViewModel.InputVM
{
    internal class LocaleInputViewModel
        : SelectableItemsDialogViewModel<KeyValuePair<string, string>>
    {
        private readonly string ASSET_PATH = PathHelper.GetFullPath(
            Properties.Path.Default.AssetLocales
        );

        private readonly List<string> _usedLocales;

        public LocaleInputViewModel()
        {
            LoadData(ASSET_PATH, new());
        }

        public LocaleInputViewModel(List<string> usedLocales)
        {
            _usedLocales = usedLocales;
            LoadData(ASSET_PATH, usedLocales.Cast<object>().ToList());
        }

        protected override void FilterData()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
                FilteredData = new ObservableCollection<
                    BooleanValuePair<KeyValuePair<string, string>>
                >(_allItems);
            else
                FilteredData = new ObservableCollection<
                    BooleanValuePair<KeyValuePair<string, string>>
                >(
                    _allItems.Where(pair =>
                        Regex.IsMatch(pair.Value.Value, SearchText, RegexOptions.IgnoreCase)
                    )
                );
        }

        protected override void LoadData(string dataFilePath, List<object> usedItems)
        {
            try
            {
                var data = AssetLoader<Dictionary<string, string>>.LoadData(dataFilePath);
                var usedItemsSet = new HashSet<string>(usedItems.OfType<string>().ToList());

                Parallel.ForEach(
                    data,
                    item =>
                    {
                        var isItemUsed = usedItemsSet.Contains(item.Key);
                        if (!isItemUsed)
                            lock (_lock)
                                _allItems.Add(
                                    new BooleanValuePair<KeyValuePair<string, string>>(
                                        isItemUsed,
                                        item
                                    )
                                );
                    }
                );
            }
            catch (Exception exc)
            {
                ExceptionHandler.HandleException(exc);
                IsRequestClose = true;
            }
        }
    }
}
