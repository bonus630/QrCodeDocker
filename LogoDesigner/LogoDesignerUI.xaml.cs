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

namespace br.corp.bonus630.plugin.LogoDesigner
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class LogoDesignerUI : UserControl,IPluginMainUI
    {
        LogoDesignerCore ldCore;
        public LogoDesignerUI()
        {
            InitializeComponent();
            this.Loaded += LogoDesignerUI_Loaded;
        }

        private void LogoDesignerUI_Loaded(object sender, RoutedEventArgs e)
        {
            ldCore = Core as LogoDesignerCore;
        }

        public IPluginCore Core { get ; set; }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ldCore.Draw();
        }
    }
}
