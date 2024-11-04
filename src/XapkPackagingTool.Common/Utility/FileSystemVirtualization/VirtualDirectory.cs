/*
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using System.Collections.ObjectModel;
using System.ComponentModel;

namespace XapkPackagingTool.Common.Utility.FileSystemVirtualization
{
    public sealed class VirtualDirectory : INotifyPropertyChanged
    {
        private string _name;
        private VirtualDirectory _parent;
        private ObservableCollection<VirtualDirectory> _subDirectories =
            new ObservableCollection<VirtualDirectory>();

        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        public VirtualDirectory Parent
        {
            get => _parent;
            set
            {
                if (_parent != value)
                {
                    _parent = value;
                    OnPropertyChanged(nameof(Parent));
                }
            }
        }

        public ObservableCollection<VirtualDirectory> SubDirectories
        {
            get => _subDirectories;
            set
            {
                if (_subDirectories != value)
                {
                    _subDirectories = value;
                    OnPropertyChanged(nameof(SubDirectories));
                }
            }
        }

        public VirtualDirectory(string name, VirtualDirectory parent = null)
        {
            Name = name;
            Parent = parent;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string GetFullPath()
        {
            var path = new Stack<string>();
            var current = this;

            while (current != null)
            {
                path.Push(current.Name);
                current = current.Parent;
            }

            return string.Join("/", path) + "/";
        }

        public VirtualDirectory CreateSubDirectory(string dirName)
        {
            if (string.IsNullOrWhiteSpace(dirName))
                throw new ArgumentNullException(nameof(dirName));
            var subDirectory = new VirtualDirectory(dirName, this);
            SubDirectories.Add(subDirectory);
            return subDirectory;
        }

        public VirtualDirectory CreateNestedSubDirectory(string[] pathParts)
        {
            var currentDirectory = this;

            foreach (var part in pathParts)
            {
                var subDirectory = currentDirectory.SubDirectories.FirstOrDefault(d =>
                    d.Name == part
                );
                if (subDirectory == null)
                {
                    subDirectory = new VirtualDirectory(part, currentDirectory);
                    currentDirectory.SubDirectories.Add(subDirectory);
                }

                currentDirectory = subDirectory;
            }

            return currentDirectory;
        }

        public VirtualDirectory FindSubDirectory(string[] pathParts)
        {
            var currentDirectory = this;

            foreach (var part in pathParts)
            {
                var subDirectory = currentDirectory.SubDirectories.FirstOrDefault(d =>
                    d.Name == part
                );
                if (subDirectory == null)
                    return null;

                currentDirectory = subDirectory;
            }

            return currentDirectory;
        }
    }
}
