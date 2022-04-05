

using br.corp.bonus630.PluginLoader;

namespace br.corp.bonus630.plugin.ZxingQrCodeConfigurator.Lang
{
    public class EN_US : LangController, Ilang
    {
        public string CB_Weld { get { return "Weld"; } }

        public string CB_NoBorder { get { return "No Border"; } }

        public string LBA_DotShape { get { return "Dot Shape:"; } }

        public string LBA_BorderColor { get { return "Border Color:"; } }

        public string LBA_DotBorderColor { get { return "Dot Border Color:"; } }

        public string LBA_DotBorderSize { get { return "Dot Border Size:"; } }

        public string BTN_Verify { get { return "Verify"; } }

        public string LBA_DotFillColor { get { return "Dot Fill Color:"; } }

        public string MBOX_QrMessage { get { return "Your Qrcode contains the follow message:"; } }

        public string MBOX_QrCodingWarning { get { return "Change your encoding to Zxing to validate!"; } }

        public string MBOX_ERRO_QRInvalid { get { return "Invalid qrcode"; } }
        public string Warning { get { return "Warning"; } }
    }
}
