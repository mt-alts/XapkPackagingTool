/* 
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using System.Windows;

namespace XapkPackagingTool.Dialogs
{
    /// <summary>
    /// SplitApkInputDialog.xaml etkileşim mantığı
    /// </summary>
    public partial class SplitApkInputDialog : HandyControl.Controls.Window
    {
        public SplitApkInputDialog()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void btnInsert_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
