using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace br.corp.bonus630.plugin.ZxingQrCodeConfigurator
{
    public static class Extensions
    {
        
            public static System.Windows.Media.SolidColorBrush ToMediaSystemColor(this Corel.Interop.VGCore.Color corelColor)
            {
                string hexValue = corelColor.HexValue;
                System.Windows.Media.Color color = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(hexValue);
                System.Windows.Media.SolidColorBrush b = new System.Windows.Media.SolidColorBrush(color);
                return b;
            }
        public static System.Drawing.Color ToSystemColor(this Corel.Interop.VGCore.Color corelColor)
        {
            string hexValue = corelColor.HexValue;
            System.Drawing.Color color = ColorTranslator.FromHtml(hexValue);
            
            
            return color;
        }



    }
}
