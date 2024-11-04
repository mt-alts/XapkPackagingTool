/* 
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using System.Windows;
using System.Windows.Controls;

namespace XapkPackagingTool.Dialogs
{
    /// <summary>
    /// PermissionInputDialog.xaml etkileşim mantığı
    /// </summary>
    public partial class PermissionInputDialog : HandyControl.Controls.Window
    {
        public PermissionInputDialog()
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

        private void DataGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is DataGrid dataGrid && dataGrid.SelectedItem != null)
            {
                var selectedRow = dataGrid.SelectedItem;
                var columnIndex = dataGrid.CurrentCell.Column.DisplayIndex;

                if (columnIndex == 0)
                {
                    var property = selectedRow.GetType().GetProperty("Bool");
                    if (property != null)
                    {
                        var currentValue = (bool)property.GetValue(selectedRow);
                        property.SetValue(selectedRow, !currentValue);
                    }
                }
            }
        }
    }
}
