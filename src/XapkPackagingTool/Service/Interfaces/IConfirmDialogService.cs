/* 
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using XapkPackagingTool.Enums;

namespace XapkPackagingTool.Service.Interfaces
{
    internal interface IConfirmDialogService
    {
        public bool Show(string message);
        public bool Show(string message, string title);
        public SaveConfirmResult ShowWithCancel(string message, string title);
    }
}
