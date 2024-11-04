/*
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using System.Windows.Input;
using XapkPackagingTool.Common.Collection.Generic;
using XapkPackagingTool.Common.Data;
using XapkPackagingTool.Service;
using XapkPackagingTool.Service.Interfaces;
using XapkPackagingTool.Service.Interfaces.DataService;
using XapkPackagingTool.Utility.Reader;
using XapkPackagingTool.ViewModel.InputVM;

namespace XapkPackagingTool.ViewModel.Main
{
    internal class PermissionsViewModel : CollectionViewModel<StringWrapper>
    {
        private bool _selectedItemIsEditing;
        private readonly IMessageDialogService _messageDialogService;
        private readonly IDialogService _dialogService;
        private readonly IPermissionService _permissionService;
        private readonly IPackageReader _packageReader;

        public ICommand MultiAddCommand { get; }

        public bool SelectedItemIsEditing
        {
            get { return _selectedItemIsEditing; }
            set
            {
                if (value == true)
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

        public PermissionsViewModel(
            IXapkConfigService xapkConfigService,
            IMessageDialogService messageDialogService,
            IPackageReader packageReader,
            IDialogService dialogService
        )
        {
            _messageDialogService = messageDialogService;
            _dialogService = dialogService;
            _packageReader = packageReader;

            _permissionService = xapkConfigService;
            xapkConfigService.DataChanged += (sender, args) =>
            {
                Items = new SortableBindingList<StringWrapper>(_permissionService.Permissions);
            };

            Items = new SortableBindingList<StringWrapper>(_permissionService.Permissions);

            MultiAddCommand = new RelayCommand(MultiAddExecute);
        }

        protected override void AddExecute()
        {
            Items.Insert(0, new StringWrapper(string.Empty));
        }

        private void MultiAddExecute()
        {
            var (isResult, result) = ShowDialogForItem(Items.Select(item => item.Content).ToList());

            if (isResult && result is List<string> permissions)
                permissions.ForEach(permission =>
                {
                    var wrappedPermission = new StringWrapper(permission);
                    if (!Items.Contains(wrappedPermission))
                        Items.Add(wrappedPermission);
                });
        }

        protected override void EditExecute()
        {
            SelectedItemIsEditing = true;
        }

        protected override List<StringWrapper> LoadItemsFromPackage(string path)
        {
            var config = _packageReader.Read(path);
            List<StringWrapper>? permissions = config.Manifest.Permissions;
            if (permissions == null || !permissions.Any())
            {
                _messageDialogService.ShowWarning(
                    "StrAppPermissionsNotContained".Localize(),
                    "StrAppName".Localize()
                );
                return new();
            }
            return permissions;
        }

        protected override (bool isResult, object result) ShowDialogForItem(object item = null)
        {
            if (item == null)
                return _dialogService.ShowDialog<PermissionInputViewModel>();
            else
                return _dialogService.ShowDialog<PermissionInputViewModel>(item);
        }

        protected override bool ConfirmShouldOverwrite()
        {
            return false;
        }
    }
}
