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
    /// Interaction logic for TextControl.xaml
    /// </summary>
    public partial class TextControl : TabItem,IMainTabControl
    {
        public TextControl()
        {
            InitializeComponent();
        }
        public string FormatedText { get; private set; }
        

        public event Action<string> AnyTextChanged;


        private void MakeFormatedText()
        {
            if (!string.IsNullOrEmpty(txt_content.Text))
            {
                FormatedText = txt_content.Text;
                if (AnyTextChanged != null)
                    AnyTextChanged(FormatedText);
            }
        }

      
        private void txt_content_TextChanged(object sender, TextChangedEventArgs e)
        {
            lba_count.Content =  (300 - txt_content.Text.Length).ToString();


            if (txt_content.IsFocused)
            {

                MakeFormatedText();

            }

        }
        private void txt_content_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (txt_content.Text.Length >= 300)
                e.Handled = true;
        }
    }
}
