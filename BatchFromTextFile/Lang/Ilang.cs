using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        
    }
}
