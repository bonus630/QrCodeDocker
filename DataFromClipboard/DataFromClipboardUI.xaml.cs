using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using br.corp.bonus630.PluginLoader;
using Corel.Interop.VGCore;
using System.Diagnostics;

namespace br.corp.bonus630.plugin.DataFromClipboard
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class DataFromClipboardUI : UserControl,IPluginUI,IPluginDataSource,IPluginDrawer
    {
        ClipboardCore clipboardCore;

        public DataFromClipboardUI()
        {
            InitializeComponent();
            clipboardCore = new ClipboardCore();

            clipboardCore.AutoDraw = true;
        }
       
        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            // Initialize the clipboard now that we have a window soruce to use
            var windowClipboardManager = new ClipboardManager(this);
            windowClipboardManager.ClipboardChanged += ClipboardChanged;
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

        public double Size { set => clipboardCore.Size = value; }
        Corel.Interop.VGCore.Application App { set => clipboardCore.App = value; }
        public ICodeGenerator CodeGenerator { set => clipboardCore.CodeGenerator = value; }
        List<object[]> IPluginDrawer.DataSource { set => clipboardCore.DataSource = value; }
        Corel.Interop.VGCore.Application IPluginDrawer.App { set => clipboardCore.App = value; }
        public int Index { get; set; }

        public event Action<object> FinishJob;
        public event Action<int> ProgressChange;

        public void Draw()
        {
            throw new NotImplementedException();
        }

        public void OnFinishJob(object obj)
        {
            throw new NotImplementedException();
        }

        public void OnProgressChange(int progress)
        {
            throw new NotImplementedException();
        }

        private void cb_monitorClipboard_Click(object sender, RoutedEventArgs e)
        {
            clipboardCore.MonitorClipboard = (bool)cb_monitorClipboard.IsChecked;
        }
    }
}
