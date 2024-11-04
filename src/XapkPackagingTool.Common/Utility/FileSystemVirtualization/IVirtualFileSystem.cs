/* 
   Copyright (c) 2024 Metin Alt?karde?
   Licensed under the MIT License. See the LICENSE.
*/

namespace XapkPackagingTool.Common.Utility.FileSystemVirtualization
{
    public interface IVirtualFileSystem
    {
        public VirtualDirectory RootDirectory { get; }
        public VirtualDirectory CreateDirectory(string path);
        public bool DirectoryExists(string path);
        public VirtualDirectory GetDirectory(string path);
    }
}