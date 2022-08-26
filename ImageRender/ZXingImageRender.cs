using System;
using System.Drawing;
using ZXing.Common;
using System.Diagnostics;
using ZXing.QrCode;
using ZXing;
using System;
using System.IO;
using System.Drawing;

namespace br.corp.bonus630.ImageRender
{
    //References
    //https://zxingnet.codeplex.com/discussions/453067
    //https://skrymerdev.wordpress.com/2012/09/22/qr-code-generation-with-zxing/
    public class ZXingImageRender : ImageRenderBase, IImageRender
    {
        //protected Graphics graphics;
        //protected Brush bWhite;
        //protected Brush bBlack;
        //protected int dotSize = 2;
        //protected double _dotSize = 1;
        //protected int quietZoneDot = 2;
        //protected int m_Padding;
        //protected string qrCodeFilePath;
        //protected string qrCodeDirPath;
        //public string QrCodeFilePath { get { return this.qrCodeFilePath; } }

        // public BitMatrix BitMatrix { get; protected set; }

        //public void  ImageRenderBase()
        //{
        //    bWhite = Brushes.White;
        //    bBlack = Brushes.Black;
        //    qrCodeDirPath = String.Format("{0}\\qrcode\\", System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData));
        //    DirectoryInfo dirInfo = new DirectoryInfo(qrCodeDirPath);
        //    if (!dirInfo.Exists)
        //        dirInfo.Create();
        //    qrCodeFilePath = String.Format("{0}temp_qrcode.jpg", qrCodeDirPath);
        //}

        //protected Size Measure(int matrixWidth)
        //{
        //    double areaWidth = dotSize * matrixWidth;
        //    m_Padding = quietZoneDot * dotSize;
        //    double padding = m_Padding;
        //    double totalWidth = areaWidth + 2 * padding;
        //    return new Size((int)totalWidth, (int)totalWidth);
        //}

        //public double InMeasure(int matrixWidth, double size)
        //{
        //    double totalWidth = size - 1;
        //    double _dotSize = (size) / (matrixWidth + 4);
        //    return _dotSize;
        // }




        private ZXing.Common.BitMatrix bitMatrix;
        public BitMatrix BitMatrixProp { get; private set; }
        private ZXing.QrCode.Internal.ErrorCorrectionLevel ErrorCorrectionLevel { get; set; }

        public ErrorCorrectionLevelEnum ErrorCorrection
        {
            get
            {
                return ErrorCorrectionLevelEnum.H;
            }
            set
            {
                switch (value)
                {
                    case ErrorCorrectionLevelEnum.L:
                        this.ErrorCorrectionLevel = ZXing.QrCode.Internal.ErrorCorrectionLevel.L;
                        break;
                    case ErrorCorrectionLevelEnum.M:
                        this.ErrorCorrectionLevel = ZXing.QrCode.Internal.ErrorCorrectionLevel.M;
                        break;
                    case ErrorCorrectionLevelEnum.Q:
                        this.ErrorCorrectionLevel = ZXing.QrCode.Internal.ErrorCorrectionLevel.Q;
                        break;
                    case ErrorCorrectionLevelEnum.H:
                        this.ErrorCorrectionLevel = ZXing.QrCode.Internal.ErrorCorrectionLevel.H;
                        break;
                }
            }
        }
        private ResultC resultC;
        public ResultC GetResult { get { return this.resultC; } }

        BarcodeWriter writer = new BarcodeWriter();

        public ZXingImageRender() : base()
        {


        }
        public void EncodeNewBitMatrix(string content, int sqrSize = 0, bool useSQRSize = false)
        {

            this.writer.Format = BarcodeFormat.QR_CODE;

            //this.writer.Options = new QrCodeEncodingOptions { Width = sqrSize , Height = sqrSize , CharacterSet = "UTF-8" };

            QrCodeEncodingOptions options = new QrCodeEncodingOptions { CharacterSet = "UTF-8", ErrorCorrection = ErrorCorrectionLevel };

            if (NoBorder)
                options.Margin = 0;
            if (useSQRSize)
            {
                options.Width = sqrSize;
                options.Height = sqrSize;
            }
            this.writer.Options = options;


            if (!String.IsNullOrEmpty(content))
            {
                this.bitMatrix = this.writer.Encode(content);
                bool[,] m = new bool[this.bitMatrix.Width, this.bitMatrix.Width];
                for (int j = 0; j < bitMatrix.Width; j++)
                {
                    for (int i = 0; i < bitMatrix.Width; i++)
                    {
                        m[i, j] = this.bitMatrix[i, j];
                    }
                }
                BitMatrixProp = new BitMatrix(m, this.bitMatrix.Width, this.bitMatrix.Height);
            }

        }


        public void SaveTempQrCodeFile(string content, int resolution, int sqrSize)
        {

            Bitmap bitmap = this.RenderBitmapToMemory(content, resolution, sqrSize);
            try
            {
                bitmap.Save(qrCodeFilePath);
            }
            catch (Exception erro)
            {
                Debug.WriteLine(erro.Message);
            }
        }
        public void SaveTempQrCodeFile(string content)
        {

            Bitmap bitmap = this.RenderBitmapToMemory2(content);
            try
            {
                bitmap.Save(qrCodeFilePath);
            }
            catch (Exception erro)
            {
                Debug.WriteLine(erro.Message);
            }
        }
        public double Measure()
        {

            if (this.bitMatrix == null)
                return 0;
            double areaWidth = _dotSize * this.bitMatrix.Width;
            double padding;
            if (NoBorder)
                padding = 0;
            else
                padding = quietZoneDot * dotSize;

            double totalWidth = areaWidth + 2 * padding;
            if (NoBorder)
                return areaWidth;
            else
                return totalWidth;
        }
        public double InMeasure(double newSize)
        {
            if (this.bitMatrix == null)
                return 0;
            double size = newSize;
            double totalWidth = size - 1;
            double _dotSize = (size) / (this.bitMatrix.Width + 4);
            return _dotSize;
        }

        public Bitmap RenderBitmapToMemory(string content, int resolution = 72, int sqrSize = 221)
        {

            EncodeNewBitMatrix(content, sqrSize, true);


            dotSize = sqrSize / bitMatrix.Width;


            Bitmap bitmap = new Bitmap(sqrSize, sqrSize);
            bitmap.SetResolution(resolution, resolution);
            using (graphics = Graphics.FromImage(bitmap))
            {
                graphics.FillRectangle(bBorder, 0, 0, sqrSize, sqrSize);

                for (int j = 0; j < bitMatrix.Width; j++)
                {
                    for (int i = 0; i < bitMatrix.Width; i++)
                    {


                        if (bitMatrix[i, j])
                        {
                            graphics.FillRectangle(bDotFill, i * dotSize + m_Padding, j * dotSize + m_Padding, dotSize, dotSize);

                        }
                        else
                        {
                            graphics.FillRectangle(bBorder, i * dotSize + m_Padding, j * dotSize + m_Padding, dotSize, dotSize);
                        }
                    }
                }

            }
            Debug.WriteLine(bitmap.Width.ToString());
            return bitmap;
        }
        /// <summary>
        /// Have weld function
        /// </summary>
        /// <param name="content"></param>
        /// <param name="resolution"></param>
        /// <param name="sqrSize"></param>
        /// <returns></returns>
        public Bitmap RenderBitmapToMemory2(string content, int resolution = 72, int sqrSize = 221)
        {

            EncodeNewBitMatrix(content, sqrSize, false);

            //dotSize = 10;
            dotSize = sqrSize / bitMatrix.Width;
            Pen pDotBorder = new Pen(bDotBorder, (float)DotBorderWidth * 10);
            Rectangle rDot;
            int bmpWidth = dotSize * bitMatrix.Width;
            Bitmap bitmap = new Bitmap(bmpWidth, bmpWidth);
            bitmap.SetResolution(resolution, resolution);
            using (graphics = Graphics.FromImage(bitmap))
            {
                if (!NoBorder)
                    graphics.FillRectangle(bBorder, 0, 0, sqrSize, sqrSize);

                for (int j = 0; j < bitMatrix.Width; j++)
                {
                    for (int i = 0; i < bitMatrix.Width; i++)
                    {

                        rDot = new Rectangle(i * dotSize + m_Padding, j * dotSize + m_Padding, dotSize, dotSize);
                        if (bitMatrix[i, j])
                        {
                            if (DotShapeType.Equals(1))
                                graphics.FillEllipse(bDotFill, rDot);
                            else
                                graphics.FillRectangle(bDotFill, rDot);
                            if (DotBorderWidth > 0)
                            {
                                if (Weld)
                                {
                                    //Dot top line
                                    if (j != 0 && !bitMatrix[i, j - 1])
                                        graphics.DrawLine(pDotBorder, i * dotSize + m_Padding, j * dotSize + m_Padding, i * dotSize + m_Padding + dotSize, j * dotSize + m_Padding);

                                    //Dot left line
                                    if (i != 0 && !bitMatrix[i - 1, j])
                                        graphics.DrawLine(pDotBorder, i * dotSize + m_Padding, j * dotSize + m_Padding + dotSize, i * dotSize + m_Padding, j * dotSize + m_Padding);
                                    //Dot right line
                                    if ((i + 1) < bitMatrix.Width && !bitMatrix[i + 1, j])
                                        graphics.DrawLine(pDotBorder, i * dotSize + m_Padding + dotSize, j * dotSize + m_Padding + dotSize, i * dotSize + m_Padding + dotSize, j * dotSize + m_Padding);

                                    //Dot bottom line
                                    if ((j + 1) < bitMatrix.Width && !bitMatrix[i, j + 1])
                                        graphics.DrawLine(pDotBorder, i * dotSize + m_Padding, j * dotSize + m_Padding + dotSize, i * dotSize + m_Padding + dotSize, j * dotSize + m_Padding + dotSize);


                                }
                                else
                                {
                                    if (DotShapeType.Equals(2))
                                        graphics.DrawEllipse(pDotBorder, rDot);
                                    else
                                        graphics.DrawRectangle(pDotBorder, rDot);
                                }
                            }
                        }
                        //else
                        //{
                        //    graphics.FillRectangle(bBorder, rDot);
                        //}
                    }
                }

            }
            Debug.WriteLine(bitmap.Width.ToString());
            return bitmap;
        }

        public string DecodeQrCode(Bitmap bitmap)
        {
            BarcodeReader reader = new BarcodeReader();
            Result result = reader.Decode(bitmap);
            if(result != null)
            {
                resultC = new ResultC();
                resultC.NumBits = result.NumBits;
                resultC.Text = result.Text;
                //resultC.ResultMetadata = result.ResultMetadata;
            }
            try
            {
                string res = result.Text;
                reader = null;
                result = null;
                return res;

            }

            catch
            {
                throw new Exception();
            }

        }
        Pen redPen = new Pen(Brushes.Red);
        Pen bluePen = new Pen(Brushes.Blue);
        Pen blackPen = new Pen(Brushes.Black);
        Pen greenPen = new Pen(Brushes.Green);

        public Bitmap RenderWireframeToMemory(string content, int resolution = 72, int sqrSize = 221)
        {
            EncodeNewBitMatrix(content, sqrSize, true);


            dotSize = sqrSize / bitMatrix.Width;


            Bitmap bitmap = new Bitmap(sqrSize, sqrSize);
            bitmap.SetResolution(resolution, resolution);
            //bitmap.MakeTransparent(Color.White);
            using (graphics = Graphics.FromImage(bitmap))
            {
                if (!NoBorder)
                    graphics.DrawRectangle(pWireframe, 0, 0, sqrSize, sqrSize);
                //graphics.FillRectangle(bBorder, 0, 0, sqrSize, sqrSize);
                for (int j = 0; j < bitMatrix.Width; j++)
                {
                    for (int i = 0; i < bitMatrix.Width; i++)
                    {
                        if (bitMatrix[i, j])
                        {
                            //graphics.DrawRectangle(pWireframe, i * dotSize + m_Padding, j * dotSize + m_Padding, dotSize, dotSize);
                            //Dot top line
                            if (j != 0 && !bitMatrix[i, j - 1])
                                graphics.DrawLine(pWireframe, i * dotSize + m_Padding, j * dotSize + m_Padding, i * dotSize + m_Padding + dotSize, j * dotSize + m_Padding);

                            //Dot left line
                            if (i != 0 && !bitMatrix[i - 1, j])
                                graphics.DrawLine(pWireframe, i * dotSize + m_Padding, j * dotSize + m_Padding + dotSize, i * dotSize + m_Padding, j * dotSize + m_Padding);
                            //Dot right line
                            if ((i + 1) < bitMatrix.Width && !bitMatrix[i + 1, j])
                                graphics.DrawLine(pWireframe, i * dotSize + m_Padding + dotSize, j * dotSize + m_Padding + dotSize, i * dotSize + m_Padding + dotSize, j * dotSize + m_Padding);

                            //Dot bottom line
                            if ((j + 1) < bitMatrix.Width && !bitMatrix[i, j + 1])
                                graphics.DrawLine(pWireframe, i * dotSize + m_Padding, j * dotSize + m_Padding + dotSize, i * dotSize + m_Padding + dotSize, j * dotSize + m_Padding + dotSize);


                        }
                    }
                }
            }
            Debug.WriteLine(bitmap.Width.ToString());
            return bitmap;
        }
    }
}
