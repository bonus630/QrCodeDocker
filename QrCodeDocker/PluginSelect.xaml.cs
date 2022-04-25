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
using System.Collections.ObjectModel;

namespace br.corp.bonus630.QrCodeDocker
{

    public partial class PluginSelect : UserControl
    {

        //private List<IPluginCore> loadedCorelist = null;
        public Ilang Lang { get; set; }
        public List<PluginMap> PluginNames;
        Loader loader;
        double size;
        public double Size { get { return this.size; } set { this.size = value; } }
        Corel.Interop.VGCore.Application app;
        private List<object[]> dataSource;
        public bool PluginFound { get; set; }
        private ICodeGenerator codeGenerator;
        private int index = 0;
        public event Action<string> AnyTextChanged;
        public event Action UpdatePreview;
        public ObservableCollection<IPluginCore> LoadedPluginList { get; set; }

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
                //loadedCorelist = new List<IPluginCore>();
                this.PluginFound = true;
                LoadedPluginList = new ObservableCollection<IPluginCore>();
                this.DataContext = this;
                //this.Dispatcher.ShutdownStarted += Dispatcher_ShutdownStarted;
                LoadConfig();
            }

            catch (Exception erro)
            {
                app.MsgShow(string.Format("{0} | {1} | {2}", erro.Message, erro.Source, erro.TargetSite.Name));
                this.PluginFound = false;
                // this.Title = "No extras found!";
            }
        }

        //private void Dispatcher_ShutdownStarted(object sender, EventArgs e)
        //{
        //    for (int i = 0; i < LoadedPluginList.Count; i++)
        //    {
        //        RemovePluginUI(LoadedPluginList[i].Index);
        //    }
        //}

        public void SetValues(double size, Corel.Interop.VGCore.Application app, ICodeGenerator codeGenerator)
        {
            if (LoadedPluginList == null)
                return;
            this.size = size;
            for (int i = 0; i < LoadedPluginList.Count; i++)
            {
                //SetValues(loadedPluginList[i], size, app, imageRender);
                SetValues(LoadedPluginList[i], size, app, codeGenerator);
            }
        }
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
            if (codeGenerator == null || LoadedPluginList == null)
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
            int index = (int)(sender as Button).Tag;
            RemovePluginUI(index);
        }
        private void RemovePluginUI(int index)
        {
            try
            {
                IPluginCore core = LoadedPluginList.Single<IPluginCore>(r => r.Index == index);
                cb_plugins.Items.Add(PluginNames.Find(r => r.DisplayName == core.GetPluginDisplayName));
                //this.loadedCorelist.Remove(core);
                LoadedPluginList.Remove(core);
                //core.UIControl.Dispatcher.InvokeShutdown();
                core = null;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }
        private void expanderExpander(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < LoadedPluginList.Count; i++)
            {
                if (typeof(IPluginDataSource).IsAssignableFrom(LoadedPluginList[i].GetType()))
                {
                    List<object[]> dataSource = (LoadedPluginList[i] as IPluginDataSource).DataSource;
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

                for (int i = 0; i < LoadedPluginList.Count; i++)
                {
                    if (typeof(IPluginDataSource).IsAssignableFrom(LoadedPluginList[i].GetType()))
                    {
                        this.dataSource = (LoadedPluginList[i] as IPluginDataSource).DataSource;
                        SetDataSource(this.dataSource);
                    }

                }
            }));
        }
        public void SetDataSource(List<object[]> dataSource)
        {
            if (LoadedPluginList == null || dataSource == null || dataSource.Count == 0)
                return;
            if (dataSource != this.dataSource)
                this.dataSource = dataSource;
            for (int i = 0; i < LoadedPluginList.Count; i++)
            {
                if (typeof(IPluginDrawer).IsAssignableFrom(LoadedPluginList[i].GetType()) && dataSource != null)
                {
                    (LoadedPluginList[i] as IPluginDrawer).DataSource = dataSource;
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
                //IPluginCore objCore  = LoadedPluginList.Single<IPluginCore>(r => r.GetPluginDisplayName.Equals(pluginMap.DisplayName));
                //if (objCore != null)
                //    return;
                IPluginCore objCore = loader.GetCore(pluginMap);
                if (objCore == null)
                    return;
                Type type = loader.GetMainUIType(pluginMap);
                IPluginMainUI mainUI = objCore.CreateOrGetMainUIIntance(type);
                if (mainUI == null)
                    return;
                objCore.Index = index;


                LoadedPluginList.Insert(0, objCore);

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
                    app.MsgShow(string.Format("{0} - {1}", Lang.MBOX_ERROR_LangException, pluginMap.DisplayName));
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
            for (int i = 0; i < LoadedPluginList.Count; i++)
            {
                plugin = LoadedPluginList[i];
                Properties.Settings.Default.PluginNameCollection.Add(plugin.GetPluginDisplayName);
                plugin.SaveConfig();
            }
            Properties.Settings.Default.Save();
        }

        private void btn_deleteConfig_Click(object sender, RoutedEventArgs e)
        {
            IPluginCore plugin;
            if (LoadedPluginList.Count < Properties.Settings.Default.PluginNameCollection.Count)
                this.app.MsgShow(Lang.MBOX_ERROR_SettingsCountNoMatch);
            for (int i = 0; i < LoadedPluginList.Count; i++)
            {
                plugin = LoadedPluginList[i];
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
                        catch (Exception e)
                        {
                            Debug.WriteLine(e.Message, "LoadConfig");
                            throw new Exception(string.Format("Inflate {0} failed", this.PluginNames[r].DisplayName));
                        }

                        try
                        {
                            this.LoadedPluginList[this.LoadedPluginList.Count - 1].LoadConfig();
                        }
                        catch (Exception e)
                        {
                            Debug.WriteLine(e.Message, "LoadConfig");
                            throw new Exception(string.Format("Load config {0} failed", this.PluginNames[r].DisplayName));
                        }
                    }
                }
            }
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Dispatcher.InvokeShutdown();
                Debug.WriteLine("Test");
            }
            catch { }
        }
    }

}
