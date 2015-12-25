using System;
using System.Drawing;
using ZXing.Common;
using System.Diagnostics;
using ZXing.QrCode;
using ZXing;


namespace br.corp.bonus630.ImageRender
{
    //https://zxingnet.codeplex.com/discussions/453067
    public class ZXingImageRender : ImageRenderBase,IImageRender
    {
        private BitMatrix bitMatrix;
        BarcodeWriter writer = new BarcodeWriter();


        public ZXingImageRender():base()
        {
          

        }
        private void EncodeNewBitMatrix(string content, int sqrSize)
        {

            this.writer.Format = BarcodeFormat.QR_CODE;
            this.writer.Options = new QrCodeEncodingOptions { Width = sqrSize, Height = sqrSize, CharacterSet = "UTF-8" };



            if (!String.IsNullOrEmpty(content))
            {
                this.bitMatrix = this.writer.Encode(content);

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
    }
}
