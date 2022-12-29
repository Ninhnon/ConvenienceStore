using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ConvenienceStore.ViewModel.Admin.Converters
{
    internal class IndexToVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var Index = (int)value;
            if (Index == -1)
            {
                return Visibility.Hidden;
            }
            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
