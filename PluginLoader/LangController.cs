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
            List<Type> defaultTypes = new List<Type>();
            for (int i = 0; i < types.Length; i++)
            {
                if (types[i].IsSubclassOf(typeof(LangController)))
                {
                    if (types[i].Name.Equals(lang.ToString()))
                        langObj = CreateInstance(types[i], lang);
                    defaultTypes.Add(types[i]);
                }
            }
            if (langObj == null && defaultTypes.Count > 0)
            {
                for (int i = 0; i < defaultTypes.Count; i++)
                {
                    if (defaultTypes[i].Name.Equals(LangTagsEnum.EN_US.ToString()))
                    {
                        langObj = CreateInstance(defaultTypes[i], LangTagsEnum.EN_US);
                    }
                }

            }
            if (langObj == null && defaultTypes.Count > 0)
            {
                langObj = CreateInstance(defaultTypes[0], (LangTagsEnum)Enum.Parse(typeof(LangTagsEnum),defaultTypes[0].Name));
            }
            return langObj;
        }
        private static LangController CreateInstance(Type type, LangTagsEnum lang)
        {
            return Activator.CreateInstance(type) as LangController;
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
