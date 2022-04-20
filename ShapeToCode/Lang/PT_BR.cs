

using br.corp.bonus630.PluginLoader;

namespace br.corp.bonus630.plugin.ShapeToCode.Lang
{
    public class PT_BR : LangController, Ilang
    {
        public string BTN_ProcessDoc { get { return "Processar Documento"; } }

        public string BTN_ProcessPage { get { return "Processar Página"; } }

        public string BTN_ProcessSelection { get { return "Processar Seleção"; } }

        public string CB_DeleteOri { get { return "Deletar Original"; } }

        public string CB_UseWidthToSize { get { return "Usar largura como tamanho"; } }
        public string MBOX_title { get { return "Faça correções"; } }
    }
}
