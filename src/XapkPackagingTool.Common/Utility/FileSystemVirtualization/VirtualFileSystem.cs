/*
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

namespace XapkPackagingTool.Common.Utility.FileSystemVirtualization
{
    public sealed class VirtualFileSystem : IVirtualFileSystem
    {
        public VirtualDirectory RootDirectory { get; }

        public VirtualFileSystem()
        {
            RootDirectory = new VirtualDirectory("/");
        }

        public VirtualDirectory CreateDirectory(string path)
        {
            var parts = path.Split('/', StringSplitOptions.RemoveEmptyEntries);
            return RootDirectory.CreateNestedSubDirectory(parts);
        }

        public bool DirectoryExists(string path)
        {
            var parts = path.Split('/', StringSplitOptions.RemoveEmptyEntries);
            return RootDirectory.FindSubDirectory(parts) != null;
        }

        public VirtualDirectory GetDirectory(string path)
        {
            var parts = path.Split('/', StringSplitOptions.RemoveEmptyEntries);
            return RootDirectory.FindSubDirectory(parts);
        }
    }
}
