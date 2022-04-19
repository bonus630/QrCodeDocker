using System;

namespace br.corp.bonus630.PluginLoader
{
    public interface IPluginMainUI
    {
        object DataContext { get; set; }
        IPluginCore Core { get; set; }
    }
}
