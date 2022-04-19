using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace br.corp.bonus630.PluginLoader
{
    public abstract class PluginCoreBase<T> : IPluginCore where T: class, new()
    {
        protected IPluginUI mainUI;

        public event Action<object> FinishJob;
        public event Action<int> ProgressChange;
        public event Action UpdatePreview;

        public abstract LangController Lang { get; set; }
        public abstract string PluginDisplayName { get; }
        public T GetCore { get { return this as T; } }
        public IPluginCore GetICore { get { return GetCore as IPluginCore; } }
        public Type GetType { get { return typeof(T); } }
        public int Index { get; set; }

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


        public void ChangeLang(LangTagsEnum langTag, System.Reflection.Assembly assembly)
        {
            LangController Lang = LangController.CreateInstance(assembly, langTag);
            Lang.AutoUpdateProperties();
        }
        
        public IPluginUI CreateOrGetMainUIIntance()
        {
            if (mainUI == null)
            {
                mainUI = Activator.CreateInstance(typeof(T)) as IPluginUI;
                mainUI.DataContext = this;
            }
            return mainUI;
        }

        public abstract void SaveConfig();

        public abstract void LoadConfig();

        public abstract void DeleteConfig();
    }
}
