namespace br.corp.bonus630.QrCodeDocker.Lang
{
    public interface Ilang
    {
        string MainTitle {get;}

        string TabTitleText { get; }
        string TabTitleURL { get; }
        string TabTitleEmail { get; }
        string TabTitlePhone { get; }
        string TabTitleVCard { get; }
        string TabTextLabel { get; }
        string TabTitleSms { get; }
        string TabTitleWifi { get; }

        string LabelMaxChars { get; }
        string LabelLeftChars { get; }
        string LabelUrlField { get; }
        string ButtonPaste { get; }
        string LabelEmailField { get; }
        string LabelPhoneField { get; }
        string LabelSmsField { get; }

        string vCardLabelHeaderHeader { get; }
        string vCardLabelHeaderName { get; }
        string vCardLabelHeaderEmail { get; }
        string vCardLabelHeaderPhone { get; }
        string vCardLabelHeaderSite { get; }
        string vCardLabelHeaderOrg { get; }
        string vCardLabelHeaderAdress { get; }
        string vCardLabelHeaderNotes { get; }


        string LabelMessage { get; }
        string LabelDotSize { get; }
        string LabelSize { get; }

        string ButtonDrawVector { get; }
        string ButtonDrawBitmap { get; }
        string ButtonExtras { get; }
        string BTN_BrowserPluginToolTip { get; }
        string BTN_SavePluginToolTip { get; }
        string BTN_DeletePluginToolTip { get; }
        string LabelPreview { get; }
        string LabelEncodingType { get; }

        string MBoxOpenDoc { get; }
        string MBoxFormatErroTitle { get; }
        string MBoxFormatErroMessage { get; }
        string MBoxContentErroMessage { get; }
        string MBOX_ERROR_SettingsCountNoMatch { get; }
        string MBOX_ERROR_LangException { get; }
        string WifiLabelSSID { get; }
        string WifiLabelSecurity { get; }
        string WifiLabelPassword { get; }
        string WifiLabelSSIDHidden { get; }

        string GroupBoxExtrasHeader { get; }
        string ToolTipBonus630 { get; }
        string ToolTipCorelNaVeia { get; }
        string LBA_APPVersion { get; }
    }
}
