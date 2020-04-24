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
    /// Interaction logic for SmsControl.xaml
    /// </summary>
    public partial class SmsControl : TabItem,IMainTabControl
    {
        public SmsControl()
        {
            InitializeComponent();
        }
        public string FormatedText { get; private set; }


        public event Action<string> AnyTextChanged;


        private void MakeFormatedText()
        {
            if (!string.IsNullOrEmpty(txt_smsMen.Text) || !string.IsNullOrEmpty(txt_smsTel.Text)) {
                FormatedText = String.Format("SMSTO:+{0}:{1}", txt_smsTel.Text, txt_smsMen.Text);
                if (AnyTextChanged != null)
                    AnyTextChanged(FormatedText);
            }
        }
        private void txt_smsTel_TextChanged(object sender, TextChangedEventArgs e)
        {
            MakeFormatedText();
        }
          
        private void txt_smsMen_TextChanged(object sender, TextChangedEventArgs e)
        {
            MakeFormatedText();
        }
        private void txt_smsTel_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            int res;
            if (!Int32.TryParse(e.Text, out res))
                e.Handled = true;
        }
    }
}
