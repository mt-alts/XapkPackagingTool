/* 
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

namespace XapkPackagingTool.Service.Interfaces
{
    internal interface IOpenFileService
    {
        string OpenDialog(string title, string filter);
        public string[] OpenDialogWithMultiSelection(string title, string filter);
    }
}
