using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using br.corp.bonus630.PluginLoader;

namespace br.corp.bonus630.plugin.DataFromClipboard
{
    public class ClipboardData : ViewModelBase
    {
       

        private string text;

        public string Text
        {
            get { return text; }
            set { text = value; NotifyPropertyChanged("Text"); }
        }
    }
}
