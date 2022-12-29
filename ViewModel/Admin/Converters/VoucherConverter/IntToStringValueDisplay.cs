using System;
using System.Globalization;
using System.Windows.Data;

namespace ConvenienceStore.ViewModel.Admin.Converters.VoucherConverter
{
    class IntToStringValueDisplay : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var number = (int)value;
            if (number == 0)
            {
                return "20000";
            }
            return "10";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
