using Corel.Interop.VGCore;
using System.Diagnostics;
using System.Windows;

namespace br.corp.bonus630.QrCodeDocker
{
    public class StyleController

    {
        private Corel.Interop.VGCore.Application app;
        private string currentTheme;
        public static string ThemeShortName = "LightestGrey";
        private ResourceDictionary Resources;
        //public static int StyleID = 0;
        public StyleController(ResourceDictionary resources, Corel.Interop.VGCore.Application app)
        {
            this.app = app;
            Resources = resources;
            this.app.OnApplicationEvent += CorelApp_OnApplicationEvent;
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
         "ComboBox.Border.Popup.Item.MouseOver",
         "InvertBlacksParam"
        };

        public void LoadStyle(string name)
        {

            string style = name.Substring(name.LastIndexOf("_") + 1);
            ThemeShortName = style;
            for (int i = 0; i < StyleKeys.Length; i++)
            {
                this.Resources[StyleKeys[i]] = this.Resources[string.Format("{0}.{1}", style, StyleKeys[i])];
            }
        }
        private void CorelApp_OnApplicationEvent(string EventName, ref object[] Parameters)
        {
//#if DEBUG
//            string debug = "Event:" + EventName;
//            if(Parameters!= null)
//            {
//                for (int i = 0; i < Parameters.Length; i++)
//                {
//                    if(Parameters[i]!= null)
//                        debug += " | Param:" + i + " "+Parameters[i].ToString();
//                }
//            }
//            Debug.WriteLine(debug);
//#endif
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
    }
}
