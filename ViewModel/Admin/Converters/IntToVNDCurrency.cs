using System;
using System.Globalization;
using System.Windows.Data;

namespace ConvenienceStore.ViewModel.Admin.Converters
{
    public class IntToVNDCurrency : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var num = (int)value;
            return string.Format("{0:n0} VND", num);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
