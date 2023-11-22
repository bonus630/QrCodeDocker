using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using Corel.Interop.VGCore;

namespace br.corp.bonus630.plugin.PlaceHere
{
    public class PreviewDataContext : NotifyPropertyBase
    {
        Corel.Interop.VGCore.Application corelApp;
        Thread th;
        Corel.Interop.VGCore.DataSourceProxy dc;
        public event Action<System.Drawing.Rectangle> NewPositionEvent;
        public double Size { get; set; }
        public bool GetContainer { get; set; }
        private bool running = true;
        private Shape shape;
        //private Curve osCurve;
        //OnScreenCurve os;


        public void Close()
        {
            running = false;
        }
    
     
        public System.Windows.Visibility isVisibility = System.Windows.Visibility.Collapsed;
        public System.Windows.Visibility IsVisibility
        {
            get { return this.isVisibility; }
            set { this.isVisibility = value; NotifyPropertyChanged(); }
        }
      

        public PreviewDataContext(Corel.Interop.VGCore.Application app)
        {
            this.corelApp = app;  
            //th = new Thread(new ThreadStart(Update));
            //th.IsBackground = true;
            
        }
        public void Start()
        {
            th.Start();
        }
        //public void SetCurve(Curve curve)
        //{
        //     os = app.CreateOnScreenCurve();
        //     os.SetCurve(curve);
        //}
        public void Update()
        {
            dc = this.corelApp.FrameWork.Application.DataContext.GetDataSource("WStatusBarDS");
           
            corelApp.Unit = corelApp.ActiveDocument.Unit;
            while (running)
            {
                
                
                try
                {
                    //(103, 396; 21,312 )
                    string coord = (string)dc.GetProperty("CursorCoords");
                    double inBarX = Double.Parse(coord.Substring(1, coord.IndexOf(';') - 1));
                    double inBarY = Double.Parse(coord.Substring(coord.IndexOf(';') + 1, coord.IndexOf(')') - coord.IndexOf(';') - 1));
                    double size = this.Size;
                    //double convertedX = this.app.ConvertUnits(inBarX, app.ActiveDocument.Rulers.HUnits, Corel.Interop.VGCore.cdrUnit.cdrInch);
                    //double convertedY = this.app.ConvertUnits(inBarY, app.ActiveDocument.Rulers.HUnits, Corel.Interop.VGCore.cdrUnit.cdrInch);
                 
                    double x = inBarX;
                    double y = inBarY;
#if !X7
                    size = corelApp.ActiveWindow.ScreenDistanceToDocumentDistance(size);


#endif

                    if (GetContainer)
                    {
#if X7

            Shape shapes = corelApp.ActiveDocument.ActivePage.SelectShapesAtPoint(inBarX, inBarY, false);
            Shape shape = shapes.Shapes[1];
            for (int i = 1; i <= shapes.Shapes.Count; i++)
            {
                if (shape.ZOrder > shapes.Shapes[i].ZOrder)
                    shape = shapes.Shapes[i];
            }
            

#else
                        Shape shape = this.corelApp.ActiveDocument.ActivePage.FindShapeAtPoint(inBarX, inBarY, false);
#endif
                       
                        if(shape != null)
                        {

                            x = shape.LeftX;
                            y = shape.TopY;
                            Debug.WriteLine("ShapeX:{0} - ShapeY:{1}", inBarX,inBarY);
                            if (shape.SizeWidth > shape.SizeHeight)
                                size = shape.SizeHeight;
                            if (shape.SizeHeight > shape.SizeWidth)
                                size = shape.SizeWidth;
                            //size = app.ActiveWindow.DocumentDistanceToScreenDistance(size);
                        }
                    }
                   // os.SetRectangle(x, y, x + size, y + size);
                    int xs = 0;
                    int ys = 0;

                    corelApp.ActiveWindow.DocumentToScreen(x, y, out xs, out ys);
                    int ws = (int)((inBarX + size) - inBarX);
                    int hs = (int)((inBarY + size) - inBarY);
                    //app.ActiveWindow.DocumentToScreen((double)x+Size,(double)y-Size,out xs,out ys);

                    System.Drawing.Rectangle rect = new System.Drawing.Rectangle(xs, ys, ws, hs);
                    Debug.WriteLine("Rect:{0}", rect.ToString());
                    if (NewPositionEvent != null)

                        NewPositionEvent(rect);

                    this.IsVisibility = System.Windows.Visibility.Visible;
                    //if (shape != null)
                    //{
                    //}
                    //else
                    //    this.IsVisibility = System.Windows.Visibility.Collapsed;
                    //int s = 100;
                    //double sn = app.ActiveWindow.ScreenDistanceToDocumentDistance(s);
                    //double sj = app.ActiveWindow.DocumentDistanceToScreenDistance(sn);

                    //Debug.WriteLine("Size:{0} - ScreenToDocument:{1} - DocumentToScreen:{2}", s, sn, sj);
                    //Debug.WriteLine("x:{0}-y{1}-ws:{2}-ys:{3}-Unit:{4}",(int)x,(int)y,ws,ys,app.ActiveDocument.Unit);
                    Thread.Sleep(100);
                }
                catch (Exception e) { Debug.WriteLine(e.Message); }
            }

        }
    }
}
