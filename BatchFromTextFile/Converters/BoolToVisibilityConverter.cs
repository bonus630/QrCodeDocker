using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace br.corp.bonus630.plugin.BatchFromTextFile.Converters
{
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var v = Visibility.Collapsed;
            if (value != null)
            {
                if ((bool)value)
                    v = Visibility.Visible;
                if(parameter!=null)
                    v = (bool)value ? Visibility.Collapsed : Visibility.Visible;
            }
            return v;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return true;
        }
    }
}
