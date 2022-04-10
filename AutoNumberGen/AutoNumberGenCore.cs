using br.corp.bonus630.PluginLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace br.corp.bonus630.plugin.AutoNumberGen
{
    public class AutoNumberGenCore : IPluginCore, IPluginDataSource
    {
        public const string PluginDisplayName = "Auto Number Generator";
        private List<object[]> dataSource;
        public List<object[]> DataSource { get { return dataSource; } }

        public event Action<object> FinishJob;
        public event Action<int> ProgressChange;
        public event Action UpdatePreview;

        public int StartValue { get; set; }
        public int EndValue { get; set; }

        public void OnFinishJob(object obj)
        {
            if (FinishJob != null)
                FinishJob(obj);
        }
        public void OnProgressChange(int progress)
        {
            
        }
        public void changeData(int startValue, int endValue)
        {
            StartValue = startValue;
            EndValue = endValue;
            dataSource = new List<object[]>();
            for (int i = startValue; i <= endValue; i++)
            {
                dataSource.Add(new object[] { i });
            }
            OnFinishJob(dataSource);
        }
      
    }
}
