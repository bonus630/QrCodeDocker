using br.corp.bonus630.ImageRender;
using Corel.Interop.VGCore;

namespace br.corp.bonus630.PluginLoader
{
    public interface ICodeGenerator
    {
        void SetRender(ImageRender.IImageRender imageRender);
        IImageRender ImageRender { get; }
        Shape CreateVetorLocal(Layer layer, string content, double strSize, double positionX = 0, double positionY = 0, string vectorName = "QrCode Vetor");
        /// This method uses trace tatics
        /// </summary>
        /// <param name="layer"></param>
        /// <param name="content"></param>
        /// <param name="strSize"></param>
        /// <param name="positionX"></param>
        /// <param name="positionY"></param>
        /// <param name="vectorName"></param>
        /// <returns></returns>
        Shape CreateVetorLocal2(Layer layer, string content, double strSize, double positionX = 0, double positionY = 0, string vectorName = "QrCode Vetor");
        Shape CreateBitmapLocal(Layer layer, string content, double strSize, double positionX = 0, double positionY = 0, string bitmapName = "QrCode Bitmap");
        string DecodeImage(string filePath);
    }
}
