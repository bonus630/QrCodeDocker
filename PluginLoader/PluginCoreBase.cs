using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace br.corp.bonus630.PluginLoader
{
    public abstract class PluginCoreBase : IPluginCore
    {
        public event Action<object> FinishJob;
        public event Action<int> ProgressChange;
        public event Action UpdatePreview;

        protected virtual void OnFinishJob(object obj)
        {
            if (FinishJob != null)
                FinishJob(obj);
        }

        protected virtual void OnProgressChange(int progress)
        {
            if (ProgressChange != null)
                ProgressChange(progress);
        }

        protected virtual void OnUpdatePreview()
        {
            if (UpdatePreview != null)
                UpdatePreview();
        }
    }
}
