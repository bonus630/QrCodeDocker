using Corel.Interop.VGCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace br.corp.bonus630.QrCodeDocker
{
    public class ColorSystem
    {
        public ColorSystem(string colorHexValue, string corelColorName, Color corelColor)
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
