using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Corel.Interop.VGCore;
using br.corp.bonus630.QrCodeDocker.Lang;
using System.ComponentModel;

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
            switch (this.app.UILanguage)
            {
                case cdrTextLanguage.cdrBrazilianPortuguese:
                    langObj = new PT_BR();
                    break;
                case cdrTextLanguage.cdrEnglishUS:
                    langObj = new US_EN();
                    break;
                default:
                    langObj = new US_EN();
                    break;
            }
        }
    }
}
