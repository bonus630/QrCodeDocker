using br.corp.bonus630.PluginLoader;

namespace br.corp.bonus630.QrCodeDocker.Lang
{
    public class PT_BR : LangController, Ilang
    {
        public string MainTitle { get { return ""; } }

        public string TabTitleText { get { return "Texto"; } }

        public string TabTitleURL { get { return "Url"; } }

        public string TabTitleEmail { get { return "Email"; } }

        public string TabTitlePhone { get { return "Tel"; } }

        public string TabTitleVCard { get { return "vCard"; } }

        public string TabTextLabel { get { return "Texto: (Máx 300 Caracteres)"; } }

        public string TabTitleSms { get { return "Sms"; } }

        public string LabelMaxChars { get { return "Texto: (Máx 300 Caracteres)"; } }

        public string LabelLeftChars { get { return "Restam:"; } }

        public string LabelUrlField { get { return "Digite sua URL no campo abaixo."; } }

        public string ButtonPaste { get { return "Colar"; } }

        public string LabelEmailField { get { return "Digite o Email no campo abaixo."; } }

        public string LabelPhoneField { get { return "Digite o Telefone no campo abaixo."; } }

        public string LabelSmsField { get { return "Digite o Telefone no campo abaixo."; } }

        public string vCardLabelHeaderHeader { get { return "Título"; } }

        public string vCardLabelHeaderName { get { return "Nome:"; } }

        public string vCardLabelHeaderEmail { get { return "Email:"; } }

        public string vCardLabelHeaderPhone { get { return "Telefone:"; } }

        public string vCardLabelHeaderSite { get { return "Web Site:"; } }

        public string vCardLabelHeaderOrg { get { return "Empresa:"; } }

        public string vCardLabelHeaderAdress { get { return "Endereço:"; } }

        public string vCardLabelHeaderNotes { get { return "Notas:"; } }

        public string LabelMessage { get { return "Mensagem: "; } }

        public string LabelDotSize { get { return "Tamanho do Ponto:"; } }

        public string LabelSize { get { return "Tamanho:"; } }

        public string ButtonDrawVector { get { return "Gerar Vetor"; } }

        public string ButtonDrawBitmap { get { return "Gerar Bitmap"; } }

        public string ButtonExtras { get { return "Extras"; } }
        public string ButtonToolTipRemovePlugin{ get { return "Remover"; } }

        public string LabelPreview { get { return "Visualização:"; } }

        public string LabelEncodingType { get { return "Método de Codificação:"; } }
        public string LBA_APPVersion { get { return "versão: ABACATE MADURO"; } }

        public string MBoxOpenDoc { get { return "Abra um novo documento para gerar"; } }

        public string MBoxFormatErroTitle { get { return "Aviso para Você!"; } }

        public string MBoxFormatErroMessage { get { return "Formato de tamanho inválido"; } }
        public string MBoxContentErroMessage { get {return "O conteúdo está vazio"; } }
        public string MBOX_ERROR_LangException { get { return "falha ao carregar linguagem"; } }

        public string WifiLabelSecurity { get { return "Segurança:"; } }

        public string TabTitleWifi { get { return "WiFi"; } }

        public string WifiLabelSSID { get { return "SSID"; } }

        public string WifiLabelPassword { get { return "Senha"; } }

        public string WifiLabelSSIDHidden { get { return "SSID Visivél"; } }

        public string GroupBoxExtrasHeader { get { return "Extras"; } }

        public string ToolTipBonus630 { get { return "Desenvolvido por Bonus630"; } }

        public string ToolTipCorelNaVeia { get { return "Traduzido por @corelnaveia"; } }

        public string BTN_BrowserPluginToolTip { get { return "Procurar extra"; } }

        public string BTN_SavePluginToolTip { get { return "Salvar estado dos extras"; } }

        public string BTN_DeletePluginToolTip { get { return "Apagar estado salvo"; } }
        public string MBOX_ERROR_SettingsCountNoMatch { get { return "Não será possível redefiner todos os estados dos extras"; } }
    }
}
