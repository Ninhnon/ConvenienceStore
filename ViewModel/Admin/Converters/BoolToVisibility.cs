using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ConvenienceStore.ViewModel.Admin.Converters
{
    internal class BoolToVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var IsChecked = (bool)value;
            if (IsChecked) return Visibility.Visible;
            else return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
