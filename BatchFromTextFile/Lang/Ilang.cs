using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Shapes;
using System.Windows;
using System.Xml.Linq;

namespace br.corp.bonus630.plugin.BatchFromTextFile.Lang
{
    public interface Ilang
    {
        string LBA_LineDelimiter { get; }
        string LBA_ColumnDelimiter { get; }
        string BTN_Browser { get; }
        string OF_File { get; }
        string Rows { get; }
        string Columns { get; }
        string CB_ScapeChars { get; }
        
    }
}
