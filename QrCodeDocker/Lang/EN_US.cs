using br.corp.bonus630.PluginLoader;

namespace br.corp.bonus630.QrCodeDocker.Lang
{
    public class EN_US : LangController, Ilang
    {
        public string MainTitle { get { return ""; } }

        public string TabTitleText { get { return "Text"; } }

        public string TabTitleURL { get { return "Url"; } }

        public string TabTitleEmail { get { return "Email"; } }

        public string TabTitlePhone { get { return "Phone"; } }

        public string TabTitleVCard { get { return "vCard"; } }

        public string TabTextLabel { get { return "Text: (Max 300 Caracters)"; } }

        public string TabTitleSms { get { return "Sms"; } }

        public string LabelMaxChars { get { return "Text: (Max 300 Caracters)"; } }

        public string LabelLeftChars { get { return "Left:"; } }

        public string LabelUrlField { get { return "Type any URL in above field."; } }

        public string ButtonPaste { get { return "Paste"; } }

        public string LabelEmailField { get { return "Type any Email in above field."; } }

        public string LabelPhoneField { get { return "Type any Phone in above field."; } }

        public string LabelSmsField { get { return "Type any Phone in above field."; } }

        public string vCardLabelHeaderHeader { get { return "Header"; } }

        public string vCardLabelHeaderName { get { return "Name:"; } }

        public string vCardLabelHeaderEmail { get { return "Email:"; } }

        public string vCardLabelHeaderPhone { get { return "Phone:"; } }

        public string vCardLabelHeaderSite { get { return "Web Site:"; } }

        public string vCardLabelHeaderOrg { get { return "Organization:"; } }

        public string vCardLabelHeaderAdress { get { return "Adress:"; } }

        public string vCardLabelHeaderNotes { get { return "Notes:"; } }

        public string LabelMessage { get { return "Message: "; } }

        public string LabelDotSize { get { return "Dot size:"; } }

        public string LabelSize { get { return "Size:"; } }

        public string ButtonDrawVector { get { return "Draw Vetor"; } }

        public string ButtonDrawBitmap { get { return "Draw Bitmap"; } }

        public string ButtonExtras { get { return "Extras"; } }
        public string ButtonToolTipRemovePlugin { get { return "Remove"; } }

        public string LabelPreview { get { return "Preview:"; } }

        public string LabelEncodingType { get { return "Encoding type:"; } }
        public string LBA_APPVersion { get { return "Version: Amora"; } }

        public string MBoxOpenDoc { get { return "First, open a new document."; } }

        public string MBoxFormatErroTitle { get { return "Warning!"; } }

        public string MBoxFormatErroMessage { get { return "Invalid format size"; } }
        public string MBoxContentErroMessage { get { return "The content is empty"; } }
        public string MBOX_ERROR_LangException { get { return "failed to load language"; } }

        public string WifiLabelSecurity { get { return "Security:"; } }
        public string TabTitleWifi { get { return "WiFi"; } }

        public string WifiLabelSSID { get { return "SSID:"; } }

        public string WifiLabelPassword { get { return "Password:"; } }

        public string WifiLabelSSIDHidden { get { return "SSID Hidden"; } }

        public string GroupBoxExtrasHeader { get { return "Extras"; } }
        public string ToolTipBonus630 { get { return "Develope by Bonus630"; } }

        public string ToolTipCorelNaVeia { get { return "Translate by @corelnaveia"; } }
        public string BTN_BrowserPluginToolTip { get { return "Browser extra"; } }

        public string BTN_SavePluginToolTip { get { return "Save extras states"; } }

        public string BTN_DeletePluginToolTip { get { return "Delete extras states"; } }

        public string MBOX_ERROR_SettingsCountNoMatch { get { return "Don't will possible reset all states of the extras"; } }
    }
}
