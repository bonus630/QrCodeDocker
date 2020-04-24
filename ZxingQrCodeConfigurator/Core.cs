using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using br.corp.bonus630.PluginLoader;

namespace br.corp.bonus630.plugin.ZxingQrCodeConfigurator
{
    public class Core : IPluginCore
    {
        public const string PluginDisplayName = "Qrcode Configuration";
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
