/* 
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace XapkPackagingTool.CustomControl.PlaceholderTextBoxStyles
{
    public class AddLeftPaddingConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is Thickness padding))
                return value;

            if (!double.TryParse(parameter.ToString(), out var amount))
                return value;

            padding.Left += amount;

            return padding;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
