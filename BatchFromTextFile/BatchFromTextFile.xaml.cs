using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using br.corp.bonus630.PluginLoader;
using System.Collections.Generic;
using System.Reflection;
using br.corp.bonus630.plugin.BatchFromTextFile.Lang;

namespace br.corp.bonus630.plugin.BatchFromTextFile
{

    public partial class BatchFromTextFile : UserControl, IPluginUI, IPluginDataSource
    {

        public event Action<object> FinishJob;
        public event Action<string> AnyTextChanged;
        Core core;
        Ilang Lang;
        public List<object[]> DataSource { get { return core.DataSource; } }

        public int Index { get ; set; }

        //public double Size { set => throw new NotImplementedException(); }
        //public object App { set => throw new NotImplementedException(); }
        //public IImageRender ImageRender { set => throw new NotImplementedException(); }



        public event Action<int> ProgressChange;
   

        public void OnProgressChange(int progress)
        {
            if(ProgressChange!=null)
                ProgressChange(progress);
        }
        public BatchFromTextFile()
        {
            InitializeComponent();
            core = new Core();
            core.ProgressChange += core_ProgressChange;
            core.FinishJob += Core_FinishJob;
            this.Loaded += BatchFromTextFile_Loaded;
        }
        public void ChangeLang(LangTagsEnum langTag)
        {
            Lang = LangController.CreateInstance(Assembly.GetAssembly(typeof(br.corp.bonus630.plugin.BatchFromTextFile.BatchFromTextFile)), langTag) as Ilang;
            this.DataContext = Lang;
            (Lang as LangController).AutoUpdateProperties();
        }
        private void BatchFromTextFile_Loaded(object sender, RoutedEventArgs e)
        {
            txt_delimiter.TextChanged += Txt_delimiter_TextChanged;
            txt_colDelimiter.TextChanged += Txt_colDelimiter_TextChanged;
        }


        void core_ProgressChange(int obj)
        {
            OnProgressChange(obj);
        }
        
        private void Core_FinishJob(object obj)
        {
            OnFinishJob(obj);
        }
        
        private void btn_file_Click(object sender, RoutedEventArgs e)
        {
            
            Microsoft.Win32.OpenFileDialog of = new Microsoft.Win32.OpenFileDialog();
            of.Filter = Lang.OF_File;
            of.Multiselect = false;
            if((bool)of.ShowDialog())
            {
                core.LoadFile(of.FileName);
                lba_file.Content = of.FileName;
                ChangeData();
            }
        }

        public void OnFinishJob(object obj)
        {
            if(FinishJob !=null)
                FinishJob(obj);
        }
        public void ChangeData()
        {
            if (core.DataSource == null)
                return;
            lba_number.Content = string.Format("{0} {1}", core.DataSource.Count,Lang.Rows);
            if (core.DataSource.Count > 0)
                lba_numberColumn.Content = string.Format("{0} {1}", core.DataSource[0].Length,Lang.Columns);
        }


        private void Txt_colDelimiter_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txt_colDelimiter.Text))
            {
                core.delimiterColumn = txt_colDelimiter.Text;
                core.ChangeData();
                ChangeData();
            }
        }

        private void Txt_delimiter_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txt_delimiter.Text))
            {
                core.delimiter = txt_delimiter.Text;
                core.ChangeData();
                ChangeData();
            }
        }

       
    }
}
