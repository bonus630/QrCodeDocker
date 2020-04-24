using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Corel.Interop.VGCore;
using System.IO;
using br.corp.bonus630.PluginLoader;

using System.Windows.Input;

namespace br.corp.bonus630.plugin.MediaSchema
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class MediaSchemaUI : UserControl,IPluginUI,IPluginDrawer
    {

        Schemes currentScheme = null;
        Corel.Interop.VGCore.Application app;
        ICodeGenerator codeGenerator;
        List<Schemes> schemesDataSource = new List<Schemes>();
        ToolTip tooltip = new ToolTip();


        public MediaSchemaUI()
        {
            InitializeComponent();
            FillSource();
        }
        private void FillSource()
        {
            schemesDataSource.Add(
                new Schemes(0, "instagram://user?username={0}", Properties.Resource.instagran,"Instagran",
                new SchemesAttribute[] {new SchemesAttribute("Instagran user",typeof(string)) })
                );
            schemesDataSource.Add(
               new Schemes(1, "twitter://user?id={0}", Properties.Resource.twitter,"Twitter",
                new SchemesAttribute[] { new SchemesAttribute("Twitter user", typeof(string)) })
               );
            //schemesDataSource.Add(
            //  new Schemes(2, "tel://{0}", Properties.Resource.tel,"Phone",
            //    new SchemesAttribute[] { new SchemesAttribute("Phone", typeof(string)) })
            //  );
            //schemesDataSource.Add(
            //  new Schemes(3, "WIFI:S:{0};T:{1};P:{2};H:{3};", Properties.Resource.wifi, "Wifi",
            //    new SchemesAttribute[] { new SchemesAttribute("SSID", typeof(string)),
            //    new SchemesAttribute("Encryption type", typeof(string)),
            //    new SchemesAttribute("Password", typeof(string)),
            //    new SchemesAttribute("SSID Hidden", typeof(bool))})
            //  );
            for (int i = 0; i < schemesDataSource.Count; i++)
            {
                System.Windows.Controls.Image image = new System.Windows.Controls.Image();
                image.Source = schemesDataSource[i].MediaImage;
                image.Width = 80;
                image.Height = 80;
                image.Tag = schemesDataSource[i].Tag;
                image.Cursor = Cursors.Hand;
                image.ToolTip = tooltip;
                image.MouseLeave += Image_MouseLeave;

                image.MouseUp += Image_MouseUp;
                image.MouseEnter += Image_MouseEnter;
                content.Children.Add(image);
            }
            //content.Children.Add(new BaseControls(null));
        }

        private void Image_MouseEnter(object sender, MouseEventArgs e)
        {
            System.Windows.Controls.Image image = sender as System.Windows.Controls.Image;
            int tag = (int)image.Tag;
            Schemes scheme = schemesDataSource.Single(r => r.Tag == tag);
            tooltip.Content = scheme.Name;
        }

        private void Image_MouseLeave(object sender, MouseEventArgs e)
        {
            tooltip.IsOpen = false;
        }

        private void Image_MouseUp(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Controls.Image image = sender as System.Windows.Controls.Image;
            int tag = (int)image.Tag;
            currentScheme =  schemesDataSource.Single(r => r.Tag == tag);
            for (int i = 0; i < content.Children.Count; i++)
            {
                content.Children[i].Opacity = 1;
            }
            image.Opacity = 0.5;
            scheme_type.Content = currentScheme.Name;
            txt_msg.Visibility = Visibility.Visible;
            Txt_msg_TextChanged(null, null);
        }

        public int Index { get; set; }
        public List<object[]> DataSource { set { } }
        public double Size { set { } }
        public Corel.Interop.VGCore.Application App { set { app = value; } }
        public ICodeGenerator CodeGenerator { set { codeGenerator = value; } }

        public event Action<object> FinishJob;
        public event Action<int> ProgressChange;
        public event Action<string> AnyTextChanged;

        //public void Draw()
        //{
        //    if (currentScheme != null)
        //    {
        //        //string text = "";
        //        //for (int i = 0; i < schemes.Children.Count; i++)
        //        //{
        //        ////    if(schemes.Children[i].t)
        //        ////    {

        //        ////    }
        //        //}
        //        codeGenerator.CreateBitmapLocal(app.ActiveLayer, currentScheme.FormatedURI(txt_msg.Text), 100);
        //    }
        //}

        public void OnFinishJob(object obj)
        {
            throw new NotImplementedException();
        }

        public void OnProgressChange(int progress)
        {
            throw new NotImplementedException();
        }

        private void Txt_msg_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (currentScheme != null && !string.IsNullOrEmpty(txt_msg.Text))
            {
                if (AnyTextChanged != null)
                    AnyTextChanged(currentScheme.FormatedURI(txt_msg.Text));
            }
        }

        public void Draw()
        {
            throw new NotImplementedException();
        }



        //private void btn_draw_Click(object sender, RoutedEventArgs e)
        //{
        //    Draw();
        //}
    }
}
