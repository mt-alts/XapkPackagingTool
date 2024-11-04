/*
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/


using System.IO;
using System.Windows;
using XapkPackagingTool.Dialogs;
using XapkPackagingTool.Service.Interfaces;
using XapkPackagingTool.ViewModel;

namespace XapkPackagingTool.Service
{
    internal class ProgressService : IProgressService
    {
        public event EventHandler CancelRequired;

        private readonly PackageProgressViewModel _progressViewModel;
        private ProgressDialog _progressDialog;

        public ProgressService(PackageProgressViewModel progressViewModel)
        {
            _progressViewModel = progressViewModel;
            _progressViewModel.CancelRequired += (sender, e) =>
            {
                CancelRequired?.Invoke(this, EventArgs.Empty);
            };
        }

        private void _progressDialog_Loaded(object sender, RoutedEventArgs e)
        {
            _progressDialog.Owner.Opacity = 0.8;
        }

        private void _progressDialog_Closed(object? sender, EventArgs e)
        {
            _progressDialog.Owner.Opacity = 1;
        }

        public void CloseProgress()
        {
            _progressDialog?.Close();
        }

        public void UpdateStatusMessage(string message)
        {
            _progressViewModel.StatusMessage = message;
        }

        public void UpdateProgress(int progressValue, string message)
        {
            _progressViewModel.StatusMessage = message;
            _progressViewModel.ProgressValue = progressValue;
        }

        public void UpdateProgress(int progressValue)
        {
            _progressViewModel.ProgressValue = progressValue;
        }

        public void UpdateStatusMessageTitle(string title)
        {
            _progressViewModel.Title = title;
        }

        public void ShowProgress()
        {
            _progressViewModel.ProcessStatus = Enums.ProgressStatus.InProgress;
            _progressDialog = NewDialogInstance();
            _progressDialog.ShowDialog();
        }

        public void UpdateStatusMessage(string message, string title)
        {
            _progressViewModel.StatusMessage = message;
            _progressViewModel.Title = title;
        }

        public void ReportFailure(string message, string title)
        {
            _progressViewModel.ProcessStatus = Enums.ProgressStatus.Failed;
            _progressViewModel.StatusMessage = message;
            _progressViewModel.Title = title;
        }

        public void ReportCompletion(string message, string title)
        {
            _progressViewModel.ProcessStatus = Enums.ProgressStatus.Completed;
            _progressViewModel.StatusMessage = message;
            _progressViewModel.Title = title;

            if (File.Exists(message))
                _progressViewModel.CompletedPackagePath = message;
        }

        private ProgressDialog NewDialogInstance()
        {
            var dialog = new ProgressDialog();
            dialog.DataContext = _progressViewModel;
            dialog.Owner = GetActiveWindow();
            dialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;

            dialog.Loaded += _progressDialog_Loaded;
            dialog.Closed += _progressDialog_Closed;

            return dialog;
        }

        public void IndeterminateModeEnable(bool isDeterminate)
        {
            _progressViewModel.IsIndeterminate = isDeterminate;
        }

        private static Window GetActiveWindow()
        {
            var activeWindow = Application
                .Current.Windows.OfType<Window>()
                .SingleOrDefault(w => w.IsActive);

            return activeWindow ?? Application.Current.MainWindow;
        }
    }
}
