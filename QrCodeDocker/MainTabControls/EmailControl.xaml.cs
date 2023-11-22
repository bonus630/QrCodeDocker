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
    /// Interaction logic for EmailControl.xaml
    /// </summary>
    public partial class EmailControl : TabItem,IMainTabControl
    {
        public EmailControl()
        {
            InitializeComponent();
        }
        public void LoadLang(string xmlFile)
        {
            var xmlDataProvider = FindResource("Lang") as XmlDataProvider;

            if (xmlDataProvider != null)
            {
                xmlDataProvider.Source = new Uri(xmlFile, UriKind.RelativeOrAbsolute);
            }
        }
        public string FormatedText { get; private set; }


        public event Action<string> AnyTextChanged;


        private void MakeFormatedText()
        {
            if (!string.IsNullOrEmpty(txt_email.Text)){
                FormatedText = string.Format("mailto:{0}", txt_email.Text);
                if (AnyTextChanged != null)
                    AnyTextChanged(FormatedText);
            }
        }
        private void txt_email_TextChanged(object sender, TextChangedEventArgs e)
        {
            MakeFormatedText();
        }
        private void btn_colar2_Click(object sender, RoutedEventArgs e)
        {
            if (System.Windows.Clipboard.ContainsText())
                txt_email.Text = System.Windows.Clipboard.GetText();
        }
    }
}
