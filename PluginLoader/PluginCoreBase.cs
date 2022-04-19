using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace br.corp.bonus630.PluginLoader
{
    public abstract class PluginCoreBase<T> : IPluginCore where T: class, new()
    {
        protected IPluginMainUI mainUI;

        public event Action<object> FinishJob;
        public event Action<int> ProgressChange;
        public event Action<string> AnyTextChanged;
        public event Action UpdatePreview;
        public event Action LoadConfigEvent;
        public event Action SaveConfigEvent;

        public  LangController Lang { get; set; }
        public abstract string GetPluginDisplayName { get; }
        public T GetCore { get { return this as T; } }
        public IPluginCore GetICore { get { return GetCore as IPluginCore; } }
        public new Type GetType { get { return typeof(T); } }
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
        protected virtual void OnAnyTextChanged(string text)
        {
            if (AnyTextChanged != null)
                AnyTextChanged(text);
        }

        public void ChangeLang(LangTagsEnum langTag, System.Reflection.Assembly assembly)
        {
            Lang = LangController.CreateInstance(assembly, langTag);
            Lang.AutoUpdateProperties();
        }
        
        public IPluginMainUI CreateOrGetMainUIIntance(Type type)
        {
            if (mainUI == null)
            {
                mainUI = Activator.CreateInstance(type) as IPluginMainUI;
                mainUI.Core = GetICore;
                mainUI.DataContext = this;
            }
            return mainUI;
        }

        public virtual void SaveConfig()
        {
            if (SaveConfigEvent != null)
                SaveConfigEvent();
        }

        public virtual void LoadConfig()
        {
            if (LoadConfigEvent != null)
                LoadConfigEvent();
        }

        public abstract void DeleteConfig();
    }
}
