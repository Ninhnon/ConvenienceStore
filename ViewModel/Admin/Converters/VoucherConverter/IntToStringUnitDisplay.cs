using System;
using System.Globalization;
using System.Windows.Data;

namespace ConvenienceStore.ViewModel.Admin.Converters.VoucherConverter
{
    class IntToStringUnitDisplay : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var number = (int)value;
            if (number == 0)
            {
                return "VND";
            }
            return "%";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
