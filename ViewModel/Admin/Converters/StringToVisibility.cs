using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ConvenienceStore.ViewModel.Admin.Converters
{
    public class StringToVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var searchContent = (string)value;
            if (searchContent != "")
                return Visibility.Hidden;
            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
