﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ConvenienceStore.ViewModel.Admin.Converters.VoucherConverter
{
    class StatusToDisplayConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var status = (int)value;
            if (status == 1)
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
