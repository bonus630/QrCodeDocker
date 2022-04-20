using System;

namespace br.corp.bonus630.PluginLoader
{
    public interface IPluginMainUI
    {
        object DataContext { get; set; }
        //[Attribute(AttributeUsageAttribute("Don't try get Core in Contructor, use Loaded event"))]
        IPluginCore Core { get; set; }
    }
}
