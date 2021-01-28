using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using Corel.Interop.VGCore;

namespace br.corp.bonus630.plugin.ZxingQrCodeConfigurator
{
    public class EyedropperDataContext : NotifyPropertyBase
    {
        Corel.Interop.VGCore.Application app;
        Thread th;
        Corel.Interop.VGCore.DataSourceProxy dc;
        public event Action<System.Windows.Point> NewPositionEvent;
        private string r;
        private bool running = true;
        private Shape shape;
        public void Close()
        {
            running = false;
        }
        public string R
        {
            get { return r; }
            set { r = value; NotifyPropertyChanged(); }
        }
        private string g;

        public string G
        {
            get { return g; }
            set { g = value; NotifyPropertyChanged(); }
        }
        private string b;

        public string B
        {
            get { return b; }
            set { b = value; NotifyPropertyChanged(); }
        }
        private string hex;

        public string Hex
        {
            get { return hex; }
            set { hex = value; NotifyPropertyChanged(); }
        }
        public System.Windows.Visibility isVisibility = System.Windows.Visibility.Collapsed;
        public System.Windows.Visibility IsVisibility
        {
            get { return this.isVisibility; }
            set { this.isVisibility = value;NotifyPropertyChanged(); }
        }
        private string colorName = "";

        public string ColorName
        {
            get { return colorName ; }
            set { colorName = value; NotifyPropertyChanged(); }
        }
        private string colorType;

        public string ColorType
        {
            get { return colorType; }
            set { colorType = value; }
        }

        public EyedropperDataContext(Corel.Interop.VGCore.Application app)
        {
            this.app = app;  
            th = new Thread(new ThreadStart(Update));
            th.IsBackground = true;
            th.Start();
        }
        public void Update()
        {
            dc = this.app.FrameWork.Application.DataContext.GetDataSource("WStatusBarDS");
            while (running)
            {
                try
                {
                    //(103, 396; 21,312 )
                    string coord = (string)dc.GetProperty("CursorCoords");
                    double x = Double.Parse(coord.Substring(1, coord.IndexOf(';') - 1));
                    double y = Double.Parse(coord.Substring(coord.IndexOf(';') + 1, coord.IndexOf(')') - coord.IndexOf(';') - 1));

                    x = this.app.ConvertUnits(x, app.ActiveDocument.Rulers.HUnits, Corel.Interop.VGCore.cdrUnit.cdrInch);
                    y = this.app.ConvertUnits(y, app.ActiveDocument.Rulers.HUnits, Corel.Interop.VGCore.cdrUnit.cdrInch);

                    int xs = 0;
                    int ys = 0;

                    app.ActiveWindow.DocumentToScreen(x, y, out xs, out ys);
                    if (NewPositionEvent != null)
                        NewPositionEvent(new System.Windows.Point(xs, ys));
                    shape = this.app.ActivePage.FindShapeAtPoint(x, y, false);
                    
                    if (shape != null)
                    {
                        this.IsVisibility = System.Windows.Visibility.Visible;
                        Color color = shape.Fill.UniformColor;
                        if(color!=null)
                        {
                            Hex = color.HexValue;
                            ColorName = color.Name;
                            ColorType = color.Type.ToString();
                            //R = color.RGBRed.ToString();
                            //G = color.RGBGreen.ToString();
                            //B = color.RGBBlue.ToString();
                            
                        }
                    }
                    else
                        this.IsVisibility = System.Windows.Visibility.Collapsed;

                    Debug.WriteLine("{0}-{1}",x,y);
                    Thread.Sleep(100);
                }
                catch (Exception e) { Debug.WriteLine(e.Message); }
            }

        }
    }
}
