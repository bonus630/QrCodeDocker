using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Corel.Interop.VGCore;
using br.corp.bonus630.ImageRender;
using br.corp.bonus630.PluginLoader;
using br.corp.bonus630.QrCodeDocker.MainTabControls;
using System.Windows.Data;
using System.Xml;
using ZXing;

namespace br.corp.bonus630.QrCodeDocker
{

    public partial class Docker : UserControl
    {
        Corel.Interop.VGCore.Application app;
        ICodeGenerator codeGenerator;
        ZXingImageRender imageRender;
        Document Doc;
        string textContent;
        PluginSelect pluginSelect;
        VisualDataContext dataContextObj;
        StyleController styleController;
        private string langFile;
        public Docker(object app)
        {
            InitializeComponent();
            try
            {
                this.app = app as Corel.Interop.VGCore.Application;
                styleController = new StyleController(this.Resources, this.app);
                codeGenerator = new QrCodeGenerator(this.app);
                LoadLang(this.app.LangCode());
                this.app.DocumentNew += app_DocumentNew;
                this.app.DocumentOpen += app_DocumentOpen;
                this.app.WindowActivate += app_WindowActivate;
                this.Loaded += Docker_Loaded;
                dataContextObj = new VisualDataContext(this.app);
                this.DataContext = dataContextObj;
            }
            catch (Exception error)
            {
                System.Windows.Forms.MessageBox.Show("Error01 " + error.Message);
            }
            //this.Dispatcher.ShutdownStarted += Dispatcher_ShutdownStarted;


        }

        //private void Dispatcher_ShutdownStarted(object sender, EventArgs e)
        //{
        //    Debug.WriteLine("teste");
        //}

        //private void App_OnApplicationEvent(string EventName, ref object[] Parameters)
        //{
        //    throw new NotImplementedException();
        //}

        public Docker()
        {
            InitializeComponent();
            codeGenerator = new QrCodeGenerator();
            dataContextObj = new VisualDataContext(this.app);
            this.DataContext = dataContextObj;
            LoadLang(this.app.LangCode());
        }
        private void LoadLang(string LanguageCode)
        {
            langFile = string.Concat(this.app.AddonPath, "QrCodeDocker\\Lang\\Main_", LanguageCode,".xml");
            if(!System.IO.File.Exists(langFile))
                langFile = string.Concat(this.app.AddonPath, "QrCodeDocker\\Lang\\Main_EN.xml");
            try
            {
                for (int i = 0; i < tabControls.Items.Count; i++)
                {
                    (tabControls.Items[i] as IMainTabControl).LoadLang(langFile);
                }
               
            }
            catch { }
            var xmlDataProvider = FindResource("Lang") as XmlDataProvider;

            if (xmlDataProvider != null)
            {
                xmlDataProvider.Source = new Uri(langFile, UriKind.RelativeOrAbsolute);
            }
        }
        public string GetLocalizedString( string key)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(langFile);
            XmlNode node = xmlDocument.SelectSingleNode(string.Concat("/root/", key));
            return node.InnerText;
        }
        private void Tabs_AnyTextChanged(string obj)
        {
            setContent(obj);
        }



        public void SetValuesPlugin()
        {
            if (pluginSelect != null)
            {
                int strSize = 221;
                if (!Int32.TryParse(txt_size.Text, out strSize))
                    return;
                pluginSelect.SetValues(strSize, this.app, this.codeGenerator);
            }
        }


        void Docker_Loaded(object sender, RoutedEventArgs e)
        {
            styleController.LoadThemeFromPreference();
            radioButton_zxing.IsChecked = true;
            imageRender = new ZXingImageRender();
            imageRender.ErrorCorrection = ZXing.QrCode.Internal.ErrorCorrectionLevel.Q;
            codeGenerator.SetRender(imageRender);

            // string pluginLoader = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Addons\\QrCodeDocker\\PluginLoader.dll");
            string pluginLoader = System.IO.Path.Combine(this.app.AddonPath, "QrCodeDocker\\PluginLoader.dll");


            if (System.IO.File.Exists(pluginLoader))
            {
                int strSize = 221;
                if (!Int32.TryParse(txt_size.Text, out strSize))
                    app.MsgShow(GetLocalizedString("MBoxFormatErroTitle"), GetLocalizedString("MBoxFormatErroMessage"));
                pluginSelect = new PluginSelect(strSize, this.app,  this.imageRender, this.codeGenerator);
                pluginSelect.AnyTextChanged += PluginSelect_AnyTextChanged;
                pluginSelect.UpdatePreview += renderImage;
                pluginSelect.OverridePreview += PluginSelect_OverridePreview;
                groupBoxPluginContainer.Content = pluginSelect;
            }
            else
                dataContextObj.CanLoadPlugin = false;

        }

        private void PluginSelect_OverridePreview(System.Drawing.Bitmap obj)
        {
            try
            {
                System.Windows.Media.Imaging.BitmapSource bitmapSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                 obj.GetHbitmap(),
                 IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                img_render.Source = bitmapSource;
            }
            catch { }
        }

        private void PluginSelect_AnyTextChanged(string obj)
        {
            setContent(obj);
        }

        /// /////////////////////////////////////
        #region Métodos

        private void renderImage()
        {
            try
            {
                System.Windows.Media.Imaging.BitmapSource bitmapSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                 imageRender.RenderBitmapToMemory2(textContent).GetHbitmap(),
                 IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                img_render.Source = bitmapSource;
            }
            catch { }

        }
        private string removeSpaces(string temp)
        {
            temp = temp.Trim();
            temp = temp.Replace(" ", ";");
            return temp;
        }


        /// <summary>
        /// Seta o contéudo para a criação do QRCode, além de controla-lo pelas abas e habilitar e desabilitar os botões de criação quando necessário.
        /// </summary>
        private void setContent(string content)
        {
            this.textContent = content;

            if (imageRender != null)
            {
                Dispatcher.Invoke(new Action(() =>
                    {
                        if (!String.IsNullOrEmpty(this.textContent))
                        {

                            img_render.Visibility = System.Windows.Visibility.Visible;
                            renderImage();
                            if (!txt_size.IsFocused)
                                txt_size.Text = imageRender.Measure().ToString();
                            double size;
                            Double.TryParse(txt_size.Text, out size);
                            txt_dot.Text = imageRender.InMeasure(size).ToString();


                        }
                        else
                            img_render.Visibility = System.Windows.Visibility.Hidden;
                    }));
            }
            if (pluginSelect != null)
            {
                pluginSelect.SetDataSource(new System.Collections.Generic.List<object[]>() { new object[] { this.textContent } });
            }


        }

        #endregion
        /// /////////////////////////////////////

        /// /////////////////////////////////////
        #region Eventos Do CorelDraw App
        void app_WindowActivate(Document Doc, Corel.Interop.VGCore.Window Window)
        {
            this.Doc = Doc;
            //Start();
        }

        void app_DocumentOpen(Document Doc, string FileName)
        {
            this.Doc = Doc;
            // Start();
        }

        void app_DocumentNew(Document Doc, bool FromTemplate, string Template, bool IncludeGraphics)
        {
            this.Doc = Doc;
            //Start();
        }
        #endregion
        /// /////////////////////////////////////
        #region Eventos Do WPF
        private void btn_gerar_Click(object sender, RoutedEventArgs e)
        {
            if (this.app.ActiveDocument == null)
            {
                app.MsgShow(GetLocalizedString("MBoxOpenDoc"));
                return;
            }
            if (string.IsNullOrEmpty(textContent))
            {
                app.MsgShow(GetLocalizedString("MBoxContentErroMessage"));
                return;
            }
            double strSize = 221;
            if (!Double.TryParse(txt_size.Text, out strSize))
            {
                app.MsgShow(GetLocalizedString("MBoxFormatErroTitle"), GetLocalizedString("MBoxFormatErroMessage"));
                return;
            }
            app.Optimization = true;
            app.ActiveDocument.BeginCommandGroup("qrcode");

            try
            {
                //generator.CreateVetorLocal2(this.app.ActiveLayer, textContent, strSize);
                codeGenerator.CreateVetorLocal(this.app.ActiveLayer, textContent, strSize);
            }
            catch (Exception erro)
            {
                app.MsgShow(erro.Message);
            }
            app.Optimization = false;
            app.ActiveDocument.EndCommandGroup();
            app.Refresh();

        }

        private void btn_gerarBitmap_Click(object sender, RoutedEventArgs e)
        {
            if (this.app.ActiveDocument == null)
            {
                app.MsgShow(GetLocalizedString("MBoxOpenDoc"));
                return;
            }
            if (string.IsNullOrEmpty(textContent))
            {
                app.MsgShow(GetLocalizedString("MBoxContentErroMessage"));
                return;
            }
            double strSize = 221;
            if (!Double.TryParse(txt_size.Text, out strSize))
            {
                app.MsgShow(GetLocalizedString("MBoxFormatErroTitle"), GetLocalizedString("MBoxFormatErroMessage"));
                return;
            }
            app.Optimization = true;
            app.ActiveDocument.BeginCommandGroup("qrcode");
            this.app.ActiveLayer.Name = "QrCode JPG";
            try
            {
                Corel.Interop.VGCore.Shape q = codeGenerator.CreateBitmapLocal(this.app.ActiveLayer, textContent, strSize);
            }
            catch (Exception erro)
            {
                app.MsgShow(erro.Message);
            }
            app.Optimization = false;
            app.ActiveDocument.EndCommandGroup();
            app.Refresh();
        }
        private void txt_size_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txt_size.IsFocused)
                setContent(this.textContent);
            SetValuesPlugin();
        }

        private void Image_MouseUp(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("https://bonus630.com.br");
        }

        private void radioButton_zxing_Checked(object sender, RoutedEventArgs e)
        {
            imageRender = new ZXingImageRender();
            codeGenerator.SetRender(imageRender);
            SetValuesPlugin();
        }
        private void radioButton_zxing_Click(object sender, RoutedEventArgs e)
        {
            setContent(textContent);
        }

        private void img_corelNaVeia_MouseUp(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.corelnaveia.com/");
        }


        private void lba_gma_MouseUp(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("https://qrcodenet.codeplex.com/");
        }

        private void lba_zxing_MouseUp(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("https://zxingnet.codeplex.com/");
        }
        #endregion
        private void GenNewCodeGenerator()
        {
            this.codeGenerator = new QrCodeGenerator(app);
            if ((bool)radioButton_zxing.IsChecked)
                imageRender = new ZXingImageRender();
        
            codeGenerator.SetRender(imageRender);
            SetValuesPlugin();
        }

        private void TabControls_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
         
            setContent((tabControls.SelectedItem as IMainTabControl).FormatedText);

        }
        private void ComboBox_CodeType_Selected(object sender, EventArgs e)
        {
            try
            {
                if (imageRender != null)
                {
                    string tagValue = ((ComboBoxItem)ComboBox_CodeType.SelectedItem).Tag.ToString();
                    BarcodeFormat selectedFormat  = (BarcodeFormat)Enum.Parse(typeof(BarcodeFormat), tagValue);
                    imageRender.CodeType = selectedFormat;
                    ChangeUIByCodeType();
                    setContent(textContent);
                }
            }
            catch { }
        }
        private void ChangeUIByCodeType()
        {

        }
     
        private void Button_CodeTypeHelp_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(imageRender.GetCodeTypeHelpURL());
        }
    }
}
