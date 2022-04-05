using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace br.corp.bonus630.plugin.ZxingQrCodeConfigurator.Lang
{
    public interface Ilang
    {
        string CB_Weld { get; }
        string CB_NoBorder { get; }
        string LBA_DotShape { get; }
        string LBA_BorderColor { get; }
        string LBA_DotBorderColor { get; }
        string LBA_DotFillColor { get; }
        string LBA_DotBorderSize { get; }
        string BTN_Verify { get; }
        string MBOX_QrMessage { get; }
        string MBOX_QrCodingWarning { get; }
        string MBOX_ERRO_QRInvalid { get; }
        string Warning { get; }

    }
}
