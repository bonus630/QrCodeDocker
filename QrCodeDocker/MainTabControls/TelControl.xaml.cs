using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace br.corp.bonus630.QrCodeDocker.MainTabControls
{
    /// <summary>
    /// Interaction logic for TelControl.xaml
    /// </summary>
    public partial class TelControl : TabItem,IMainTabControl
    {
        public TelControl()
        {
            InitializeComponent();
        }
        public string FormatedText { get; private set; }


        public event Action<string> AnyTextChanged;


        private void MakeFormatedText()
        {
            if (!string.IsNullOrEmpty(txt_tel.Text))
            {
                FormatedText = string.Format("TEL:+{0}", txt_tel.Text);
                if (AnyTextChanged != null)
                    AnyTextChanged(FormatedText);
            }
        }
        private void txt_tel_TextChanged(object sender, TextChangedEventArgs e)
        {
            MakeFormatedText();
        }
        private void txt_tel_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            int res;
            if (!Int32.TryParse(e.Text, out res))
                e.Handled = true;
        }
    }
}
