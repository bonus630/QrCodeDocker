﻿using br.corp.bonus630.PluginLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace br.corp.bonus630.plugin.LogoDesigner.Lang
{
    public class PT_BR : LangController, ILang
    {
        public string BTN_Draw { get { return "Desenhar o QrCode"; } }

        public string BTN_Browser { get { return "Procurar Logo"; } }
        public string BTN_UseSelection { get { return "Usar Seleção como Logo"; } }
        public string OF_BrowserFile { get { return "Selecione um Arquivo de Imagem"; } }
        public string BTN_Reset { get { return "Redefinir"; } }
    }
}