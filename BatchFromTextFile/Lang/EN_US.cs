

using br.corp.bonus630.PluginLoader;

namespace br.corp.bonus630.plugin.BatchFromTextFile.Lang
{
    public class EN_US : LangController, Ilang
    {
        public string LBA_LineDelimiter { get { return "Line Delimiter:"; } }

        public string LBA_ColumnDelimiter { get { return "Column Delimiter:"; } }

        public string BTN_Browser { get { return "Browser"; } }

        public string OF_File { get { return "text files| *.txt|CSV Files| *.csv"; } }

        public string Rows { get { return "Rows"; } }

        public string Columns { get { return "Columns"; } }
        public string CB_ScapeChars { get { return "Use Special Chars"; } }
        
    }
}
