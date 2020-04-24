using System;


namespace br.corp.bonus630.PluginLoader
{
    public interface IPluginConfig
    {
        Corel.Interop.VGCore.Application App { set; }
        ICodeGenerator CodeGenerator { get; set; }
        void OnGetCodeGenerator(object sender,Type type);
        event Action<object,Type> GetCodeGenerator;
    }
}
