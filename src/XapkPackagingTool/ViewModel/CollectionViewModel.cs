/*
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using System.Collections;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using XapkPackagingTool.Common.Collection.Generic;
using XapkPackagingTool.Common.Data.Equality;
using XapkPackagingTool.Constants;
using XapkPackagingTool.Helper;
using XapkPackagingTool.Service;
using XapkPackagingTool.Service.Interfaces;

namespace XapkPackagingTool.ViewModel
{
    internal abstract class CollectionViewModel<T> : ViewModelBase
        where T : class, ICustomEquality<T>
    {
        protected SortableBindingList<T> _items = new();
        private IList _selectedItems = new ArrayList();

        private readonly IConfirmDialogService _confirmDialogService =
            App.ServiceProvider.GetRequiredService<IConfirmDialogService>();

        private readonly IOpenFileService _openFileService =
            App.ServiceProvider.GetRequiredService<IOpenFileService>();

        private readonly IMessageDialogService _messageDialogService =
            App.ServiceProvider.GetRequiredService<IMessageDialogService>();

        public virtual SortableBindingList<T> Items
        {
            get { return _items; }
            set
            {
                _items = value;
                OnPropertyChanged(nameof(Items));
            }
        }

        public IList SelectedItems
        {
            get { return _selectedItems; }
            set
            {
                _selectedItems = value;
                OnPropertyChanged(nameof(SelectedItems));
            }
        }

        public int SelectedIndex { get; set; } = -1;
        public ICommand DeleteCommand { get; }
        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; set; }
        public ICommand ImportFromPackageCommand { get; }

        protected CollectionViewModel()
        {
            SelectedItems = new List<T>();
            DeleteCommand = new RelayCommand(DeleteExecute);
            AddCommand = new RelayCommand(AddExecute);
            EditCommand = new RelayCommand(EditExecute);
            ImportFromPackageCommand = new RelayCommand(ImportFromPackage);
        }

        protected virtual void ImportFromPackage()
        {
            try
            {
                var path = GetPackagePath(DialogFilters.PackageFiles);
                if (string.IsNullOrWhiteSpace(path))
                    return;

                var newItems = LoadItemsFromPackage(path);

                if (!newItems.Any())
                    return;

                if (HasConflictingItems(newItems))
                    HandleConflictingItems(newItems);
                else
                    AddNewItems(newItems);
            }
            catch (Exception exc)
            {
                ExceptionHandler.HandleException(exc);
            }
        }

        protected abstract (bool isResult, object result) ShowDialogForItem(object item = null);

        protected virtual void AddExecute()
        {
            try
            {
                var (isResult, result) = ShowDialogForItem();

                if (isResult && result is T item)
                {
                    if (Items.Any(existingItem => existingItem.IsEqualTo(item)))
                    {
                        ShowDuplicateWarning();
                        return;
                    }

                    Items.Add(item);
                }
            }
            catch (Exception exc)
            {
                ExceptionHandler.HandleException(exc);
            }
        }

        protected virtual void EditExecute()
        {
            int selectedIndex = SelectedIndex;
            if (selectedIndex < 0)
                return;

            var currentItem = Items[selectedIndex];
            var (isResult, result) = ShowDialogForItem(currentItem);

            if (!isResult || !(result is T updatedItem))
                return;

            if (IsItemUpdated(currentItem, updatedItem))
                return;

            if (HasDuplicateItem(updatedItem, currentItem))
            {
                ShowDuplicateWarning();
                return;
            }

            UpdateItem(selectedIndex, updatedItem);
        }

        private static bool IsItemUpdated(T currentItem, T updatedItem)
        {
            return updatedItem.Equals(currentItem);
        }

        private bool HasDuplicateItem(T updatedItem, T currentItem)
        {
            return Items.Any(item =>
                item.IsEqualTo(updatedItem) && !ReferenceEquals(item, currentItem)
            );
        }

        private void UpdateItem(int index, T updatedItem)
        {
            Items[index] = updatedItem;
        }

        protected virtual void DeleteExecute()
        {
            if (!CanDeleteItems())
            {
                _messageDialogService.ShowWarning(
                    "StrNoItemsToDelete".Localize(),
                    "StrAppName".Localize()
                );
                return;
            }

            var itemsToRemove = GetItemsToRemove();

            foreach (var item in itemsToRemove)
                _items.Remove(item);

            RemoveSelectedItemAtIndex();

            OnPropertyChanged(nameof(Items));
        }

        protected string GetPackagePath(string filter)
        {
            return _openFileService.OpenDialog("StrAppName".Localize(), filter);
        }

        protected abstract List<T> LoadItemsFromPackage(string path);

        private void HandleConflictingItems(IEnumerable<T> newItems)
        {
            var shouldOverwrite = ConfirmShouldOverwrite();

            if (shouldOverwrite)
            {
                RemoveConflictingItems(newItems);
                AddNewItems(newItems);
            }
            else
            {
                var nonConflictingItems = FilterNonConflictingItems(newItems);
                AddNewItems(nonConflictingItems);
            }
        }

        protected virtual bool ConfirmShouldOverwrite()
        {
            var shouldOverwrite = _confirmDialogService.Show(
                "StrImportConflictMessage".Localize(),
                "StrAppName".Localize()
            );
            return shouldOverwrite;
        }

        protected bool HasConflictingItems(IEnumerable<T> newItems)
        {
            return newItems.Any(item => _items.Any(existingItem => existingItem.IsEqualTo(item)));
        }

        private void RemoveConflictingItems(IEnumerable<T> newItems)
        {
            var conflictingItems = _items
                .Where(existingItem => newItems.Any(newItem => existingItem.IsEqualTo(newItem)))
                .ToList();

            foreach (var item in conflictingItems)
                _items.Remove(item);
        }

        private IEnumerable<T> FilterNonConflictingItems(IEnumerable<T> newItems)
        {
            return newItems
                .Where(item => !_items.Any(existingItem => existingItem.IsEqualTo(item)))
                .ToList();
        }

        private void AddNewItems(IEnumerable<T> newItems)
        {
            _items.AddRange(newItems);
            OnPropertyChanged(nameof(Items));
        }

        private bool CanDeleteItems()
        {
            if (_items == null || SelectedItems == null || SelectedItems.Count <= 0)
                return false;
            return true;
        }

        private List<T> GetItemsToRemove()
        {
            var selectedHashCodes = SelectedItems
                .Cast<T>()
                .Select(item => item.GetHashCode())
                .ToList();
            return _items.Where(item => selectedHashCodes.Contains(item.GetHashCode())).ToList();
        }

        private void RemoveSelectedItemAtIndex()
        {
            if (SelectedIndex >= 0 && SelectedItems.Count < 1)
                _items.RemoveAt(SelectedIndex);
        }

        private void ShowDuplicateWarning()
        {
            _messageDialogService.ShowWarning(
                "StrDuplicateItemMessage".Localize(),
                "StrDuplicateItemTitle".Localize()
            );
        }
    }
}
