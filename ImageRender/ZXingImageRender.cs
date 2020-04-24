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
    //https://zxingnet.codeplex.com/discussions/453067
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
        BarcodeWriter writer = new BarcodeWriter();
        
        public ZXingImageRender():base()
        {
          

        }
        public void EncodeNewBitMatrix(string content, int sqrSize =0)
        {

            this.writer.Format = BarcodeFormat.QR_CODE;
            
            //this.writer.Options = new QrCodeEncodingOptions { Width = sqrSize , Height = sqrSize , CharacterSet = "UTF-8" };
            this.writer.Options = new QrCodeEncodingOptions { CharacterSet = "UTF-8" };


            if (!String.IsNullOrEmpty(content))
            {
                //this.bitMatrix = this.writer.Encode(content);
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

            Bitmap bitmap = this.RenderBitmapToMemory(content);
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
            double padding = quietZoneDot * _dotSize;
            double totalWidth = areaWidth + 2 * padding;
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

            EncodeNewBitMatrix(content, sqrSize);


            dotSize = sqrSize / bitMatrix.Width;


            Bitmap bitmap = new Bitmap(sqrSize, sqrSize);
            bitmap.SetResolution(resolution, resolution);
            using (graphics = Graphics.FromImage(bitmap))
            {
                graphics.FillRectangle(bWhite, 0, 0, sqrSize, sqrSize);

                for (int j = 0; j < bitMatrix.Width; j++)
                {
                    for (int i = 0; i < bitMatrix.Width; i++)
                    {


                        if (bitMatrix[i, j])
                        {
                            graphics.FillRectangle(bBlack, i * dotSize + m_Padding, j * dotSize + m_Padding, dotSize, dotSize);

                        }
                        else
                        {
                            graphics.FillRectangle(bWhite, i * dotSize + m_Padding, j * dotSize + m_Padding, dotSize, dotSize);
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
    }
}
