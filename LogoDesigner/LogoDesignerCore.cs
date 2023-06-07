using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Corel.Interop.VGCore;
using br.corp.bonus630.PluginLoader;
using System.Drawing;
using System.IO;
using Microsoft.Win32;
using br.corp.bonus630.plugin.LogoDesigner.Lang;
using br.corp.bonus630.QrCodeDocker;

namespace br.corp.bonus630.plugin.LogoDesigner
{
    public class LogoDesignerCore : PluginCoreBase<LogoDesignerCore>, IPluginDrawer
    {
        public List<object[]> DataSource
        {
            get
            {
                return dataSource;
            }
            set
            {
                if (!userChangeImage || dataSource == null)
                {
                    dataSource = value;
                   
                }
                else
                {
                    dataSource[0][0] = value[0][0];
                }
                CreatePreview();
            }
        }
        public double Size { get; set; }
        public Application App { get; set; }
        public ICodeGenerator CodeGenerator { get; set; }
        private Shape logoShape = null;

        public const string PluginDisplayName = "Logo Designer";
        private List<object[]> dataSource;

        public override string GetPluginDisplayName { get { return PluginDisplayName; } }
        private float logoProp = 0.25f;
        private bool userChangeImage = false;
        private bool useSelection = false;
        public override void DeleteConfig()
        {
            Properties.Settings.Default.Reset();
        }
        private void ImportImage(string path)
        {
            StructImportOptions sio = new StructImportOptions();
            sio.MaintainLayers = true;
            ImportFilter importFilter = this.App.ActiveLayer.ImportEx(path);
            importFilter.Finish();
            logoShape = App.ActiveShape;
        }
        public void CreatePreview()
        {
            if (this.dataSource == null)
                return;
            if (this.dataSource[0] == null)
                return;
            if (this.dataSource[0].Length <= 1)
                return;
            if(!this.dataSource[0][1].GetType().Equals(typeof(System.Drawing.Bitmap)))
                return;

            CodeGenerator.ImageRender.ErrorCorrection = ImageRender.ErrorCorrectionLevelEnum.H;

            System.Drawing.Bitmap code = CodeGenerator.ImageRender.RenderBitmapToMemory2(this.DataSource[0][0].ToString());
            double m = CodeGenerator.ImageRender.InMeasure(Size);
            CodeGenerator.ImageRender.ErrorCorrection = ImageRender.ErrorCorrectionLevelEnum.Q;
            Graphics g = Graphics.FromImage(code);
            int logoWidth = 0;
            int logoHeight = 0;
            string tempPath = Path.Combine(Path.GetTempPath(), string.Format("{0}.png", Guid.NewGuid().ToString()));
            //if (this.dataSource[0].Length > 1)
            //{

            (this.DataSource[0][1] as System.Drawing.Bitmap).Save(tempPath, System.Drawing.Imaging.ImageFormat.Png);
            System.Drawing.Bitmap logo = new System.Drawing.Bitmap(tempPath);
            logoWidth = logo.Width;
            logoHeight = logo.Height;
            // }
            //else
            //{
            //    logoWidth = logoShape.si;
            //    logoHeight = logo.Height;
            //}

            //logoProp = (float)(8 * m);

            int w = 0, h = 0;
            if (logoWidth > logoHeight)
            {
                w = (int)(code.Width * logoProp);
                h = w * logoHeight / logoWidth;
                //    w = (int)logoProp;
                //    h = (int)(w * logoProp / logoWidth);
            }
            else if (logoHeight > logoWidth)
            {
                h = (int)(code.Width * logoProp);
                w = h * logoWidth / logoHeight;
                //h = (int)logoProp;
                //w = (int)(h * logoWidth / logoHeight);
            }
            else
            {
                w = (int)(code.Width * logoProp);
                h = (int)(code.Width * logoProp);
                //w = (int)logoProp;
                //h = (int)logoProp;
            }

            int deltaX = code.Width / 2 - w / 2;
            int deltaY = code.Height / 2 - h / 2;
            System.Drawing.Rectangle rect = new System.Drawing.Rectangle(deltaX, deltaY, w, h);

            g.DrawImage(logo, rect);
            OnOverridePreview(code);
        }
        public void Draw()
        {
            try
            {
                if(logoShape == null && this.dataSource == null)
                {
                    return;
                    
                }
             
                 App.Optimization = true;
                App.ActiveDocument.BeginCommandGroup();
                
                CodeGenerator.ImageRender.ErrorCorrection = ImageRender.ErrorCorrectionLevelEnum.H;
                double m = CodeGenerator.ImageRender.InMeasure(Size);
                ShapeRange sr = this.App.CreateShapeRange();
                if (!useSelection)
                {
                    string tempPath = Path.Combine(Path.GetTempPath(), string.Format("{0}.png", Guid.NewGuid().ToString()));
                    (this.DataSource[0][1] as System.Drawing.Bitmap).Save(tempPath, System.Drawing.Imaging.ImageFormat.Png);
                    //CreatePreview(tempPath);

                   
                    ImportImage(tempPath);
                }
                Layer tempLayer = this.App.ActiveDocument.ActivePage.CreateLayer("temp_qrcode");
                sr.Add(logoShape);
                tempLayer.Activate();

                //imageRender.SaveTempQrCodeFile(content, this.app.ActivePage.Resolution, 221);
                Shape code = CodeGenerator.CreateVetorLocal(tempLayer, this.DataSource[0][0].ToString(), Size);
                //(CodeGenerator as QrCodeGenerator).BorderColor
                //logoProp = (float)(8 * m);

         



                double logoWidth = sr[1].SizeWidth;
                double logoHeight = sr[1].SizeHeight;
                double w = 0, h = 0;
                if (logoWidth > logoHeight)
                {
                    w = (code.SizeWidth * logoProp);
                    h = w * logoHeight / logoWidth;
                }
                else if (logoHeight > logoWidth)
                {
                    h = (code.SizeWidth * logoProp);
                    w = h * logoWidth / logoHeight;
                }
                else
                {
                    w = (code.SizeWidth * logoProp);
                    h = (code.SizeWidth * logoProp);
                }

                sr[1].SetSize(w, h);

               // Shape bg = tempLayer.createre

                sr.Add(code);
                sr.AlignAndDistribute(cdrAlignDistributeH.cdrAlignDistributeHAlignCenter, cdrAlignDistributeV.cdrAlignDistributeVAlignCenter);
                sr[1].OrderFrontOf(code);
                sr.Group();
                App.ActiveDocument.EndCommandGroup();

            }
            catch { }
            finally
            {
                App.Optimization = false;
                App.Refresh();
                CodeGenerator.ImageRender.ErrorCorrection = ImageRender.ErrorCorrectionLevelEnum.Q;
            }

        }
        public void Browser()
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Title = (Lang as ILang).OF_BrowserFile;
            openFile.Multiselect = false;
            openFile.Filter = "Image Files|*.png;*.jpeg;*.jpg;*.gif;*.bmp";
            if ((bool)openFile.ShowDialog())
            {
                userChangeImage = true;
                useSelection = false;
                var bitmap = new System.Drawing.Bitmap(openFile.FileName);
                if (this.dataSource == null)
                    this.dataSource = new List<object[]>() { new object[2] { "", bitmap } };
                else
                    this.dataSource[0] = new object[2] { this.dataSource[0][0], bitmap };
            }
        }
        public void UseSelection()
        {
            userChangeImage = true;
            useSelection = true;
            if (App.ActiveSelection == null)
                return;
            logoShape = App.ActiveSelection;
            logoShape.Group();
        }
        public void Reset()
        {
            userChangeImage = false;
            useSelection = false;
        }
    }
}
