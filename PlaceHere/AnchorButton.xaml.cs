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
        public Anchor ReferencePoint { get; set; }
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
                    ReferencePoint = Anchor.TopLeft;
                    FactorX = 0;
                    FactorY = 0;
                break;
                case 1:
                    ReferencePoint = Anchor.TopMiddle;
                    FactorX = 0.5;
                    FactorY = 0;
                break;
                case 2:
                    ReferencePoint = Anchor.TopRight;
                    FactorX = 1;
                    FactorY = 0;
                    break;
                case 3:
                    ReferencePoint = Anchor.MiddleLeft;
                    FactorX = 0;
                    FactorY = -0.5;
                    break;
                case 4:
                    ReferencePoint = Anchor.Center;
                    FactorX = 0.5;
                    FactorY = -0.5;
                    break;
                case 5:
                    ReferencePoint = Anchor.MiddleRight;
                    FactorX = 1;
                    FactorY = -0.5;
                    break;
                case 6:
                    ReferencePoint = Anchor.BottonLeft;
                    FactorX = 0;
                    FactorY = -1;
                    break;
                case 7:
                    ReferencePoint = Anchor.BottonMiddle;
                    FactorX = 0.5;
                    FactorY = -1;
                    break;
                case 8:
                    ReferencePoint = Anchor.BottonRight;
                    FactorX = 1;
                    FactorY = -1;
                    break;
            }
            if (FactorChanged != null)
                FactorChanged(FactorX, FactorY);
        }
    }

    public enum Anchor
    {
        TopLeft,
        TopMiddle,
        TopRight,
        MiddleLeft,
        Center,
        MiddleRight,
        BottonLeft,
        BottonMiddle,
        BottonRight
    }
}
