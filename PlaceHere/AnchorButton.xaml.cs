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

namespace br.corp.bonus630.plugin.PlaceHere
{
    /// <summary>
    /// Interaction logic for AnchorButton.xaml
    /// </summary>
    public partial class AnchorButton : UserControl
    {
        public double FactorX { get; set; }
        public double FactorY { get; set; }

        public event Action<double,double> FactorChanged;

        public AnchorButton()
        {
            InitializeComponent();
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            //0 1 2
            //3 4 5
            //6 7 8

            int tag = Int32.Parse((sender as RadioButton).Tag.ToString());
            
            switch (tag)
            {
                case 0:
                    FactorX = 0;
                    FactorY = 0;
                break;
                case 1:
                    FactorX = 0.5;
                    FactorY = 0;
                break;
                case 2:
                    FactorX = 1;
                    FactorY = 0;
                    break;
                case 3:
                    FactorX = 0;
                    FactorY = -0.5;
                    break;
                case 4:
                    FactorX = 0.5;
                    FactorY = -0.5;
                    break;
                case 5:
                    FactorX = 1;
                    FactorY = -0.5;
                    break;
                case 6:
                    FactorX = 0;
                    FactorY = -1;
                    break;
                case 7:
                    FactorX = 0.5;
                    FactorY = -1;
                    break;
                case 8:
                    FactorX = 1;
                    FactorY = -1;
                    break;
            }
            if (FactorChanged != null)
                FactorChanged(FactorX, FactorY);
        }
    }
}
