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
using System.Windows.Shapes;

namespace br.corp.bonus630.plugin.ZxingQrCodeConfigurator
{
    /// <summary>
    /// Interaction logic for ValidateWindow.xaml
    /// </summary>
    public partial class ValidateWindow : Window
    {
        public ValidateWindow(string msg, string filePath)
        {
            InitializeComponent();
            txt_message.Text = msg;
            BitmapImage bitmap = new BitmapImage(new Uri(filePath));
            img_card.Source = bitmap;
        }
    }
}
