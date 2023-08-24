using System;
using System.Collections.Generic;
using br.corp.bonus630.PluginLoader;
using System.IO;
using Microsoft.Win32;
using br.corp.bonus630.QrCodeDocker;
using Corel.Interop.VGCore;
using System.Drawing;
using System.ComponentModel;
using System.Threading;

namespace br.corp.bonus630.plugin.QrTimer
{
    public class QrTimerCore : PluginCoreBase<QrTimerCore>, IPluginDrawer, INotifyPropertyChanged
    { 

        public List<object[]> DataSource
        {
            get
            {
                return dataSource;
            }
            set
            {
                if (dataSource == null)
                {
                    dataSource = value;
                   
                }
                else
                {
                    dataSource[0][0] = value[0][0];
                }
                CreatePreview("");
            }
        }
        public QrItemList QrItemList { get { return qrItemList; } set { qrItemList = value; } }
        public RelayCommand<QrItem> PlayCommand { get; set; }
        public double Size { get; set; }
        public Application App { get; set; }
        public ICodeGenerator CodeGenerator { get; set; }

        private QrItem currentItem;
        private Thread timer;
        private int tick = 100;
        private int cont = 0;
        public const string PluginDisplayName = "Qr Timer";
        private List<object[]> dataSource;
        private QrItemList qrItemList = new QrItemList();

        public override string GetPluginDisplayName { get { return PluginDisplayName; } }

        public override void DeleteConfig()
        {
            Properties.Settings.Default.Reset();
        }
        public QrTimerCore()
        {
            PlayCommand = new RelayCommand<QrItem>(play);
            timer = new Thread(Run);
            timer.IsBackground = true;
            timer.Start();
        }
        public void CreatePreview(string content)
        {
            //if (this.dataSource == null)
            //    return;
            //if (this.dataSource[0] == null)
            //    return;
            //if (this.dataSource[0].Length <= 1)
            //    return;
            //if(!this.dataSource[0][1].GetType().Equals(typeof(System.Drawing.Bitmap)))
            //    return;


            //System.Drawing.Bitmap code = CodeGenerator.ImageRender.RenderBitmapToMemory2(this.DataSource[0][0].ToString());
            System.Drawing.Bitmap code = CodeGenerator.ImageRender.RenderBitmapToMemory2(content);
            double m = CodeGenerator.ImageRender.InMeasure(Size);



            OnOverridePreview(code);
        }
        public void Draw()
        {
           

        }
        private bool running = true;
        public void Abort()
        {
            running = false;
            if(timer!=null && timer.IsAlive)
            {
                try
                {
                    timer.Abort();
                    timer = null;
                }
                catch { }
            }
        }
        private void Run()
        {
            while(running)
            {
                if(currentItem!=null && currentItem.Running)
                {
                    if (cont >= 1000)
                    {
                        currentItem.Time--;
                        cont = 0;
                    }
                    if(currentItem.Time < 1)
                    {
                        currentItem.Reset();
                        int index = qrItemList.IndexOf(currentItem);
                        index++;
                        if (index == qrItemList.Count)
                            index = 0;
                       play(qrItemList[index]);
                    }
                }
                cont += tick;
                Thread.Sleep(tick);
            }
        }
        public void play(QrItem item)
        {
            QrItemList.SetRunning(item,!item.Running);
            if(item.Running)
            {
                currentItem = item;
                OnAnyTextChanged(item.Content);
                //CreatePreview(item.Content);
            }

        }
        public void Browser()
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Title = "Selecione um arquivo de texto";
            openFile.Multiselect = false;
            openFile.Filter = "txt file|*.txt";
            if ((bool)openFile.ShowDialog())
            {
               var lines =  File.ReadAllLines(openFile.FileName);
                foreach (var line in lines)
                {
                   string[] itens = line.Split(new char[]{'|'},StringSplitOptions.RemoveEmptyEntries);
                    QrItem item = new QrItem(itens[0], Int32.Parse(itens[1]));
                    item.Running = false;
                    QrItemList.Add(item);
                
                }
               
            }
        }
    
    }
}
