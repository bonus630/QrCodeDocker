using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using br.corp.bonus630.PluginLoader;

using System.Diagnostics;
using System.Windows.Threading;

namespace br.corp.bonus630.plugin.DataFromClipboard
{
    /// <summary>
    /// Interaction logic for DataFromClipboardUI.xaml
    /// </summary>
    public partial class DataFromClipboardUI : UserControl,IPluginUI,IPluginDataSource
    {
        ClipboardCore clipboardCore;

        public DataFromClipboardUI()
        {
            InitializeComponent();
            Dispatcher dispatcher = this.Dispatcher;
            clipboardCore = new ClipboardCore(dispatcher);
            this.DataContext = clipboardCore;
            clipboardCore.FinishJob += ClipboardCore_FinishJob;
            //System.Windows.MessageBox.Show("1");
            //clipboardCore.AutoDraw = true;
        }

        private void ClipboardCore_FinishJob(object obj)
        {
            OnFinishJob(obj);
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            // Initialize the clipboard now that we have a window soruce to use
            //var windowClipboardManager = new ClipboardManager(this);
            //windowClipboardManager.ClipboardChanged += ClipboardChanged;
        }
    
        private void ClipboardChanged(object sender, EventArgs e)
        {
            // Handle your clipboard update here, debug logging example:
            if (System.Windows.Clipboard.ContainsText())
            {
                Debug.WriteLine(System.Windows.Clipboard.GetText());
            }
        }

        public List<object[]> DataSource { get { return clipboardCore.DataSource; } }

        public double Size { set{ clipboardCore.Size = value; } }
       // Corel.Interop.VGCore.Application App { set { clipboardCore.App = value; } }
        public ICodeGenerator CodeGenerator { set { clipboardCore.CodeGenerator = value; } }
        //List<object[]> IPluginDrawer.DataSource { set => clipboardCore.DataSource = value; }
        //Corel.Interop.VGCore.Application IPluginDrawer.App { set => clipboardCore.App = value; }
        public int Index { get; set; }

        public event Action<object> FinishJob;
        public event Action<int> ProgressChange;
        public event Action<string> AnyTextChanged;

       

        public void OnFinishJob(object obj)
        {
            if (FinishJob != null)
                FinishJob(obj);
        }

        public void OnProgressChange(int progress)
        {
           
        }

        private void cb_monitorClipboard_Click(object sender, RoutedEventArgs e)
        {
            if(clipboardCore == null)
                System.Windows.MessageBox.Show(clipboardCore.ToString());
            clipboardCore.MonitorClipboard = (bool)cb_monitorClipboard.IsChecked;
        }
    }
}
