/* 
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using XapkPackagingTool.Service.Interfaces;
using XapkPackagingTool.View;
using XapkPackagingTool.ViewModel;
using XapkPackagingTool.ViewModel.Main;
using XapkPackagingTool.ViewModel.Startup;

namespace XapkPackagingTool.Service
{
    internal class WindowService : IWindowService
    {

        private readonly Dictionary<Type, Type> _mappings;
        private readonly IServiceProvider _serviceProvider;

        public WindowService(IServiceProvider serviceProvider)
        {
            _mappings = new Dictionary<Type, Type>();
            _serviceProvider = serviceProvider;
            Configure();
        }

        private void Configure()
        {
            _mappings.Add(typeof(MainViewModel), typeof(MainWindow));
            _mappings.Add(typeof(StartupViewModel), typeof(StartupWindow));
            _mappings.Add(typeof(NewPackageViewModel), typeof(NewPackageWindow));
        }

        public void ShowWindow<TViewModel>() where TViewModel : ViewModelBase
        {
            var window = CreateWindow(typeof(TViewModel));
            window.Show();
        }

        public bool? ShowDialog<TViewModel>() where TViewModel : ViewModelBase
        {
            var window = CreateWindow(typeof(TViewModel));
            return window.ShowDialog();
        }

        public void CloseWindow<TViewModel>()
        {
            Type viewModelType = _mappings[typeof(TViewModel)];
            var window = Application.Current.Windows
                .OfType<Window>()
                .FirstOrDefault(w => w.GetType().Name.Equals(viewModelType.Name));
            window?.Close();
        }

        public void RefreshWindow<TViewModel>() where TViewModel : ViewModelBase
        {
            Type viewModelType = _mappings[typeof(TViewModel)];
            var window = Application.Current.Windows
                .OfType<Window>()
                .FirstOrDefault(w => w.GetType().Name.Equals(viewModelType.Name));

            if (window != null)
            {
                var viewModel = _serviceProvider.GetRequiredService(window.DataContext.GetType());

                if (viewModel == null)
                    throw new InvalidOperationException($"ViewModel of type {viewModelType.FullName} could not be resolved.");

                window.DataContext = viewModel;
            }
        }

        private Window CreateWindow(Type viewModelType)
        {
            try
            {
                if (!_mappings.ContainsKey(viewModelType))
                    throw new ArgumentException($"No window mapped for {viewModelType.FullName}");

                var windowType = _mappings[viewModelType];
                var window = (Window)Activator.CreateInstance(windowType);

                var viewModel = (ViewModelBase)App.ServiceProvider.GetRequiredService(viewModelType);

                if (viewModel == null)
                    throw new InvalidOperationException($"ViewModel of type {viewModelType.FullName} could not be resolved.");

                window.DataContext = viewModel;
                return window;
            }
            catch (Exception exc)
            {

                throw exc;
            }
        }
    }
}
