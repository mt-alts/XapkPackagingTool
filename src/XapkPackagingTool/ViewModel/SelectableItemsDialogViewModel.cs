/*
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using System.Collections.ObjectModel;
using XapkPackagingTool.Common.Collection.Generic;
using XapkPackagingTool.ViewModel.InputVM;

namespace XapkPackagingTool.ViewModel
{
    internal abstract class SelectableItemsDialogViewModel<TObj> : InputViewModelBase
    {
        protected readonly ObservableCollection<BooleanValuePair<TObj>> _allItems;
        protected ObservableCollection<BooleanValuePair<TObj>> _filteredItems;
        protected string _searchText;
        protected readonly object _lock = new object();

        public ObservableCollection<BooleanValuePair<TObj>> FilteredData
        {
            get { return _filteredItems; }
            set
            {
                _filteredItems = value;
                OnPropertyChanged(nameof(FilteredData));
            }
        }

        public string SearchText
        {
            get { return _searchText; }
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
                FilterData();
            }
        }

        public List<BooleanValuePair<TObj>> SelectedItems
        {
            get { return _allItems.Where(item => item.Bool).ToList(); }
        }

        public override object Result
        {
            get => _allItems.Where(pair => pair.Bool == true).Select(pair => pair.Value).ToList();
        }

        protected SelectableItemsDialogViewModel()
        {
            _allItems = new ObservableCollection<BooleanValuePair<TObj>>();
            FilteredData = _allItems;
        }

        protected abstract void LoadData(string dataFilePath, List<object> usedItems);

        protected abstract void FilterData();
    }
}
