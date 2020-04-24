using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using br.corp.bonus630.PluginLoader;
using br.corp.bonus630.QrCodeDocker;
using Corel.Interop.VGCore;
using System.Threading;


namespace br.corp.bonus630.plugin.DataFromClipboard
{
    public class ClipboardCore : IPluginCore, IPluginDataSource , IPluginDrawer

    {
        private double size;
        private Application app;
        private ICodeGenerator codeGenerator;
        private List<object[]> dataSouce;
        private string lastText = "";
        public bool AutoDraw { get; set; }
        private bool monitorClipboard = false;
        public bool MonitorClipboard { get { return this.monitorClipboard; } set { this.monitorClipboard = value;  } }

        public const string PluginDisplayName = "Data From Clipboard";

        public List<object[]> DataSource { get { return this.dataSouce; }set { this.dataSouce = value; } }

        public double Size { set => this.size = value; }
        public Application App { set => this.app = value; }
        public ICodeGenerator CodeGenerator { set => this.codeGenerator = value; }
        List<object[]> IPluginDrawer.DataSource { set => this.dataSouce = value; }

        public event Action<object> FinishJob;
        public event Action<int> ProgressChange;

       // private Thread thread;

        public ClipboardCore()
        {
            //thread = new Thread(new ThreadStart(AddItemFromClipboard));
            //thread.IsBackground = true;
            
        }

        public void Draw()
        {
            throw new NotImplementedException();
        }
        public void DrawItem(int index)
        {
           
            double x1 = 0;
            double y1 = 0;
            double x2 = 0;
            double y2 = 0;
            int shift = 0;
            app.ActiveDocument.GetUserArea(out x1, out y1, out x2, out y2, out shift, 0, true, cdrCursorShape.cdrCursorPick);
            Shape code = this.codeGenerator.CreateVetorLocal(app.ActiveLayer, this.dataSouce[index][0].ToString(), x2 - x1, x1, y1);
            OnFinishJob(code);
        }
        public void OnFinishJob(object obj)
        {
            FinishJob?.Invoke(obj);
        }

        public void OnProgressChange(int progress)
        {
            throw new NotImplementedException();
        }
        private void AddItemFromClipboard()
        {
            if (System.Windows.Clipboard.ContainsText() && MonitorClipboard)
            {
                string text;
                text = System.Windows.Clipboard.GetText();
              
                // Clipboard cp = app.Clipboard;
                //string text = cp.Parent.ToString();
                if (text != lastText)
                {
                    lastText = text;
                    if (this.dataSouce == null)
                        this.dataSouce = new List<object[]>();
                    this.dataSouce.Add(new string[] { text });
                    if (AutoDraw)
                        DrawItem(this.dataSouce.Count - 1);
                }
            }

        }
        //private void AddItemFromClipboard()
        //{
        //    if (System.Windows.Clipboard.ContainsText() && MonitorClipboard)
        //    {
        //        string text;
        //        lock (thread)
        //        {
        //            text = System.Windows.Clipboard.GetText();
        //        }
        //       // Clipboard cp = app.Clipboard;
        //        //string text = cp.Parent.ToString();
        //        if(text != lastText)
        //        {
        //            lastText = text;
        //            if (this.dataSouce == null)
        //                this.dataSouce = new List<object[]>();
        //            this.dataSouce.Add(new string[] { text });
        //            if (AutoDraw)
        //                DrawItem(this.dataSouce.Count - 1);
        //        }
        //    }

        //}
    }
}
