using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using c = Corel.Interop.VGCore;
using br.corp.bonus630.PluginLoader;
using System.Reflection;
using br.corp.bonus630.plugin.PlaceHere.Lang;
using System.Windows.Media.Imaging;
using System.Drawing;

namespace br.corp.bonus630.plugin.PlaceHere
{
    /// <summary>
    /// Interaction logic for PlaceHereUI.xaml
    /// </summary>
    public partial class PlaceHereUI : UserControl, IPluginMainUI
    {
        PlaceHereCore phCore;
        public IPluginCore Core { get ; set; }
        Ilang Lang;
        public PlaceHereUI()
        {
            InitializeComponent();
            this.Loaded += PlaceHereUI_Loaded;
        }

        private void PlaceHereUI_Loaded(object sender, RoutedEventArgs e)
        {
            phCore = Core as PlaceHereCore;
            phCore.LoadConfigEvent += PhCore_LoadConfigEvent;
            Lang = phCore.Lang as Ilang;
        }

        private void PhCore_LoadConfigEvent()
        {
            ck_getContainer.IsChecked = phCore.GetContainer;
        }

        private void btn_start_Click(object sender, RoutedEventArgs e)
        {
            if (phCore.DataSource == null || phCore.DataSource.Count == 0)
                return;
            btn_start.Content = Lang.BTN_Restart;
           
            phCore.Draw(true);
        }
        private void btn_continue_Click(object sender, RoutedEventArgs e)
        {
            if (phCore.DataSource == null || phCore.DataSource.Count == 0)
                return;
            phCore.Draw(false);
        }
        private void AnchorButton_FactorChanged(double factorX, double factorY)
        {
            phCore.FactorX = factorX;
            phCore.FactorY = factorY;
            phCore.ReferencePoint = anchorButton.ReferencePoint;
        }
        private void ck_getContainer_Click(object sender, RoutedEventArgs e)
        {
            phCore.GetContainer = (bool)(sender as CheckBox).IsChecked;
        }

    }
}
