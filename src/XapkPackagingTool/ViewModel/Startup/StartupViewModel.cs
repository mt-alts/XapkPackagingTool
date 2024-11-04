/*
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using Microsoft.Extensions.DependencyInjection;
using XapkPackagingTool.Constants;
using XapkPackagingTool.Service;

namespace XapkPackagingTool.ViewModel.Startup
{
    internal class StartupViewModel : ViewModelBase
    {
        public IViewModelHost VMHost { get; init; }

        public StartupViewModel(IViewModelHost viewModelHost)
        {
            VMHost = viewModelHost;
            RegisterViewModels();
        }

        private void RegisterViewModels()
        {
            var newPackageVM = App.ServiceProvider.GetRequiredService<NewPackageViewModel>();
            var gettingStartedVM =
                App.ServiceProvider.GetRequiredService<GettingStartedViewModel>();

            newPackageVM.SwitchBackRequested += NewProjectViewModelSwitchBackRequested;
            gettingStartedVM.SwitchVMRequested += ProjectsViewModelSwitchViewModelRequested;

            VMHost.AddViewModel(StartupViewNavigationKeys.CREATE_NEW, newPackageVM);
            VMHost.AddViewModel(StartupViewNavigationKeys.GETTING_STARTED, gettingStartedVM);

            VMHost.SwitchViewModelCommand.Execute(StartupViewNavigationKeys.GETTING_STARTED);
        }

        private void ProjectsViewModelSwitchViewModelRequested(object? sender, string key)
        {
            VMHost.SwitchViewModelCommand.Execute(key);
        }

        private void NewProjectViewModelSwitchBackRequested(object? sender, EventArgs e)
        {
            if (VMHost.CanGoBack)
                VMHost.GoBack();
        }
    }
}
