using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using br.corp.bonus630.PluginLoader;

namespace br.corp.bonus630.plugin.DataFromMysql
{
    public class MysqlCore : IPluginCore, IPluginDataSource
    {
        public const string PluginDisplayName = "Data From Mysql";

        public List<object[]> DataSource => throw new NotImplementedException();

        public event Action<object> FinishJob;
        public event Action<int> ProgressChange;

        public void OnFinishJob(object obj)
        {
            throw new NotImplementedException();
        }

        public void OnProgressChange(int progress)
        {
            throw new NotImplementedException();
        }
    }
}
