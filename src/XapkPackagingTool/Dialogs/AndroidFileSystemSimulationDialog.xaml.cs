/* 
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using HandyControl.Controls;
using XapkPackagingTool.ViewModel;

namespace XapkPackagingTool.Dialogs
{
    /// <summary>
    /// DroidDirectorySelectionDialog.xaml etkileşim mantığı
    /// </summary>
    public partial class DroidDirectorySelectionDialog : Window
    {
        public DroidDirectorySelectionDialog()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void btnInsert_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void TreeView_SelectedItemChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<object> e)
        {
            if (DataContext is AndroidFileSystemSimulationViewModel viewModel)
            {
                viewModel.SelectedDirectory = e.NewValue as DirectoryViewModel;
            }
        }
    }
}
