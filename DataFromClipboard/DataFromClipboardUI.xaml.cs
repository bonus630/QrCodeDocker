using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

using System.Diagnostics;
using System.Windows.Threading;
using System.Reflection;

namespace br.corp.bonus630.plugin.DataFromClipboard
{
    /// <summary>
    /// Interaction logic for DataFromClipboardUI.xaml
    /// </summary>
    public partial class DataFromClipboardUI : UserControl, IPluginMainUI
    {
        ClipboardCore clipboardCore;
        public IPluginCore Core { get; set; }
        public DataFromClipboardUI()
        {
            InitializeComponent();
            this.Loaded += DataFromClipboardUI_Loaded;
                     
        }

        private void DataFromClipboardUI_Loaded(object sender, RoutedEventArgs e)
        {
            clipboardCore = (Core as ClipboardCore);
            clipboardCore.UIDispatcher = this.Dispatcher;
        }


        private void cb_monitorClipboard_Click(object sender, RoutedEventArgs e)
        {
            if (clipboardCore == null)
                System.Windows.MessageBox.Show(clipboardCore.ToString());
            clipboardCore.MonitorClipboard = (bool)cb_monitorClipboard.IsChecked;
        }

   
    }
}
