using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Xml;
using br.corp.bonus630.PluginLoader;
using Corel.Interop.VGCore;

namespace br.corp.bonus630.plugin.MediaSchema
{
    public class MediaSchemaCore : PluginCoreBase<MediaSchemaCore>, IPluginDrawer, IPluginDataSource
    {
        public const string PluginDisplayName = "Media Scheme";

        private List<object[]> dataSource;
        private double size;
        private Application app;
        private ICodeGenerator codeGenerator;
        public List<object[]> DataSource { get { return this.dataSource; } set { this.dataSource = value; } }
        public double Size { set { this.size = value; } }
        public Application App { set { this.app = value; } }
        public ICodeGenerator CodeGenerator { set { this.codeGenerator = value; } }
        Schemes currentScheme = null;
        public Schemes CurrentScheme { get { return currentScheme; } set { currentScheme = value; } }

        List<Schemes> schemesDataSource = new List<Schemes>();

        public List<Schemes> SchemesDataSource { get { return this.schemesDataSource; } set { this.schemesDataSource = value; } }

        public string GroupName { get { return "RadioGroupSchemasIcons"; } }
        public override string GetPluginDisplayName { get { return MediaSchemaCore.PluginDisplayName; } }

        public MediaSchemaCore()
        {
            FillSourceXml();
            // FillSource();
        }
        public void Draw()
        {

        }
        public void OnAnyTextChanged(string tag, object[] param)
        {
            currentScheme.SetAttributeValue(tag, param);
            if (currentScheme.NotEmpty)
            {
                base.OnAnyTextChanged(currentScheme.FormatedURI(currentScheme.AttributesValues));

                this.dataSource = new List<object[]>() { new object[] { currentScheme.FormatedURI(currentScheme.AttributesValues), currentScheme.Resource } };
                OnFinishJob(this.dataSource);
            }
        }
        public void FillSourceXml()
        {
            try
            {
                string spath = System.Reflection.Assembly.GetExecutingAssembly().Location;
                spath = spath.Substring(0, spath.LastIndexOf('\\'));
                string path = string.Format("{0}\\Schemas.xml", spath);
                XmlDocument doc = new XmlDocument();
                doc.Load(path);
                XmlNode mainNode = doc.LastChild;

                for (int i = 0; i < mainNode.ChildNodes.Count; i++)
                {
                    XmlNode schemaNode = mainNode.ChildNodes[i];
                    string name = "";
                    string image = "";
                    string invariablePart = "";
                    SchemesAttribute[] attributes = null;
                    for (int r = 0; r < schemaNode.ChildNodes.Count; r++)
                    {

                        XmlNode itemNode = schemaNode.ChildNodes[r];
                        switch (itemNode.Name)
                        {
                            case "InvariablePart":
                                invariablePart = SecurityElement.Escape(itemNode.InnerText);
                                break;
                            case "Image":
                                image = string.Format("{0}\\icons\\{1}", spath, itemNode.InnerText);
                                break;
                            case "Name":
                                name = itemNode.InnerText;
                                break;
                            case "Attributes":
                                attributes = new SchemesAttribute[itemNode.ChildNodes.Count];
                                for (int j = 0; j < itemNode.ChildNodes.Count; j++)
                                {
                                    XmlNode attributeNode = itemNode.ChildNodes[j];
                                    switch (attributeNode.Attributes[0].Value)
                                    {
                                        case "string":
                                            attributes[j] = new SchemesAttribute(attributeNode.InnerText, typeof(string));
                                            break;
                                        case "bool":
                                            attributes[j] = new SchemesAttribute(attributeNode.InnerText, typeof(bool));
                                            break;
                                    }
                                }
                                break;
                        }

                    }
                    schemesDataSource.Add(new Schemes(i,invariablePart,new System.Drawing.Bitmap(image),name,attributes));
                }

            }
            catch (IOException ioe)
            {
                throw ioe;
            }
            catch (XmlException xmle)
            {
                throw xmle;
            }
            catch (Exception e)
            {
                throw e;
            }


        }
        public void FillSource()
        {
            schemesDataSource.Add(
                new Schemes(0, "instagram://user?username={0}", Properties.Resource.instagran, "Instagran",
                new SchemesAttribute[] { new SchemesAttribute("User", typeof(string)) })
                { IsSelected = true }
                );
            schemesDataSource.Add(
               new Schemes(1, "twitter://user?id={0}", Properties.Resource.twitter, "Twitter",
                new SchemesAttribute[] { new SchemesAttribute("User", typeof(string)) })
               );
            schemesDataSource.Add(
             new Schemes(2, "https://api.whatsapp.com/send?phone={0}{1}{2}&text={3}", Properties.Resource.whatsapp, "Whatsapp",
              new SchemesAttribute[] {
              new SchemesAttribute("DDI", typeof(string)),
              new SchemesAttribute("DDD", typeof(string)) ,
              new SchemesAttribute("TEL", typeof(string)),
              new SchemesAttribute("MSG", typeof(string)) })
             );
            schemesDataSource.Add(
              new Schemes(3, "tel://{0}{1}{2}", Properties.Resource.tel, "Phone",
                new SchemesAttribute[] {
                    new SchemesAttribute("DDI", typeof(string)),
                    new SchemesAttribute("DDD", typeof(string)) ,
                    new SchemesAttribute("Phone", typeof(string)) })
              );
            schemesDataSource.Add(
              new Schemes(4, "https://www.snapchat.com/add/{0}", Properties.Resource.snapchat, "Snapchat",
               new SchemesAttribute[] { new SchemesAttribute("User", typeof(string)) })
              );

            schemesDataSource.Add(
              new Schemes(5, "https://t.me/{0}", Properties.Resource.telegran, "Telegran",
               new SchemesAttribute[] { new SchemesAttribute("User", typeof(string)) })
              );
            schemesDataSource.Add(
             new Schemes(6, "https://bonus630.com.br/a/{0}", Properties.Resource.logo_circle_m, "Artigos bonus630",
              new SchemesAttribute[] { new SchemesAttribute("ID", typeof(string)) })
             );
            //schemesDataSource.Add(
            //  new Schemes(4, "WIFI:S:{0};T:{1};P:{2};H:{3};", Properties.Resource.wifi, "Wifi",
            //    new SchemesAttribute[] { new SchemesAttribute("SSID", typeof(string)),
            //    new SchemesAttribute("Encryption type", typeof(string)),
            //    new SchemesAttribute("Password", typeof(string)),
            //    new SchemesAttribute("SSID Hidden", typeof(bool))})
            //  );
            OnNotifyPropertyChanged("SchemesDataSource");
        }
        public override void SaveConfig()
        {
            if (currentScheme != null)
            {
                Properties.Settings1.Default.CurrentSchema = currentScheme.Tag;
                ArrayList schemasAttributes = new ArrayList();
                for (int i = 0; i < currentScheme.SchemesAttributes.Length; i++)
                {
                    schemasAttributes.Add(currentScheme.SchemesAttributes[i].param);
                }
                Properties.Settings1.Default.SchemaData = schemasAttributes;
                Properties.Settings1.Default.Save();
            }
        }
        public override void LoadConfig()
        {
            //tenho que alterar o valor da imagem correspondente do schema
            currentScheme = schemesDataSource.Single(r => r.Tag == Properties.Settings1.Default.CurrentSchema);
            currentScheme.IsSelected = true;
            if (Properties.Settings1.Default.SchemaData != null)
            {
                for (int i = 0; i < currentScheme.SchemesAttributes.Length; i++)
                {
                    currentScheme.SchemesAttributes[i].param = Properties.Settings1.Default.SchemaData[i] as object[];
                }
            }
            base.LoadConfig();
        }
        public override void DeleteConfig()
        {
            Properties.Settings1.Default.Reset();
        }
    }
}
