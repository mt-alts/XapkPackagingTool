/*
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using System.Windows;

namespace XapkPackagingTool.Helper
{
    internal static class WindowHelper
    {
        public static Window GetActiveWindow()
        {
            var activeWindow = Application
                .Current.Windows.OfType<Window>()
                .SingleOrDefault(w => w.IsActive);

            return activeWindow ?? Application.Current.MainWindow;
        }
    }
}
