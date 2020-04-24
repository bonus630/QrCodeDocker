namespace br.corp.bonus630.PluginLoader
{
    public class PluginMap
    {
        private int index;

        public int Index
        {
            get { return index; }
            set { index = value; }
        }

        private string dllFile;

        public string DllFile
        {
            get { return dllFile; }
            set { dllFile = value; }
        }
        private string displayName;

        public string DisplayName
        {
            get { return displayName; }
            set { displayName = value; }
        }
        public PluginMap(int index,string dllFile,string displayName)
        {
            this.index = index;
            this.dllFile = dllFile;
            this.displayName = displayName;
        }
        public override string ToString()
        {
            return this.displayName;
        }
    }
}
