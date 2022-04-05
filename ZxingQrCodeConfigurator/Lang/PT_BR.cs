

using br.corp.bonus630.PluginLoader;

namespace br.corp.bonus630.plugin.ZxingQrCodeConfigurator.Lang
{
    public class PT_BR : LangController, Ilang
    {
        public string CB_Weld { get { return "Soldar"; } }

        public string CB_NoBorder { get { return "Sem Borda"; } }

        public string LBA_DotShape { get { return "Forma do ponto:"; } }

        public string LBA_BorderColor { get { return "Cor da borda:"; } }

        public string LBA_DotBorderColor { get { return "Cor da borda do ponto:"; } }

        public string LBA_DotBorderSize { get { return "Tamanho da borda do ponto:"; } }

        public string BTN_Verify { get { return "Verificar"; } }

        public string LBA_DotFillColor { get { return "Cor do preenchimento do ponto:"; } }
        public string MBOX_QrMessage { get { return "Seu QRCode contém a seguinte mensagem:"; } }

        public string MBOX_QrCodingWarning { get { return "Altere o sistema de codificaçõa para Zxing para validar!"; } }

        public string MBOX_ERRO_QRInvalid { get { return "QRCode inválido!"; } }
        public string Warning { get { return "Aviso"; } }
    }
}
