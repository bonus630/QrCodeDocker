using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
//using Corel.Interop.VGCore;
using System.IO;
using br.corp.bonus630.PluginLoader;

using System.Windows.Input;
using System.Collections;

namespace br.corp.bonus630.plugin.MediaSchema
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class MediaSchemaUI : UserControl, IPluginUI, IPluginDrawer
    {

        Schemes currentScheme = null;
        Corel.Interop.VGCore.Application app;
        ICodeGenerator codeGenerator;
        List<Schemes> schemesDataSource = new List<Schemes>();
        public int Index { get; set; }
        public List<object[]> DataSource { set { } }
        public double Size { set { } }
        public Corel.Interop.VGCore.Application App { set { app = value; } }
        public ICodeGenerator CodeGenerator { set { codeGenerator = value; } }

        public string GroupName { get { return "RadioGroupSchemasIcons"; } }

        public event Action<object> FinishJob;
        public event Action<int> ProgressChange;
        public event Action<string> AnyTextChanged;
        public event Action UpdatePreview;
        public List<Schemes> SchemesDataSource { get { return this.schemesDataSource; } set { this.schemesDataSource = value; } }
        ToolTip tooltip = new ToolTip();
        public string PluginDisplayName { get { return Core.PluginDisplayName; } }

        public MediaSchemaUI()
        {
            InitializeComponent();

            FillSource();
            this.DataContext = this;

        }
        public void ChangeLang(LangTagsEnum langTag)
        {

        }
        private void FillSource()
        {
            schemesDataSource.Add(
                new Schemes(0, "instagram://user?username={0}", Properties.Resource.instagran, "Instagran",
                new SchemesAttribute[] { new SchemesAttribute("User", typeof(string)) })
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
            //schemesDataSource.Add(
            //  new Schemes(4, "WIFI:S:{0};T:{1};P:{2};H:{3};", Properties.Resource.wifi, "Wifi",
            //    new SchemesAttribute[] { new SchemesAttribute("SSID", typeof(string)),
            //    new SchemesAttribute("Encryption type", typeof(string)),
            //    new SchemesAttribute("Password", typeof(string)),
            //    new SchemesAttribute("SSID Hidden", typeof(bool))})
            //  );
        
        }

        private void OnUpdatePreview()
        {
            if (UpdatePreview != null)
                UpdatePreview();
        }
     

        private void buildCheckBox(SchemesAttribute attribute, int index)
        {
            CheckBox ck = new CheckBox();
            ck.Tag = attribute.Name;
            ck.Content = attribute.Name;

        }

        private TextBox buildTextBox(SchemesAttribute attribute, int index)
        {
            TextBox textBox = new TextBox();
            textBox.HorizontalAlignment = HorizontalAlignment.Left;
            textBox.VerticalAlignment = VerticalAlignment.Center;
            textBox.VerticalContentAlignment = VerticalAlignment.Center;
            textBox.MinWidth = 200;
            textBox.Height = 32;
            //textBox.Text = attribute.param[0].ToString();
            textBox.Tag = attribute.Name;
            textBox.TextChanged += txt_any_TextChanged;
            return textBox;
        }

        private Label buildLabel(SchemesAttribute attribute, int index)
        {
            Label label = new Label();
            label.Content = attribute.Name;
            label.Width = 70;
            label.VerticalAlignment = VerticalAlignment.Center;
            return label;
        }
        private Grid buildGrid(int numRows, int numColumns)
        {
            Grid grid = new Grid();
            GridLengthConverter glc = new GridLengthConverter();
            GridLength height = (GridLength)glc.ConvertFromString("36px");
            GridLength width = (GridLength)glc.ConvertFromString("74px");
            for (int i = 0; i < numRows; i++)
            {
                RowDefinition rd = new RowDefinition();
                rd.Height = height;
                grid.RowDefinitions.Add(rd);
            }
            for (int i = 0; i < numColumns; i++)
            {
                ColumnDefinition cd = new ColumnDefinition();
                if (i < numColumns - 1)
                    cd.Width = width;
                grid.ColumnDefinitions.Add(cd);
            }
            return grid;

        }



        public void OnFinishJob(object obj)
        {
            throw new NotImplementedException();
        }

        public void OnProgressChange(int progress)
        {
            throw new NotImplementedException();
        }

   
        private void txt_any_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox txt = sender as TextBox;
            currentScheme.SetAttributeValue(txt.Tag.ToString(), new object[] { txt.Text });
            if (AnyTextChanged != null && currentScheme.NotEmpty)
                AnyTextChanged(currentScheme.FormatedURI(currentScheme.AttributesValues));

        }
        public void Draw()
        {
            throw new NotImplementedException();
        }

        public void SaveConfig()
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

        public void LoadConfig()
        {
            //tenho que alterar o valor da imagem correspondente do schema
            currentScheme = schemesDataSource.Single(r => r.Tag == Properties.Settings1.Default.CurrentSchema);
            currentScheme.IsSelected = true;
            if (Properties.Settings1.Default.SchemaData != null) {
                for (int i = 0; i < currentScheme.SchemesAttributes.Length; i++)
                {
                    currentScheme.SchemesAttributes[i].param = Properties.Settings1.Default.SchemaData[i] as object[];
                }
            }
            SelectSchema(currentScheme.Tag);
        }

        public void DeleteConfig()
        {
            Properties.Settings1.Default.Reset();
        }
        private void SelectSchema(int schemaTag)
        {
            sp_attributesContent.Children.Clear();
            currentScheme = schemesDataSource.Single(r => r.Tag == schemaTag);

            scheme_type.Content = currentScheme.Name;

            Grid grid = buildGrid(currentScheme.SchemesAttributes.Length, 2);
            for (int i = 0; i < currentScheme.SchemesAttributes.Length; i++)
            {
                SchemesAttribute attribute = currentScheme.SchemesAttributes[i];

                switch (Type.GetTypeCode(attribute.type))
                {
                    case TypeCode.String:
                        Label label = buildLabel(attribute, i);
                        TextBox txt = buildTextBox(attribute, i);
                        
                        Grid.SetColumn(label, 0);
                        Grid.SetRow(label, i);
                        Grid.SetColumn(txt, 1);
                        Grid.SetRow(txt, i);
                        grid.Children.Add(label);
                        grid.Children.Add(txt);
                        break;
                    case TypeCode.Boolean:
                        buildCheckBox(attribute, i);
                        break;
                }
            }
            sp_attributesContent.Children.Add(grid);
        }
        private void RadioButton_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.RadioButton button = sender as System.Windows.Controls.RadioButton;
            int tag = (int)button.Tag;
            SelectSchema(tag);
        }

    }
}
