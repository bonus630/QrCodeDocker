using System;

namespace br.corp.bonus630.PluginLoader
{
    public interface IPluginUI
    {
        void ChangeLang(LangTagsEnum langTag);
        void SaveConfig();
        void LoadConfig();
        void DeleteConfig();

        int Index { get; set; }
        string PluginDisplayName { get; }
        object DataContext { get; set; }

    }
}
