using System;
using System.Globalization;
using System.Windows.Data;

namespace ConvenienceStore.ViewModel.Admin.Converters.VoucherConverter
{
    class StatusToDisplayStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var status = (int)value;
            if (status == 0)
            {
                return "Giảm tiền";
            }
            return "Giảm theo phần trăm";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
