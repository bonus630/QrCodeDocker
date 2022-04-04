using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Corel.Interop.VGCore;
using br.corp.bonus630.QrCodeDocker.Lang;
using System.ComponentModel;
using br.corp.bonus630.PluginLoader;
using System.Reflection;

namespace br.corp.bonus630.QrCodeDocker
{
    public class VisualDataContext: INotifyPropertyChanged
    {
        Application app;
        Ilang langObj;
        public event PropertyChangedEventHandler PropertyChanged;
        public Ilang Lang {get{return this.langObj;} }
        public VisualDataContext(Application corelApp)
        {
            this.app = corelApp;
            LoadLang();
        }
        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        private void LoadLang()
        {
            langObj = LangController.CreateInstance(Assembly.GetAssembly(typeof(br.corp.bonus630.QrCodeDocker.Docker)), this.app.UILanguage.cdrLangToSys()) as Ilang;
            (langObj as LangController).AutoUpdateProperties();
        }
    }
}
