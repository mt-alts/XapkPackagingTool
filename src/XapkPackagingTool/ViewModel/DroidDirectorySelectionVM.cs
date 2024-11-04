/*
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using System.Collections.ObjectModel;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using XapkPackagingTool.Common.Utility.FileSystemVirtualization;
using XapkPackagingTool.Constants;
using XapkPackagingTool.Service.Interfaces.DataService;
using XapkPackagingTool.ViewModel.InputVM;

namespace XapkPackagingTool.ViewModel
{
    internal class AndroidFileSystemSimulationViewModel : InputViewModelBase
    {
        private readonly IVirtualFileSystem _virtualFileSystem =
            App.ServiceProvider.GetRequiredService<IVirtualFileSystem>();

        public ObservableCollection<DirectoryViewModel> RootDirectories { get; }

        public DirectoryViewModel SelectedDirectory { get; set; }

        public override object Result
        {
            get
            {
                string path = SelectedDirectory.FullPath;
                while (path.StartsWith('/'))
                    path = path.Substring(1);
                return path;
            }
        }

        // Constructor to initialize the virtual file system and root directories
        public AndroidFileSystemSimulationViewModel()
        {
            InitializeAndroidDirectories();

            IMetadataService metadata =
                App.ServiceProvider.GetRequiredService<IXapkConfigService>();
            var appDataDirectoryName = metadata.PackageName;

            if (!string.IsNullOrWhiteSpace(appDataDirectoryName))
                CreateAppDataDirectories(appDataDirectoryName);

            RootDirectories = new ObservableCollection<DirectoryViewModel>
            {
                new DirectoryViewModel(_virtualFileSystem.RootDirectory, null)
            };
        }

        // Constructor to initialize with a configuration
        public AndroidFileSystemSimulationViewModel(string startDirectoryPath)
        {
            InitializeAndroidDirectories();

            IMetadataService metadata =
                App.ServiceProvider.GetRequiredService<IXapkConfigService>();
            var appDataDirectoryName = metadata.PackageName;

            if (!string.IsNullOrWhiteSpace(appDataDirectoryName))
                CreateAppDataDirectories(appDataDirectoryName);

            RootDirectories = new ObservableCollection<DirectoryViewModel>
            {
                new DirectoryViewModel(_virtualFileSystem.RootDirectory, null)
            };

            if (!string.IsNullOrWhiteSpace(startDirectoryPath))
                NavigateToDirectory(startDirectoryPath);
        }

        // Initializes the standard Android directories
        private void InitializeAndroidDirectories()
        {
            var androidDirectory = _virtualFileSystem.CreateDirectory(AndroidDirectories.ANDROID);
            androidDirectory.CreateSubDirectory(AndroidDirectories.DATA);
            androidDirectory.CreateSubDirectory(AndroidDirectories.OBB);
            _virtualFileSystem.CreateDirectory(AndroidDirectories.ALARMS);
            _virtualFileSystem.CreateDirectory(AndroidDirectories.DCIM);
            _virtualFileSystem.CreateDirectory(AndroidDirectories.DOCUMENTS);
            _virtualFileSystem.CreateDirectory(AndroidDirectories.DOWNLOAD);
            _virtualFileSystem.CreateDirectory(AndroidDirectories.MOVIES);
            _virtualFileSystem.CreateDirectory(AndroidDirectories.MUSIC);
            _virtualFileSystem.CreateDirectory(AndroidDirectories.NOTIFICATIONS);
            _virtualFileSystem.CreateDirectory(AndroidDirectories.PICTURES);
            _virtualFileSystem.CreateDirectory(AndroidDirectories.PODCASTS);
            _virtualFileSystem.CreateDirectory(AndroidDirectories.RECORDINGS);
            _virtualFileSystem.CreateDirectory(AndroidDirectories.RINGTONES);
        }

        // Creates app-specific directories for the provided directory name
        private void CreateAppDataDirectories(string directoryName)
        {
            _virtualFileSystem.CreateDirectory($"Android/data/{directoryName}");
            _virtualFileSystem.CreateDirectory($"Android/obb/{directoryName}");
        }

        // Converts an explicit path to an implicit path (e.g., "/a/b/c" -> "//a/b/")
        private static string ConvertToImplicitPath(string explicitPath)
        {
            var implicitPathBuilder = new StringBuilder();
            implicitPathBuilder.Append("//");
            var pathSegments = explicitPath.Split('/', StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < pathSegments.Length - 1; i++)
                implicitPathBuilder.Append(pathSegments[i]).Append("/");
            return implicitPathBuilder.ToString();
        }

        // Navigates to a directory by its path
        private void NavigateToDirectory(string directoryPath)
        {
            var implicitPath = ConvertToImplicitPath(directoryPath);
            var targetDirectory = FindDirectoryByPath(implicitPath);
            if (targetDirectory != null)
            {
                targetDirectory.ExpandPath();
                SelectedDirectory = targetDirectory;
            }
        }

        // Finds a directory by its implicit path
        private DirectoryViewModel FindDirectoryByPath(string path)
        {
            var pathSegments = path.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            DirectoryViewModel? currentDirectory = RootDirectories.FirstOrDefault();

            foreach (var segment in pathSegments)
            {
                if (currentDirectory == null)
                    return null;

                currentDirectory = currentDirectory.SubDirectories.FirstOrDefault(d =>
                    d.Name == segment
                );
            }

            return currentDirectory;
        }
    }
}
