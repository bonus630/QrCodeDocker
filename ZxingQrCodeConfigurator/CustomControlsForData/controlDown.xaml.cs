using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace br.corp.bonus630.topologia.CustomControlsForData
{

    /// <summary>
    /// Interaction logic for controlDown.xaml
    /// </summary>
    public partial class controlDown : UserControl
    {
        public string UserInputText { get { return this.txt_content.Text; } set { this.txt_content.Text = value; } }
        public Visibility SetVisible { set { this.Visibility = value; } }
        private ControlDownNumericType numericType;
        public controlDown(string label, string si,ControlDownNumericType numericType)
        {
            InitializeComponent();
            this.label.Content = label;
            this.SI.Content = si;
            this.numericType = numericType;
        }

        public void restoreDefault()
        {
            this.txt_content.Text = "0";
        }
        private void checkTextFormat(object sender, TextCompositionEventArgs e)
        {
            string pattern = "";
            if (this.numericType == ControlDownNumericType._Double)
                pattern = @"^(-)?(\d+)?([\.|,]{1})?([\d]{0,2})?$";
            if (this.numericType == ControlDownNumericType._Int)
                pattern = @"^(-)?(\d+)?$";


            Regex rg = new Regex(pattern);
            string text = (e.Source as TextBox).Text;
            string prevText = e.Text;
            
            bool stop = false;
            stop = !rg.IsMatch(prevText);
            
            if(prevText == "," || prevText == ".")
            {
                
                stop = (text.Contains(",") || text.Contains("."));
                if (this.numericType == ControlDownNumericType._Int)
                    stop = true;
                if (prevText == "." && !stop)
                {
               
                    TextBox tb = sender as TextBox;
                    tb.AppendText(",");
                    tb.CaretIndex = tb.Text.Length;
                    e.Handled = true;
                    return;
                 }
            }
                
                
            e.Handled = stop;
                   
        }

        

        private void txt_content_GotFocus(object sender, RoutedEventArgs e)
        {
            if (this.txt_content.Text == "0")
                this.txt_content.Text = "";
            
        }

        private void txt_content_LostFocus(object sender, RoutedEventArgs e)
        {
            if (this.txt_content.Text == "")
                this.txt_content.Text = "0";
        }
    }
}
