/* 
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

namespace XapkPackagingTool.Dialogs
{
    /// <summary>
    /// ProgressDialog.xaml etkileşim mantığı
    /// </summary>
    public partial class ProgressDialog : HandyControl.Controls.Window
    {
        public ProgressDialog()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
