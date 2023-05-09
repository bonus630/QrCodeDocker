using br.corp.bonus630.PluginLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace br.corp.bonus630.plugin.LogoDesigner.Lang
{
    public class EN_US : LangController, ILang
    {
        public string BTN_Draw { get { return "Draw QrCode"; } }

        public string BTN_Browser { get { return "Browser Logo"; } }
        public string BTN_UseSelection { get { return "Use Selection like Logo"; } }
        public string OF_BrowserFile { get { return "Select a Image File"; } }
        public string BTN_Reset { get { return "Reset"; } }
    
    
    }
}
