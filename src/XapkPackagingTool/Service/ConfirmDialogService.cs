/* 
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using System.Windows;
using XapkPackagingTool.Enums;

namespace XapkPackagingTool.Service
{
    class ConfirmDialogService : Interfaces.IConfirmDialogService
    {
        public bool Show(string message)
        {
            return Show(message, "DialogTitle_Confirm".Localize());
        }

        public bool Show(string message, string title)
        {
            var resultMessageBox = MessageBox.Show(message, title, MessageBoxButton.OKCancel);
            return resultMessageBox == MessageBoxResult.OK;
        }

        public SaveConfirmResult ShowWithCancel(string message, string title)
        {
            var result = MessageBox.Show(
                message,
                title,
                MessageBoxButton.YesNoCancel,
                MessageBoxImage.Question
            );

            return result switch
            {
                MessageBoxResult.Yes => SaveConfirmResult.Yes,
                MessageBoxResult.No => SaveConfirmResult.No,
                MessageBoxResult.Cancel => SaveConfirmResult.Cancel,
                _ => SaveConfirmResult.Cancel
            };
        }
    }
}
