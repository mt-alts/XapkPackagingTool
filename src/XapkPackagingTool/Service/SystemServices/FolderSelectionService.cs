using Microsoft.Win32;
/*
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XapkPackagingTool.Service.SystemServices
{
    internal class FolderSelectionService : Interfaces.IFolderSelectionService
    {
        public (bool,string) OpenDialog(string title)
        {
            var dialog = new OpenFolderDialog();
            dialog.Title = title;

            bool isSelected = dialog.ShowDialog() == true;
            return (isSelected, dialog.FolderName);
        }
    }
}
