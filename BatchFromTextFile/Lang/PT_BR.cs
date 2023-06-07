

using br.corp.bonus630.PluginLoader;

namespace br.corp.bonus630.plugin.BatchFromTextFile.Lang
{
    public class PT_BR : LangController, Ilang
    {
        public string LBA_LineDelimiter { get { return "Delimitador de linha:"; } }

        public string LBA_ColumnDelimiter { get { return "Delimitador de coluna:"; } }

        public string BTN_Browser { get { return "Procurar"; } }

        public string OF_File { get { return "Arquivos de Texto| *.txt|Arquivos CSV| *.csv"; } }

        public string Rows { get { return "Linhas"; } }

        public string Columns { get { return "Colunas"; } }
        public string CB_ScapeChars { get { return "Usar Caracteres Especiais"; } }
    }
}
