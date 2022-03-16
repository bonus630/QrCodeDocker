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
using c = Corel.Interop.VGCore;

namespace br.corp.bonus630.plugin.PlaceHere
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class PlaceHereUI : UserControl, IPluginUI, IPluginDrawer
    {
        c.Application corelApp;
        ICodeGenerator codeGenerator;
        PlaceHereCorecs core;
        public PlaceHereUI()
        {
            InitializeComponent();
            core = new PlaceHereCorecs();
            
            core.ProgressChange += Core_ProgressChange;
            
        }

        private void Core_ProgressChange(int obj)
        {
            OnProgressChange(obj);
        }

        public int Index { get; set; }
        private List<object[]> dataSource;
        public List<object[]> DataSource { set { 
                this.dataSource = value; } }
        private double size = 0;
        public double Size { get { return this.size; } set { 
                this.size = value; 
                core.Size = value; 
            } }
        public Corel.Interop.VGCore.Application App { set { this.corelApp = value; core.App = value; } }
        public ICodeGenerator CodeGenerator { set { this.codeGenerator = value;core.CodeGenerator = value; } }

        public event Action<object> FinishJob;
        public event Action<string> AnyTextChanged;
        public event Action<int> ProgressChange;

        public void Draw()
        {
            core.DataSource = this.dataSource;
            core.DSCursor = 0;
            core.Draw();
        }

        public void OnFinishJob(object obj)
        {
            
        }

        public void OnProgressChange(int progress)
        {
            if (ProgressChange != null)
                ProgressChange(progress);
        }

        private void btn_start_Click(object sender, RoutedEventArgs e)
        {
            btn_start.Content = "Restart";
            Draw();
        }

        private void AnchorButton_FactorChanged(double factorX, double factorY)
        {
            core.FactorX = factorX;
            core.FactorY = factorY;
        }
    }
}
