using System;
using System.Collections.Generic;
using System.Linq;
using br.corp.bonus630.PluginLoader;
using System.IO;
using System.Text;
using br.corp.bonus630.plugin.BatchFromTextFile.Lang;

namespace br.corp.bonus630.plugin.BatchFromTextFile
{
    public class BatchFromTextFileCore : PluginCoreBase<BatchFromTextFileCore>, IPluginDataSource
    {
        public const string PluginDisplayName = "Data From Text File";

        public List<string> content;
        private string rowDelimiter = "\n";
        public string RowDelimiter
        {
            get
            {
                return rowDelimiter;
            }
            set
            {
               rowDelimiter = value;
                
                OnNotifyPropertyChanged("RowDelimiter");
                
            }
        }
        private string columnDelimiter = "|";
        public string ColumnDelimiter
        {
            get
            {
                return columnDelimiter;
            }
            set
            {
                columnDelimiter = value;
                OnNotifyPropertyChanged("ColumnDelimiter"); 
                
            }
        }
        private string rowCountText;

        public string RowCountText
        {
            get { return rowCountText; }
            set { rowCountText = value; OnNotifyPropertyChanged("RowCountText"); }
        }
        private string columnCountText;

        public string ColumnCountText
        {
            get { return columnCountText; }
            set { columnCountText = value; OnNotifyPropertyChanged("ColumnCountText"); }
        }

        private List<object[]> dataSource;
        public List<object[]> DataSource { get { return dataSource; } }
        public override string GetPluginDisplayName { get { return BatchFromTextFileCore.PluginDisplayName; } }

        private string filePath;
        public string FilePath { get { return filePath; } set { filePath = value; OnNotifyPropertyChanged("FilePath"); } }

        public  SpecialCharsTable[] SpecialCharsList { get; internal set; }

        public BatchFromTextFileCore()
        {
            SpecialCharsList = new SpecialCharsTable[] {
                new SpecialCharsTable("\'","\'"),
                new SpecialCharsTable("\"", "\""),
                new SpecialCharsTable("\\", "\\"),
                new SpecialCharsTable("\n","NewLine"),
                new SpecialCharsTable("\t","Tab"),
                new SpecialCharsTable("\b","Backspace") };
        }

        public void Process()
        {

            //OnFinishJob();
        }

        public void LoadFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                this.filePath = filePath;
                ChangeData();

            }
        }
        public void ChangeTextData()
        {
            try
            {
                if (this.DataSource == null)
                    return;
                RowCountText = string.Format("{0} {1}", this.DataSource.Count, (Lang as Ilang).Rows);
                if (this.DataSource.Count > 0)
                    ColumnCountText = string.Format("{0} {1}", this.DataSource[0].Length, (Lang as Ilang).Columns);
            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show(e.Message);
            }
        }
        public void ChangeData()
        {
            if (string.IsNullOrEmpty(filePath))
                return;
            dataSource = new List<object[]>();
            try
            {
                using (StreamReader sr = new StreamReader(filePath, true))

                {

                    int primarySize = 0;
                    string todo = sr.ReadToEnd();
                    string[] pieces = todo.Split(new String[] { rowDelimiter }, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < pieces.Length; i++)
                    {
                        try
                        {
                            object[] temp = pieces[i].Split(new String[] { columnDelimiter }, StringSplitOptions.RemoveEmptyEntries);

                            if (i == 0)
                                primarySize = temp.Length;
                            if (primarySize == temp.Length)
                                dataSource.Add(temp);
                        }
                        catch (Exception e)
                        {
                            throw e;
                        }
                    }
                    ChangeTextData();
                    OnFinishJob(this.dataSource);
                }
            }
            catch (IOException e)
            {
                throw e;
            }
        }

        protected override void OnProgressChange(int progress)
        {
            int p = (int)100 * progress / content.Count;
            base.OnProgressChange(p);
        }

        public override void SaveConfig()
        {
            Properties.Settings1.Default.FilePath = filePath;
            Properties.Settings1.Default.ColumnDelimiter = columnDelimiter;
            Properties.Settings1.Default.RowDelimiter = rowDelimiter;
            Properties.Settings1.Default.Save();
            base.SaveConfig();

        }

        public override void LoadConfig()
        {
            FilePath = Properties.Settings1.Default.FilePath;
            ColumnDelimiter = Properties.Settings1.Default.ColumnDelimiter;
            RowDelimiter = Properties.Settings1.Default.RowDelimiter;
            ChangeData();
            base.LoadConfig();
        }

        public override void DeleteConfig()
        {
            Properties.Settings1.Default.Reset();
        }
    }
    public class SpecialCharsTable
    {
        public string Text { get; set; }
        public string Value { get; set; }

        public SpecialCharsTable(string value,string text)
        {
            Text = text;
            Value = value;
        }
        public override string ToString()
        {
            return Text;
        }
    }
}
