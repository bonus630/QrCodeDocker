using System;
using System.Drawing;
using Gma.QrCodeNet.Encoding;
using System.Diagnostics;

namespace br.corp.bonus630.ImageRender
{
    
    public class GmaImageRender :ImageRenderBase, IImageRender
    {
        private QrEncoder qrEncoder = new QrEncoder(ErrorCorrectionLevel.H);
        private QrCode qrCode;
        private Gma.QrCodeNet.Encoding.BitMatrix bitMatrix;
        public BitMatrix BitMatrixProp { get; private set; }

        public GmaImageRender():base()
        {
         
         
        }
        public void EncodeNewBitMatrix(string content, int sqrSize = 0, bool useSQRSize = false)
        {
            if (!String.IsNullOrEmpty(content))
            {
                qrCode = qrEncoder.Encode(content);
                bitMatrix = qrCode.Matrix;
                BitMatrixProp = new BitMatrix(bitMatrix.InternalArray, bitMatrix.Width, bitMatrix.Height);
            }
          
        }
    
        public void SaveTempQrCodeFile(string content,int resolution, int sqrSize)
        {

            Bitmap bitmap = this.RenderBitmapToMemory(content,resolution, sqrSize);
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
           double padding   = quietZoneDot * _dotSize;
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

            EncodeNewBitMatrix(content);
           
           
            dotSize = sqrSize / bitMatrix.Width;
            Size size = Measure(bitMatrix.Width);
            while (size.Width > sqrSize)
            {
                dotSize--;
                size = Measure(bitMatrix.Width);
            }
            
            Bitmap bitmap = new Bitmap(size.Width,size.Height);
            bitmap.SetResolution(resolution, resolution);
            using(graphics = Graphics.FromImage(bitmap))
            {
                graphics.FillRectangle(bBorder, 0, 0, size.Width, size.Height);
             
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

        public string DecodeQrCode(Bitmap bitmap)
        {
            throw new NotImplementedException();
        }

        public Bitmap RenderBitmapToMemory2(string content, int resolution = 72, int sqrSize = 221)
        {
            throw new NotImplementedException();
        }

        public Bitmap RenderWireframeToMemory(string content, int resolution = 72, int sqrSize = 221)
        {
            throw new NotImplementedException();
        }
    }
}
