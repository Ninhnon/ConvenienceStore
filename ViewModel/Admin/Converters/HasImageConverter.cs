using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ConvenienceStore.ViewModel.Admin.Converters
{
    public class HasImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return Visibility.Visible;
            }
            return Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
