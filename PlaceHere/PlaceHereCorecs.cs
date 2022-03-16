using br.corp.bonus630.PluginLoader;
using Corel.Interop.VGCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using c = Corel.Interop.VGCore;
namespace br.corp.bonus630.plugin.PlaceHere
{
    public class PlaceHereCorecs : IPluginCore, IPluginDrawer
    {
        public const string PluginDisplayName = "Place Here";
        c.Application corelApp;
        ICodeGenerator codeGenerator;
        public int DSCursor { set { this.dsCursor = value; } }
        int dsCursor = 0;
        public double FactorX { get; set; }
        public double FactorY { get; set; }
        private List<object[]> dataSource;
        public List<object[]> DataSource { set {
                this.dataSource = value;
               
            } }
        public double Size { get; set; }
        
        public Corel.Interop.VGCore.Application App { set { this.corelApp = value; } }
        public ICodeGenerator CodeGenerator { set { this.codeGenerator = value; } }

        public event Action<object> FinishJob;
        public event Action<int> ProgressChange;

        cdrUnit prevUnit = cdrUnit.cdrInch;
        public void Draw()
        {
            try
            {
                if (dsCursor < dataSource.Count)
                {
                    corelApp.Optimization = true;
                    corelApp.Unit = cdrUnit.cdrMillimeter;
                    corelApp.ActiveDocument.BeginCommandGroup();
                    if (corelApp.Unit != prevUnit)
                        corelApp.Unit = prevUnit;
                    c.Shape code = null;
                    double x = 0, y = 0,x2=0,y2=0;
                    int s = 0;

                    //corelApp.ActiveDocument.GetUserArea(out x, out y, out x2, out y2, out s, 0, true, cdrCursorShape.cdrCursorExtPick);


                    corelApp.ActiveDocument.GetUserClick(out x, out y, out s, 0, true, c.cdrCursorShape.cdrCursorSmallcrosshair);

                    code = this.codeGenerator.CreateVetorLocal(corelApp.ActiveLayer, 
                        dataSource[dsCursor][0].ToString()
                        , corelApp.ConvertUnits(Size,corelApp.ActiveDocument.Unit,cdrUnit.cdrMillimeter)
                        , 0, 0, string.Format("QR-{0}", dataSource[dsCursor][0].ToString()));
                    Console.WriteLine("Unit: App|{0} Doc|{1}",corelApp.Unit,corelApp.ActiveDocument.Unit);
                    //code = corelApp.ActiveLayer.CreateArtisticText(x, y, dataSource[dsCursor][0].ToString());
                    //double x1 = corelApp.ConvertUnits(x, corelApp.ActiveDocument.Unit, corelApp.Unit);
                    //double y1 = corelApp.ConvertUnits(y, corelApp.ActiveDocument.Unit, corelApp.Unit);

                    double h, w;
                    //h = corelApp.ConvertUnits(code.SizeHeight, corelApp.Unit, corelApp.ActiveDocument.Rulers.HUnits);
                    //w = corelApp.ConvertUnits(code.SizeWidth, corelApp.Unit, corelApp.ActiveDocument.Rulers.HUnits);

                    h = code.SizeHeight;
                    w = code.SizeWidth;
                    x = x - w * FactorX;
                    y = y- h * FactorY;

                    //code.SetPosition(corelApp.ConvertUnits(x1,corelApp.Unit,corelApp.ActiveDocument.Unit),
                    //corelApp.ConvertUnits(y1, corelApp.Unit, corelApp.ActiveDocument.Unit));
                    code.SetPosition(x,y);
                    OnProgressChange(dsCursor++);
                    //corelApp.Refresh();
                    
                    corelApp.Optimization = false;
                    corelApp.Refresh();
                    Draw();
                }
                else
                    dsCursor = 0;
                
            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show(e.Message);
            }
            finally
            {
                if (corelApp.ActiveDocument != null)
                    corelApp.ActiveDocument.EndCommandGroup();
                corelApp.Optimization = false;
                corelApp.Refresh();
            }
            
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
    }
}
