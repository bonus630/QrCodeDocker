using System;
using System.Windows.Controls;

namespace br.corp.bonus630.PluginLoader
{
    public interface IPluginCore
    {
        event Action<object> FinishJob;
        event Action<string> AnyTextChanged;
        event Action<string> CorelThemeChanged;
        event Action<int> ProgressChange;
        event Action LoadConfigEvent;
        event Action SaveConfigEvent;
        event Action UpdatePreview;
        event Action<System.Drawing.Bitmap> OverridePreview;
        int Index { get; set; }
        string GetPluginDisplayName { get; }
        string Theme { get; set; }
        IPluginCore GetICore { get; }
        Type GetType { get; }
        IPluginMainUI CreateOrGetMainUIIntance(Type type);
        UserControl UIControl { get; }
        void SaveConfig();
        void LoadConfig();
        void DeleteConfig();
        void ChangeLang(string langCode);
        string GetLocalizedString(string key);



    }
}
