using System;

namespace br.corp.bonus630.PluginLoader
{
    public interface IPluginUI
    {
         //string PluginDisplayName{get;}
        
        void OnProgressChange(int progress);
        void OnFinishJob(object obj);

        void ChangeLang(LangTagsEnum langTag);


        event Action<object> FinishJob;
        event Action<string> AnyTextChanged;
        event Action<int> ProgressChange;
        int Index { get; set; }
    }
}
