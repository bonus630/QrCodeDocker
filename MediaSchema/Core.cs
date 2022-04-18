using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using br.corp.bonus630.PluginLoader;
using Corel.Interop.VGCore;

namespace br.corp.bonus630.plugin.MediaSchema
{
    public class Core : PluginCoreBase,IPluginDrawer
    {
        public const string PluginDisplayName = "Media Scheme";

        private List<object[]> dataSource;
        private double size;
        private Application app;
        private ICodeGenerator codeGenerator;
        public List<object[]> DataSource { set { this.dataSource = value; } }
        public double Size { set { this.size = value; } }
        public Application App { set { this.app = value; } }
        public ICodeGenerator CodeGenerator { set { this.codeGenerator = value; } }

        public void Draw()
        {
            throw new NotImplementedException();
        }
     
    }
}
