using Corel.Interop.VGCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace br.corp.bonus630.PluginLoader
{
    public abstract class LangController : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public static LangController CreateInstance(Assembly assembly, LangTagsEnum lang)
        {
            LangController langObj = null;
            Type[] types = assembly.GetTypes();
            for (int i = 0; i < types.Length; i++)
            {
                if (types[i].IsSubclassOf(typeof(LangController)) && types[i].Name.Equals(lang.ToString()))
                    langObj = Activator.CreateInstance(types[i]) as LangController;
            }
            return langObj;
        }
        public void AutoUpdateProperties()
        {
            Type type = this.GetType();
            PropertyInfo[] properties = type.GetProperties();
            for (int i = 0; i < properties.Length; i++)
            {
                OnPropertyChanged(properties[i].Name);
            }
        }
    }

}
