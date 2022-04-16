using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Corel.Interop.VGCore;

namespace br.corp.bonus630.PluginLoader
{
    public interface ICodeConfig
    {
        bool Weld { get; set; }
        bool NoBorder { get; set; }

        Color BorderColor { get; set; }
        Color DotFillColor { get; set; }
        Color DotOutlineColor { get; set; }
        double DotBorderSize { get; set; }
        
    }
}
