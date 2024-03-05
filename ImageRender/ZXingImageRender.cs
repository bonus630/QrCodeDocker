using System;
using System.Drawing;
using ZXing.Common;
using System.Diagnostics;
using ZXing.QrCode;
using ZXing;
using System;
using System.IO;
using System.Drawing;
using System.Collections.Generic;

namespace br.corp.bonus630.ImageRender
{
    //References
    //https://zxingnet.codeplex.com/discussions/453067
    //https://skrymerdev.wordpress.com/2012/09/22/qr-code-generation-with-zxing/
    public class ZXingImageRender : ImageRenderBase
    {


        private ZXing.Common.BitMatrix bitMatrix;
        public BitMatrix BitMatrixProp { get; private set; }
        private ZXing.QrCode.Internal.ErrorCorrectionLevel ErrorCorrectionLevel { get; set; }


        public ZXing.QrCode.Internal.ErrorCorrectionLevel ErrorCorrection
        {
            get
            {
                return ZXing.QrCode.Internal.ErrorCorrectionLevel.H;
            }
            set
            {
                this.ErrorCorrectionLevel = value;
            }
        }
        private Result resultC;
        public Result GetResult { get { return this.resultC; } }

        BarcodeWriter writer = new BarcodeWriter();
        private readonly Dictionary<int, string> enumMapping = new Dictionary<int, string>
        {
            { 1, "https://en.wikipedia.org/wiki/Aztec_Code" },
            { 2, "https://en.wikipedia.org/wiki/Codabar" },
            { 4, "" },
            { 8, "" },
            { 0x10, "" },
            { 0x20, "" },
            { 0x40, "" },
            { 0x80, "" },
            { 0x100, "" },
            { 0x200, "" },
            { 0x400, "" },
            { 0x800, "https://www.qrcode.com/en/about/standards.html" },
            { 0x1000, "" },
            { 0x2000, "" },
            { 0x4000, "" },
            { 0x8000, "" },
            { 0x10000, "" },
            { 0x20000, "" },
            { 0x40000, "" },
            { 0x80000, "" },
            { 0x100000, "" },
            { 0xF1DE, "" }
        };

        public string GetCodeTypeHelpURL()
        {
            int value = (int)CodeType;
            return enumMapping.ContainsKey(value) && !string.IsNullOrEmpty(enumMapping[value])  ? enumMapping[value] : "https://www.corelnaveia.com";
        }

        public bool IsMatrixCode()
        {
            if (CodeType == BarcodeFormat.QR_CODE || CodeType == BarcodeFormat.AZTEC || CodeType == BarcodeFormat.DATA_MATRIX || CodeType == BarcodeFormat.PDF_417)
                return true;
            return false;
        }

        public ZXingImageRender() : base()
        {


        }
        public void EncodeNewBitMatrix(string content, int sqrSize = 0, bool useSQRSize = false)
        {
            //formatos tipo codigo de barra
            //CODABAR, CODE 39, CODE 93,CODE 128,EAN 8,EAN 13,ITF,MSI,PLESSEY,

            //FORMATOS RETANGULARES
            //QRCODE,AZTEC,DATAMATRIX,PDF417

            //FORMATOS INVALIDOS
            //RSS 14,RSS EXPANDED,MAXICODE,UPC EAN EXPANDED,IMB,PHARMA CODE, ALL 1D

            //FORMATOS NÃO DEFENIDOS
            //UPC A,UPC B

            this.writer.Format = CodeType;

            //this.writer.Options = new QrCodeEncodingOptions { Width = sqrSize , Height = sqrSize , CharacterSet = "UTF-8" };

            QrCodeEncodingOptions options = new QrCodeEncodingOptions
            {
                CharacterSet = "UTF-8",

            };
            if (CodeType != BarcodeFormat.AZTEC)
                options.ErrorCorrection = ErrorCorrectionLevel;


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
                //bool[,] m = new bool[this.bitMatrix.Width, this.bitMatrix.Width];
                //for (int j = 0; j < bitMatrix.Width; j++)
                //{
                //    for (int i = 0; i < bitMatrix.Width; i++)
                //    {
                //        m[i, j] = this.bitMatrix[i, j];
                //    }
                //}
                //BitMatrixProp = new BitMatrix(m, this.bitMatrix.Width, this.bitMatrix.Height);
                BitMatrixProp = this.bitMatrix;
            }

        }


        public void SaveTempQrCodeFile(string content, int resolution, int sqrSize,int dotHeight)
        {

            Bitmap bitmap = this.RenderBitmapToMemory2(content, resolution, sqrSize,dotHeight);
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

        public Bitmap RenderBitmapToMemory(string content, int resolution = 72, int sqrSize = 221,int dotHeight = 10)
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
                    for (int i = 0; i < bitMatrix.Height; i++)
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
        public Bitmap RenderBitmapToMemory2(string content, int resolution = 72, int sqrSize = 221, int dotHeight = 10)
        {

            EncodeNewBitMatrix(content, sqrSize, false);

            //dotSize = 10;
            dotSize = sqrSize / bitMatrix.Width;
            Pen pDotBorder = new Pen(bDotBorder, (float)DotBorderWidth * 10);
            Rectangle rDot;
            int bmpWidth = dotSize * bitMatrix.Width;
            int bmpHeight = dotSize * bitMatrix.Height;
            if (bitMatrix.Height == 1)
                bmpHeight = dotHeight;
            Bitmap bitmap = new Bitmap(bmpWidth, bmpHeight);

            bitmap.SetResolution(resolution, resolution);
            //bitmap = this.writer.Write(content);
            //return bitmap;
            using (graphics = Graphics.FromImage(bitmap))
            {
                if (!NoBorder)
                {
                    graphics.FillRectangle(bBorder, 0, 0, sqrSize, sqrSize);
                }
                for (int j = 0; j < bitMatrix.Height; j++)
                {
                    for (int i = 0; i < bitMatrix.Width; i++)
                    {
                        if (IsMatrixCode())
                            rDot = new Rectangle(i * dotSize + m_Padding, j * dotSize + m_Padding, dotSize, dotSize);
                        else
                            rDot = new Rectangle(i * dotSize + m_Padding, j * dotSize + m_Padding, dotSize, dotHeight);
                        Debug.WriteLine(string.Format("i:{0} j:{1}", i, j));
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
                                    if ((j + 1) < bitMatrix.Height && !bitMatrix[i, j + 1])
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

                        else
                        {
                            graphics.FillRectangle(bBorder, rDot);
                        }
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

        public Bitmap RenderWireframeToMemory(string content, int resolution = 72, int sqrSize = 221,int dotHeight = 10)
        {
            EncodeNewBitMatrix(content, sqrSize, true);


            dotSize = sqrSize / bitMatrix.Width;
            int sqrHeight = sqrSize;
            if (!IsMatrixCode())
                sqrHeight = dotHeight;

            Bitmap bitmap = new Bitmap(sqrSize, sqrHeight);
            bitmap.SetResolution(resolution, resolution);
            //bitmap.MakeTransparent(Color.White);
            using (graphics = Graphics.FromImage(bitmap))
            {
                if (!NoBorder)
                {
                   
                    graphics.DrawRectangle(pWireframe, 0, 0, sqrSize, sqrHeight);
                }
                //graphics.FillRectangle(bBorder, 0, 0, sqrSize, sqrSize);
                for (int j = 0; j < bitMatrix.Height; j++)
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
                            if ((j + 1) < bitMatrix.Height && !bitMatrix[i, j + 1])
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
