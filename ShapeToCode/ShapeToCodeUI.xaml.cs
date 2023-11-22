using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
    public partial class ShapeToCodeUI : UserControl, IPluginMainUI
    {
        private ShapeToCodeCore scCore;

        public ShapeToCodeUI()
        {
            InitializeComponent();
            this.Loaded += ShapeToCodeUI_Loaded;
        
        }

        private void ShapeToCodeUI_Loaded(object sender, RoutedEventArgs ev)
        {
            scCore = Core as ShapeToCodeCore;
            btn_Doc.Click += (s, e) => { scCore.Sr_Range = Range.Doc; Draw(); };
            btn_Page.Click += (s, e) => { scCore.Sr_Range = Range.Page; Draw(); };
            btn_Selection.Click += (s, e) => { scCore.Sr_Range = Range.Selection; Draw(); };
            ck_deleteOri.Click += (s, e) => { scCore.DeleteOri = (bool)(s as CheckBox).IsChecked; };
            ck_overWidth.Click += (s, e) => { SetSize(); };
            ck_bestFit.Click += (s, e) => { SetSize(); };
            ck_sizeField.Click += (s, e) => { SetSize(); };
        }

        public void SetSize()
        {
            scCore.OverrideWidth = (bool)ck_overWidth.IsChecked;
            scCore.BestFit = (bool)ck_bestFit.IsChecked;
            scCore.SizeField = (bool)ck_sizeField.IsChecked;
        }

        public IPluginCore Core { get ; set; }


        public void Draw()
        {
            scCore.Draw();
        }

        
    }
}
