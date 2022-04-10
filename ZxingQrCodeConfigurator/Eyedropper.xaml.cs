using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Threading;
using System.Diagnostics;

namespace br.corp.bonus630.plugin.ZxingQrCodeConfigurator
{
    /// <summary>
    /// Interaction logic for Eyedropper.xaml
    /// </summary>
    public partial class Eyedropper : Window
    {

        EyedropperDataContext eye;
        Corel.Interop.VGCore.Application app;
        private string currentTheme;
        public Eyedropper(Corel.Interop.VGCore.Application app)
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.Manual;
             eye = new EyedropperDataContext(app);
            this.DataContext = eye;
            eye.NewPositionEvent += Eye_NewPositionEvent;
            this.app = app;
            this.app.OnApplicationEvent += CorelApp_OnApplicationEvent;
        }

        private void Eye_NewPositionEvent(Point obj)
        {
            this.Dispatcher.Invoke(
                new Action(() =>
                {
                    this.Left = obj.X+30;
                    this.Top = obj.Y-30;
                }
            ));
        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            eye.Close();
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
