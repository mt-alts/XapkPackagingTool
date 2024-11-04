/*
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace XapkPackagingTool.CustomControl
{
    internal class CustomDataGrid : DataGrid
    {
        public static readonly DependencyProperty IsRowEditingProperty =
            DependencyProperty.Register(
                nameof(IsRowEditing),
                typeof(bool),
                typeof(CustomDataGrid),
                new PropertyMetadata(false, OnIsRowEditingChanged)
            );

        public static readonly DependencyProperty IsUniqueEnabledProperty =
            DependencyProperty.Register(
                nameof(IsUniqueEnabled),
                typeof(bool),
                typeof(CustomDataGrid),
                new PropertyMetadata(false)
            );

        public static readonly DependencyProperty UniqueColumnsProperty =
            DependencyProperty.Register(
                nameof(UniqueColumns),
                typeof(int[]),
                typeof(CustomDataGrid),
                new PropertyMetadata(new int[] { 0 })
            );

        public static readonly DependencyProperty IsOneItemSelectedProperty =
            DependencyProperty.Register(
                nameof(IsOneItemSelected),
                typeof(bool),
                typeof(CustomDataGrid),
                new PropertyMetadata(false)
            );

        public bool IsOneItemSelected
        {
            get { return (bool)GetValue(IsOneItemSelectedProperty); }
            private set { SetValue(IsOneItemSelectedProperty, value); }
        }

        public bool IsRowEditing
        {
            get => (bool)GetValue(IsRowEditingProperty);
            set => SetValue(IsRowEditingProperty, value);
        }

        public bool IsUniqueEnabled
        {
            get => (bool)GetValue(IsUniqueEnabledProperty);
            set => SetValue(IsUniqueEnabledProperty, value);
        }

        public int[] UniqueColumns
        {
            get => (int[])GetValue(UniqueColumnsProperty);
            set => SetValue(UniqueColumnsProperty, value);
        }

        private static void OnIsRowEditingChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e
        )
        {
            var grid = d as CustomDataGrid;
            var selectedIndex = grid.SelectedIndex;

            var editableFirstCellIndex = 0;

            for (int i = 0; i < grid.Columns.Count; i++)
            {
                if (!grid.Columns[i].IsReadOnly)
                {
                    grid.CurrentCell = new DataGridCellInfo(
                        grid.Items[selectedIndex],
                        grid.Columns[editableFirstCellIndex]
                    );
                    grid.BeginEdit();
                    return;
                }
                editableFirstCellIndex++;
            }
        }

        public CustomDataGrid()
        {
            this.SelectionChanged += CustomDataGrid_SelectionChanged;
            this.CellEditEnding += CustomDataGrid_CellEditEnding;
            this.CurrentCellChanged += CustomDataGrid_CurrentCellChanged;
            this.RowEditEnding += CustomDataGrid_RowEditEnding;
        }

        private void CustomDataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            IsRowEditing = false;
        }

        private void CustomDataGrid_CurrentCellChanged(object sender, EventArgs e)
        {
            IsRowEditing = false;
        }

        private void CustomDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            IsRowEditing = false;

            if (
                IsUniqueEnabled
                && e.EditAction == DataGridEditAction.Commit
                && UniqueColumns != null
            )
            {
                if (UniqueColumns.Contains(e.Column.DisplayIndex))
                {
                    var editingElement = e.EditingElement as TextBox;
                    if (editingElement != null)
                    {
                        string newValue = editingElement.Text;
                        var bindingPath = (e.Column as DataGridBoundColumn)?.Binding as Binding;

                        if (bindingPath != null)
                        {
                            var path = bindingPath.Path?.Path;
                            var items = ItemsSource;
                            if (items != null)
                            {
                                var duplicates = items
                                    .Cast<object>()
                                    .Where(x => x != e.Row.Item)
                                    .Select(x => x.GetType().GetProperty(path)?.GetValue(x, null))
                                    .Any(val => val != null && val.Equals(newValue));

                                if (duplicates)
                                {
                                    MessageBox.Show(
                                        "Bu değer zaten mevcut! Değişiklik yapılamaz.",
                                        "Uyarı",
                                        MessageBoxButton.OK,
                                        MessageBoxImage.Warning
                                    );
                                    e.Cancel = true; // Değişikliği iptal et
                                }
                            }
                            if (string.IsNullOrWhiteSpace(newValue))
                            {
                                MessageBox.Show(
                                    "Boş değer girilemez!",
                                    "Warning",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Warning
                                );
                                e.Cancel = true;
                            }
                        }
                    }
                }
            }
        }

        private void CustomDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.SelectedItemsList = this.SelectedItems;
            IsOneItemSelected = SelectedItems.Count == 1 && SelectedIndex != -1;
        }

        public IList SelectedItemsList
        {
            get => (IList)GetValue(SelectedItemsListProperty);
            set => SetValue(SelectedItemsListProperty, value);
        }

        public static readonly DependencyProperty SelectedItemsListProperty =
            DependencyProperty.Register(
                "SelectedItemsList",
                typeof(IList),
                typeof(CustomDataGrid),
                new PropertyMetadata(null)
            );
    }
}
