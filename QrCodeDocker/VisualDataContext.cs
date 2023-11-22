using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Corel.Interop.VGCore;
using System.ComponentModel;
using br.corp.bonus630.PluginLoader;
using System.Reflection;

namespace br.corp.bonus630.QrCodeDocker
{
    public class VisualDataContext: INotifyPropertyChanged
    {
        Application app;
        public event PropertyChangedEventHandler PropertyChanged;
        private bool canLoadPlugin = true;
        public bool CanLoadPlugin { get { return canLoadPlugin; } set { canLoadPlugin = value; OnPropertyChanged("CanLoadPlugin"); } }
        public VisualDataContext(Application corelApp)
        {
            this.app = corelApp;
        }
        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
 
    }
}
