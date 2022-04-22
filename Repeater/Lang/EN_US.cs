using br.corp.bonus630.PluginLoader;


namespace br.corp.bonus630.plugin.Repeater.Lang
{
    public class EN_US : LangController, Ilang
    {
        public string CB_Vector { get { return "Vector"; } }

        public string CB_Bitmap { get { return "Bitmap"; } }

        public string CB_FitToPage { get { return "Fit To Page"; } }

        public string LBA_QR_Format { get { return "QrCode Format:"; } }

        public string LBA_Left { get { return "Left X:"; } }

        public string LBA_Top { get { return "Top Y:"; } }

        public string LBA_Gap { get { return "Gap:"; } }

        public string LBA_InitialValue { get { return "Initial Value"; } }

        public string LBA_Increment { get { return "Increment"; } }

        public string LBA_Mask { get { return "Mask"; } }

        public string GBH_FirstLineTpl { get { return "First Line Model"; } }

        public string GBH_Position { get { return "Position"; } }

        public string GBH_Enumaration { get { return "Enumeration"; } }

        public string ToolTip_GBH_FirstLineTpl { get { return "Choose a type and a shape model for each column"; } }

        public string BTN_Process { get { return "MultiDraw"; } }
        public string BTN_Enumerator { get { return "Enumerator"; } }

        public string MBOX_ERROR_PerfectSquare { get { return "You need get a perfect square"; } }

        public string MBOX_ERROR_TextShape { get { return "You need an text shape"; } }

        public string MBOX_ERROR_CorrectShape { get { return "Is correct shape?"; } }

        public string MBOX_ERROR_ValidDataSource { get { return "You need load a valid Data Source"; } }

        public string MBOX_ERROR_DataSourceEmpty { get { return "Your Data Source is empty!"; } }

        public string MBOX_ERROR_NoShapes { get { return "No shapes selected"; } }

        public string Warning { get { return "Warning"; } }
        public string CK_IgnoreFirstLine { get { return "Ignore first line"; } }
    }
}
