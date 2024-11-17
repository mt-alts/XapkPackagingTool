/*
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using XapkPackagingTool.Common.Collection.Generic;
using XapkPackagingTool.Helper;
using XapkPackagingTool.Service.Interfaces;
using XapkPackagingTool.Utility.AssetUtility;

namespace XapkPackagingTool.ViewModel.InputVM
{
    internal class PermissionInputViewModel : SelectableItemsDialogViewModel<string>
    {
        private readonly string ASSET_PATH = AssetPath.Xapk.PredefinedPermissions;

        private readonly IMessageDialogService _messageDialogService;

        public PermissionInputViewModel(IMessageDialogService messageDialogService)
        {
            _messageDialogService = messageDialogService;
            LoadData(ASSET_PATH, new());
        }

        public PermissionInputViewModel(List<string> usedPermissions)
        {
            LoadData(ASSET_PATH, usedPermissions.Cast<object>().ToList());
        }

        protected override void FilterData()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
                FilteredData = new ObservableCollection<BooleanValuePair<string>>(_allItems);
            else
                FilteredData = new ObservableCollection<BooleanValuePair<string>>(
                    _allItems.Where(pair =>
                        Regex.IsMatch(pair.Value, SearchText, RegexOptions.IgnoreCase)
                    )
                );
        }

        protected override void LoadData(string dataFilePath, List<object> usedPermissions)
        {
            try
            {
                var data = AssetLoader<List<string>>.LoadData(dataFilePath);
                var usedPermissionsSet = new HashSet<string>(
                    usedPermissions.OfType<string>().ToList()
                );

                Parallel.ForEach(
                    data,
                    permission =>
                    {
                        var isPermissionUsed = usedPermissionsSet.Contains(permission);
                        lock (_lock)
                            if (!isPermissionUsed)
                                _allItems.Add(new BooleanValuePair<string>(false, permission));
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
