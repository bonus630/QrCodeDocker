using br.corp.bonus630.plugin.PlaceHere.Lang;
using br.corp.bonus630.PluginLoader;
using Corel.Interop.VGCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace br.corp.bonus630.plugin.PlaceHere
{
    public class PlaceHereCorecs : IPluginCore, IPluginDrawer
    {
        public const string PluginDisplayName = "Place Here";
        public Application corelApp;
        ICodeGenerator codeGenerator;
        public Ilang Lang { get; set; }
        //private int DSCursor { private get { return dsCursor; } set { this.dsCursor = value; } }
        int dsCursor = 0;
        public double FactorX
        {
            get { return factorX; }
            set
            {
                factorX = value;
                setViewConfig();
            }
        }
        public double FactorY { get { return factorY; } set { factorY = value; setViewConfig(); } }
        public Anchor ReferencePoint { get { return referencePoint; } set { referencePoint = value; setViewConfig(); } }
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
        public event Action<bool> ViewFinishJob;
        public event Action<int> ProgressChange;
        public event Action UpdatePreview;

        public Action DrawAction;
        private View view;
        //private ICUITaskManager taskManager;

        cdrUnit prevUnit = cdrUnit.cdrInch;
        private double factorX;
        private double factorY;
        private Anchor referencePoint;
        private bool getContainer;

        public bool GetContainer { get { return getContainer; } set { getContainer = value; setViewConfig(); } }
        public PlaceHereCorecs(Application app)
        {
            //DrawAction = new Action(drawFunction);
            this.corelApp = app;
           
        }



        public void Draw(bool restart = false)
        {
            try
            {
                if (view == null || restart)
                {
                    dsCursor = 0;
                    StartJob();
                }
                if (dsCursor >= dataSource.Count)
                {
                    view.Close();
                    view = null;
                    return;
                }
                else
                {

                    view.QrCodeText = this.dataSource[dsCursor][0].ToString();
                    Debug.WriteLine(view.QrCodeText, "Core");
                    dsCursor++;
                }


            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show(e.Message);
            }
        }

        private void drawFunction()
        {
            Shape code = null;
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
        public Shape FindShape(double x, double y)
        {
            Shape shape = null;
#if X7
            Shape shapes = app.ActiveDocument.ActivePage.SelectShapesAtPoint(x, y, false);
            shape = shapes.Shapes[1];
            for (int i = 1; i <= shapes.Shapes.Count; i++)
            {
                if (shape.ZOrder > shapes.Shapes[i].ZOrder)
                    shape = shapes.Shapes[i];
            }
#else
            shape = corelApp.ActiveDocument.ActivePage.FindShapeAtPoint(x, y);
#endif
            return shape;

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
        private void OnViewFinishJob(bool sucess)
        {
            if (ViewFinishJob != null)
                ViewFinishJob(sucess);
        }
        private void StartJob()
        {
            try
            {
                if (corelApp.ActiveDocument == null)
                    return;
                else
                    corelApp.ActiveDocument.Unit = corelApp.ActiveDocument.Rulers.HUnits;
                int x = 0, y = 0, w = 0, h = 0;

                //CorelApp.Unit = cdrUnit.cdrPixel;

                corelApp.FrameWork.Automation.GetItemScreenRect("ab303a90-464d-5191-423f-613c4d1dcb2c", "1cd5b342-3211-24af-486d-55bb329594f8", out x, out y, out w, out h);

                view = new View(this.corelApp,this);
                view.Render = codeGenerator.ImageRender;
                view.Left = (int)x;
                view.Width = (int)w;
                view.Top = (int)y;
                view.Height = (int)h;
                view.Lang = Lang;
                setViewConfig();
                view.QRSize = Size;

                //CorelApp.ActiveWindow.ScreenDistanceToDocumentDistance(distance);

                // view.FormClosing += View_FormClosing;;
                view.Show();
            }
            catch { OnFinishJob(false); }


        }
        private void setViewConfig()
        {
            if (view != null)
            {
                view.FactorX = FactorX;
                view.FactorY = FactorY;
                view.ReferencePoint = ReferencePoint;
                view.GetContainer = GetContainer;
            }
        }
        public void Draw(System.Drawing.Point screenP, System.Drawing.Rectangle screenRect)
        {
            try
            {
                corelApp.Optimization = true;
                corelApp.EventsEnabled = false;
                corelApp.ActiveDocument.Unit = corelApp.ActiveDocument.Rulers.HUnits;
                corelApp.ActiveDocument.BeginCommandGroup();
                //Debug.WriteLine((sender as View).ClickedPoint.ToString());
                //System.Drawing.Point screenP = (sender as View).ClickedPoint;
                //System.Drawing.Rectangle screenRect = (sender as View).ScreenRect;
                double x = 0, y = 0, w = 0, h = 0;

                corelApp.ActiveDocument.ActiveWindow.ScreenToDocument(screenP.X, screenP.Y, out x, out y);
                //corelApp.ActiveWindow.ScreenToDocument(screenRect.Right, screenRect.Bottom, out w, out h);

                //w = corelApp.ActiveDocument.ActiveWindow.ScreenDistanceToDocumentDistance(screenRect.Width);
                //h = corelApp.ActiveDocument.ActiveWindow.ScreenDistanceToDocumentDistance(screenRect.Height);
                w = this.Size;
                h = this.Size;

                double Size = this.Size;


                if (GetContainer)
                {
                    Shape shape = FindShape(x, y);
                    if (shape != null)
                    {
                        x = shape.LeftX;
                        y = shape.TopY;
                        w = shape.SizeWidth;
                        h = shape.SizeHeight;

                        if (w > h)
                            Size = h;
                        else
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
                }
                else
                {
                    x = x - w * FactorX;
                    y = y - h * FactorY;
                }
                Shape code = codeGenerator.CreateVetorLocal(corelApp.ActiveLayer, view.QrCodeText, w, x, y);

                code.SetPosition(x, y);
                code.SetSize(Size, Size);
                Draw(false);
                OnFinishJob(true);
            }
            catch (Exception err) { System.Windows.MessageBox.Show(err.Message); OnFinishJob(false); }
            finally
            {
                corelApp.EventsEnabled = true;
                corelApp.ActiveDocument.EndCommandGroup();
                corelApp.Optimization = false;
                corelApp.Refresh();
            }


        }
        public void CheckAndRestart()
        {

        }
        private void View_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            CheckAndRestart();
        }

        public void Draw()
        {

        }
    }
}


