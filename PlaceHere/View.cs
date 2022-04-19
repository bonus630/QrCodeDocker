using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Threading;
using c = Corel.Interop.VGCore;
using A = br.corp.bonus630.plugin.PlaceHere.Anchor;
using br.corp.bonus630.ImageRender;
using br.corp.bonus630.plugin.PlaceHere.Lang;

namespace br.corp.bonus630.plugin.PlaceHere
{
    public partial class View : Form
    {
        public Point ClickedPoint { get; set; }
        private bool isClicked = false;
        private int positionX = 0, positionY = 0, width = 100, height = 100;
        double cPositionX = 0, cPositionY = 0, cW = 1, cH = 1;
        c.Application app;
        PlaceHereCore core;

        private Thread mainDrawThread;
        private bool running = true;

        public double QRSize { get; set; }
        private int screenQRSize = 0;
        private int screenSup = 0;
        public string QrCodeText { get; set; }
        private c.Shape shapeInPoint = null;
        public c.Shape ShapeInPoint { get { return shapeInPoint; } }

        private Pen greenPen = new Pen(Color.Green);
        private Rectangle ScreenRect;
        public Rectangle ResultRect { get; protected set; }
        private Rectangle escRect;
        public IImageRender Render { get; set; }
        public bool GetContainer { get; set; }
        public bool IsClicked { get { return isClicked; } }
        public double FactorX { get; set; }
        public double FactorY { get; set; }
        public Anchor ReferencePoint { get; set; }
        public Ilang Lang { get; set; }

        public View(c.Application app,PlaceHereCore vc)
        {
            this.app = app;
            this.core = vc;
           
            InitializeComponent();
            escRect = new Rectangle(this.Left + 20, this.Bottom - 40, 80, 20);
            mainDrawThread = new Thread(update);
            mainDrawThread.IsBackground = true;
            mainDrawThread.Start();
        }
        public void SetSizes(int w, int h)
        {
            width = w;
            height = h;
        }
        private void update()
        {
            int w, h;
            while (running)
            {
                try
                {
                    positionX = MousePosition.X - this.Left;
                    positionY = MousePosition.Y - this.Top;
                    app.ActiveWindow.ScreenToDocument(MousePosition.X, MousePosition.Y, out cPositionX, out cPositionY);
                    app.ActiveWindow.DocumentToScreen(cPositionX + QRSize, cPositionY + QRSize, out screenQRSize, out screenSup);

                    screenQRSize -= MousePosition.X;
                    w = screenQRSize;
                    h = screenQRSize;
                    if (GetContainer)
                    {

                        shapeInPoint = core.FindShape(cPositionX, cPositionY);

                        if (shapeInPoint != null && shapeInPoint.SizeWidth >0 && shapeInPoint.SizeHeight > 0)
                        {

                            MakeShapeStuffs(shapeInPoint);
                            w = width;
                            h = height;
                        }
                        else
                        {
                         
                            positionX = (int)(positionX - screenQRSize * FactorX);
                            positionY = (int)(positionY + screenQRSize * FactorY);

                        }

                    }
                    else
                    {
                        // positionX = (int)(positionX - width * FactorX);
                        //positionY = (int)(positionY + height * FactorY);
                       // width = (int)QRSize;
                        // height = (int)QRSize;
                        positionX = (int)(positionX - screenQRSize * FactorX);
                        positionY = (int)(positionY + screenQRSize * FactorY);
                       
                    }
                    //ScreenRect = new Rectangle(positionX, positionY, width, height);
                    ScreenRect = new Rectangle(positionX, positionY, w, h);
                    Invalidate();
                    Thread.Sleep(10);
                }
                catch { }
            }
        }
    
        private void MakeShapeStuffs(c.Shape shape)
        {
            try
            {
                app.ActiveWindow.DocumentToScreen(shape.LeftX, shape.TopY, out positionX, out positionY);
                app.ActiveWindow.DocumentToScreen(shape.RightX, shape.BottomY, out width, out height);
                width -= positionX;
                height -= positionY;
                int Size = screenQRSize;
                if (width > height)
                    //width = height;
                    Size = height;
                else
                    // height = width;
                    Size = width;
                
                switch (ReferencePoint)
                {
                    case A.Center:
                        if (width > Size)
                            positionX = positionX + (width / 2) - (Size / 2);
                        if (height > Size)
                            positionY = positionY + (height / 2) - (Size / 2);
                        break;
                    case A.TopMiddle:
                        if (width > Size)
                            positionX = positionX + (width / 2) - (Size / 2);
                        break;
                    case A.TopRight:
                        if (width > Size)
                            positionX = positionX + (width) - (Size);
                        break;
                    case A.MiddleLeft:

                        if (height > Size)
                            positionY = positionY + (height / 2) - (Size / 2);
                        break;
                    case A.MiddleRight:
                        if (width > Size)
                            positionX = positionX + (width) - (Size);
                        if (height > Size)
                            positionY = positionY + (height / 2) - (Size / 2);
                        break;
                    case A.BottonLeft:

                        if (height > Size)
                            positionY = positionY + (height) - (Size);
                        break;
                    case A.BottonMiddle:
                        if (width > Size)
                            positionX = positionX + (width / 2) - (Size / 2);
                        if (height > Size)
                            positionY = positionY + (height) - (Size);
                        break;
                    case A.BottonRight:
                        if (width > Size)
                            positionX = positionX + (width) - (Size);
                        if (height > Size)
                            positionY = positionY + (height) - (Size);
                        break;
                }

                positionX -= this.Left;
                positionY -= this.Top;
                width = Size;
                height = Size;
            }
            catch { }
        }






        //Vamos usar o click do mouse somente quando não tiver nenhum shape no range do ponto
        protected override void OnClick(EventArgs e)
        {
            isClicked = true;
            ClickedPoint = MousePosition;
            core.Draw(ClickedPoint, ScreenRect);
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            
           
        }
        protected override void OnKeyUp(KeyEventArgs e)
        {

            base.OnKeyUp(e);
            if (e.KeyCode.Equals(Keys.Escape))
                this.Close();
        }
        //protected override void OnMouseMove(MouseEventArgs e)
        //{
        //    positionX = e.X;
        //    positionY = e.Y;
        //    Invalidate();
        //}
        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
           this.Close();
        }
        protected override void OnClosed(EventArgs e)
        {
            this.running = false;
            try
            {
                mainDrawThread.Abort();
            }
            catch { }
            base.OnClosed(e);
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;


            // graphics.DrawRectangle(redPen, ScreenRect);
         
            Bitmap qrBitmap = Render.RenderWireframeToMemory(QrCodeText);
            
            graphics.DrawImage(qrBitmap, ScreenRect);

            graphics.FillEllipse(Brushes.Green, MousePosition.X - 5 - this.Left, MousePosition.Y - 5 - this.Top, 10, 10);
            graphics.DrawLine(greenPen, MousePosition.X - this.Left, this.Height, MousePosition.X - this.Left, 0);
            graphics.DrawLine(greenPen, 0, MousePosition.Y - this.Top, this.Width, MousePosition.Y - this.Top);
            graphics.DrawString(Lang.OC_MSG_Exit, SystemFonts.DefaultFont, Brushes.Red, this.Left + 2, this.Top + 2);
            //graphics.DrawLine(redPen, ScreenRect.X, ScreenRect.Y, ScreenRect.Right, ScreenRect.Bottom);
            //graphics.DrawLine(redPen, ScreenRect.X, ScreenRect.Bottom, ScreenRect.Right, ScreenRect.Y);
        }
    }
}
