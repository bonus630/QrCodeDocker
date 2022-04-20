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

    public partial class BatchFromTextFile : UserControl, IPluginMainUI
    {
     
        public IPluginCore Core { get; set ; }
        BatchFromTextFileCore bCore;
        Ilang Lang;
  
        public BatchFromTextFile()
        {
            InitializeComponent();
            this.Loaded += BatchFromTextFile_Loaded;
        }
      
        private void BatchFromTextFile_Loaded(object sender, RoutedEventArgs e)
        {
            bCore = Core as BatchFromTextFileCore;
            Lang = Core.Lang as Ilang;
            txt_delimiter.TextChanged += Txt_delimiter_TextChanged;
            txt_colDelimiter.TextChanged += Txt_colDelimiter_TextChanged;
        }

        
        private void btn_file_Click(object sender, RoutedEventArgs e)
        {
            
            Microsoft.Win32.OpenFileDialog of = new Microsoft.Win32.OpenFileDialog();
            of.Filter = Lang.OF_File;
            of.Multiselect = false;
            if((bool)of.ShowDialog())
            {
                bCore.LoadFile(of.FileName);
                lba_file.Content = of.FileName;
                ChangeData();
            }
        }

      
        public void ChangeData()
        {
            if (bCore.DataSource == null)
                return;
            lba_number.Content = string.Format("{0} {1}", bCore.DataSource.Count,Lang.Rows);
            if (bCore.DataSource.Count > 0)
                lba_numberColumn.Content = string.Format("{0} {1}", bCore.DataSource[0].Length,Lang.Columns);
        }


        private void Txt_colDelimiter_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txt_colDelimiter.Text))
            {
                bCore.delimiterColumn = txt_colDelimiter.Text;
                bCore.ChangeData();
                ChangeData();
            }
        }

        private void Txt_delimiter_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txt_delimiter.Text))
            {
                bCore.delimiter = txt_delimiter.Text;
                bCore.ChangeData();
                ChangeData();
            }
        }

    }
}
