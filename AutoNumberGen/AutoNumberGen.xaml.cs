using br.corp.bonus630.PluginLoader;
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
using System.Reflection;

namespace br.corp.bonus630.plugin.AutoNumberGen
{
    /// <summary>
    /// Interaction logic for AutoNumberGen.xaml
    /// </summary>
    public partial class AutoNumberGen : UserControl, IPluginMainUI
    {
        public AutoNumberGen()
        {
            InitializeComponent();
            this.Loaded += AutoNumberGen_Loaded;
        }

        private void AutoNumberGen_Loaded(object sender, RoutedEventArgs e)
        {
            Core.LoadConfigEvent += LoadConfig;
        }

        public IPluginCore Core { get; set; }

   
        private void ChangeText(object sender, TextChangedEventArgs e)
        {
            int start, end;
            AutoNumberGenCore core = Core as AutoNumberGenCore;
            if(Int32.TryParse(TextBoxStart.Text,out start) && Int32.TryParse(TextBoxEnd.Text,out end))
            {
                core.changeData(start, end);
                LabelResult.Content = string.Format("{0} Rows",core.DataSource.Count);
               
            }
        }


        public void LoadConfig()
        {
            AutoNumberGenCore core = Core as AutoNumberGenCore;
            TextBoxStart.Text = Properties.Settings1.Default.RangeStart.ToString();
            TextBoxEnd.Text = Properties.Settings1.Default.RangeEnd.ToString();
            ChangeText(null, null);
        }

        public void DeleteConfig()
        {
            Properties.Settings1.Default.Reset();
        }
    }
}
