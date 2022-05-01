using br.corp.bonus630.PluginLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace br.corp.bonus630.plugin.MediaSchema.Lang
{
    public class PT_BR : LangController, ILang
    {
        public string MenuItem_OpenXML { get { return "Abrir arquivo XML dos \"Schemas\""; } }

        public string MenuItem_OpenIconFolder { get { return "Abrir pasta das Imagens"; } }
    }
}
