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

namespace ExtraTest
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class ExtraTestUI : UserControl,IPluginMainUI
    {
        public ExtraTestUI()
        {
            InitializeComponent();
            this.Loaded += UserControl1_Loaded;
        }

        private void UserControl1_Loaded(object sender, RoutedEventArgs e)
        {
            Core.LoadConfigEvent += Core_LoadConfigEvent;
        }

        private void Core_LoadConfigEvent()
        {
            //todo
        }

        public IPluginCore Core { get; set; }

        private void btn_gerar_Click(object sender, RoutedEventArgs e)
        {
            (Core as ExtraTestCore).Draw();
        }
    }
}
