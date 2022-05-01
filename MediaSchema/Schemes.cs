using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Media.Imaging;
using br.corp.bonus630.QrCodeDocker;

namespace br.corp.bonus630.plugin.MediaSchema
{
    public class Schemes : INotifyPropertyChanged
    {
        private int tag;

        private string invariablePart;
        private System.Drawing.Bitmap resource;

        public int Tag { get { return tag; } }
        public string Name { get; set; }
        public SchemesAttribute[] SchemesAttributes { get; set; }
        public System.Drawing.Bitmap Resource { get { return this.resource; } }
        private bool isSelected = false;

        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                isSelected = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("IsSelected"));
            }
        }

        public BitmapSource MediaImage
        {
            get { return BitmapResources.generateBitmapSource(resource); }
        }
        public string FormatedURI(params string[] variablePart)
        {
            return string.Format(invariablePart, variablePart);
        }

        public Schemes(int tag, string invariablePart, System.Drawing.Bitmap resource, string name, SchemesAttribute[] attributes)
        {
            this.tag = tag;
            this.invariablePart = invariablePart;
            this.resource = resource;
            this.Name = name;
            SchemesAttributes = attributes;
        }
        public void SetAttributeValue(string attributeName, object[] value)
        {
            for (int i = 0; i < SchemesAttributes.Length; i++)
            {
                if (SchemesAttributes[i].Name.Equals(attributeName))
                {
                    SchemesAttributes[i].param = value;
                }
            }
        }
        public string[] AttributesValues
        {
            get
            {
                List<string> st = new List<string>();
                for (int i = 0; i < SchemesAttributes.Length; i++)
                {
                    if (SchemesAttributes[i].param != null)
                    {
                        for (int r = 0; r < SchemesAttributes[i].param.Length; r++)
                        {
                            st.Add(Uri.EscapeDataString(SchemesAttributes[i].param[r].ToString()));
                        }
                    }
                }
                return st.ToArray();
            }
        }
        public bool NotEmpty
        {
            get
            {
                for (int i = 0; i < SchemesAttributes.Length; i++)
                {
                    SchemesAttribute attribute = SchemesAttributes[i];
                    if (attribute.param == null)
                    {
                        return false;
                    }
                    if (attribute.param.Length == 0)
                        return false;
                }
                return true;
            }
        }
    }
    public class SchemesAttribute
    {
        public string Name { get; set; }
        public Type type { get; set; }
        public object[] param { get; set; }

        public SchemesAttribute(string name, Type type, params object[] param)
        {
            Name = name;
            this.type = type;
        }
    }
}
