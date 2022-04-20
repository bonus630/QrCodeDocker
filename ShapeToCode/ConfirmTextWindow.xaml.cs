using br.corp.bonus630.plugin.ShapeToCode.Lang;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace br.corp.bonus630.plugin.ShapeToCode
{
    /// <summary>
    /// Interaction logic for ConfirmTextWindow.xaml
    /// </summary>
    public partial class ConfirmTextWindow : Window, INotifyPropertyChanged
    {
        public Ilang Lang { get; set; }
        public ConfirmTextWindow(Ilang lang)
        {
            InitializeComponent();
            Lang = lang;
            this.DataContext = this;
            this.Loaded += ConfirmTextWindow_Loaded;
           
        }

        private void ConfirmTextWindow_Loaded(object sender, RoutedEventArgs e)
        {
           // this.DialogResult = false;
        }

        private string text;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Text
        {
            get { return text; }
            set
            {
                text = value;
                OnNotifyPropertyChanged("Text");
            }
        }

        private void btn_ok_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }
        private void OnNotifyPropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }
    }
}
