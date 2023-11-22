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
    /// Interaction logic for URLControl.xaml
    /// </summary>
    public partial class URLControl : TabItem,IMainTabControl
    {
        public URLControl()
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
            if (!string.IsNullOrEmpty(txt_url.Text))
            {
                FormatedText = txt_url.Text;
                if (AnyTextChanged != null)
                    AnyTextChanged(FormatedText);
            }
        }
        private void btn_colar_Click(object sender, RoutedEventArgs e)
        {
            if (System.Windows.Clipboard.ContainsText())
                txt_url.Text = System.Windows.Clipboard.GetText();
        }
        private void txt_url_TextChanged(object sender, TextChangedEventArgs e)
        {
            MakeFormatedText();

        }
    }
}
