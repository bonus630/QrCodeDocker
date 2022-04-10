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
    public partial class AutoNumberGen : UserControl, IPluginUI, IPluginDataSource
    {
        public string PluginDisplayName { get { return AutoNumberGenCore.PluginDisplayName; } }
        public AutoNumberGen()
        {
            InitializeComponent();
            core = new AutoNumberGenCore();
            //ChangeLang(LangTagsEnum.PT_BR);
            
        }
        public AutoNumberGen(LangTagsEnum lang)
        {
            InitializeComponent();
            core = new AutoNumberGenCore();
            ChangeLang(lang);
            this.DataContext = LangBase;
        }
        private LangController LangBase;
        AutoNumberGenCore core;
        public List<object[]> DataSource { get { return core.DataSource; } }

        public int Index { get; set; }

        public event Action<object> FinishJob;
        public event Action<string> AnyTextChanged;
        public event Action<int> ProgressChange;
        public event Action UpdatePreview;

        public void OnFinishJob(object obj)
        {
            if (FinishJob != null)
                FinishJob(obj);
        }

        public void OnProgressChange(int progress)
        {
            
        }
        public void ChangeLang(LangTagsEnum lang)
        {
            LangBase = LangController.CreateInstance(Assembly.GetAssembly(typeof(br.corp.bonus630.plugin.AutoNumberGen.AutoNumberGen)), lang);
            this.DataContext = LangBase;
            LangBase.AutoUpdateProperties();
        }
        private void ChangeText(object sender, TextChangedEventArgs e)
        {
            int start, end;
            if(Int32.TryParse(TextBoxStart.Text,out start) && Int32.TryParse(TextBoxEnd.Text,out end))
            {
                core.changeData(start, end);
                LabelResult.Content = string.Format("{0} Rows",core.DataSource.Count);
                OnFinishJob(core.DataSource);
            }
        }

        public void SaveConfig()
        {
            Properties.Settings1.Default.RangeStart = core.StartValue;
            Properties.Settings1.Default.RangeEnd = core.EndValue;
            Properties.Settings1.Default.Save();
        }

        public void LoadConfig()
        {
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
