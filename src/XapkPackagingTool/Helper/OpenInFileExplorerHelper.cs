/*
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using System.Diagnostics;
using System.IO;

namespace XapkPackagingTool.Helper
{
    internal class OpenInFileExplorerHelper
    {
        public static void Open(string path)
        {
            if (Directory.Exists(path) || File.Exists(path))
                Process.Start("explorer.exe", $"/select,\"{path}\"");
        }
    }
}
