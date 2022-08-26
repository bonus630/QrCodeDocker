using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Controls;


namespace br.corp.bonus630.PluginLoader
{
    public abstract class PluginCoreBase<T> : IPluginCore, INotifyPropertyChanged where T : class, new()
    {
        protected IPluginMainUI mainUI;
        private LangController lang;

        public event Action<object> FinishJob;
        public event Action<int> ProgressChange;
        public event Action<string> AnyTextChanged;
        public event Action<System.Drawing.Bitmap> OverridePreview;
        public event Action UpdatePreview;
        public event Action LoadConfigEvent;
        public event Action SaveConfigEvent;
        public event Action LangChanged;
        public event PropertyChangedEventHandler PropertyChanged;

        public LangController Lang { get { return this.lang; } set { lang = value; if (LangChanged != null) LangChanged(); } }
        public UserControl UIControl { get { return (UserControl)this.mainUI; } }
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
        protected virtual void OnOverridePreview(Bitmap bitmap)
        {
            if (OverridePreview != null)
                OverridePreview(bitmap);
        }
        public virtual void OnNotifyPropertyChanged(string propertyName = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        public void ChangeLang(LangTagsEnum langTag, System.Reflection.Assembly assembly)
        {
            try
            {
                this.Lang = LangController.CreateInstance(assembly, langTag);
                if(this.Lang != null)
                    this.Lang.AutoUpdateProperties();
            }
            catch { }
        }

        public IPluginMainUI CreateOrGetMainUIIntance(Type type)
        {
            if (mainUI == null)
            {
                try
                {
                    mainUI = Activator.CreateInstance(type) as IPluginMainUI;
                    mainUI.Core = this;
                    mainUI.DataContext = this;
                }
                catch { }
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
