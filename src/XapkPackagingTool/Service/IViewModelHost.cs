/* 
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using System.ComponentModel;
using System.Windows.Input;
using XapkPackagingTool.ViewModel;

namespace XapkPackagingTool.Service
{
    interface IViewModelHost : INotifyPropertyChanged
    {
        public ICommand SwitchViewModelCommand { get; }
        ViewModelBase CurrentViewModel { get; }
        public bool CanGoBack { get; }
        void AddViewModel<T>(string key);
        void AddViewModel(string key, ViewModelBase viewModel);
        public void GoBack();
        void Clear();
    }
}
