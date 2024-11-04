/*
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using System.Windows.Input;
using XapkPackagingTool.Common.Collection.Generic;

namespace XapkPackagingTool.ViewModel.Interfaces
{
    internal interface ICollectionViewModel<T> : IViewModelBase
    {
        public SortableBindingList<T> Items { get; set; }
        public int SelectedIndex { get; set; }
        public ICommand DeleteCommand { get; }
        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }

        public ICommand ImportFromPackageCommand { get; }
    }
}