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
    /// <summary>
    /// A service for managing dialogs in a WPF application.
    /// </summary>
    internal sealed class DialogService : IDialogService
    {
        /// <summary>
        /// A dictionary mapping ViewModel types to their corresponding dialog Window types.
        /// </summary>
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

        /// <summary>
        /// Displays a dialog bound to the specified ViewModel and returns the result.
        /// </summary>
        /// <typeparam name="TViewModel">The ViewModel type associated with the dialog.</typeparam>
        /// <param name="dataContext">Optional data context for initializing the ViewModel.</param>
        /// <returns>A tuple indicating success and the result from the ViewModel.</returns>
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

        /// <summary>
        /// Displays a dialog without returning a result, using the specified ViewModel.
        /// </summary>
        /// <typeparam name="TViewModel">The ViewModel type associated with the dialog.</typeparam>
        public void ShowDialogWithoutResult<TViewModel>()
        {
            var viewModel = CreateViewModel<TViewModel>();
            var dialog = CreateDialog(typeof(TViewModel));
            dialog.DataContext = viewModel;

            ConfigureDialog(dialog);
            dialog.ShowDialog();
            ResetDialogOwnerOpacity(dialog);
        }

        /// <summary>
        /// Displays a dialog without returning a result, using a data context to initialize the ViewModel.
        /// </summary>
        /// <typeparam name="TViewModel">The ViewModel type associated with the dialog.</typeparam>
        /// <param name="dataContext">The data context for initializing the ViewModel.</param>
        public void ShowDialogWithoutResult<TViewModel>(object dataContext)
        {
            var viewModel = (ViewModelBase)Activator.CreateInstance(typeof(TViewModel), dataContext);
            var dialog = CreateDialog(typeof(TViewModel));
            dialog.DataContext = viewModel;

            ConfigureDialog(dialog);
            dialog.ShowDialog();
            ResetDialogOwnerOpacity(dialog);
        }

        /// <summary>
        /// Creates an instance of the ViewModel.
        /// </summary>
        private static ViewModelBase CreateViewModel<TViewModel>()
        {
            var viewModelType = typeof(TViewModel);
            return (ViewModelBase)Activator.CreateInstance(viewModelType);
        }

        /// <summary>
        /// Creates an instance of the ViewModel with the specified data context.
        /// </summary>
        private static IResultable CreateViewModel<TViewModel>(object dataContext)
            where TViewModel : IResultable
        {
            return dataContext != null
                ? (IResultable)Activator.CreateInstance(typeof(TViewModel), dataContext)
                : (IResultable)Activator.CreateInstance(typeof(TViewModel));
        }

        /// <summary>
        /// Creates a dialog instance associated with the given ViewModel type.
        /// </summary>
        private static Window CreateDialog(Type viewModelType)
        {
            if (!_dialogs.TryGetValue(viewModelType, out var dialogType))
                throw new ArgumentException($"No dialog type found for ViewModel type {viewModelType.Name}!");

            return (Window)Activator.CreateInstance(dialogType);
        }

        /// <summary>
        /// Configures the dialog window settings, such as owner and startup location.
        /// </summary>
        private static void ConfigureDialog(Window dialog)
        {
            var owner = WindowHelper.GetActiveWindow();
            dialog.Owner = owner;
            owner.Opacity = 0.8;
            dialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;
        }

        /// <summary>
        /// Resets the owner's opacity to its original value after the dialog is closed.
        /// </summary>
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
