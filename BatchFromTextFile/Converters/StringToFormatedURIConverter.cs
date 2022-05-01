using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace br.corp.bonus630.plugin.BatchFromTextFile.Converters
{
    [ValueConversion(typeof(string), typeof(string))]
    public class StringToFormatedURIConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.GetType() != typeof(string))
                throw new InvalidCastException("require a string");
            return Uri.EscapeDataString(value.ToString());
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.GetType() != typeof(string))
                throw new InvalidCastException("require a string");
            return Uri.UnescapeDataString(value.ToString());
        }
    }
}
