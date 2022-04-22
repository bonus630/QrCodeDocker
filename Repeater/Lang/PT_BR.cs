using br.corp.bonus630.PluginLoader;

namespace br.corp.bonus630.plugin.Repeater.Lang
{
    public class PT_BR : LangController, Ilang
    {
        public string CB_Vector { get { return "Vetor"; } }

        public string CB_Bitmap { get { return "Bitmap"; } }

        public string CB_FitToPage { get { return "Ajustar a página"; } }

        public string LBA_QR_Format { get { return "Formato do QrCode:"; } }

        public string LBA_Left { get { return "Esquerda X:"; } }

        public string LBA_Top { get { return "Cima Y:"; } }

        public string LBA_Gap { get { return "Espaço:"; } }

        public string LBA_InitialValue { get { return "Valor inicial"; } }

        public string LBA_Increment { get { return "Incremento"; } }

        public string LBA_Mask { get { return "Mascára"; } }

        public string GBH_FirstLineTpl { get { return "Modelo da primeira linha"; } }

        public string GBH_Position { get { return "Posição"; } }

        public string GBH_Enumaration { get { return "Enumeração"; } }

        public string ToolTip_GBH_FirstLineTpl { get { return "Escolha um tipo e uma forma para cada coluna"; } }

        public string BTN_Process { get { return "MultiDraw"; } }
        public string BTN_Enumerator { get { return "Enumerador"; } }

        public string MBOX_ERROR_PerfectSquare { get { return "Você precisa selecionar um quadrado perfeito"; } }

        public string MBOX_ERROR_TextShape { get { return "Você precisa de uma forma de texto"; } }

        public string MBOX_ERROR_CorrectShape { get { return "É a forma correta?"; } }

        public string MBOX_ERROR_ValidDataSource { get { return "Você precisa carregar um bando de dados válido"; } }

        public string MBOX_ERROR_DataSourceEmpty { get { return "Seu bando de dados está vazio!"; } }

        public string MBOX_ERROR_NoShapes { get { return "Nenhuma forma selecionada"; } }

        public string Warning { get { return "Aviso"; } }
        public string CK_IgnoreFirstLine { get { return "Ignorar primeira linha"; } }
    }
}
