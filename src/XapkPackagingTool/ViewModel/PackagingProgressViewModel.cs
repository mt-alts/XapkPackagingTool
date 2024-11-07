/*
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using System.IO;
using System.Windows.Input;
using XapkPackagingTool.Enums;
using XapkPackagingTool.Helper;
using XapkPackagingTool.Service;

namespace XapkPackagingTool.ViewModel
{
    internal class PackagingProgressViewModel : ViewModelBase
    {
        public event EventHandler CancelRequired;

        private string _title;
        private string _statusMessage;
        private int _progressValue;
        private bool _isIndeterminate = true;
        private ProgressStatus _processStatus = ProgressStatus.InProgress;

        public ICommand OpenInExplorerCommand { get; }
        public ICommand CancelCommand { get; }

        public string CompletedPackagePath { get; set; }

        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                OnPropertyChanged(nameof(Title));
            }
        }

        public string StatusMessage
        {
            get { return _statusMessage; }
            set
            {
                _statusMessage = value;
                OnPropertyChanged(nameof(StatusMessage));
            }
        }

        public int ProgressValue
        {
            get { return _progressValue; }
            set
            {
                _progressValue = value;
                OnPropertyChanged(nameof(ProgressValue));
            }
        }

        public bool IsIndeterminate
        {
            get { return _isIndeterminate; }
            set
            {
                _isIndeterminate = value;
                OnPropertyChanged(nameof(IsIndeterminate));
            }
        }

        public ProgressStatus ProcessStatus
        {
            get { return _processStatus; }
            set
            {
                _processStatus = value;
                OnPropertyChanged(nameof(ProcessStatus));
            }
        }

        public PackagingProgressViewModel()
        {
            CancelCommand = new RelayCommand(CancelExecute);
            OpenInExplorerCommand = new RelayCommand(OpenInExplorerExecute);
        }

        private void CancelExecute()
        {
            CancelRequired?.Invoke(this, EventArgs.Empty);
        }

        private void OpenInExplorerExecute()
        {
            if (File.Exists(CompletedPackagePath))
                OpenInFileExplorerHelper.Open(CompletedPackagePath);
        }
    }
}
