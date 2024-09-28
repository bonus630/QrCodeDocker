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
using Xceed.Wpf.Toolkit;

namespace br.corp.bonus630.QrCodeDocker.MainTabControls
{
    /// <summary>
    /// Interaction logic for EventControl.xaml
    /// </summary>
    public partial class EventControl : TabItem, IMainTabControl
    {
        /* Evento
            BEGIN:VEVENT\r\n
            SUMMARY:Reunião de Projeto\r\n
            DTSTART:20240909T140000Z\r\n
            DTEND:20240909T150000Z\r\n
            LOCATION:Escritório Central\r\n
            DESCRIPTION:Discutir os próximos passos do projeto.\r\n
            END:VEVENT\r\n
         */
        public EventControl()
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
        private void txt_event(object sender, TextChangedEventArgs e)
        {
            summary = txt_eventName.Text;
            location = txt_eventLocation.Text;
            description = txt_eventDescription.Text;
            MakeFormatedText();
        }
        private void dateTimePicker_start_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            timeStart = (DateTime)(sender as DateTimePicker).Value;
            MakeFormatedText();
        }
        private void dateTimePicker_end_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            timeEnd = (DateTime)(sender as DateTimePicker).Value;
            MakeFormatedText();
        }
        private string ConvertDateTime(DateTime dateTime)
        {
            if(dateTime.Kind != DateTimeKind.Utc)
            {
                dateTime = dateTime.ToUniversalTime();
            }
            return dateTime.ToString("yyyyMMddTHHmmssZ");
        }
        private string summary = string.Empty;
        private string location = string.Empty;
        private string description = string.Empty;
        private DateTime timeStart = DateTime.Now;
        private DateTime timeEnd = DateTime.Now;
        private string CreateVCard()
        {//3Titulo 4nome
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("BEGIN:VEVENT");

           

            if (!String.IsNullOrEmpty(summary))
            {
                //temp = removeSpaces(temp);
                sb.AppendLine("SUMMARY:" + summary);

            }
        
                sb.AppendLine("DTSTART:" + ConvertDateTime(timeStart));

  
                sb.AppendLine("DTEND:" + ConvertDateTime(timeEnd));

         
         
            if (!String.IsNullOrEmpty(location))
            {
                //temp = removeSpaces(temp);
                sb.AppendLine("LOCATION:" + location);

            } 
           
            if (!String.IsNullOrEmpty(description))
            {
                //temp = removeSpaces(temp);
                sb.AppendLine("DESCRIPTION:" + description);

            }
            

            sb.AppendLine("END:VEVENT");
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
