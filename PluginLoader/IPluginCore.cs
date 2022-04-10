using System;

namespace br.corp.bonus630.PluginLoader
{
    public interface IPluginCore
    {
        void OnFinishJob(object obj);
        event Action<object> FinishJob;
        
        void OnProgressChange(int progress);
        event Action<int> ProgressChange;
       
    }
}
