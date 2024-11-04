/* 
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using System.Windows;

namespace XapkPackagingTool.Dialogs
{
    /// <summary>
    /// ExpansionInputDialog.xaml etkileşim mantığı
    /// </summary>
    public partial class ExpansionInputDialog : HandyControl.Controls.Window
    {
        public ExpansionInputDialog()
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
