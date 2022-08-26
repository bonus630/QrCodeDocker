using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Windows.Input;
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
        public RoutedCommand<string> OpenXmlCommand { get; set; }
        public RoutedCommand<string> OpenIconFolderCommand { get; set; }
        //public CommandBinding OpenXmlCommand { get; set; }
        private string xmlPath,iconPath;

        public System.Windows.Media.Imaging.BitmapSource OpenXmlIcon { get { return BitmapResources.generateBitmapSource(Properties.Resource.xml); } }
        public System.Windows.Media.Imaging.BitmapSource OpenFolderIcon { get { return BitmapResources.generateBitmapSource(Properties.Resource.folder); } }

        public override string GetPluginDisplayName { get { return MediaSchemaCore.PluginDisplayName; } }

        public MediaSchemaCore()
        {
            try
            {
                FillSourceXml();
            }
            catch(Exception e)
            {
                throw e;
            }
            OpenXmlCommand = new RoutedCommand<string>(openXmlCommand);
            OpenIconFolderCommand = new RoutedCommand<string>(openIconFolderCommand);
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
                string spath = Path.Combine(System.Environment.GetEnvironmentVariable("localappdata"),"bonus630\\QrCodeDocker\\MediaScheme");
                //spath = spath.Substring(0, spath.LastIndexOf('\\'));
                iconPath = string.Format("{0}\\icons", spath);
                xmlPath = string.Format("{0}\\Schemes.xml", spath);
                XmlDocument doc = new XmlDocument();
                doc.Load(xmlPath);
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
                                image = string.Format("{0}\\{1}", iconPath, itemNode.InnerText);
                                if (!File.Exists(image))
                                    throw new Exception("Image not found in icons folder, please check, and reopen the Extra");
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
        private  void openXmlCommand(string i)
        {
            System.Diagnostics.Process.Start(xmlPath);
        }
        private void openIconFolderCommand(string i)
        {
            System.Diagnostics.Process.Start(iconPath);
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
