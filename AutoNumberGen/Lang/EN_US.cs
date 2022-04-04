using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using br.corp.bonus630.PluginLoader;

namespace br.corp.bonus630.plugin.AutoNumberGen.Lang
{
    public class EN_US : LangController, ILang
    {
        public string LBA_Start { get { return "Start:"; } }

        public string LBA_End { get { return "End:"; } }
    }
}