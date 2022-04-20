using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.IO;
using br.corp.bonus630.PluginLoader;
using System.Windows.Input;
using System.Collections;

namespace br.corp.bonus630.plugin.MediaSchema
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class MediaSchemaUI : UserControl, IPluginMainUI
    {
        MediaSchemaCore msCore;
        ToolTip tooltip = new ToolTip();
        public IPluginCore Core { get ; set ; }
        public MediaSchemaUI()
        {
            InitializeComponent();
            this.Loaded += MediaSchemaUI_Loaded;
        }
        private void MediaSchemaUI_Loaded(object sender, RoutedEventArgs e)
        {
            msCore = Core as MediaSchemaCore;
            this.DataContext = msCore;
            msCore.LoadConfigEvent += MsCore_LoadConfigEvent;
        }

        private void MsCore_LoadConfigEvent()
        {
            InflateUISchema(msCore.CurrentScheme.Tag);
        }

        private CheckBox buildCheckBox(SchemesAttribute attribute, int index)
        {
            CheckBox ck = new CheckBox();
            ck.Tag = attribute.Name;
            ck.Content = attribute.Name;
            ck.Checked += Ck_Clicked;
            return ck;
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
   
        private void txt_any_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox txt = sender as TextBox;
            msCore.OnAnyTextChanged(txt.Tag.ToString(), new object[] { txt.Text });

        }
        private void Ck_Clicked(object sender, RoutedEventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            msCore.OnAnyTextChanged(cb.Tag.ToString(), new object[] { cb.IsChecked });
        }
        private void InflateUISchema(int schemaTag)
        {
            sp_attributesContent.Children.Clear();
            Schemes currentScheme = msCore.SchemesDataSource.Single(r => r.Tag == schemaTag);

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
                        CheckBox cb =  buildCheckBox(attribute, i);
                        Grid.SetColumn(cb, 0);
                        Grid.SetColumnSpan(cb, 2);
                        Grid.SetRow(cb, i);
                        grid.Children.Add(cb);
                        break;
                }
            }
            sp_attributesContent.Children.Add(grid);
        }
        private void RadioButton_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.RadioButton button = sender as System.Windows.Controls.RadioButton;
            int tag = (int)button.Tag;
            InflateUISchema(tag);
        }

    }
}
