using System;

namespace br.corp.bonus630.PluginLoader
{
    public interface IPluginCore
    {
        //void OnFinishJob(object obj);
        event Action<object> FinishJob;
        
        //void OnProgressChange(int progress);
        event Action<int> ProgressChange;
        event Action UpdatePreview;
        LangController Lang { get; set; }
        int Index { get; set; }
        string PluginDisplayName { get; }
        IPluginCore GetICore { get; }
        Type GetType { get; }
        IPluginUI CreateOrGetMainUIIntance();
        void SaveConfig();
        void LoadConfig();
        void DeleteConfig();
        void ChangeLang(LangTagsEnum langTag, System.Reflection.Assembly assembly);
    }
}
