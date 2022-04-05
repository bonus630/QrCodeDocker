using br.corp.bonus630.PluginLoader;
using Corel.Interop.VGCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using c = Corel.Interop.VGCore;
namespace br.corp.bonus630.plugin.PlaceHere
{
    public class PlaceHereCorecs : IPluginCore, IPluginDrawer
    {
        public const string PluginDisplayName = "Place Here";
        c.Application corelApp;
        ICodeGenerator codeGenerator;
        public int DSCursor { set { this.dsCursor = value; } }
        int dsCursor = 0;
        public double FactorX { get; set; }
        public double FactorY { get; set; }
        public Anchor ReferencePoint { get; set; }
        private List<object[]> dataSource;
        public List<object[]> DataSource
        {
            set
            {
                this.dataSource = value;

            }
        }
        private Rect[] points;

        public double Size { get; set; }

        public Corel.Interop.VGCore.Application App { set { this.corelApp = value; } }
        public ICodeGenerator CodeGenerator { set { this.codeGenerator = value; } }

        public event Action<object> FinishJob;
        public event Action<int> ProgressChange;
        public Action DrawAction;
        //private ICUITaskManager taskManager;

        cdrUnit prevUnit = cdrUnit.cdrInch;
        public bool GetContainer { get; set; }
        public PlaceHereCorecs()
        {
            DrawAction = new Action(drawFunction);
        }
        public void Draw()
        {
            try
            {
                dataSource = new List<object[]>();
                dataSource.Add(new object[] { "square" });
                dataSource.Add(new object[] { "Portrait" });
                dataSource.Add(new object[] { "Landscape" });
                corelApp.Unit = cdrUnit.cdrMillimeter;
                corelApp.ActiveDocument.Unit = cdrUnit.cdrMillimeter;
                points = new Rect[dataSource.Count];
           
                double x = 0, y = 0, w = 0, h = 0;
                int s = 0;
                for (int i = 0; i < dataSource.Count; i++)
                {


                    corelApp.ActiveDocument.GetUserClick(out x, out y, out s, 0, true, c.cdrCursorShape.cdrCursorSmallcrosshair);

                    if (GetContainer)
                    {
#if X7

                        Shape shapes = app.ActiveDocument.ActivePage.SelectShapesAtPoint(x, y, false);
                        Shape shape = shapes.Shapes[1];
                        for (int i = 1; i <= shapes.Shapes.Count; i++)
                        {
                            if (shape.ZOrder > shapes.Shapes[i].ZOrder)
                                shape = shapes.Shapes[i];
                        }
            

#else
                        Shape shape = corelApp.ActiveDocument.ActivePage.FindShapeAtPoint(x, y);
#endif
                        // corelApp.ActiveDocument.PreserveSelection = preservSelection;
                        if (shape == null)
                            return;
                        x = shape.LeftX;
                        y = shape.TopY;
                        w = shape.SizeWidth;
                        h = shape.SizeHeight;


                    }


                    points[i] = corelApp.CreateRect(x, y, w, h);
                    
                }
                drawFunction();
             

            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show(e.Message);
            }
            finally
            {
                if (corelApp.ActiveDocument != null)
                    corelApp.ActiveDocument.EndCommandGroup();
                corelApp.Optimization = false;
                corelApp.EventsEnabled = true;
                corelApp.Refresh();
            }

        }

        private void drawFunction()
        {
            c.Shape code = null;
            double x = 0, y = 0;
            int s = 0;
            corelApp.Optimization = true;
            corelApp.EventsEnabled = false;
            
            corelApp.ActiveDocument.BeginCommandGroup();
            //if (corelApp.Unit != prevUnit)
            //    corelApp.Unit = prevUnit;
            for (int i = 0; i < dataSource.Count; i++)
            {
                if (GetContainer)
                    Size = points[i].Height;
                // corelApp.ActiveDocument.GetUserClick(out x, out y, out s, 0, true, c.cdrCursorShape.cdrCursorSmallcrosshair);
                code = this.codeGenerator.CreateVetorLocal(corelApp.ActiveLayer,
                  dataSource[i][0].ToString()
                  , corelApp.ConvertUnits(Size, corelApp.ActiveDocument.Unit, cdrUnit.cdrMillimeter)
                  , 0, 0, string.Format("QR-{0}", dataSource[dsCursor][0].ToString()));
                Console.WriteLine("Unit: App|{0} Doc|{1}", corelApp.Unit, corelApp.ActiveDocument.Unit);
                double h, w;
                x = points[i].x;
                y = points[i].y;
                w = Size;
                h = Size;
                if (GetContainer)
                {
                    w = points[i].Width;
                    h = points[i].Height;
                    if (w > h)
                        Size = h;
                    if (h > w)
                        Size = w;
                    switch (ReferencePoint)
                    {
                        case Anchor.Center:
                            if (w > Size)
                                x = x + (w / 2) - (Size / 2);
                            if (h > Size)
                                y = y - (h / 2) + (Size / 2);
                            break;
                        case Anchor.TopMiddle:
                            if (w > Size)
                                x = x + (w / 2) - (Size / 2);
                            break;
                        case Anchor.TopRight:
                            if (w > Size)
                                x = x + (w) - (Size);

                            break;
                        case Anchor.MiddleLeft:

                            if (h > Size)
                                y = y - (h / 2) + (Size / 2);
                            break;
                        case Anchor.MiddleRight:
                            if (w > Size)
                                x = x + (w) - (Size);
                            if (h > Size)
                                y = y - (h / 2) + (Size / 2);
                            break;
                        case Anchor.BottonLeft:

                            if (h > Size)
                                y = y - (h) + (Size);
                            break;
                        case Anchor.BottonMiddle:
                            if (w > Size)
                                x = x + (w / 2) - (Size / 2);
                            if (h > Size)
                                y = y - (h) + (Size);
                            break;
                        case Anchor.BottonRight:
                            if (w > Size)
                                x = x + (w) - (Size);
                            if (h > Size)
                                y = y - (h) + (Size);
                            break;
                    }
                }
                else
                {
                    x = x - w * FactorX;
                    y = y - h * FactorY;
                }
                code.SetPosition(x, y);
                code.SetSize(Size, Size);
                
            }
            //corelApp.Refresh();
            corelApp.EventsEnabled = true;
            corelApp.ActiveDocument.EndCommandGroup();
            corelApp.Optimization = false;
            corelApp.Refresh();
        }
        public void OnFinishJob(object obj)
        {
            if (FinishJob != null)
                FinishJob(obj);
        }

        public void OnProgressChange(int progress)
        {
            if (ProgressChange != null)
                ProgressChange(progress);
        }
    }

}
