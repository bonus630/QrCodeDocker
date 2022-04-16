using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace br.corp.bonus630.QrCodeDocker.Converters
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility visibility = Visibility.Collapsed;
            if(value != null)
            {
                if ((bool)value)
                    visibility = Visibility.Visible;
            }
            return visibility;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool re = false;
            if (value != null)
            {
                if ((Visibility)value == Visibility.Visible)
                    re = true;
            }
            return re;
        }
    }
}
