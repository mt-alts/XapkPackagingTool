/* 
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using XapkPackagingTool.ViewModel;

namespace XapkPackagingTool.Service.Interfaces
{
    internal interface IWindowService
    {
        public void ShowWindow<TViewModel>() where TViewModel : ViewModelBase;
        public bool? ShowDialog<TViewModel>() where TViewModel : ViewModelBase;
        public void CloseWindow<TViewModel>();
        public void RefreshWindow<TViewModel>() where TViewModel : ViewModelBase;
    }
}
