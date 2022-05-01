using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Corel.Interop.VGCore;
using br.corp.bonus630.PluginLoader;
using System.Drawing;
using System.IO;

namespace br.corp.bonus630.plugin.LogoDesigner
{
    public class LogoDesignerCore : PluginCoreBase<LogoDesignerCore>, IPluginDrawer
    {
        public List<object[]> DataSource { get; set; }
        public double Size { get; set; }
        public Application App { get; set; }
        public ICodeGenerator CodeGenerator { get; set; }

        public const string PluginDisplayName = "Logo Designer";

        public override string GetPluginDisplayName { get { return PluginDisplayName; } }

        public override void DeleteConfig()
        {
            Properties.Settings.Default.Reset();
        }

        public void Draw()
        {
            try
            {
                App.Optimization = true;
                string tempPath = Path.Combine(Path.GetTempPath(), string.Format("{0}.png", Guid.NewGuid().ToString()));
                CodeGenerator.ImageRender.ErrorCorrection = ImageRender.ErrorCorrectionLevelEnum.H;
                double m = CodeGenerator.ImageRender.InMeasure(Size);

                (this.DataSource[0][1] as System.Drawing.Bitmap).Save(tempPath, System.Drawing.Imaging.ImageFormat.Png);
               

                Layer tempLayer = this.App.ActiveDocument.ActivePage.CreateLayer("temp_qrcode");



                tempLayer.Activate();
                //imageRender.SaveTempQrCodeFile(content, this.app.ActivePage.Resolution, 221);
                Shape code = CodeGenerator.CreateVetorLocal(tempLayer, this.DataSource[0][0].ToString(), Size);
                StructImportOptions sio = new StructImportOptions();
                sio.MaintainLayers = true;
                ImportFilter importFilter = this.App.ActiveLayer.ImportEx(tempPath);
                importFilter.Finish();
                ShapeRange sr = new ShapeRange();
                sr.Add(this.App.ActiveShape);

                double proportion = 0;
                if (sr[1].SizeWidth > sr[1].SizeHeight)
                    proportion = sr[1].SizeWidth;
                else
                    proportion = sr[1].SizeHeight;

                
                this.App.ActiveShape.SetSize((8 * m),(8 * m));
               
                
                sr.Add(code);
                sr.AlignAndDistribute(cdrAlignDistributeH.cdrAlignDistributeHAlignCenter, cdrAlignDistributeV.cdrAlignDistributeVAlignCenter);
                sr[1].OrderFrontOf(code);
                
            }
            catch { }
            finally
            {
                App.Optimization = false;
                App.Refresh();
                CodeGenerator.ImageRender.ErrorCorrection = ImageRender.ErrorCorrectionLevelEnum.Q;
            }

        }
    }
}
