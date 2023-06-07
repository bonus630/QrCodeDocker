
using br.corp.bonus630.PluginLoader;

namespace br.corp.bonus630.plugin.PlaceHere.Lang
{
    public class PT_BR : LangController, Ilang
    {
        public string LBA_ObjOrigin { get { return "Origem:"; } }
        public string LBA_data0 { get { return "Próximo Dado:"; } }

        public string BTN_Start { get { return "Iniciar"; } }

        public string CB_GetContainer { get { return "Colocar no recipiente"; } }

        public string BTN_Restart { get { return "Recomeçar"; } }
        public string BTN_Continue { get { return "Continuar"; } }
        public string OC_MSG_Exit { get { return "Pressione \"ESC\" para Cancelar"; } }
    }
}
