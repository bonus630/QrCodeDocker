using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace br.corp.bonus630.plugin.MediaSchema.Converters
{
    [ValueConversion(typeof(bool), typeof(double))]
    public class BoolToOpacityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double opacity = 0.6;
            double param = opacity;
            if (parameter != null)
            {
                Double.TryParse(parameter.ToString(), out param);
            }
            if (value != null)
            {
                if ((bool)value)
                    opacity = 1;
                else
                    opacity = param;
            }
            return opacity;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
