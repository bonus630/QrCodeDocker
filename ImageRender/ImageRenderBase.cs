using System;
using System.IO;
using System.Drawing;

namespace br.corp.bonus630.ImageRender
{
    public class ImageRenderBase
    {

        protected Graphics graphics;
        protected Brush bWhite;
        protected Brush bBlack;
        protected int dotSize = 2;
        protected double _dotSize = 1;
        protected int quietZoneDot = 2;
        protected int m_Padding;
        protected string qrCodeFilePath;
        protected string qrCodeDirPath;
        public string QrCodeFilePath { get { return this.qrCodeFilePath; } }

       // public BitMatrix BitMatrix { get; protected set; }

        public ImageRenderBase()
        {
            bWhite = Brushes.White;
            bBlack = Brushes.Black;
            qrCodeDirPath = String.Format("{0}\\qrcode\\", System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData));
            DirectoryInfo dirInfo = new DirectoryInfo(qrCodeDirPath);
            if (!dirInfo.Exists)
                dirInfo.Create();
            qrCodeFilePath = String.Format("{0}temp_qrcode.jpg", qrCodeDirPath);
        }

        protected Size Measure(int matrixWidth)
        {
            double areaWidth = dotSize * matrixWidth;
            m_Padding = quietZoneDot * dotSize;
            double padding = m_Padding;
            double totalWidth = areaWidth + 2 * padding;
            return new Size((int)totalWidth, (int)totalWidth);
        }
      
        public double InMeasure(int matrixWidth, double size)
        {
            double totalWidth = size - 1;
            double _dotSize = (size) / (matrixWidth + 4);
            return _dotSize;
        }
    }
}


