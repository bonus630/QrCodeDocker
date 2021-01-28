using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace br.corp.bonus630.plugin.ZxingQrCodeConfigurator
{
    /// <summary>
    /// Interaction logic for ColorPicker.xaml
    /// </summary>
    public partial class ColorPicker : Window
    {
        ColorManager colorManager;
        public ColorPicker(Corel.Interop.VGCore.Palette palette)
        {
            InitializeComponent();
            colorManager = new ColorManager(palette,this);
            this.DataContext = colorManager;
            //colorManager.ExitEyedropper += ColorManager_ExitEyedropper;
            btn_ok.Click += (s, e) => { this.DialogResult = true; };
        }
        public ColorPicker(ColorManager colorManager)
        {
            InitializeComponent();
            this.DataContext = colorManager;
            btn_ok.Click += (s, e) => { this.DialogResult = true; };
        }
        //private void ColorManager_ExitEyedropper()
        //{
        //    this.Visibility = Visibility.Visible;
        //    this.Focus();
        //}

        public ColorSystem SelectedColor { get { return colorManager.SelectedColor; } }
        protected override void OnDeactivated(EventArgs e)
        {
            onClose();
        }
        protected override void OnLostFocus(RoutedEventArgs e)
        {
            onClose();
        }

        public void onClose()
        {
            try
            {
                if (colorManager.InEyedropper)
                    this.DialogResult = true;
                //    this.Visibility = Visibility.Visible;
                //else
                this.Close();
                ////colorManager.Close();
            }
            catch { }
        }
    }

}
