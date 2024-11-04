namespace XapkPackagingTool.Utility.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    internal class IndexConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var item = values[0];
            var items = values[1] as System.Collections.IList;

            if (items != null)
            {
                return items.IndexOf(item);
            }

            return -1;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
