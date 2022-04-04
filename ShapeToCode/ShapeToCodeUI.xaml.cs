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
using Corel.Interop.VGCore;

namespace br.corp.bonus630.plugin.ShapeToCode
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class ShapeToCodeUI : UserControl, IPluginUI,IPluginDrawer
    {
        private ShapeToCodeCore core;
        public ShapeToCodeUI()
        {
            InitializeComponent();
            core = new ShapeToCodeCore();
            btn_Doc.Click += (s,e) => { core.Sr_Range = Range.Doc; Draw(); };
            btn_Page.Click += (s, e) => { core.Sr_Range = Range.Page; Draw(); };
            btn_Selection.Click += (s, e) => { core.Sr_Range = Range.Selection; Draw(); };
            ck_deleteOri.Click += (s, e) => { core.DeleteOri = (bool) (s as CheckBox).IsChecked; };
            ck_overWidth.Click += (s, e) => { core.OverrideWidth = (bool)(s as CheckBox).IsChecked; };
        }
        public void ChangeLang(LangTagsEnum langTag)
        {
            //Lang = LangController.CreateInstance(Assembly.GetAssembly(typeof(br.corp.bonus630.plugin.Repeater.SimpleRepeater)), langTag) as Ilang;
            //this.DataContext = Lang;
            //(Lang as LangController).AutoUpdateProperties();
        }
        public int Index { get; set; }
        public List<object[]> DataSource { get; set; }
        public double Size { get; set; }
        private Corel.Interop.VGCore.Application app;
        public Corel.Interop.VGCore.Application App { set { app = value; core.App = value; } }
        private ICodeGenerator code;
        public ICodeGenerator CodeGenerator { set { code = value;core.CodeGenerator = value; } }

        public event Action<object> FinishJob;
        public event Action<string> AnyTextChanged;
        public event Action<int> ProgressChange;

        public void Draw()
        {
            core.Draw();
        }

        public void OnFinishJob(object obj)
        {
            
        }

        public void OnProgressChange(int progress)
        {
           
        }

    }
}
