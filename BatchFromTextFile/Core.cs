﻿using System;
using System.Collections.Generic;
using System.Linq;
using br.corp.bonus630.PluginLoader;
using System.IO;
using System.Text;

namespace br.corp.bonus630.plugin.BatchFromTextFile
{
    public class Core : IPluginCore, IPluginDataSource
    {
        public const string PluginDisplayName = "Data From Text File";

        public List<string> content;
        public string delimiter = "\n\r";
        public string delimiterColumn = "|";
        private List<object[]> dataSource;
        public List<object[]> DataSource { get { return dataSource; } }

        private string filePath;

        public Core()
        {

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
        public void ChangeData()
        {
            if (string.IsNullOrEmpty(filePath))
                return;
            dataSource = new List<object[]>();
            using (StreamReader sr = new StreamReader(filePath, true))

            {

                int primarySize = 0;
                string todo = sr.ReadToEnd();
                string[] pieces = todo.Split(delimiter.ToArray());
                for (int i = 0; i < pieces.Length; i++)
                {
                    try
                    {
                        object[] temp = pieces[i].Split(delimiterColumn.ToArray());

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
                OnFinishJob(this.dataSource);
            }
        }
        public void OnFinishJob(object obj)
        {
            if (FinishJob != null)
                FinishJob(obj);
        }
        public void OnProgressChange(int progress)
        {

            int p = (int)100 * progress / content.Count;
            if (ProgressChange != null)
                ProgressChange(progress);
        }

      

        public event Action<object> FinishJob;

        public event Action<int> ProgressChange;
       
    }
}
