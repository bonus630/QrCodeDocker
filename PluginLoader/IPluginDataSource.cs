using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace br.corp.bonus630.PluginLoader
{
    public interface IPluginDataSource
    {
        List<object[]> DataSource { get; }
    }
}
