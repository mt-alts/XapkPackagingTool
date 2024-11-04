/* 
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using System.Windows.Controls;
using System.Windows.Data;

namespace XapkPackagingTool.CustomControl
{
    public class StringDataGridTextColumn : DataGridTextColumn
    {
        public StringDataGridTextColumn()
        {
            this.Binding = new Binding(".")
            {
                Mode = BindingMode.TwoWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            };
        }
    }
}
