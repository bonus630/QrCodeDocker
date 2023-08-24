using br.corp.bonus630.plugin.QrTimer;
using br.corp.bonus630.PluginLoader;
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

namespace QrTimer
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class QrTimerUI : UserControl,IPluginMainUI
    {
        QrTimerCore qrTimeCore;
        public QrTimerUI()
        {
            InitializeComponent();
            this.Loaded += QrTimerUI_Loaded;
            this.Unloaded += QrTimerUI_Unloaded;
        }

        private void QrTimerUI_Unloaded(object sender, RoutedEventArgs e)
        {
            if (qrTimeCore != null)
                qrTimeCore.Abort();
        }

        private void QrTimerUI_Loaded(object sender, RoutedEventArgs e)
        {
            qrTimeCore = Core as QrTimerCore;
            this.DataContext = qrTimeCore;
        }

        public IPluginCore Core { get; set; }

        private void btn_select_Click(object sender, RoutedEventArgs e)
        {
            qrTimeCore.Browser();
            btn_select.Visibility = Visibility.Collapsed;
        }
    }
}
