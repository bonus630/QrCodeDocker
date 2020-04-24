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

namespace br.corp.bonus630.plugin.DataFromMysql
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class DataFromMysqlUI : UserControl,IPluginUI,IPluginDataSource
    {
        public DataFromMysqlUI()
        {
            InitializeComponent();
        }

        public List<object[]> DataSource => throw new NotImplementedException();

        public int Index { get; set; }

        public event Action<object> FinishJob;
        public event Action<int> ProgressChange;

        public void OnFinishJob(object obj)
        {
            throw new NotImplementedException();
        }

        public void OnProgressChange(int progress)
        {
            throw new NotImplementedException();
        }
    }
}
