using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace br.corp.bonus630.QrCodeDocker.Converters
{
    [ValueConversion(typeof(double), typeof(double))]
    class WidthSubConverter : IValueConverter
    {
        double toOp = 38;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (double)value - toOp;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (double)value + toOp;
        }
    }
}
