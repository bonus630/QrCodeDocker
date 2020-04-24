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

namespace br.corp.bonus630.QrCodeDocker
{

    public partial class PluginSelect : UserControl
    {
        
        private List<IPluginUI> loadedPluginList = null;
      
        Loader loader;
        double size;
        Corel.Interop.VGCore.Application app;
        //ImageRender.IImageRender imageRender;
        private List<object[]> dataSource;
        public bool PluginFound { get; set; }
        private ICodeGenerator codeGenerator;
        private int index = 0;
        public event Action<string> AnyTextChanged;
        //public void SetValues(double size, Corel.Interop.VGCore.Application app,ImageRender.IImageRender imageRender)
        public void SetValues(double size, Corel.Interop.VGCore.Application app, ICodeGenerator codeGenerator)
        {
            if (loadedPluginList == null)
                return;
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
            if (typeof(IPluginDrawer).IsAssignableFrom(obj.GetType()))
            {
                (obj as IPluginDrawer).App = app;
                (obj as IPluginDrawer).Size = size;
                //(obj as IPluginDrawer).ImageRender = imageRender;
                (obj as IPluginDrawer).CodeGenerator = codeGenerator;
            }

        }
        public void SetValues(IPluginConfig plugin,Type type, Corel.Interop.VGCore.Application app)
        {
            if (codeGenerator == null || loadedPluginList == null)
                throw new Exception("Erros ");
            if (this.codeGenerator.GetType() == type)
            {
                plugin.CodeGenerator = codeGenerator;
                plugin.App = app;
            }
                      
        }
        public PluginSelect( double size,Corel.Interop.VGCore.Application app,ImageRender.IImageRender imageRender,ICodeGenerator codeGenerator)
        {
            InitializeComponent();
            try
            {
                loader = new Loader();
                List<PluginMap> pluginNames = loader.PluginList();
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
            }
            
            catch(Exception erro)
            {
                app.MsgShow(erro.Message+" | "+erro.Source);
                this.PluginFound = false;
               // this.Title = "No extras found!";
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
                if(index == this.loadedPluginList[i].Index)
                {
                    this.loadedPluginList.RemoveAt(i);
                    grid_controlUI.Children.RemoveAt(i);
                    if(grid_controlUI.Children.Count==0)
                    {
                        //this.Title = "Extras Select";
                    }
                }
            }
            

        }
        private void expanderExpander(object sender, RoutedEventArgs e)
        {
            //this.Title = (sender as Expander).Tag.ToString();
                    
             
        }

        private void PluginSelect_GetCodeGenerator(object sender, Type type)
        {
            SetValues(sender as IPluginConfig,type,this.app);
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
        void SetDataSource(List<object[]> dataSource)
        {
            if (loadedPluginList == null && dataSource == null && dataSource.Count == 0)
                return;
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
                var btnWidth = lc.ConvertFrom("15px");
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
                cont.AddHandler(Expander.ExpandedEvent, new RoutedEventHandler(expanderExpander));
                grid_controlUI.Children.Add(cont);
                loadedPluginList.Add(objUI as IPluginUI);

                SetValues(objUI, size, app, codeGenerator);
               // this.Title = pluginMap.DisplayName;

                (objUI as IPluginUI).ProgressChange += PluginSelect_ProgressChange;
                (objUI as IPluginUI).FinishJob += PluginSelect_FinishJob;
                (objUI as IPluginUI).AnyTextChanged += PluginSelect_AnyTextChanged;
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

        private void PluginSelect_AnyTextChanged(string obj)
        {
            if (AnyTextChanged != null)
                AnyTextChanged(obj);
        }
    }
}
