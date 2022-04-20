using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace br.corp.bonus630.PluginLoader
{
    public interface IPluginDrawer
    {
        List<object[]> DataSource { set; }
        void Draw();
        double Size { set; }
       
        Corel.Interop.VGCore.Application App { set; }
        
        ICodeGenerator CodeGenerator { set; }
    }
}
