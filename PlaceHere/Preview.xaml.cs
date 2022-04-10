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
using br.corp.bonus630.ImageRender;

namespace br.corp.bonus630.plugin.PlaceHere
{
    /// <summary>
    /// Interaction logic for Eyedropper.xaml
    /// </summary>
    public partial class Preview : Window
    {

        PreviewDataContext eye;
        Corel.Interop.VGCore.Application app;
        private string currentTheme;
        public IImageRender Render { get; set; }
        public Preview(Corel.Interop.VGCore.Application app)
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.Manual;
             eye = new PreviewDataContext(app);
            this.DataContext = eye;
            eye.NewPositionEvent += Eye_NewPositionEvent;
            this.app = app;
           
        }

        private void Eye_NewPositionEvent(System.Drawing.Rectangle obj)
        {
            this.Dispatcher.Invoke(
                new Action(() =>
                {
                    this.Left = obj.X+1;
                    this.Top = obj.Y-1;
                    this.Width = obj.Width+2;
                    this.Height = obj.Height+2;
                }
            ));
        }
        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (e.Key == Key.Up)
                this.Top++;
            if (e.Key == Key.Down)
                this.Top--;
            if (e.Key == Key.Left)
                this.Left--;
            if (e.Key == Key.Right)
                this.Left++;
        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            
            eye.Close();

        }
        protected override void OnLostFocus(RoutedEventArgs e)
        {
            eye.IsVisibility = Visibility.Collapsed;
        }
       
        public void renderImage(string textContent)
        {

            System.Windows.Media.Imaging.BitmapSource bitmapSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
             Render.RenderWireframeToMemory(textContent).GetHbitmap(),
             IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            img_preview.Source = bitmapSource;
            
            this.Width = img_preview.Source.Width;
            this.Height = img_preview.Source.Height;

        }
        public void SetSize(double size)
        {
            eye.Size = size;
        }
       public void SetGetContainer(bool getContainer)
        {
            eye.GetContainer = getContainer;
        }
 

    }
}
