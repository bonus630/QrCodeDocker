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
using System.Threading;
using System.Diagnostics;

namespace br.corp.bonus630.plugin.ZxingQrCodeConfigurator
{
    /// <summary>
    /// Interaction logic for Eyedropper.xaml
    /// </summary>
    public partial class Eyedropper : Window
    {

        EyedropperDataContext eye;
        public Eyedropper(Corel.Interop.VGCore.Application app)
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.Manual;
             eye = new EyedropperDataContext(app);
            this.DataContext = eye;
            eye.NewPositionEvent += Eye_NewPositionEvent;
        }

        private void Eye_NewPositionEvent(Point obj)
        {
            this.Dispatcher.Invoke(
                new Action(() =>
                {
                    this.Left = obj.X+30;
                    this.Top = obj.Y-30;
                }
            ));
        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            eye.Close();
        }
    }
}
