using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace br.corp.bonus630.plugin.Repeater.Lang
{
    interface Ilang
    {
        string CB_Vector { get; }
        string CB_Bitmap { get; }
        string CB_FitToPage { get; }
        string LBA_QR_Format { get; }
        string LBA_Left { get; }
        string LBA_Top { get; }
        string LBA_Gap { get; }
        string LBA_InitialValue { get; }
        string LBA_Increment { get; }
        string LBA_Mask { get; }
        string GBH_FirstLineTpl { get; }
        string GBH_Position { get; }
        string GBH_Enumaration { get; }
        string ToolTip_GBH_FirstLineTpl { get; }
        string BTN_Process { get; }
         string BTN_Enumerator { get;  }
        string MBOX_ERROR_PerfectSquare { get; }
        string MBOX_ERROR_TextShape { get; }
        string MBOX_ERROR_CorrectShape { get; }
        string MBOX_ERROR_ValidDataSource { get; }
        string MBOX_ERROR_DataSourceEmpty { get; }
        string MBOX_ERROR_NoShapes { get; }
        string Warning { get; }
        string CK_IgnoreFirstLine { get; }
    }
}
