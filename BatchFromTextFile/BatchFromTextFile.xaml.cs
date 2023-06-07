using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using br.corp.bonus630.PluginLoader;
using System.Collections.Generic;
using System.Reflection;
using br.corp.bonus630.plugin.BatchFromTextFile.Lang;
using System.Diagnostics;

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
                if (of.SafeFileName.Contains(".csv"))
                {
                    txt_colDelimiter.Text = ",";
                    txt_delimiter.Text = "\n\r";
                }
                bCore.ChangeData();
            }
        }

      
   


        private void Txt_colDelimiter_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txt_colDelimiter.Text))
            {
                bCore.ColumnDelimiter = txt_colDelimiter.Text;
               
                bCore.ChangeData();
                
            }
        }

        private void Txt_delimiter_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txt_delimiter.Text))
            {
                bCore.RowDelimiter = txt_delimiter.Text;
                
                bCore.ChangeData();
                
            }
        }

        private void lba_file_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Process.Start(lba_file.Content.ToString());
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = (sender as ComboBox).SelectedIndex;

            bCore.RowDelimiter = bCore.SpecialCharsList[index].Value;

            bCore.ChangeData();
        }
        private void ComboBoxCol_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = (sender as ComboBox).SelectedIndex;

            bCore.RowDelimiter = bCore.SpecialCharsList[index].Value;

            bCore.ChangeData();
        }
    }
}
