﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using c = Corel.Interop.VGCore;
using br.corp.bonus630.PluginLoader;
using System.Reflection;
using br.corp.bonus630.plugin.PlaceHere.Lang;

namespace br.corp.bonus630.plugin.PlaceHere
{
    /// <summary>
    /// Interaction logic for PlaceHereUI.xaml
    /// </summary>
    public partial class PlaceHereUI : UserControl, IPluginUI, IPluginDrawer
    {
        c.Application corelApp;
        ICodeGenerator codeGenerator;
        PlaceHereCorecs core;
        Ilang Lang;
        public PlaceHereUI()
        {
            InitializeComponent();
            core = new PlaceHereCorecs();
            
            core.ProgressChange += Core_ProgressChange;
            
        }
        public void ChangeLang(LangTagsEnum langTag)
        {
            Lang = LangController.CreateInstance(Assembly.GetAssembly(typeof(br.corp.bonus630.plugin.PlaceHere.PlaceHereUI)), langTag) as Ilang;
            this.DataContext = Lang;
            (Lang as LangController).AutoUpdateProperties();
        }
        private void Core_ProgressChange(int obj)
        {
            OnProgressChange(obj);
        }

        public int Index { get; set; }
        private List<object[]> dataSource;
        public List<object[]> DataSource { set { 
                this.dataSource = value; } }
        private double size = 0;
        public double Size { get { return this.size; } set { 
                this.size = value; 
                core.Size = value; 
            } }
        public Corel.Interop.VGCore.Application App { set { this.corelApp = value; core.App = value; } }
        public ICodeGenerator CodeGenerator { set { this.codeGenerator = value;core.CodeGenerator = value; } }

        public event Action<object> FinishJob;
        public event Action<string> AnyTextChanged;
        public event Action<int> ProgressChange;

        public void Draw()
        {
            //this.corelApp.OpenDocument("C:\\Users\\bonus\\OneDrive\\Ambiente de Trabalho\\TestPlaceHereTPL.cdr");
            core.DataSource = this.dataSource;
            core.DSCursor = 0;
            core.Draw();
        }

        public void OnFinishJob(object obj)
        {
            
        }

        public void OnProgressChange(int progress)
        {
            if (ProgressChange != null)
                ProgressChange(progress);
        }

        private void btn_start_Click(object sender, RoutedEventArgs e)
        {
            btn_start.Content = Lang.BTN_Restart;
            Draw();
        }

        private void AnchorButton_FactorChanged(double factorX, double factorY)
        {
            core.FactorX = factorX;
            core.FactorY = factorY;
            core.ReferencePoint = anchorButton.ReferencePoint;
        }

        private void ck_getContainer_Click(object sender, RoutedEventArgs e)
        {
            core.GetContainer = (bool)(sender as CheckBox).IsChecked;
        }

      
    }
}
