﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace br.corp.bonus630.plugin.PlaceHere.Lang
{
    public interface Ilang
    {
        string LBA_ObjOrigin { get; }
        string LBA_data0 { get; }
        string BTN_Start { get; }
        string CB_GetContainer { get; }
        string BTN_Restart { get; }
        string BTN_Continue { get; }

        string OC_MSG_Exit { get; }
    }
}
