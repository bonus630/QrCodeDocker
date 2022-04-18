using br.corp.bonus630.PluginLoader;

namespace br.corp.bonus630.QrCodeDocker.Lang
{
    public class ES_ES : LangController, Ilang
    {
        public string MainTitle { get { return ""; } }

        public string TabTitleText { get { return "Texto"; } }

        public string TabTitleURL { get { return "Url"; } }

        public string TabTitleEmail { get { return "Email"; } }

        public string TabTitlePhone { get { return "Teléfono"; } }

        public string TabTitleVCard { get { return "vCard"; } }

        public string TabTextLabel { get { return "Texto: (Máx 300 Caracteres)"; } }

        public string TabTitleSms { get { return "Sms"; } }

        public string LabelMaxChars { get { return "Texto: (Máx 300 Caracteres)"; } }

        public string LabelLeftChars { get { return "Izquierdo:"; } }

        public string LabelUrlField { get { return "Escriba cualquier URL en el campo de arriba."; } }

        public string ButtonPaste { get { return "Pegar"; } }

        public string LabelEmailField { get { return "Escriba cualquier URL en el campo de arriba."; } }

        public string LabelPhoneField { get { return "Escriba cualquier URL en el campo de arriba."; } }

        public string LabelSmsField { get { return "Escriba cualquier URL en el campo de arriba."; } }

        public string vCardLabelHeaderHeader { get { return "Título"; } }

        public string vCardLabelHeaderName { get { return "Nombre:"; } }

        public string vCardLabelHeaderEmail { get { return "Email:"; } }

        public string vCardLabelHeaderPhone { get { return "Teléfono:"; } }

        public string vCardLabelHeaderSite { get { return "Web Site:"; } }

        public string vCardLabelHeaderOrg { get { return "Oganización:"; } }

        public string vCardLabelHeaderAdress { get { return "Dirección:"; } }

        public string vCardLabelHeaderNotes { get { return "Notas:"; } }

        public string LabelMessage { get { return "Mensaje: "; } }

        public string LabelDotSize { get { return "Tamaño de punto:"; } }

        public string LabelSize { get { return "Tamaño:"; } }

        public string ButtonDrawVector { get { return "Dibujar Vetor"; } }

        public string ButtonDrawBitmap { get { return "Dibujar Bitmap"; } }

        public string ButtonExtras { get { return "Extras"; } }

        public string LabelPreview { get { return "El preestreno:"; } }

        public string LabelEncodingType { get { return "Tipo de codificación:"; } }

        public string MBoxOpenDoc { get { return "Primero, abra un nuevo documento."; } }

        public string MBoxFormatErroTitle { get { return "Advertencia!"; } }

        public string MBoxFormatErroMessage { get { return "Tamaño de formato no válido"; } }
        public string MBoxContentErroMessage { get { return "El contenido esta vacio"; } }

        public string WifiLabelSecurity { get { return "Seguridad:"; } }
        public string TabTitleWifi { get { return "WiFi"; } }

        public string WifiLabelSSID { get { return "SSID:"; } }

        public string WifiLabelPassword { get { return "La contraseña:"; } }

        public string WifiLabelSSIDHidden { get { return "SSID encubierto"; } }

        public string GroupBoxExtrasHeader { get { return "Extras"; } }
        public string ToolTipBonus630 { get { return "Desarrollado por Bonus630"; } }

        public string ToolTipCorelNaVeia { get { return "Traducida por @corelnaveia"; } }
        public string BTN_BrowserPluginToolTip { get { return "Buscar extra"; } }

        public string BTN_SavePluginToolTip { get { return "Guardar estados extras"; } }

        public string BTN_DeletePluginToolTip { get { return "Eliminar estados extras"; } }

        public string MBOX_ERROR_SettingsCountNoMatch { get { return "No será posible restablecer todos los estados de los extras."; } }
    }
}
