using System.Drawing;


namespace br.corp.bonus630.ImageRender
{
    public interface IImageRender
    {
        string QrCodeFilePath { get; }
        void SaveTempQrCodeFile(string content, int resolution, int sqrSize);

         void SaveTempQrCodeFile(string content);

         Bitmap RenderBitmapToMemory(string content, int resolution = 72, int sqrSize = 221);

         double Measure();

         double InMeasure(double newSize);
    }   
}
