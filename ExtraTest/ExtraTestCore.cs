using System;
using System.Collections.Generic;
using br.corp.bonus630.PluginLoader;
using Corel.Interop.VGCore;
namespace ExtraTest
{
    class ExtraTestCore : PluginCoreBase<ExtraTestCore>,IPluginDrawer
    {
        public const string PluginDisplayName = "Extra Test";
        public override string GetPluginDisplayName { get { return ExtraTestCore.PluginDisplayName; } }

        private int start;

        public int Start
        {
            get { return start; }
            set { start = value; OnNotifyPropertyChanged("Start"); }
        }
        private int end;

        public int End
        {
            get { return end; }
            set { end = value;OnNotifyPropertyChanged("End"); }
        }

        public List<object[]> DataSource { get; set; }
        public double Size { get; set; }
        public global::Corel.Interop.VGCore.Application App { get; set; }
        public ICodeGenerator CodeGenerator { get; set; }

        public override void DeleteConfig()
        {
            Properties.Settings.Default.Reset();
        }

        public void Draw()
        {
            for (int i = Start; i <= End; i++)
            {
                CodeGenerator.CreateVetorLocal2(App.ActiveLayer, i.ToString(), Size);
            }
        }
    }
}
