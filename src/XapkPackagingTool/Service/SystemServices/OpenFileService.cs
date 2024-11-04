/* 
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using Microsoft.Win32;

namespace XapkPackagingTool.Service.SystemServices
{
    internal class OpenFileService : Interfaces.IOpenFileService
    {
        public string OpenDialog(string title, string filter)
        {
            var openFileDialog = new OpenFileDialog
            {
                Title = title,
                Filter = filter
            };

            var result = openFileDialog.ShowDialog();

            if (result == true)
                return openFileDialog.FileName;

            return null;
        }

        public string[] OpenDialogWithMultiSelection(string title, string filter)
        {
            var openFileDialog = new OpenFileDialog
            {
                Title = title,
                Filter = filter,
                Multiselect = true
            };

            var result = openFileDialog.ShowDialog();

            if (result == true)
                return openFileDialog.FileNames;

            return null;
        }
    }
}
