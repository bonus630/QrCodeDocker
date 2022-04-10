using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace br.corp.bonus630.ImageRender
{
    public interface IImageRenderConfig
    {

        Color BorderColor { get; set; }
        Color DotBorderColor { get; set; }
        Color DotFillColor { get; set; }
        double DotSize { get; set; }
        double DotBorderWidth { get; set; }
        
        int DotShapeType { get; set; }
        bool Weld { get; set; }
        bool NoBorder { get; set; }

    }
}
