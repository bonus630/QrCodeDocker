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
    /// Interaction logic for WiFiControl.xaml
    /// </summary>
    public partial class WiFiControl : TabItem, IMainTabControl
    {
        public WiFiControl()
        {
            InitializeComponent();
        }

        public string FormatedText { get; private set; }
        

        public event Action<string> AnyTextChanged;

       
        private void MakeFormatedText()
        {
            if (ComboBoxSecurity.SelectedItem != null && !string.IsNullOrEmpty(TextBoxSSID.Text))
            {
                FormatedText = string.Format("WIFI:S:{0};T:{1};P:{2};H:{3};", TextBoxSSID.Text, (ComboBoxSecurity.SelectedItem as ComboBoxItem).Content, TextBoxPassword.Text, (bool)CheckBoxSSIDHidden.IsChecked ? "true" : "false");
                if (AnyTextChanged != null)
                    AnyTextChanged(FormatedText);
            }
        }

        private void TextBoxSSID_TextChanged(object sender, TextChangedEventArgs e)
        {
            MakeFormatedText();
        }

        private void ComboBoxSecurity_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MakeFormatedText();
        }

        private void TextBoxPassword_TextChanged(object sender, TextChangedEventArgs e)
        {
            MakeFormatedText();
        }

        private void CheckBoxSSIDHidden_Click(object sender, RoutedEventArgs e)
        {
            MakeFormatedText();
        }
    }
}
