using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using br.corp.bonus630.PluginLoader;
using System.Threading;
using System.Windows;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using br.corp.bonus630.plugin.DataFromClipboard.Lang;

namespace br.corp.bonus630.plugin.DataFromClipboard
{
    public class ClipboardCore : IPluginCore, IPluginDataSource

    {
        private double size;
        //private Application app;
        private ICodeGenerator codeGenerator;
        private List<object[]> dataSouce;
        private string lastText = "";
        //public bool AutoDraw { get; set; }
        private bool monitorClipboard = false;
        public bool MonitorClipboard { get { return this.monitorClipboard; } set { this.monitorClipboard = value; } }

        public const string PluginDisplayName = "Data From Clipboard";
        public Ilang Lang { get; set; }
        public List<object[]> DataSource
        {
            get
            {
                convertObservableToList();
                return this.dataSouce;
            }
            set
            {
                this.dataSouce = value;
            }
        }

        private void convertObservableToList()
        {
            if (this.dataSouce == null)
                this.dataSouce = new List<object[]>();
            this.dataSouce.Clear();
            for (int i = 0; i < clipboardDatas.Count; i++)
            {
                this.dataSouce.Add(new object[] { clipboardDatas[i].Text });
            }

        }

        ObservableCollection<ClipboardData> clipboardDatas = new ObservableCollection<ClipboardData>();
        public double Size { set { this.size = value; } }
        //public Application App { set => this.app = value; }
        public ICodeGenerator CodeGenerator { set { this.codeGenerator = value; } }
        //List<object[]> IPluginDrawer.DataSource { set => this.dataSouce = value; }

        public event Action<object> FinishJob;
        public event Action<int> ProgressChange;
        public event Action UpdatePreview;

        public RoutedCommand<ClipboardData> DeleteCommand { get; set; }
        public RoutedCommand<object> ClearAllCommand { get; set; }
        private Dispatcher dispatcher = null;

        public ObservableCollection<ClipboardData> ClipboardDatas
        {
            get { return clipboardDatas; }
            set { clipboardDatas = value; }
        }
        private Thread monitor;

        public ClipboardCore(Dispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
            DeleteCommand = new RoutedCommand<ClipboardData>(DeleteClipboardData);
            ClearAllCommand = new RoutedCommand<object>(ClearAll);
            monitor = new Thread(new ThreadStart(Process));
            monitor.SetApartmentState(ApartmentState.STA);
            monitor.IsBackground = true;
            monitor.Start();

        }
        public void OnFinishJob(object obj)
        {
            if (FinishJob != null)
                FinishJob(obj);
        }
        private void ClearAll(object obj)
        {
            this.dispatcher.Invoke(new Action(() =>
            {
                ClipboardDatas.Clear();
            }));
        }
        private void DeleteClipboardData(ClipboardData obj)
        {
            if (clipboardDatas.Contains(obj))
            {
                this.dispatcher.Invoke(new Action(() =>
                {
                    if (clipboardDatas.Remove(obj))
                        OnFinishJob(DataSource);
                }));

            }
        }
        public void Process()
        {

            while (true)
            {
                if (monitorClipboard)
                {
                    try
                    {

                        if (Clipboard.ContainsText())
                        {
                            string currentText = Clipboard.GetText();
                            if (!lastText.Equals(currentText))
                            {
                                this.dispatcher.Invoke(new Action(() =>
                                {
                                    ClipboardDatas.Add(new ClipboardData() { Text = currentText });
                                }));
                                lastText = currentText;
                                OnFinishJob(DataSource);
                            }
                        }
                    }
                    catch { }
                }
                Thread.Sleep(100);
            }
        }
        //public void DrawItem(int index)
        //{

        //    double x1 = 0;
        //    double y1 = 0;
        //    double x2 = 0;
        //    double y2 = 0;
        //    int shift = 0;
        //    app.ActiveDocument.GetUserArea(out x1, out y1, out x2, out y2, out shift, 0, true, cdrCursorShape.cdrCursorPick);
        //    Shape code = this.codeGenerator.CreateVetorLocal(app.ActiveLayer, this.dataSouce[index][0].ToString(), x2 - x1, x1, y1);
        //    OnFinishJob(code);
        //}


        public void OnProgressChange(int progress)
        {
            throw new NotImplementedException();
        }
        //private void AddItemFromClipboard()
        //{
        //    if (System.Windows.Clipboard.ContainsText() && MonitorClipboard)
        //    {
        //        string text;
        //        text = System.Windows.Clipboard.GetText();

        //        // Clipboard cp = app.Clipboard;
        //        //string text = cp.Parent.ToString();
        //        if (text != lastText)
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

