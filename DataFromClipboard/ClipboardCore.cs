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
    public class ClipboardCore : PluginCoreBase<ClipboardCore>, IPluginDataSource

    {
     
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
     
      
  
        public RoutedCommand<ClipboardData> DeleteCommand { get; set; }
        public RoutedCommand<object> ClearAllCommand { get; set; }
        private Dispatcher dispatcher = null;
        public Dispatcher UIDispatcher { get { return dispatcher; } set { dispatcher = value; } }
        public ObservableCollection<ClipboardData> ClipboardDatas
        {
            get { return clipboardDatas; }
            set { clipboardDatas = value; }
        }

        public override string GetPluginDisplayName
        {
            get { return ClipboardCore.PluginDisplayName; }
        }

        private Thread monitor;

        public ClipboardCore()
        {
           
            DeleteCommand = new RoutedCommand<ClipboardData>(DeleteClipboardData);
            ClearAllCommand = new RoutedCommand<object>(ClearAll);
            monitor = new Thread(new ThreadStart(Process));
            monitor.SetApartmentState(ApartmentState.STA);
            monitor.IsBackground = true;
            monitor.Start();

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

        public override void DeleteConfig()
        {
            
        }

    }

}

