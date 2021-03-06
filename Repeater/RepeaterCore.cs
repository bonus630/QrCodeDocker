﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using br.corp.bonus630.PluginLoader;
using Corel.Interop.VGCore;
using System.IO;

namespace br.corp.bonus630.plugin.Repeater
{
    public class RepeaterCore :  IPluginCore, IPluginDrawer
    {
        public const string PluginDisplayName = "Simple Repeater";

        double size = 221;
        private Application app;
        //private br.corp.bonus630.ImageRender.IImageRender imageRender;
        private br.corp.bonus630.PluginLoader.ICodeGenerator codeGenerator;
        private ShapeRange modelShape;
        private ItemTuple<Shape> shapeContainer;
        private double leftMargin = 10;
        private double gap = 0.1;
        private List<object[]> dataSource;
        private bool fitToPage = false;
        
        private double startX = 0.1;
        private double startY = 0.1;
        private int enumerator = 0;
        private int enumeratorIncrement = 1;

        public double Gap { set { this.gap = value; } }
        public double StartX { set { this.startX = value; } }
        public double StartY { set { this.startY = value; } }
        public int Enumerator { set { this.enumerator = value; } }
        public int EnumeratorIncrement { set { this.enumeratorIncrement = value; } }
        private int numColumns, numLines;
        private string mask = "0000";

        public string Mask{ set { mask =  value; }
        }



        public bool FitToPage { set { this.fitToPage = value; } }

        public RepeaterCore()
        {

        }
        public double Size
        {
            set { this.size = value; }
        }

        public Application App
        {
            set { this.app = value; }
        }

        //public IImageRender ImageRender
        //{
        //    set { this.imageRender = value; }
        //}
        public ShapeRange ModelShape
        {
            set { this.modelShape = value; }
        }
        public ItemTuple<Shape> ShapeContainer
        {
            set
            {
                if (value == null)
                    return;
                this.shapeContainer = value;
                this.size = shapeContainer.Item.SizeWidth;
            }
        }
        public List<object[]> DataSource { get { return this.dataSource; } set { this.dataSource = value; } }
        Application IPluginDrawer.App { set { this.app = value; } }

        public List<ItemTuple<Shape>> ShapeContainerText { get; internal set; }
        public List<ItemTuple<Shape>> ShapeContainerEnumerator { get; internal set; }
        public List<ItemTuple<Shape>> ShapeContainerImageFile { get; internal set; }
        public ICodeGenerator CodeGenerator { set { this.codeGenerator = value; } }

        public event Action<object> FinishJob;
        public event Action<int> ProgressChange;

        public void Draw()
        {
            
        }

        public void OnFinishJob(object obj)
        {
            if (FinishJob != null)
                FinishJob(obj);
        }

        public void OnProgressChange(int progress)
        {
            int p = (int)100 * progress / this.dataSource.Count;

            if(ProgressChange!=null)
                ProgressChange(p);
        }
        //public double LeftXRelativePage(ShapeRange shape, Page page)
        //{
        //    return shape.LeftX - page.LeftX;
        //}
        //public double TopYRelativePage(ShapeRange shape, Page page)
        //{
        //    return shape.TopY - page.TopY;
        //}
        //public double LeftX(double x, Page page)
        //{
        //    return x + page.LeftX;
        //}
        //public double TopY(double y, Page page)
        //{
        //    return y + page.TopY;
        //}
        //public void ColorDuplicateOrder(List<Color> colors, double margin)
        //{

        //    this.corelApp.ActiveDocument.Unit = this.corelApp.ActiveDocument.Rulers.VUnits;
        //    //double margin = 10;
        //    bool createPage = false;
        //    ShapeRange shapeRange = this.corelApp.ActiveSelectionRange;
        //    ShapeRange pageShapes = this.corelApp.ActivePage.Shapes.All();
        //    pageShapes.RemoveRange(shapeRange);
        //    double x, y, w, h, startX, startY = 0;
        //    int xc = 1;

        //    int yc = 0;
        //    //int pageC = 1;
        //    startX = this.LeftXRelativePage(shapeRange, this.corelApp.ActivePage);
        //    startY = this.TopYRelativePage(shapeRange, this.corelApp.ActivePage);
        //    //shapeRange.GetPosition(out startX, out startY);
        //    shapeRange.GetSize(out w, out h);

        //    Page page = this.corelApp.ActiveDocument.ActivePage;



        //    for (int i = 0; i < colors.Count; i++)
        //    {
        //        shapeRange.AlignAndDistribute(cdrAlignDistributeH.cdrAlignDistributeHAlignCenter, cdrAlignDistributeV.cdrAlignDistributeVNone);
        //        //if (!useOrigin)
        //        //{
        //        x = startX + (w + margin) * xc;
        //        y = startY - (h + margin) * yc;

        //        //x = ((w + margin) * xc);
        //        //y = ((h + margin) * yc);
        //        ////  MessageBox.Show(x.ToString());
        //        if (page.SizeWidth - (x + w) >= w)
        //        {
        //            xc++;

        //        }
        //        else
        //            xc = 0;
        //        if (xc == 0)
        //        {
        //            yc++;
        //            ////MessageBox.Show(string.Format("y:{0} page:{1}",  y - startY - h,this.app.ActiveDocument.ActivePage.SizeHeight));
        //            if (page.SizeHeight + (y - h) < h)
        //            {
        //                createPage = true;
        //                //pageC++;


        //                xc = 0;
        //                yc = 0;
        //            }

        //        }

        //        foreach (Shape item in shapeRange.Shapes)
        //        {
        //            if (item.Name == "cor")
        //                item.Fill.ApplyUniformFill(colors[i]);
        //            if (item.Type == cdrShapeType.cdrTextShape)
        //                item.Text.Contents = colors[i].Name;

        //        }
        //        if (i == colors.Count - 1)
        //            return;
        //        shapeRange = shapeRange.Duplicate();


        //        shapeRange.TopY = this.TopY(y, this.corelApp.ActivePage);
        //        shapeRange.LeftX = this.LeftX(x, this.corelApp.ActivePage);
        //        shapeRange.MoveToLayer(page.ActiveLayer);

        //        if (createPage)
        //        {
        //            page = this.corelApp.ActiveDocument.InsertPages(1, false, this.corelApp.ActivePage.Index);
        //            page.Activate();

        //            pageShapes.Duplicate();
        //            pageShapes.MoveToLayer(page.ActiveLayer);
        //            //foreach (Shape item in pageShapes)
        //            //{
        //            //    item.Duplicate();
        //            //    item.MoveToLayer(page.ActiveLayer);
        //            //}


        //            createPage = false;
        //        }

        //        string pageName = page.Name;

        //    }
        //}
        
       
        public void ProcessVector(int qrcodeContentIndex = -1,bool vector = true)
        {
            try
            {
                //QrCodeGenerator generator = new QrCodeGenerator(app);
                //generator.SetRender(imageRender);
                int colCount = 0;
                int linCount = 0;
                int colMax = 0;
                int linMax = 0;
                double pageWidth = this.app.ActiveDocument.ActivePage.SizeWidth;
                double pageHeight = this.app.ActiveDocument.ActivePage.SizeHeight;

                bool newPage = false;

                colMax = Convert.ToInt32(((pageWidth - startX) - ((pageWidth - startX) % (modelShape.SizeWidth + gap))) / (modelShape.SizeWidth + gap));
                linMax = Convert.ToInt32(((pageHeight - startY) - ((pageHeight - startY) % (modelShape.SizeHeight + gap))) / (modelShape.SizeHeight + gap));
                System.Diagnostics.Debug.WriteLine(string.Format("modelW{0} modelH{1}", modelShape.SizeWidth, modelShape.SizeHeight), "contador");
                System.Diagnostics.Debug.WriteLine(string.Format("W{0} H{1}", ((pageWidth - startX) - ((pageWidth - startX) % (modelShape.SizeWidth + gap))) / (modelShape.SizeWidth + gap), ((pageHeight - startY) - ((pageHeight - startY) % (modelShape.SizeHeight + gap))) / (modelShape.SizeHeight + gap)), "contador");
                int numPerPage = colMax * linMax;
                for (int i = 0; i < dataSource.Count; i++)
                {
                    app.Optimization = true;
                    app.EventsEnabled = false;
                    app.ActiveDocument.BeginCommandGroup("Duplicate");
                    //ShapeRange duplicate = modelShape.Duplicate();
                    Page page = this.app.ActiveDocument.ActivePage;
                    ShapeRange duplicate = modelShape.CopyToLayer(page.ActiveLayer);
                    Shape code = null;
                    if (shapeContainer != null)
                    {
                        if (vector)
                        {
                            code = codeGenerator.CreateVetorLocal(page.ActiveLayer, dataSource[i][qrcodeContentIndex].ToString(), this.size, i * (this.size + 2), 20, dataSource[i][qrcodeContentIndex].ToString());
                            //codeGenerator.ImageRender.EncodeNewBitMatrix(dataSource[i][qrcodeContentIndex].ToString());

                            //code = codeGenerator.CreateVetorLocal(page.ActiveLayer, dataSource[i][qrcodeContentIndex].ToString(), codeGenerator.ImageRender.Measure(), i * (this.size + 2), 20, dataSource[i][qrcodeContentIndex].ToString());
                            //code.SetSize(this.size, this.size);
                            //code = codeGenerator.CreateVetorLocal(page.ActiveLayer, dataSource[i][qrcodeContentIndex].ToString(), (int)app.ConvertUnits(this.size, app.ActiveDocument.Unit, cdrUnit.cdrPixel), i * (this.size + 2), 20, dataSource[i][qrcodeContentIndex].ToString());
                        }
                        else
                            code = codeGenerator.CreateBitmapLocal(page.ActiveLayer, dataSource[i][qrcodeContentIndex].ToString(), (int)app.ConvertUnits(this.size, app.ActiveDocument.Unit, cdrUnit.cdrPixel), i * (this.size + 2), 20, dataSource[i][qrcodeContentIndex].ToString());
                    }
                    foreach (Shape item in duplicate.Shapes)
                    {
                        if (shapeContainer!=null && item.Name == shapeContainer.Item.Name)
                        {
                            code.PositionX = item.PositionX;
                            code.PositionY = item.PositionY;
                            code.SizeHeight = item.SizeHeight;
                            code.SizeWidth = item.SizeWidth;
                            item.Delete();
                            duplicate.Add(code);
                            continue;
                        }
                        ItemTuple<Shape> tuple = this.ShapeContainerText.FirstOrDefault(r => r.Item.Name == item.Name && r.Item.SizeHeight == item.SizeHeight && r.Item.SizeWidth == item.SizeWidth);
                        if (tuple != null)
                        {
                            item.Text.Story.Replace(this.dataSource[i][tuple.Index].ToString());
                        }
                        tuple = null;
                        tuple = this.ShapeContainerEnumerator.FirstOrDefault(r => r.Item.Name == item.Name && r.Item.SizeHeight == item.SizeHeight && r.Item.SizeWidth == item.SizeWidth);
                        if (tuple != null)
                        {
                            item.Text.Story.Replace(this.enumerator.ToString(mask));
                            this.enumerator += this.enumeratorIncrement;
                        }
                        tuple = null;
                        tuple = this.ShapeContainerImageFile.FirstOrDefault(r => r.Item.Name == item.Name && r.Item.SizeHeight == item.SizeHeight && r.Item.SizeWidth == item.SizeWidth);
                        if (tuple != null)
                        {
                            //code.PositionX = item.PositionX;
                            //code.PositionY = item.PositionY;
                            //code.SizeHeight = item.SizeHeight;
                            //code.SizeWidth = item.SizeWidth;
                            Shape image = ImportImageFile(page.ActiveLayer, this.dataSource[i][tuple.Index].ToString(), (int)item.SizeWidth, (int)item.SizeHeight);
                            if (image == null)
                                continue;
                            image.PositionX = item.PositionX;
                            image.PositionY = item.PositionY;
                            item.Delete();
                            duplicate.Add(image);
                            continue;
                        }
                    }

                    if (fitToPage)
                    {

                        System.Diagnostics.Debug.WriteLine(string.Format("col{0} lin{1} page{2} | i{3}", colCount, linCount, numPerPage, i), "contador");

                        double posX = startX + page.LeftX + (duplicate.SizeWidth + gap) * colCount;
                        double posY = -startY + page.TopY - (duplicate.SizeHeight + gap) * linCount;
                        duplicate.PositionX = posX;
                        duplicate.PositionY = posY;
                        numPerPage--;

                        if (colCount == colMax - 1)
                        {
                            colCount = 0;
                            linCount++;

                        }
                        else
                        {
                            colCount++;

                        }
                        if (linCount == linMax)
                        {
                            linCount = 0;
                        }

                        if (numPerPage == 0 && i < dataSource.Count - 1)
                        {
                            newPage = true;
                            numPerPage = colMax * linMax;
                        }
                        else
                            newPage = false;




                        //new page, start zero again
                        if (newPage)
                        {
                            page = this.app.ActiveDocument.InsertPages(1, false, this.app.ActivePage.Index);
                            page.Activate();

                        }
                        
                    }
                    else
                    {
                        duplicate.PositionX = (duplicate.SizeWidth + leftMargin) * (i + 1);
                        duplicate.PositionY = duplicate.PositionY;
                    }

                   
                    OnProgressChange(i);
                }
                app.Optimization = false;
                app.EventsEnabled = true;
                app.ActiveDocument.EndCommandGroup();
                app.Refresh();
            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message, "Debug");
                System.Diagnostics.Debug.WriteLine(e.Source, "Source");
            }
            finally
            {
                app.Optimization = false;
                app.EventsEnabled = true;
                app.ActiveDocument.EndCommandGroup();
                app.Refresh();
               
            }
            OnFinishJob(null);
        }
        
        private Shape ImportImageFile(Layer layer,string imagePath, int width, int height)
        {
            if (!File.Exists(imagePath))
                return null;
            try
            {
                Layer tempLayer = app.ActiveDocument.ActivePage.CreateLayer("temp_image120");
                StructImportOptions sio = new StructImportOptions();
                sio.ResampleWidth = width;
                sio.ResampleHeight = height;

                sio.MaintainLayers = true;
                ImportFilter importFilter = tempLayer.ImportEx(imagePath);
                importFilter.Finish();
                Shape s = null;
                if (tempLayer.Shapes.Count > 0)
                    s = tempLayer.Shapes[1];
                s.MoveToLayer(layer);
                tempLayer.Delete();
                s.SizeWidth = width;
                s.SizeHeight = height;
                return s;
            }
            catch
            {
                return null;
            }

        }
       
    }
}
