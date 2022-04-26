using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Corel.Interop.VGCore;
using System.ComponentModel;

namespace br.corp.bonus630.PluginLoader
{
    //https://docs.microsoft.com/pt-br/openspecs/windows_protocols/ms-lcid/a9eac961-e77d-41a6-90a5-ce1a8b0cdb9c
    public enum LangTagsEnum
    {
        PT_BR,
        EN_US,
        ES_ES,
        RU_RU,
        DE_DE,
        FR_FR,
        ZH_Hant
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
                case cdrTextLanguage.cdrSpanish:
                    tag = LangTagsEnum.ES_ES;
                    break;
                case cdrTextLanguage.cdrTraditionalChinese:
                    tag = LangTagsEnum.ZH_Hant;
                    break;
                case cdrTextLanguage.cdrRussian:
                    tag = LangTagsEnum.RU_RU;
                    break;
                case cdrTextLanguage.cdrGerman:
                    tag = LangTagsEnum.DE_DE;
                    break;
                case cdrTextLanguage.cdrFrench:
                    tag = LangTagsEnum.FR_FR;
                    break;
            }
            return tag;
        }
    }

}
