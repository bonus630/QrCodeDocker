using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using br.corp.bonus630.PluginLoader;
using Corel.Interop.VGCore;
using Tesseract;

namespace br.corp.bonus630.plugin.ShapeToCode
{
    public class ShapeToCodeCore : IPluginCore, IPluginDrawer
    {
        public const string PluginDisplayName = "Shape To Code";
        public List<object[]> DataSource { get; set; }
        public double Size { get; set; }
        public bool DeleteOri { get; set; }
        public bool OverrideWidth { get; set; }
        private Application app;
        public Application App { set { app = value; } }
        private ICodeGenerator code;
        public ICodeGenerator CodeGenerator { set { code = value; } }
        public Range Sr_Range { get; set; }

        public event Action<object> FinishJob;
        public event Action<int> ProgressChange;

        public void Draw()
        {
            try
            {
                app.Optimization = true;
                app.EventsEnabled = false;
                app.Unit = cdrUnit.cdrMillimeter;
                app.ActiveDocument.BeginCommandGroup();
                ShapeRange range = GetRange();
                processRange(range);
            }
            catch { }
            finally
            {
                if (app.ActiveDocument != null)
                    app.ActiveDocument.EndCommandGroup();
                app.Optimization = false;
                app.EventsEnabled = true;
                app.Refresh();
            }
        }
        private void processRange(ShapeRange range)
        {
            try
            {
               
                for (int i = 1; i <= range.Count; i++)
                {
                    if (range[i].Type.Equals(cdrShapeType.cdrTextShape))
                    {
                        CreateCode(range[i]);
                    }
                    if(range[i].Type.Equals(cdrShapeType.cdrGroupShape))
                    {
                        processRange(range[i].Shapes.Range());
                    }
                    if (range[i].Type.Equals(cdrShapeType.cdrCurveShape))
                    {
                        string text = processCurve(range[i]);
                        if (!String.IsNullOrEmpty(text))
                            CreateCode(range[i], text);
                    }
                }
            }
            catch(Exception e) {
                Console.WriteLine(e.Message);
            }
        }

        private string processCurve(Shape curve)
        {

            // System.Windows.MessageBox.Show("ToText");

            string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\bonus630\\OCR";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            path += "\\1.png";

            string tessdataPath = this.app.AddonPath + "QrCodeDocker\\extras\\tessdata";

            var toDelete = this.app.ActiveShape;
            var rect = this.app.ActiveShape.BoundingBox;
            var exportFilter = this.app.ActiveDocument.ExportBitmap(path, cdrFilter.cdrPNG, cdrExportRange.cdrSelection, cdrImageType.cdrRGBColorImage);
            exportFilter.Finish();

            if (File.Exists(path))
            {
                try
                {
                    using (var engine = new TesseractEngine(tessdataPath, "eng", EngineMode.Default))
                    {
                        using (var img = Pix.LoadFromFile(path))
                        {
                            using (var page = engine.Process(img))
                            {
                                var text = page.GetText();

                                ResultIterator resultInterator = page.GetIterator();
                                FontAttributes fontAttributes = resultInterator.GetWordFontAttributes();

                                if (!string.IsNullOrEmpty(text))
                                {
                                    return text;
                                }
                                //if (fontAttributes != null)
                                //    this.app.MsgShow(fontAttributes.FontInfo.Name);



                                Console.WriteLine("Taxa:{0}", page.GetMeanConfidence());
                                Console.WriteLine(text);

                            }
                        }
                    }

                }
                catch (Exception erro)
                {
                    Console.WriteLine(erro.Message);
                }
            }

            return "";
        }
        private Shape CreateCode(Shape shape, string text = "")
        {
            if (String.IsNullOrEmpty(text))
                text = shape.Text.Story.Text;
            Size = shape.SizeWidth;
            if (shape.SizeWidth > shape.SizeHeight && !OverrideWidth)
                Size = shape.SizeHeight;
            Shape code = this.code.CreateVetorLocal(shape.Layer, text, Size, shape.LeftX, shape.TopY, string.Format("QR-{0}", text));
            code.SetPosition(shape.LeftX, shape.TopY);
            code.SetSize(Size, Size);
            if (DeleteOri)
                shape.Delete();
            return code;
        }
        private ShapeRange GetRange()
        {
            ShapeRange range = app.CreateShapeRange();
            switch (Sr_Range)
            {
                case Range.Selection:
                    range = app.ActiveSelectionRange;
                    break;
                case Range.Page:
                    range = app.ActiveDocument.ActivePage.FindShapes();
                    break;
                case Range.Doc:
                    Pages pages = app.ActiveDocument.Pages;
                    foreach (Corel.Interop.VGCore.Page page in pages)
                    {
                        range.AddRange(page.FindShapes());
                    }
                    break;
            }
            return range;
        }
        public void OnFinishJob(object obj)
        {

        }

        public void OnProgressChange(int progress)
        {

        }
    }
    public enum Range
    {
        Doc, Page, Selection
    }
}
