using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using br.corp.bonus630.PluginLoader;
using Corel.Interop.VGCore;
using System.IO;

namespace br.corp.bonus630.plugin.Repeater
{
    public class RepeaterCore :  PluginCoreBase<RepeaterCore>, IPluginDrawer
    {
        public const string PluginDisplayName = "Simple Repeater";

        double size = 221;
        private Application app;
     
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

        public double Gap { get { return this.gap; } set { this.gap = value; } }
        public double StartX { get { return this.startX; } set { this.startX = value; } }
        public double StartY { get { return this.startY; } set { this.startY = value; } }
        public int Enumerator { get { return this.enumerator; } set { this.enumerator = value; } }
        public int EnumeratorIncrement { get { return this.enumeratorIncrement; } set { this.enumeratorIncrement = value; } }
        private int numColumns, numLines;
        private string mask = "0000";

        public string Mask{get { return mask; } set { mask =  value; }
        }
        public bool QrFormatVector { get; set; }


        public bool FitToPage { get {return this.fitToPage; } set { this.fitToPage = value; } }

        public RepeaterCore()
        {

        }
        public double Size
        {
            set { this.size = value; }
        }

        public Application App
        {
            get { return this.app; }
            set { this.app = value; }
        }

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
        public List<object[]> DataSource { get { return this.dataSource; } set { this.dataSource = value; (mainUI as SimpleRepeater).FillButtons(); } }
        Application IPluginDrawer.App { set { this.app = value; } }

        public List<ItemTuple<Shape>> ShapeContainerText { get; internal set; }
        public List<ItemTuple<Shape>> ShapeContainerEnumerator { get; internal set; }
        public List<ItemTuple<Shape>> ShapeContainerImageFile { get; internal set; }
        public ICodeGenerator CodeGenerator { set { this.codeGenerator = value; } }

        public override string GetPluginDisplayName { get { return RepeaterCore.PluginDisplayName; } }

        public void Draw()
        {
            
        }


        protected override void OnProgressChange(int progress)
        {
            int p = (int)100 * progress / this.dataSource.Count;
            base.OnProgressChange(p);
        }

        
       
        public void ProcessVector(int qrcodeContentIndex = -1,bool vector = true)
        {
            try
            {
            
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
        public override void SaveConfig()
        {
            Properties.Settings1.Default.QrFormatVector = QrFormatVector;
            Properties.Settings1.Default.InitialValue = Enumerator;
            Properties.Settings1.Default.Increment = EnumeratorIncrement;
            Properties.Settings1.Default.Mask = Mask;
            Properties.Settings1.Default.FitToPage = FitToPage;
            Properties.Settings1.Default.StartX = StartX;
            Properties.Settings1.Default.Starty = StartY;
            Properties.Settings1.Default.Gap = Gap;
            Properties.Settings1.Default.Save();
            base.SaveConfig();
        }
        public override void LoadConfig()
        {
            QrFormatVector = Properties.Settings1.Default.QrFormatVector;
            Enumerator = (int)Properties.Settings1.Default.InitialValue;
            EnumeratorIncrement = (int)Properties.Settings1.Default.Increment;
           Mask = Properties.Settings1.Default.Mask.ToString();
            FitToPage = Properties.Settings1.Default.FitToPage;
            StartX= Properties.Settings1.Default.StartX;
            StartY= Properties.Settings1.Default.Starty;
            Gap = Properties.Settings1.Default.Gap;
            base.LoadConfig();  
        }
        public override void DeleteConfig()
        {
            Properties.Settings1.Default.Reset();
        }
    }
}
