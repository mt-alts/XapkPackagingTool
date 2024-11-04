/*
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using System.Collections.ObjectModel;
using System.ComponentModel;
using XapkPackagingTool.Common.Utility.FileSystemVirtualization;

namespace XapkPackagingTool.ViewModel
{
    public sealed class DirectoryViewModel : INotifyPropertyChanged
    {
        private readonly VirtualDirectory _directory;
        private readonly DirectoryViewModel _parent;
        private bool _isExpanded;
        private bool _isSelected;

        public DirectoryViewModel(VirtualDirectory directory, DirectoryViewModel parent)
        {
            _directory = directory;
            _parent = parent;
            SubDirectories = new ObservableCollection<DirectoryViewModel>(
                directory.SubDirectories.Select(subDir => new DirectoryViewModel(subDir, this))
            );
        }

        public string Name => _directory.Name;

        public ObservableCollection<DirectoryViewModel> SubDirectories { get; }

        public DirectoryViewModel Parent => _parent;

        public string FullPath =>
            _parent == null ? _directory.Name : $"{_parent.FullPath}/{_directory.Name}";

        public bool IsExpanded
        {
            get => _isExpanded;
            set
            {
                if (_isExpanded != value)
                {
                    _isExpanded = value;
                    OnPropertyChanged(nameof(IsExpanded));
                }
            }
        }

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    OnPropertyChanged(nameof(IsSelected));
                }
            }
        }

        public void ExpandPath()
        {
            var parent = this;
            while (parent != null)
            {
                parent.IsExpanded = true;
                parent = parent.Parent;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
