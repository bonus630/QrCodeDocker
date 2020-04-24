using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace br.corp.bonus630.QrCodeDocker.MainTabControls
{
    public class MainTabBase : UserControl, IMainTabControl
    {
        protected string formatedText;
        public string FormatedText
        {
            get { return formatedText; }
        }

        public event Action AnyTextChanged;

        public virtual void OnAnyTextChanged()
        {
            if (AnyTextChanged != null)
                AnyTextChanged();
        }
    }
}
