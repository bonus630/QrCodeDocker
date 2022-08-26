using System.Drawing;


namespace br.corp.bonus630.ImageRender
{
    public interface IImageRender
    {
        string QrCodeFilePath {  get; }
        void SaveTempQrCodeFile(string content, int resolution, int sqrSize);

        void SaveTempQrCodeFile(string content);
        string DecodeQrCode(Bitmap bitmap);
        Bitmap RenderBitmapToMemory(string content, int resolution = 72, int sqrSize = 221);
        Bitmap RenderBitmapToMemory2(string content, int resolution = 72, int sqrSize = 221);
        Bitmap RenderWireframeToMemory(string content, int resolution = 72, int sqrSize = 221);
        double Measure();

        double InMeasure(double newSize);

        BitMatrix BitMatrixProp {  get; }

        ErrorCorrectionLevelEnum ErrorCorrection { get; set; }

        void EncodeNewBitMatrix(string content,int strSize = 0, bool useSQRSize = false);
        ResultC GetResult { get; }
    }   
}
