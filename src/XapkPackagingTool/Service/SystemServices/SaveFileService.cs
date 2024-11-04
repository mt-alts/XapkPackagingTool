/* 
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using Microsoft.Win32;

namespace XapkPackagingTool.Service.SystemServices
{
    internal class SaveFileService : Interfaces.ISaveFileService
    {
        public string OpenDialog(string title, string filter)
        {
            var save = new SaveFileDialog();
            save.OverwritePrompt = false;
            save.CreatePrompt = true;
            save.Title = title;
            save.Filter = filter;
            save.ShowDialog();
            return save.FileName;
        }
    }
}
