/* 
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using XapkPackagingTool.ViewModel;

namespace XapkPackagingTool.Service
{
    internal class ViewModelHost : IViewModelHost
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private ViewModelBase _currentViewModel;
        private ViewModelBase _previousViewModel;

        private readonly Dictionary<string, ViewModelBase> _viewModels;

        public bool CanGoBack => _previousViewModel != null;

        public ICommand SwitchViewModelCommand { get; }

        public ViewModelBase CurrentViewModel
        {
            get => _currentViewModel;
            private set
            {
                if (_currentViewModel != value)
                {
                    _previousViewModel = _currentViewModel;
                    _currentViewModel = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(CanGoBack));
                }
            }
        }

        public ViewModelHost()
        {
            _viewModels = new Dictionary<string, ViewModelBase>();
            SwitchViewModelCommand = new RelayCommand<string>(SwitchViewModelExecute);
        }

        public void AddViewModel<TViewModel>(string key)
        {
            if (!_viewModels.ContainsKey(key))
            {
                var viewModel = App.ServiceProvider.GetRequiredService<TViewModel>();
                if (viewModel is ViewModelBase vm)
                    _viewModels.Add(key, vm);
            }
        }

        public void AddViewModel(string key, ViewModelBase vm)
        {
            if (!_viewModels.ContainsKey(key))
                _viewModels.Add(key, vm);
        }

        public void RemoveViewModel(string key)
        {
            if (_viewModels.ContainsKey(key))
                _viewModels.Remove(key);
        }

        private void SwitchViewModelExecute(string key)
        {
            if (_viewModels.TryGetValue(key, out var viewModel))
                CurrentViewModel = viewModel;
        }

        public void GoBack()
        {
            if (_previousViewModel != null)
            {
                var tempViewModel = _currentViewModel;
                CurrentViewModel = _previousViewModel;
                _previousViewModel = tempViewModel;
            }
        }

        public void Clear()
        {
            if (_viewModels.Count > 0)
                _viewModels.Clear();
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
