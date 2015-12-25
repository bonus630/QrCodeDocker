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
        private BitMatrix bitMatrix;
    
        public GmaImageRender():base()
        {
         
         
        }
        private void EncodeNewBitMatrix(string content)
        {
            if (!String.IsNullOrEmpty(content))
            {
                qrCode = qrEncoder.Encode(content);
                bitMatrix = qrCode.Matrix;
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
                graphics.FillRectangle(bWhite, 0, 0, size.Width, size.Height);
             
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
    }
}
