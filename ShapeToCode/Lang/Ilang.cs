using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace br.corp.bonus630.plugin.ShapeToCode.Lang
{
    public interface Ilang
    {
        string BTN_ProcessDoc { get; }
        string BTN_ProcessPage { get; }
        string BTN_ProcessSelection { get; }
        string CB_DeleteOri { get; }
        string CB_UseWidthToSize { get; }
        string MBOX_title { get; }
    }
}
