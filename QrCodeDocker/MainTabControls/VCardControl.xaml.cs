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
    public partial class VCardControl : TabItem, IMainTabControl
    {
        /* VCARD
         * *************************
         * BEGIN:VCARD
            VERSION:3.0
            ADR:;;Conde Matarazzo;goiania;goias;74463360;brasil
            EMAIL:bonus630@gmail.com
            FN:Reginaldo Santos
            MSG:
            N:Santos;Reginaldo
            NOTE:este é um teste
            TEL:
            TEL;TYPE=CELL,VOICE:55629912365
            TEL;TYPE=HOME,VOICE:556239281361
            TEL;TYPE=WORK,FAX:55965874556
            TEL;TYPE=WORK,VOICE:556299878963
            URL:https://bonus630.com.br
            END:VCARD
        ***********************
        meCard
        MECARD:ADR:,,Conde Matarazzo,goiania,goias,74463360,brasil;EMAIL:bonus630@gmail.com;MSG:;N:Santos,Reginaldo;NOTE:este é um teste;TEL:;TEL;TYPE=CELL,VOICE:55629912365;TEL;TYPE=HOME,VOICE:556239281361;TEL;TYPE=WORK,FAX:55965874556;TEL;TYPE=WORK,VOICE:556299878963;URL:https://bonus630.com.br;;
         */
        public VCardControl()
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
