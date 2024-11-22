/* 
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using System.Windows;
using XapkPackagingTool.Enums;

namespace XapkPackagingTool.Service
{
    /// <summary>
    /// Service class for displaying confirmation dialogs to the user.
    /// </summary>
    internal class ConfirmDialogService : Interfaces.IConfirmDialogService
    {
        /// <summary>
        /// Displays a confirmation dialog with the default title.
        /// </summary>
        /// <param name="message">The message to display in the dialog.</param>
        /// <returns>True if the user confirms (Yes), false otherwise.</returns>
        public bool Show(string message)
        {
            return Show(message, "DialogTitle_Confirm".Localize());
        }

        /// <summary>
        /// Displays a confirmation dialog with a custom title.
        /// </summary>
        /// <param name="message">The message to display in the dialog.</param>
        /// <param name="title">The title of the dialog window.</param>
        /// <returns>True if the user confirms (Yes), false otherwise.</returns>
        public bool Show(string message, string title)
        {
            var resultMessageBox = MessageBox.Show(message, title, MessageBoxButton.YesNo, MessageBoxImage.Warning);
            return resultMessageBox == MessageBoxResult.Yes;
        }

        /// <summary>
        /// Displays a confirmation dialog with Yes, No, and Cancel options.
        /// </summary>
        /// <param name="message">The message to display in the dialog.</param>
        /// <param name="title">The title of the dialog window.</param>
        /// <returns>
        /// A <see cref="SaveConfirmResult"/> value indicating the user's choice: 
        /// Yes, No, or Cancel.
        /// </returns>
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
