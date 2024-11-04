/* 
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using System.Windows;
using XapkPackagingTool.Service.Interfaces;

namespace XapkPackagingTool.Service
{
    internal class MessageDialogService : IMessageDialogService
    {
        public void ShowInfo(string message, string title)
        {
            MessageBox.Show(
                messageBoxText: message,
                caption: title,
                button: MessageBoxButton.OK,
                icon: MessageBoxImage.Information
                );
        }

        public void ShowWarning(string message, string title)
        {
            MessageBox.Show(
                messageBoxText: message,
                caption: title,
                button: MessageBoxButton.OK,
                icon: MessageBoxImage.Warning
                );
        }

        public void ShowError(string message, string title)
        {
            MessageBox.Show(
                messageBoxText: message,
                caption: title,
                button: MessageBoxButton.OK,
                icon: MessageBoxImage.Error
                );
        }
    }
}
