using br.corp.bonus630.PluginLoader;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Linq;
using Microsoft.Win32;
using br.corp.bonus630.QrCodeDocker.Lang;

namespace br.corp.bonus630.QrCodeDocker
{

    public partial class PluginSelect : UserControl
    {

        private List<IPluginUI> loadedPluginList = null;
        public Ilang Lang { get; set; }
        List<PluginMap> pluginNames;
        Loader loader;
        double size;
        public double Size { get { return this.size; } set { this.size = value; } }
        Corel.Interop.VGCore.Application app;
        //ImageRender.IImageRender imageRender;
        private List<object[]> dataSource;
        public bool PluginFound { get; set; }
        private ICodeGenerator codeGenerator;
        private int index = 0;
        public event Action<string> AnyTextChanged;
        public event Action UpdatePreview;


        public PluginSelect(double size, Corel.Interop.VGCore.Application app, Ilang lang, ImageRender.IImageRender imageRender, ICodeGenerator codeGenerator)
        {
            InitializeComponent();
            try
            {
                loader = new Loader(app.AddonPath);
                Lang = lang;
                pluginNames = loader.PluginList();
                if (pluginNames.Count == 0)
                {
                    app.MsgShow("No extras found!");
                    //return;
                }
                for (int i = 0; i < pluginNames.Count; i++)
                {
                    cb_plugins.Items.Add(pluginNames[i]);
                }
                this.size = size;
                this.app = app;
                //this.imageRender = imageRender;
                this.codeGenerator = codeGenerator;
                loadedPluginList = new List<IPluginUI>();
                this.PluginFound = true;
                this.DataContext = this;
                LoadConfig();
            }

            catch (Exception erro)
            {
                app.MsgShow(string.Format("{0} | {1} | {2}", erro.Message,erro.Source,erro.TargetSite.Name));
                this.PluginFound = false;
                // this.Title = "No extras found!";
            }
        }





        //public void SetValues(double size, Corel.Interop.VGCore.Application app,ImageRender.IImageRender imageRender)

        public void SetValues(double size, Corel.Interop.VGCore.Application app, ICodeGenerator codeGenerator)
        {
            if (loadedPluginList == null)
                return;
            this.size = size;
            for (int i = 0; i < loadedPluginList.Count; i++)
            {
                //SetValues(loadedPluginList[i], size, app, imageRender);
                SetValues(loadedPluginList[i], size, app, codeGenerator);
            }
        }
        //public void SetValues(object obj, double size, Corel.Interop.VGCore.Application app, ImageRender.IImageRender imageRender)
        public void SetValues(object obj, double size, Corel.Interop.VGCore.Application app, ICodeGenerator codeGenerator)
        {
            if (obj == null)
                return;
            this.size = size;
            if (typeof(IPluginDrawer).IsAssignableFrom(obj.GetType()))
            {
                (obj as IPluginDrawer).App = app;
                (obj as IPluginDrawer).Size = size;
                //(obj as IPluginDrawer).ImageRender = imageRender;
                (obj as IPluginDrawer).CodeGenerator = codeGenerator;
            }
            if (typeof(IPluginConfig).IsAssignableFrom(obj.GetType()))
                SetValues(obj as IPluginConfig, typeof(QrCodeGenerator), app);

        }
        public void SetValues(IPluginConfig plugin, Type type, Corel.Interop.VGCore.Application app)
        {
            if (codeGenerator == null || loadedPluginList == null)
                throw new Exception("Erros ");
            if (this.codeGenerator.GetType() == type)
            {
                plugin.CodeGenerator = codeGenerator;
                plugin.App = app;
            }

        }




        private void cb_plugins_DropDownClosed(object sender, EventArgs e)
        {
            PluginMap pluginMap = (PluginMap)cb_plugins.SelectedItem;
            InflateUI(pluginMap);
            cb_plugins.SelectedIndex = -1;

        }
        private void btn_loadPlugin_Click(object sender, RoutedEventArgs e)
        {

            OpenFileDialog of = new OpenFileDialog();
            of.Filter = "DLL|*.dll";
            of.Title = "Select a dll file";
            of.Multiselect = true;
            if ((bool)of.ShowDialog())
            {
                for (int i = 0; i < of.FileNames.Length; i++)
                {
                    PluginMap pluginMap = loader.GetPluginMap(of.FileNames[i]);
                    if (pluginMap == null)
                    {
                        app.MsgShow(of.FileNames[i], "File invalid!", QrCodeDocker.MessageBox.DialogButtons.Ok);

                    }
                    InflateUI(pluginMap);
                }

            }
        }

        private void expanderRemoveClick(object sender, RoutedEventArgs e)
        {

            //var teste = ((sender as Button).TemplatedParent as StackPanel).Parent;
            int index = (int)(sender as Button).Tag;


            for (int i = 0; i < this.loadedPluginList.Count; i++)
            {
                if (index == this.loadedPluginList[i].Index)
                {
                    this.loadedPluginList.RemoveAt(i);
                    grid_controlUI.Children.RemoveAt(i);
                    if (grid_controlUI.Children.Count == 0)
                    {
                        //this.Title = "Extras Select";
                    }
                }
            }


        }
        private void expanderExpander(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < loadedPluginList.Count; i++)
            {
                if (typeof(IPluginDataSource).IsAssignableFrom(loadedPluginList[i].GetType()))
                {
                    List<object[]> dataSource = (loadedPluginList[i] as IPluginDataSource).DataSource;
                    if (dataSource != null && dataSource.Count > 0)
                        SetDataSource(dataSource);
                }

            }


        }

        private void PluginSelect_GetCodeGenerator(object sender, Type type)
        {
            SetValues(sender as IPluginConfig, type, this.app);
        }

        void PluginSelect_FinishJob(object obj)
        {
            pb_progress.Dispatcher.Invoke
                (new Action(() =>
            {
                pb_progress.Visibility = Visibility.Collapsed;

                for (int i = 0; i < loadedPluginList.Count; i++)
                {
                    if (typeof(IPluginDataSource).IsAssignableFrom(loadedPluginList[i].GetType()))
                    {
                        this.dataSource = (loadedPluginList[i] as IPluginDataSource).DataSource;
                        SetDataSource(this.dataSource);
                    }

                }
            }));
        }
        public void SetDataSource(List<object[]> dataSource)
        {
            if (loadedPluginList == null && dataSource == null && dataSource.Count == 0)
                return;
            if (dataSource != this.dataSource)
                this.dataSource = dataSource;
            for (int i = 0; i < loadedPluginList.Count; i++)
            {
                if (typeof(IPluginDrawer).IsAssignableFrom(loadedPluginList[i].GetType()) && dataSource != null)
                {
                    (loadedPluginList[i] as IPluginDrawer).DataSource = dataSource;
                }

            }
        }
        void PluginSelect_ProgressChange(int obj)
        {
            pb_progress.Dispatcher.Invoke(new Action(() =>
            {
                pb_progress.Visibility = Visibility.Visible;
                pb_progress.Value = obj;
            }));
        }
        private void InflateUI(PluginMap pluginMap)
        {
            try
            {
                object objUI = loader.GetUIControl(pluginMap);
                if (objUI == null)
                    return;

                (objUI as IPluginUI).Index = index;

                Expander cont = new Expander();

                DataTemplate dt = new DataTemplate(typeof(Expander));
                FrameworkElementFactory stackPanel = new FrameworkElementFactory(typeof(StackPanel));
                stackPanel.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);

                FrameworkElementFactory label = new FrameworkElementFactory(typeof(Label));
                label.SetValue(Label.ContentProperty, pluginMap.DisplayName);
                LengthConverter lc = new LengthConverter();
                string qualifiedDouble = "250px";

                var converted = lc.ConvertFrom(qualifiedDouble);

                label.SetValue(Label.WidthProperty, converted);
                FrameworkElementFactory btn = new FrameworkElementFactory(typeof(Button));
                btn.SetValue(Button.ContentProperty, "-");
                var btnWidth = lc.ConvertFromInvariantString("20px");
                btn.SetValue(Button.WidthProperty, btnWidth);
                btn.SetValue(Button.TagProperty, index);
                btn.AddHandler(Button.ClickEvent, new RoutedEventHandler(expanderRemoveClick));
                stackPanel.AppendChild(label);
                stackPanel.AppendChild(btn);


                cont.HeaderTemplate = dt;
                dt.VisualTree = stackPanel;


                cont.IsExpanded = true;
                cont.SetValue(Expander.TagProperty, pluginMap.DisplayName);
                cont.Content = (UserControl)objUI;
                var t = new ThicknessConverter();
                object thi = t.ConvertFromInvariantString("0,0,0,14");
                cont.SetValue(Control.MarginProperty, thi);
                cont.AddHandler(Expander.ExpandedEvent, new RoutedEventHandler(expanderExpander));
                grid_controlUI.Children.Add(cont);
                loadedPluginList.Add(objUI as IPluginUI);

                SetValues(objUI, size, app, codeGenerator);
                // this.Title = pluginMap.DisplayName;

                (objUI as IPluginUI).ProgressChange += PluginSelect_ProgressChange;
                (objUI as IPluginUI).FinishJob += PluginSelect_FinishJob;
                (objUI as IPluginUI).AnyTextChanged += PluginSelect_AnyTextChanged;
                (objUI as IPluginUI).UpdatePreview += PluginSelect_UpdatePreview; ;
                (objUI as IPluginUI).ChangeLang(app.UILanguage.cdrLangToSys());
                if (typeof(IPluginConfig).IsAssignableFrom(objUI.GetType()))
                    (objUI as IPluginConfig).GetCodeGenerator += PluginSelect_GetCodeGenerator;
                SetDataSource(this.dataSource);

                index++;
            }
            catch (Exception erro)
            {
                app.MsgShow(erro.Message);
            }
        }

        private void PluginSelect_UpdatePreview()
        {
            if (UpdatePreview != null)
                UpdatePreview();
        }

        private void PluginSelect_AnyTextChanged(string obj)
        {
            if (AnyTextChanged != null)
                AnyTextChanged(obj);
        }

        private void btn_saveConfig_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.PluginNameCollection = new System.Collections.Specialized.StringCollection();
            IPluginUI plugin;
            for (int i = 0; i < loadedPluginList.Count; i++)
            {
                plugin = loadedPluginList[i];
                Properties.Settings.Default.PluginNameCollection.Add(plugin.PluginDisplayName);
                plugin.SaveConfig();
            }
            Properties.Settings.Default.Save();
        }

        private void btn_deleteConfig_Click(object sender, RoutedEventArgs e)
        {
            IPluginUI plugin;
            if (loadedPluginList.Count < Properties.Settings.Default.PluginNameCollection.Count)
                this.app.MsgShow(Lang.MBOX_ERROR_SettingsCountNoMatch);
            for (int i = 0; i < loadedPluginList.Count; i++)
            {
                plugin = loadedPluginList[i];
                for (int r = 0; r < this.pluginNames.Count; r++)
                {
                    if (plugin.PluginDisplayName.Equals(pluginNames[r].DisplayName))
                        plugin.DeleteConfig();
                }
            }
            Properties.Settings.Default.PluginNameCollection.Clear();
            Properties.Settings.Default.Save();
        }
        public void LoadConfig()
        {
            var pluginNames = Properties.Settings.Default.PluginNameCollection;
            for (int i = 0; i < pluginNames.Count; i++)
            {
                for (int r = 0; r < this.pluginNames.Count; r++)
                {
                    if (pluginNames[i].Equals(this.pluginNames[r].DisplayName))
                    {
                        try
                        {
                            InflateUI(this.pluginNames[r]);
                        }
                        catch(Exception e) { Debug.WriteLine(e.Message,"LoadConfig"); }
                      
                        try
                        {
                            this.loadedPluginList[this.loadedPluginList.Count - 1].LoadConfig();
                        }
                        catch (Exception e) { Debug.WriteLine(e.Message, "LoadConfig"); }
                    }
                }
            }
        }
    }
}
