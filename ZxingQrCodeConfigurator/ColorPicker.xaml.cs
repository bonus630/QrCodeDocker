using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace br.corp.bonus630.plugin.ZxingQrCodeConfigurator
{
    /// <summary>
    /// Interaction logic for ColorPicker.xaml
    /// </summary>
    public partial class ColorPicker : Window
    {
        ColorManager colorManager;
        private string currentTheme;
        Corel.Interop.VGCore.Application app;
        public ColorPicker(Corel.Interop.VGCore.Palette palette)
        {
            InitializeComponent();
            colorManager = new ColorManager(palette,this);
            this.DataContext = colorManager;
            //colorManager.ExitEyedropper += ColorManager_ExitEyedropper;
            btn_ok.Click += (s, e) => { this.DialogResult = true; };
            app = palette.Application as Corel.Interop.VGCore.Application;
            this.app.OnApplicationEvent += CorelApp_OnApplicationEvent;
        }
        public ColorPicker(ColorManager colorManager)
        {
            InitializeComponent();
            this.DataContext = colorManager;
            btn_ok.Click += (s, e) => { this.DialogResult = true; };
            app = colorManager.CorelApp;
            this.app.OnApplicationEvent += CorelApp_OnApplicationEvent;
        }
        //private void ColorManager_ExitEyedropper()
        //{
        //    this.Visibility = Visibility.Visible;
        //    this.Focus();
        //}

        public ColorSystem SelectedColor { get { return colorManager.SelectedColor; } }
        protected override void OnDeactivated(EventArgs e)
        {
            onClose();
        }
        protected override void OnLostFocus(RoutedEventArgs e)
        {
            onClose();
        }

        public void onClose()
        {
            try
            {
                if (colorManager.InEyedropper)
                    this.DialogResult = true;
                //    this.Visibility = Visibility.Visible;
                //else
                this.Close();
                ////colorManager.Close();
            }
            catch { }
        }
        #region theme select
        //Keys resources name follow the resource order to add a new value, order to works you need add 5 resources colors and Resources/Colors.xaml
        //1º is default, is the same name of StyleKeys string array
        //2º add LightestGrey. in start name of 1º for LightestGrey style in corel
        //3º MediumGrey
        //4º DarkGrey
        //5º Black
        public readonly string[] StyleKeys = new string[] {
         "TabControl.Static.Border",
         "TabItem.Static.Border" ,
         "TabItem.Disabled.Background",
         "TabItem.Selected.Background",
         "TabItem.Static.Background",
         "TabItem.Selected.MouseOver.Background" ,
         "TabItem.Static.MouseOver.Background",
         "Button.MouseOver.Background" ,
         "Button.MouseOver.Border",
         "Button.Static.Border" ,
         "Button.Static.Background" ,
         "Button.Pressed.Background" ,
         "Button.Pressed.Border" ,
         "Button.Disabled.Foreground",
         "Button.Disabled.Background",
         "Default.Static.Foreground" ,
         "Container.Text.Static.Background" ,
         "Container.Text.Static.Foreground" ,
         "Container.Static.Background" ,
         "Default.Static.Inverted.Foreground" ,
         "ComboBox.Border.Popup.Item.MouseOver"
        };

        public void LoadStyle(string name)
        {

            string style = name.Substring(name.LastIndexOf("_") + 1);
            for (int i = 0; i < StyleKeys.Length; i++)
            {
                this.Resources[StyleKeys[i]] = this.Resources[string.Format("{0}.{1}", style, StyleKeys[i])];
            }
        }
        private void CorelApp_OnApplicationEvent(string EventName, ref object[] Parameters)
        {
            if (EventName.Equals("WorkspaceChanged") || EventName.Equals("OnColorSchemeChanged"))
            {
                LoadThemeFromPreference();
            }
        }
        public void LoadThemeFromPreference()
        {
            try
            {
                string result = "";
#if !X7
                result = app.GetApplicationPreferenceValue("WindowScheme", "Colors").ToString();
#endif

                if (!result.Equals(currentTheme))
                {
                    if (!result.Equals(string.Empty))
                    {
                        currentTheme = result;
                        LoadStyle(currentTheme);
                    }
                }
            }
            catch { }

        }
        #endregion

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadThemeFromPreference();
        }
    }

}
