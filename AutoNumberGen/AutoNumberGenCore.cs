using br.corp.bonus630.PluginLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace br.corp.bonus630.plugin.AutoNumberGen
{
    public class AutoNumberGenCore : PluginCoreBase<AutoNumberGenCore>, IPluginDataSource
    {
        public  const string PluginDisplayName = "Auto Number Generator";
        private List<object[]> dataSource;
        public List<object[]> DataSource { get { return dataSource; } }

        public int StartValue { get; set; }
        public int EndValue { get; set; }
      
        public override string GetPluginDisplayName { get { return AutoNumberGenCore.PluginDisplayName; } }

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

        public override void DeleteConfig()
        {
            Properties.Settings1.Default.Reset();
        }

        public override void LoadConfig()
        {
            this.StartValue = Properties.Settings1.Default.RangeStart;
            this.EndValue = Properties.Settings1.Default.RangeEnd;
            base.LoadConfig();
        }

        public override void SaveConfig()
        {
            Properties.Settings1.Default.RangeStart = this.StartValue;
            Properties.Settings1.Default.RangeEnd = this.EndValue;
            Properties.Settings1.Default.Save();

        }
    }
}
