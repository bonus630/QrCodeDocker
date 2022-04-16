using System;
using System.IO;
using System.Drawing;

namespace br.corp.bonus630.ImageRender
{
    public class ImageRenderBase:IImageRenderConfig
    {

        protected Graphics graphics;
        protected Brush bBorder;
        protected Brush bDotFill;
        protected Brush bDotBorder;
        protected Pen pWireframe = new Pen(Brushes.Blue);
        protected int dotSize = 2;
        protected double _dotSize = 1;
        protected double dotBorderWidth = 0;
        protected int quietZoneDot = 2;
        protected int m_Padding;
        protected string qrCodeFilePath;
        protected string qrCodeDirPath;
        public string QrCodeFilePath { get { return this.qrCodeFilePath; } }

        private Color borderColor;
        public Color BorderColor { get { return borderColor; } set {borderColor = value; bBorder = new SolidBrush(value); } }  
        private Color dotBorderColor;
        public Color DotBorderColor { get { return DotBorderColor; } set { dotBorderColor = value; bDotBorder = new SolidBrush(value); } }       
        private Color dotFillColor;
        public Color DotFillColor { get { return dotFillColor; } set { dotFillColor = value; bDotFill = new SolidBrush(value); } }
        
       
        public double DotSize { get ; set ; }
        public double DotBorderWidth { get { return dotBorderWidth; } set { dotBorderWidth = value; } }
        public int DotShapeType { get ; set ; }
        public bool Weld { get ; set ; }
        public bool NoBorder { get ; set ; }

        // public BitMatrix BitMatrix { get; protected set; }

        public ImageRenderBase()
        {
            bBorder = Brushes.White;
            bDotFill = Brushes.Black;
            bDotBorder = bDotFill;
            qrCodeDirPath = String.Format("{0}\\qrcode\\", System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData));
            DirectoryInfo dirInfo = new DirectoryInfo(qrCodeDirPath);
            if (!dirInfo.Exists)
                dirInfo.Create();
            qrCodeFilePath = String.Format("{0}temp_qrcode.jpg", qrCodeDirPath);
        }

        protected Size Measure(int matrixWidth)
        {
            double areaWidth = dotSize * matrixWidth;
            if (NoBorder)
                m_Padding = 0;
            else
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


