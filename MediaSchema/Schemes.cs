using System;
using System.Collections;
using System.Windows.Media.Imaging;
using br.corp.bonus630.QrCodeDocker;

namespace br.corp.bonus630.plugin.MediaSchema
{
    public class Schemes
    {
        private int tag;
        
        private string invariablePart;
        private System.Drawing.Bitmap resource;

        public int Tag { get { return tag; } }
        public string Name { get; set; }
        public SchemesAttribute[] SchemesAttributes { get; set; }
        public BitmapSource MediaImage { get { return BitmapResources.genereteBitmapSource(resource); } }
        public string FormatedURI(params string[] variablePart)
        {
            return string.Format(invariablePart, variablePart);
        }

        public Schemes(int tag, string invariablePart,System.Drawing.Bitmap resource,string name,SchemesAttribute[] attributes)
        {
            this.tag = tag;
            this.invariablePart = invariablePart;
            this.resource = resource;
            this.Name = name;
            SchemesAttributes = attributes;
        }
        public void SetAttributeValue(string attributeName,object[] value)
        {
            for (int i = 0; i < SchemesAttributes.Length; i++)
            {
                if(SchemesAttributes[i].Name.Equals(attributeName))
                {
                    SchemesAttributes[i].param = value;
                }
            }
        }
    }
  
    public class SchemesAttribute
    {
        public string Name { get; set; }
        public Type type { get; set; }
        public object[] param { get; set; }
        public SchemesAttribute(string name,Type type,params object[] param)
        {
            Name = name;
            this.type = type;
        }
    }
}
