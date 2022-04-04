using br.corp.bonus630.ImageRender;
using br.corp.bonus630.PluginLoader;
using Corel.Interop.VGCore;
using System;


namespace br.corp.bonus630.QrCodeDocker
{
    public enum DotShape
    {
            Square,
            Ellipse,
            Triangle,
            Star
    }
    public class QrCodeGenerator : ICodeGenerator
    {
        
        private Application app;
        IImageRender imageRender;
        private bool weld = true;
        public bool Weld {
            get { return this.weld; }
            set { this.weld = value; } }
        private bool noBorder;

        public bool NoBorder
        {
            get { return noBorder; }
            set { noBorder = value; }
        }

        private Color borderColor; 
        public Color BorderColor { get { return borderColor; } set { borderColor = value; } }
        private Color dotFillColor;
        public Color DotFillColor { get { return dotFillColor; } set { dotFillColor = value; } }
        private Color dotOutlineColor;
        public Color DotOutlineColor { get { return dotOutlineColor; } set { dotOutlineColor = value; } }
        private double dotOutlineWidth = 0;
        public double DotBorderSize { get { return dotOutlineWidth; } set { dotOutlineWidth = value; } }
        private DotShape dotShapeType = DotShape.Square;
        public DotShape DotShapeType     {            get { return dotShapeType ; }            set { dotShapeType =  value; }        }

        public IImageRender ImageRender { get { return this.imageRender; } }

        public QrCodeGenerator(Application app)
        {
            this.app = app;
            borderColor = app.CreateColor();
            borderColor.CMYKAssign(0, 0, 0, 0);
            dotFillColor = app.CreateColor();
            dotFillColor.CMYKAssign(0, 0, 0, 100);
            dotOutlineColor = dotFillColor;
        }
        public QrCodeGenerator() { }
        public void SetRender(IImageRender render)
        {
            imageRender = render;
        }
        public Shape CreateVetorLocal(Layer layer, string content, double strSize, double positionX = 0, double positionY = 0, string vectorName = "QrCode Vetor")
        {
            
            this.app.ActiveDocument.Unit = this.app.ActiveDocument.Rulers.HUnits;
            Layer layerTemp = app.ActiveDocument.ActivePage.CreateLayer("temp_layer");
            layerTemp.Activate();
            
            double m_Padding = 0.0;
            imageRender.EncodeNewBitMatrix(content, (int)strSize);
            double dotSize = imageRender.InMeasure((int)strSize);

            ShapeRange shapeRange = app.CreateShapeRange();

            for (int j = 0; j < imageRender.BitMatrixProp.Width; j++)
            {
                for (int i = 0; i < imageRender.BitMatrixProp.Width; i++)
                {
                    if (imageRender.BitMatrixProp[i, j])
                    {
                        Shape dot;
                        switch(dotShapeType)
                        {
                            case DotShape.Square:
                                dot = this.app.ActiveDocument.ActiveLayer.CreateRectangle2(i * dotSize + m_Padding, j * dotSize + m_Padding, dotSize, dotSize);
                            break;
                            case DotShape.Ellipse:
                                dot = this.app.ActiveDocument.ActiveLayer.CreateEllipse(i * dotSize + m_Padding, j * dotSize + m_Padding, (i * dotSize + m_Padding) + dotSize, (j * dotSize + m_Padding) + dotSize);
                            break;
                            case DotShape.Triangle:
                                dot = this.app.ActiveDocument.ActiveLayer.CreatePolygon(i * dotSize + m_Padding, j * dotSize + m_Padding, (i * dotSize + m_Padding) + dotSize, (j * dotSize + m_Padding) + dotSize,6);

                                break;
                            case DotShape.Star:
                                dot = this.app.ActiveDocument.ActiveLayer.CreatePolygon(i * dotSize + m_Padding, j * dotSize + m_Padding, (i * dotSize + m_Padding) + dotSize, (j * dotSize + m_Padding) + dotSize,5,1,1,true,8);

                                break;
                            default:
                                dot = this.app.ActiveDocument.ActiveLayer.CreateRectangle2(i * dotSize + m_Padding, j * dotSize + m_Padding, dotSize, dotSize);
                            break;

                        }
                        dot.Fill.ApplyUniformFill(this.dotFillColor);
                        if (dotOutlineColor == null || dotOutlineWidth == 0)
                            dot.Outline.SetNoOutline();
                        else
                        {
                            dot.Outline.Color = this.dotOutlineColor;
                            dot.Outline.Width = this.dotOutlineWidth;
                        }
                        shapeRange.Add(dot);
                    }
                }
            }
            
            shapeRange.CreateSelection();
            Shape qr = this.app.ActiveSelection;
            Shape qrCodeShape;
            if (weld)
                qrCodeShape = this.app.ActiveSelection.Weld(qr);
            else
                qrCodeShape = this.app.ActiveSelection.Group();
            qrCodeShape.Flip(cdrFlipAxes.cdrFlipVertical);
            Shape g = null;
            if (!noBorder)
            {
                Shape border = layerTemp.CreateRectangle2(0, 0, strSize, strSize);
                border.Fill.ApplyUniformFill(borderColor);
                border.Outline.SetNoOutline();

                border.OrderToBack();
                border.AddToSelection();
                qrCodeShape.AddToSelection();
                this.app.ActiveSelection.AlignToPoint(cdrAlignType.cdrAlignHCenter, border.SizeWidth / 2, border.SizeHeight / 2);
                this.app.ActiveSelection.AlignToPoint(cdrAlignType.cdrAlignVCenter, border.SizeWidth / 2, border.SizeHeight / 2);
                g = this.app.ActiveSelection.Group();
            }
            else
                g = qrCodeShape;
            
            
            g.MoveToLayer(layer);
            layerTemp.Delete();
        
            return g;
        }
        public Shape CreateVetorLocal2(Layer layer, string content, double strSize,double positionX = 0,double positionY =0, string vectorName = "QrCode Vetor")
        {
           this.app.ActiveDocument.Unit = this.app.ActiveDocument.Rulers.HUnits;
           Layer layerTemp = app.ActiveDocument.ActivePage.CreateLayer("temp_layer");
           layerTemp.Activate();
            try
            {
                Shape shape = CreateTempBitmap(content, layer);


                if (shape != null)
                {
                    if (shape.Name == "temp_qrcode.jpg")
                    {

                        double x, y, w, h;
                        shape.GetPosition(out x, out y);
                        shape.GetSize(out w, out h);

                        TraceSettings traceSettings = shape.Bitmap.Trace(cdrTraceType.cdrTraceLineDrawing);
                        traceSettings.DetailLevelPercent = 100;
                        traceSettings.BackgroundRemovalMode = cdrTraceBackgroundMode.cdrTraceBackgroundAutomatic;
                        traceSettings.CornerSmoothness = 0;
                        traceSettings.DeleteOriginalObject = true;
                        traceSettings.RemoveBackground = true;
                        traceSettings.RemoveEntireBackColor = true;
                        traceSettings.RemoveOverlap = true;
                        traceSettings.SetColorCount(2);
                        traceSettings.SetColorMode(cdrColorType.cdrColorGray, cdrPaletteID.cdrCustom);
                        traceSettings.Smoothing = 0;
                        traceSettings.TraceType = cdrTraceType.cdrTraceClipart;
                        traceSettings.Finish();
                        ShapeRange sr = new ShapeRange();
                        foreach (Shape s in layerTemp.Shapes)
                        {
                            sr.Add(s);
                        }
                        sr.CreateSelection();
                        Shape qrShape = this.app.ActiveSelection;
                        Shape cod = this.app.ActiveSelection.Weld(qrShape);
                        cod.Name = "Cod";

                        Color cWhite = new Color();
                        cWhite.CMYKAssign(0, 0, 0, 0);
                        
                        Shape g = null;
                        if (!noBorder)
                        {
                            Shape border = layerTemp.CreateRectangle2(x, y - h, w, h);
                            border.Fill.ApplyUniformFill(cWhite);
                            border.Outline.Width = 0;
                            border.Name = "Borda";
                            border.AddToSelection();
                            cod.AddToSelection();
                            this.app.ActiveSelection.AlignToPoint(cdrAlignType.cdrAlignHCenter, border.SizeWidth / 2, border.SizeHeight / 2);
                            this.app.ActiveSelection.AlignToPoint(cdrAlignType.cdrAlignVCenter, border.SizeWidth / 2, border.SizeHeight / 2);
                            g = this.app.ActiveSelection.Group();
                            g.Shapes[1].OrderToBack();
                        }
                        else
                            g = cod;


                        g.SetSize(strSize, strSize);
                        g.Name = vectorName;
                        layer.Activate();
                        g.MoveToLayer(layer);
                        layerTemp.Delete();
                        
                        g.RemoveFromSelection();

                        return g;
                    }
                }
            }
            catch(Exception erro)
            {
                throw erro;
            }
            return null;
        }
      
        private Shape CreateTempBitmap(string content,Layer layer)
        {
            try
            {
                Shape bitmapCode = null;
                this.app.ActiveDocument.Unit = this.app.ActiveDocument.Rulers.HUnits;

                Layer tempLayer = this.app.ActiveDocument.ActivePage.CreateLayer("temp_qrcode");
                tempLayer.Activate();
                imageRender.SaveTempQrCodeFile(content, this.app.ActivePage.Resolution, 221);

                StructImportOptions sio = new StructImportOptions();
                sio.MaintainLayers = true;
                ImportFilter importFilter = this.app.ActiveLayer.ImportEx(imageRender.QrCodeFilePath);
                importFilter.Finish();

                Shapes shapes = tempLayer.Shapes;
                foreach (Shape shape in shapes)
                {
                    if (shape.Name == "temp_qrcode.jpg")
                    {

                        bitmapCode = shape;
                    }
                }
                if (bitmapCode != null)
                    bitmapCode.MoveToLayer(layer);
                tempLayer.Delete();

                return bitmapCode;
            }
            catch(Exception erro)
            {
                throw new Exception("qrcode import file error");
            }
        }
        public Shape CreateBitmapLocal(Layer layer, string content, double strSize, double positionX = 0, double positionY = 0, string bitmapName = "QrCode Bitmap")
        {
            Shape bitmapShape = CreateTempBitmap(content, layer);
            bitmapShape.SetPosition(positionX, positionY);
            bitmapShape.SetSize(strSize, strSize);
            bitmapShape.Name = bitmapName;
            return bitmapShape;

           
        }

        public string DecodeImage(string filePath)
        {
            try
            {
                using (System.Drawing.Bitmap bitmap = (System.Drawing.Bitmap)System.Drawing.Bitmap.FromFile(filePath))
                {
                    return imageRender.DecodeQrCode(bitmap);
                }
            }
            
            catch(NotImplementedException e)
            {
                
                throw e;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
