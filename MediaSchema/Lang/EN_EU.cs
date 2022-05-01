using br.corp.bonus630.PluginLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace br.corp.bonus630.plugin.MediaSchema.Lang
{
    public class EN_EU : LangController, ILang
    {
        public string MenuItem_OpenXML { get { return "Open XML \"Schemas\" file"; } }

        public string MenuItem_OpenIconFolder { get { return "Open Image Folder"; } }
    }
}
