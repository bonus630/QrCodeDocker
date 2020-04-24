using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Corel.Interop.VGCore;


namespace br.corp.bonus630.plugin.ZxingQrCodeConfigurator
{
    public class ColorManager
    {
        private ColorSystem[] colorArray;
        public ColorSystem[] ColorArray { get {return this.colorArray; } }

        public ColorManager(Palette palette)
        {
            if (palette == null)
                return;
            colorArray = new ColorSystem[palette.ColorCount];
            for (int i = 1; i < palette.ColorCount; i++)
            {
                Color color = palette.Color[i];
                colorArray[i - 1] = new ColorSystem(color.HexValue, color.Name, color);
            }
            
        }

    }
    public class ColorSystem
    {
        public ColorSystem(string colorHexValue,string corelColorName, Color corelColor)
        {
            this.colorHexValue = colorHexValue;
            this.corelColorName = corelColorName;
            this.corelColor = corelColor;
        }

        private string colorHexValue;

        public string ColorHexValue
        {
            get { return colorHexValue; }
            set { colorHexValue = value; }
        }
        private string corelColorName;

        public string CorelColorName
        {
            get { return corelColorName; }
            set { corelColorName = value; }
        }
        private Color corelColor;
        public Color CorelColor
        {
            get { return corelColor; }
            set { corelColor = value; }
        }


    }
}
