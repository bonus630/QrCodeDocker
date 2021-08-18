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
    /// Interaction logic for VCardControl.xaml
    /// </summary>
    public partial class VCardControl : TabItem,IMainTabControl
    {
        public VCardControl()
        {
            InitializeComponent();
        }
        public string FormatedText { get; private set; }

        public event Action<string> AnyTextChanged;


        private void MakeFormatedText()
        {
            FormatedText = CreateVCard();
            if (AnyTextChanged != null)
                AnyTextChanged(FormatedText);
        }
        private void txt_vcard(object sender, TextChangedEventArgs e)
        {
            MakeFormatedText();
        }
        private string CreateVCard()
        {//3Titulo 4nome
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("BEGIN:VCARD");

            string temp = txt_vcardName.Text;

            if (!String.IsNullOrEmpty(temp))
            {
                //temp = removeSpaces(temp);
                sb.AppendLine("N:" + temp);

            }
            temp = txt_vcardTitle.Text;
            if (!String.IsNullOrEmpty(temp))
            {
                //temp = removeSpaces(temp);
                sb.AppendLine("TITLE:" + temp);

            }
            temp = txt_vcardUrl.Text;
            if (!String.IsNullOrEmpty(temp))
            {
                temp = removeSpaces(temp);
                sb.AppendLine("URL:" + temp);

            }
            temp = txt_vcardTel.Text;
            if (!String.IsNullOrEmpty(temp))
            {
                temp = removeSpaces(temp);
                sb.AppendLine("TEL:" + temp);

            }
            temp = txt_vcardEmail.Text;
            if (!String.IsNullOrEmpty(temp))
            {
                temp = removeSpaces(temp);
                sb.AppendLine("EMAIL:" + temp);

            }
            temp = txt_vcardJob.Text;
            if (!String.IsNullOrEmpty(temp))
            {
               // temp = removeSpaces(temp);
                sb.AppendLine("ORG:" + temp);

            }
            temp = txt_vcardAdd.Text;
            if (!String.IsNullOrEmpty(temp))
            {
             //   temp = removeSpaces(temp);
                sb.AppendLine("ADR:" + temp);

            }
            temp = txt_vcardNote.Text;
            if (!String.IsNullOrEmpty(temp))
            {
                temp = removeSpaces(temp);
                sb.AppendLine("NOTE:" + temp);

            }
            //

            sb.AppendLine("END:VCARD");
            ///Ainda preciso terminar essa parte

            return sb.ToString();
        }
        private string removeSpaces(string temp)
        {
            temp = temp.Trim();
            temp = temp.Replace(" ", ";");
            return temp;
        }
    }
}
