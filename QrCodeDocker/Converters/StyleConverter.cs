using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace br.corp.bonus630.QrCodeDocker.Converters
{
    public class StyleConverter: IMultiValueConverter
    {
       
            public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            {
                FrameworkElement targetElement = values[0] as FrameworkElement;
                string styleName = values[1] as string;

                if (styleName == null)
                    return null;

                Style newStyle = (Style)targetElement.TryFindResource(styleName);

                if (newStyle == null)
                    newStyle = (Style)targetElement.TryFindResource("MyDefaultStyleName");

                return newStyle;
            }

            public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }
    
}
