using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Corel.Interop.VGCore;
using System.ComponentModel;

namespace br.corp.bonus630.PluginLoader
{
    
    public enum LangTagsEnum
    {
        PT_BR,
        EN_US
    }
    public static class LangConvertion
    {
        public static LangTagsEnum cdrLangToSys(this cdrTextLanguage language)
        {
            LangTagsEnum tag = LangTagsEnum.EN_US;
            switch (language)
            {
                case cdrTextLanguage.cdrBrazilianPortuguese:
                    tag = LangTagsEnum.PT_BR;
                    break;
                case cdrTextLanguage.cdrEnglishUS:
                    tag = LangTagsEnum.EN_US;
                    break;

            }
            return tag;
        }
    }

}
