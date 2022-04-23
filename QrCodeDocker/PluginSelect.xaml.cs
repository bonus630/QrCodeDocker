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

        private List<IPluginCore> loadedPluginList = null;
        public Ilang Lang { get; set; }
        public List<PluginMap> PluginNames;
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
                PluginNames = loader.PluginList();
                if (PluginNames.Count == 0)
                {
                    app.MsgShow("No extras found!");
                    //return;
                }
                for (int i = 0; i < PluginNames.Count; i++)
                {
                    cb_plugins.Items.Add(PluginNames[i]);
                }
                this.size = size;
                this.app = app;
                //this.imageRender = imageRender;
                this.codeGenerator = codeGenerator;
                loadedPluginList = new List<IPluginCore>();
                this.PluginFound = true;
                this.DataContext = this;
                LoadConfig();
            }

            catch (Exception erro)
            {
                app.MsgShow(string.Format("{0} | {1} | {2}", erro.Message, erro.Source, erro.TargetSite.Name));
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
            if (pluginMap == null)
                return;
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
            RemovePluginUI(index);
        }
        private void RemovePluginUI(int index)
        {
            for (int i = 0; i < this.loadedPluginList.Count; i++)
            {
                if (index == this.loadedPluginList[i].Index)
                {
                    cb_plugins.Items.Add(PluginNames.Find(r => r.DisplayName == this.loadedPluginList[i].GetPluginDisplayName));
                    
                    this.loadedPluginList.RemoveAt(i);
                    grid_controlUI.Children.RemoveAt(i);


                    try
                    {
                        //Preciso remover a instance da interface?
                    }
                    catch
                    {

                    }

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
                IPluginCore objCore  = loadedPluginList.Find(r => r.GetPluginDisplayName.Equals(pluginMap.DisplayName));
                if (objCore != null)
                    return;
                objCore = loader.GetCore(pluginMap);
                if (objCore == null)
                    return;
                Type type = loader.GetMainUIType(pluginMap);
                IPluginMainUI mainUI = objCore.CreateOrGetMainUIIntance(type);
                if (mainUI == null)
                    return;
                objCore.Index = index;

                Expander cont = new Expander();

                DataTemplate dt = new DataTemplate(typeof(Expander));
               // FrameworkElementFactory stackPanel = new FrameworkElementFactory(typeof(StackPanel));
                //stackPanel.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);

               

                FrameworkElementFactory label = new FrameworkElementFactory(typeof(Label));
                label.SetValue(Label.ContentProperty, pluginMap.DisplayName);
                //label.SetValue(Label.FontWeightProperty, new FontWeightConverter().ConvertFromInvariantString("Bold"));
                LengthConverter lc = new LengthConverter();
                var height28 = lc.ConvertFromInvariantString("28px");
                FrameworkElementFactory grid = new FrameworkElementFactory(typeof(Grid));
                grid.SetValue(Grid.HeightProperty, height28);
                FrameworkElementFactory col1 = new FrameworkElementFactory(typeof(ColumnDefinition));
                FrameworkElementFactory col2 = new FrameworkElementFactory(typeof(ColumnDefinition));

                col1.SetValue(ColumnDefinition.WidthProperty, new GridLength(1, GridUnitType.Auto));
                col2.SetValue(ColumnDefinition.WidthProperty, new GridLength(1, GridUnitType.Star));
                grid.AppendChild(col1);
                grid.AppendChild(col2);

                string qualifiedDouble = "250px";

                var converted = lc.ConvertFrom(qualifiedDouble);

                label.SetValue(Label.WidthProperty, converted);
                FrameworkElementFactory btn = new FrameworkElementFactory(typeof(Button));
                btn.SetValue(Button.ContentProperty, "-");
                //var btnWidth = lc.ConvertFromInvariantString("28px");
                btn.SetValue(Button.WidthProperty, height28);
                btn.SetValue(Button.HeightProperty, height28);
                btn.SetValue(Button.TagProperty, index);
                btn.AddHandler(Button.ClickEvent, new RoutedEventHandler(expanderRemoveClick));
                // stackPanel.AppendChild(label);
                //stackPanel.AppendChild(btn);
                grid.AppendChild(label);
                label.SetValue(Grid.ColumnProperty, 0);

                grid.AppendChild(btn);
                btn.SetValue(Grid.ColumnProperty, 1);
                cont.HeaderTemplate = dt;
                // dt.VisualTree = stackPanel;
                dt.VisualTree = grid;

                cont.IsExpanded = true;
                cont.SetValue(Expander.TagProperty, pluginMap.DisplayName);
                cont.Content = (UserControl)mainUI;
                var t = new ThicknessConverter();
                object thi = t.ConvertFromInvariantString("0,0,0,14");
                cont.SetValue(Control.MarginProperty, thi);
                cont.AddHandler(Expander.ExpandedEvent, new RoutedEventHandler(expanderExpander));
               
                

                grid_controlUI.Children.Insert(0,cont);
                loadedPluginList.Insert(0,objCore);

                SetValues(objCore, size, app, codeGenerator);
                // this.Title = pluginMap.DisplayName;

                objCore.ProgressChange += PluginSelect_ProgressChange;
                objCore.FinishJob += PluginSelect_FinishJob;
                objCore.AnyTextChanged += PluginSelect_AnyTextChanged;
                objCore.UpdatePreview += PluginSelect_UpdatePreview;
                try
                {
                    objCore.ChangeLang(app.UILanguage.cdrLangToSys(), loader.GetAssembly(pluginMap));
                }
                catch (Exception e) 
                { 
                    app.MsgShow(string.Format("{0} - {1}", Lang.MBOX_ERROR_LangException,pluginMap.DisplayName)); 
                }
                if (typeof(IPluginConfig).IsAssignableFrom(objCore.GetType()))
                    (objCore as IPluginConfig).GetCodeGenerator += PluginSelect_GetCodeGenerator;
                SetDataSource(this.dataSource);

                index++;
                cb_plugins.Items.Remove(pluginMap);
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
            IPluginCore plugin;
            for (int i = 0; i < loadedPluginList.Count; i++)
            {
                plugin = loadedPluginList[i];
                Properties.Settings.Default.PluginNameCollection.Add(plugin.GetPluginDisplayName);
                plugin.SaveConfig();
            }
            Properties.Settings.Default.Save();
        }

        private void btn_deleteConfig_Click(object sender, RoutedEventArgs e)
        {
            IPluginCore plugin;
            if (loadedPluginList.Count < Properties.Settings.Default.PluginNameCollection.Count)
                this.app.MsgShow(Lang.MBOX_ERROR_SettingsCountNoMatch);
            for (int i = 0; i < loadedPluginList.Count; i++)
            {
                plugin = loadedPluginList[i];
                for (int r = 0; r < this.PluginNames.Count; r++)
                {
                    if (plugin.GetPluginDisplayName.Equals(PluginNames[r].DisplayName))
                        plugin.DeleteConfig();
                }
            }
            Properties.Settings.Default.PluginNameCollection.Clear();
            Properties.Settings.Default.Save();
        }
        public void LoadConfig()
        {
            if (Properties.Settings.Default.PluginNameCollection == null)
                return;
            var pluginNames = Properties.Settings.Default.PluginNameCollection;
            for (int i = 0; i < pluginNames.Count; i++)
            {
                for (int r = 0; r < this.PluginNames.Count; r++)
                {
                    if (pluginNames[i].Equals(this.PluginNames[r].DisplayName))
                    {
                        try
                        {
                            InflateUI(this.PluginNames[r]);
                        }
                        catch (Exception e) { 
                            Debug.WriteLine(e.Message, "LoadConfig");
                            throw new Exception(string.Format("Inflate {0} failed", this.PluginNames[r].DisplayName));
                        }

                        try
                        {
                            this.loadedPluginList[this.loadedPluginList.Count - 1].LoadConfig();
                        }
                        catch (Exception e) { Debug.WriteLine(e.Message, "LoadConfig");
                            throw new Exception(string.Format("Load config {0} failed", this.PluginNames[r].DisplayName));
                        }
                    }
                }
            }
        }
    }
}
