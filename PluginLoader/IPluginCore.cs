using System;
using System.Windows.Controls;

namespace br.corp.bonus630.PluginLoader
{
    public interface IPluginCore
    {
        //void OnFinishJob(object obj);
        event Action<object> FinishJob;
        event Action<string> AnyTextChanged;
        //void OnProgressChange(int progress);
        event Action<int> ProgressChange;
        event Action LoadConfigEvent;
        event Action SaveConfigEvent;
        event Action UpdatePreview;

        LangController Lang { get; set; }
        int Index { get; set; }
        string GetPluginDisplayName { get; }
        IPluginCore GetICore { get; }
        Type GetType { get; }
        IPluginMainUI CreateOrGetMainUIIntance(Type type);
        UserControl UIControl { get; }
        void SaveConfig();
        void LoadConfig();
        void DeleteConfig();
        void ChangeLang(LangTagsEnum langTag, System.Reflection.Assembly assembly);




    }
}
