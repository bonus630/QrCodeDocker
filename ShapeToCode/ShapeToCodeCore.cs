using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using br.corp.bonus630.PluginLoader;
using Corel.Interop.VGCore;
using Tesseract;

namespace br.corp.bonus630.plugin.PlaceHere
{
    public class ShapeToCodeCore : PluginCoreBase<ShapeToCodeCore>, IPluginDrawer
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

        public override string GetPluginDisplayName { get { return ShapeToCodeCore.PluginDisplayName; } }

        public void Draw()
        {
            try
            {
                app.Optimization = true;
                app.EventsEnabled = false;
                app.Unit = cdrUnit.cdrMillimeter;
                app.ActiveDocument.Unit = cdrUnit.cdrMillimeter;
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
                    if (range[i].Type.Equals(cdrShapeType.cdrGroupShape))
                    {
                        processRange(range[i].Shapes.All());
                    }
                    if (range[i].Type.Equals(cdrShapeType.cdrCurveShape))
                    {
                        string text = processCurve(range[i]);
                        if (!String.IsNullOrEmpty(text))
                            CreateCode(range[i], text);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private string processCurve(Shape curve)
        {
            string tessdataPath = this.app.AddonPath + "QrCodeDocker\\extras\\tessdata";


            // System.Windows.MessageBox.Show("ToText");

            string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\bonus630\\OCR";
            try
            {
                using (var engine = new TesseractEngine(tessdataPath, "eng", EngineMode.Default))
                {
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);
                    path += "\\1.png";


                    //var toDelete = this.app.ActiveShape;
                    var rect = curve.BoundingBox;
                    ShapeRange originalSelection = this.app.ActiveSelectionRange;
                    originalSelection.RemoveFromSelection();
                    curve.AddToSelection();
                    //double w, h;
                    // w = curve.SizeWidth;
                    // h = curve.SizeHeight;
                    //curve.SetSize(w * 4, h * 4);
                    var exportFilter = this.app.ActiveDocument.ExportBitmap(path, cdrFilter.cdrPNG, cdrExportRange.cdrSelection, cdrImageType.cdrRGBColorImage, 0, 0, 300, 300);
                    exportFilter.Finish();
                    //curve.SetSize(w, h);
                    curve.RemoveFromSelection();
                    originalSelection.AddToSelection();

                    if (File.Exists(path))
                    {
                        using (var img = Pix.LoadFromFile(path))
                        {
                            using (var page = engine.Process(img))
                            {
                                var text = page.GetText();

                                if (!string.IsNullOrEmpty(text))
                                    return text;
                                Console.WriteLine("Taxa:{0}", page.GetMeanConfidence());
                                Console.WriteLine(text);

                            }
                        }
                    }
                }
            }
            catch (Exception erro)
            {
                Console.WriteLine(erro.Message);
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

        public override void DeleteConfig()
        {
            
        }
    }
    public enum Range
    {
        Doc, Page, Selection
    }
}
