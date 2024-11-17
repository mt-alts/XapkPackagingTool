/* 
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using System.Windows;
using XapkPackagingTool.Dialogs;
using XapkPackagingTool.Helper;
using XapkPackagingTool.View;
using XapkPackagingTool.ViewModel;
using XapkPackagingTool.ViewModel.InputVM;
using XapkPackagingTool.ViewModel.Interfaces;
using Window = System.Windows.Window;

namespace XapkPackagingTool.Service
{
    internal sealed class DialogService : IDialogService
    {
        private static readonly Dictionary<Type, Type> _dialogs = new Dictionary<Type, Type>
    {
        { typeof(ExpansionInputViewModel), typeof(ExpansionInputDialog) },
        { typeof(LocaleInputViewModel), typeof(LocaleInputDialog) },
        { typeof(PermissionInputViewModel), typeof(PermissionInputDialog) },
        { typeof(SplitInputViewModel), typeof(SplitApkInputDialog) },
        { typeof(AndroidFileSystemSimulationViewModel), typeof(DroidDirectorySelectionDialog) },
        { typeof(AboutViewModel), typeof(AboutDialog) },
        { typeof(DocumentViewerVM), typeof(DocumentViewerDialog) }
    };

        public (bool IsSuccess, object Result) ShowDialog<TViewModel>(object dataContext = null)
            where TViewModel : IResultable
        {
            var viewModel = (InputViewModelBase)CreateViewModel<TViewModel>(dataContext);
            var dialog = CreateDialog(typeof(TViewModel));
            dialog.DataContext = viewModel;

            if (viewModel.IsRequestClose)
                return (false, null);

            ConfigureDialog(dialog);

            var dialogResult = dialog.ShowDialog() ?? false;

            ResetDialogOwnerOpacity(dialog);
            return (dialogResult, dialogResult ? viewModel.Result : null);
        }

        public void ShowDialogWithoutResult<TViewModel>()
        {
            var viewModel = CreateViewModel<TViewModel>();
            var dialog = CreateDialog(typeof(TViewModel));
            dialog.DataContext = viewModel;

            ConfigureDialog(dialog);
            dialog.ShowDialog();
            ResetDialogOwnerOpacity(dialog);
        }

        public void ShowDialogWithoutResult<TViewModel>(object dataContext)
        {
            var viewModel = (ViewModelBase)Activator.CreateInstance(typeof(TViewModel), dataContext);
            var dialog = CreateDialog(typeof(TViewModel));
            dialog.DataContext = viewModel;

            ConfigureDialog(dialog);
            dialog.ShowDialog();
            ResetDialogOwnerOpacity(dialog);
        }

        private static ViewModelBase CreateViewModel<TViewModel>()
        {
            var viewModelType = typeof(TViewModel);
            return (ViewModelBase)Activator.CreateInstance(viewModelType);
        }

        private static IResultable CreateViewModel<TViewModel>(object dataContext)
            where TViewModel : IResultable
        {
            return dataContext != null
                ? (IResultable)Activator.CreateInstance(typeof(TViewModel), dataContext)
                : (IResultable)Activator.CreateInstance(typeof(TViewModel));
        }

        private static Window CreateDialog(Type viewModelType)
        {
            if (!_dialogs.TryGetValue(viewModelType, out var dialogType))
                throw new ArgumentException($"No dialog type found for ViewModel type {viewModelType.Name}!");

            return (Window)Activator.CreateInstance(dialogType);
        }

        private static void ConfigureDialog(Window dialog)
        {
            var owner = WindowHelper.GetActiveWindow();
            dialog.Owner = owner;
            owner.Opacity = 0.8;
            dialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;
        }

        private static void ResetDialogOwnerOpacity(Window dialog)
        {
            if (dialog.Owner != null)
                dialog.Owner.Opacity = 1;
        }
    }


    internal interface IDialogService
    {
        public (bool IsSuccess, object Result) ShowDialog<TViewModel>(object dataContext = null)
            where TViewModel : IResultable;

        public void ShowDialogWithoutResult<TViewModel>();

        public void ShowDialogWithoutResult<TViewModel>(object dataContext);
    }
}
