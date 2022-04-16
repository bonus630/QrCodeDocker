using br.corp.bonus630.ImageRender;
using br.corp.bonus630.PluginLoader;
using Corel.Interop.VGCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace br.corp.bonus630.plugin.PlaceHere
{
    public class ViewController
    {
        private System.Drawing.Point clickedPoint;
        public Application CorelApp { get; set; }
        public ICodeGenerator CodeGenerator { get; set; }
        public string QrCodeText { get; set; }
        public double FactorX { get; set; }
        public double FactorY { get; set; }
        public double Size { get; set; }
        public Anchor ReferencePoint { get; set; }
        public bool GetContainer { get; set; }
        public event Action<bool> FinishJob;
        public ViewController(Application app)
        {
            this.CorelApp = app;

        }
        private void OnFinishJob(bool sucess)
        {
            if (FinishJob != null)
                FinishJob(sucess);
        }
        public void StartJob()
        {
            try
            {
                int x = 0, y = 0, w = 0, h = 0;

                //CorelApp.Unit = cdrUnit.cdrPixel;

                CorelApp.FrameWork.Automation.GetItemScreenRect("ab303a90-464d-5191-423f-613c4d1dcb2c", "1cd5b342-3211-24af-486d-55bb329594f8", out x, out y, out w, out h);

                View view = new View(this.CorelApp,this);
                view.Render = CodeGenerator.ImageRender;
                view.QrCodeText = QrCodeText;
                view.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
                view.Left = (int)x;
                view.Width = (int)w;
                view.Top = (int)y;
                view.Height = (int)h;
                view.FactorX = FactorX;
                view.FactorY = FactorY;
                view.ReferencePoint = ReferencePoint;


                // double distance = CorelApp.ActiveDocument.FromUnits(Size, CorelApp.ActiveDocument.Rulers.HUnits);
                view.QRSize = Size;
                //CorelApp.ActiveWindow.ScreenDistanceToDocumentDistance(distance);
                view.GetContainer = GetContainer;
               // view.FormClosing += View_FormClosing;
                view.Show();
            }
            catch { OnFinishJob(false); }


        }
        public void Draw(System.Drawing.Point screenP,System.Drawing.Rectangle screenRect)
        {
            try
            {
                CorelApp.Optimization = true;
                CorelApp.EventsEnabled = false;
                CorelApp.ActiveDocument.BeginCommandGroup();
                //Debug.WriteLine((sender as View).ClickedPoint.ToString());
                //System.Drawing.Point screenP = (sender as View).ClickedPoint;
                //System.Drawing.Rectangle screenRect = (sender as View).ScreenRect;
                double x, y, w, h;

                CorelApp.ActiveDocument.ActiveWindow.ScreenToDocument(screenP.X, screenP.Y, out x, out y);

                w = CorelApp.ActiveDocument.ActiveWindow.ScreenDistanceToDocumentDistance(screenRect.Width);
                h = CorelApp.ActiveDocument.ActiveWindow.ScreenDistanceToDocumentDistance(screenRect.Height);

                // CorelApp.ActiveLayer.CreateRectangle2(x, y, w, h);
                Shape code = CodeGenerator.CreateVetorLocal2(CorelApp.ActiveLayer, QrCodeText, w, x, y);




                if (GetContainer)
                {

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
                OnFinishJob(true);
            }
            catch { OnFinishJob(false); }
            finally
            {
                CorelApp.EventsEnabled = true;
                CorelApp.ActiveDocument.EndCommandGroup();
                CorelApp.Optimization = false;
                CorelApp.Refresh();
            }

        
    }
        private void View_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            if ((sender as View).IsClicked)
            {
            }
                
        }
    }
}
