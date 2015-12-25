using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Corel.Interop.VGCore;
using br.corp.bonus630.ImageRender;
using System.Text;




namespace br.corp.bonus630.QrCodeDocker
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class Docker : UserControl
    {

       
        Corel.Interop.VGCore.Application app;
        IImageRender imageRender;
        Document Doc;
        string textContent;
       
        public Docker(Corel.Interop.VGCore.Application app)
        {
            InitializeComponent();
            this.app = app;
           
            BitmapResources br = new BitmapResources();
            img_bonus.Source = br.Bonus630;
            img_corelNaVeia.Source = br.CorelNaVeia2015;
            app.DocumentNew += app_DocumentNew;
            app.DocumentOpen += app_DocumentOpen;
            app.WindowActivate += app_WindowActivate;
            this.Loaded += Docker_Loaded;
    
        }

        void Docker_Loaded(object sender, RoutedEventArgs e)
        {
            Start();
            this.Doc = this.app.ActiveDocument;
        }
        /// /////////////////////////////////////
        #region Métodos
        public void Start()
        {
            if(this.Doc != null && !String.IsNullOrEmpty(this.textContent))
            {
                btn_gerar.IsEnabled = true;
                btn_gerarBitmap.IsEnabled = true;
                
            }
            else
            {
                
                btn_gerar.IsEnabled = false;
                btn_gerarBitmap.IsEnabled = false;
            }
        }
        private void renderImage()
        {

            System.Windows.Media.Imaging.BitmapSource bitmapSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
             imageRender.RenderBitmapToMemory(textContent).GetHbitmap(),
             IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            img_render.Source = bitmapSource;

        }
        private void CreateBitmapLocal(int strSize = 221)
        {
            Layers layers = this.app.ActiveDocument.ActivePage.Layers;
            foreach (Layer l in layers)
            {
                
                if (l.Shapes.Count == 0 && !l.IsSpecialLayer)
                    l.Delete();
            }

            Layer tempLayer = this.app.ActiveDocument.ActivePage.CreateLayer("temp_qrcode");
            tempLayer.Activate();
            BitmapSource imageSource = (BitmapSource)img_render.Source;

            imageRender.SaveTempQrCodeFile(txt_content.Text, this.app.ActivePage.Resolution, strSize);
            StructImportOptions sio = new StructImportOptions();
            sio.MaintainLayers = true;

            ImportFilter importFilter = this.app.ActiveLayer.ImportEx(imageRender.QrCodeFilePath);

            importFilter.Finish();

            //Corel.Interop.VGCore.Clipboard cp = new Corel.Interop.VGCore.Clipboard();

            //System.Windows.Clipboard.SetImage(imageSource);


        }
        private void CreateVetorLocal2()
        {
            CreateBitmapLocal();
            Shapes shapes = this.app.ActiveLayer.Shapes;
            foreach (Corel.Interop.VGCore.Shape shape in shapes)
            {
                if (shape.Name == "temp_qrcode.jpg")
                {

                    double x, y, w, h;
                    shape.GetPosition(out x, out y);
                    shape.GetSize(out w, out h);

                    TraceSettings traceSettings = shape.Bitmap.Trace(cdrTraceType.cdrTraceLineDrawing);
                    traceSettings.DetailLevelPercent = 100;
                    traceSettings.BackgroundRemovalMode = cdrTraceBackgroundMode.cdrTraceBackgroundAutomatic;
                    traceSettings.CornerSmoothness = 0;
                    traceSettings.DeleteOriginalObject = true;
                    traceSettings.RemoveBackground = true;
                    traceSettings.RemoveEntireBackColor = true;
                    traceSettings.RemoveOverlap = true;
                    traceSettings.SetColorCount(2);
                    traceSettings.SetColorMode(cdrColorType.cdrColorGray, cdrPaletteID.cdrCustom);
                    traceSettings.Smoothing = 0;
                    traceSettings.TraceType = cdrTraceType.cdrTraceClipart;
                    traceSettings.Finish();
                    Corel.Interop.VGCore.ShapeRange sr = new Corel.Interop.VGCore.ShapeRange();
                    foreach (Corel.Interop.VGCore.Shape s in this.app.ActiveLayer.Shapes)
                    {
                        sr.Add(s);
                    }
                    sr.CreateSelection();
                    Corel.Interop.VGCore.Shape qrShape = this.app.ActiveSelection;
                    Corel.Interop.VGCore.Shape cod = this.app.ActiveSelection.Weld(qrShape);
                    cod.Name = "Cod";

                    Corel.Interop.VGCore.Color cWhite = new Corel.Interop.VGCore.Color();
                    cWhite.CMYKAssign(0, 0, 0, 0);
                    Corel.Interop.VGCore.Shape border = this.app.ActiveLayer.CreateRectangle2(x, y - h, w, h);
                    border.Fill.ApplyUniformFill(cWhite);
                    border.Outline.Width = 0;
                    border.OrderToBack();
                    border.Name = "Borda";
                    int strSize = 221;
                    


                    cod.AddToSelection();
                    border.AddToSelection();
                    Corel.Interop.VGCore.Shape g = this.app.ActiveSelection.Group();
                    this.app.ActiveDocument.Unit = this.app.ActiveDocument.Rulers.HUnits;
                    if(Int32.TryParse(txt_size.Text, out strSize))
                        g.SetSize(strSize, strSize);
                    else
                        System.Windows.MessageBox.Show("Formato de tamanho inválido","Aviso para Você!");
                }
            }
            this.app.ActiveLayer.Name = "QrCode Vetor";
        }
        private string removeSpaces(string temp)
        {
           
            temp = temp.Trim();
            
            temp = temp.Replace(" ", ";");
            return temp;
        }
        private string CreateVCard()
        {//3Titulo 4nome
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("BEGIN:VCARD");

            string temp = txt_vcardName.Text;
           
            if(!String.IsNullOrEmpty(temp))
            {
                temp = removeSpaces(temp);
                sb.AppendLine("N:"+temp);
                
            }
            temp = txt_vcardTitle.Text;
            if (!String.IsNullOrEmpty(temp))
            {
                temp = removeSpaces(temp);
                sb.AppendLine("TITLE:" + temp);

            }
            temp = txt_vcardUrl.Text;
            if (!String.IsNullOrEmpty(temp))
            {
                temp = removeSpaces(temp);
                sb.AppendLine("URL:" + temp);

            }
            temp = txt_vcardTel.Text;
            if (!String.IsNullOrEmpty(temp))
            {
                temp = removeSpaces(temp);
                sb.AppendLine("TEL:" + temp);

            }
            temp = txt_vcardEmail.Text;
            if (!String.IsNullOrEmpty(temp))
            {
                temp = removeSpaces(temp);
                sb.AppendLine("EMAIL:" + temp);

            }
            temp = txt_vcardJob.Text;
            if (!String.IsNullOrEmpty(temp))
            {
                temp = removeSpaces(temp);
                sb.AppendLine("ORG:" + temp);

            }
            temp = txt_vcardAdd.Text;
            if (!String.IsNullOrEmpty(temp))
            {
                temp = removeSpaces(temp);
                sb.AppendLine("ADR:" + temp);

            }
            temp = txt_vcardNote.Text;
            if (!String.IsNullOrEmpty(temp))
            {
                temp = removeSpaces(temp);
                sb.AppendLine("NOTE:" + temp);

            }
            //
          
            sb.AppendLine("END:VCARD");
            ///Ainda preciso terminar essa parte
            
            return sb.ToString();
        }

        /// <summary>
        /// Seta o contéudo para a criação do QRCode, além de controla-lo pelas abas e habilitar e desabilitar os botões de criação quando necessário.
        /// </summary>
        private void setContent()
        {
            this.textContent = "";
            switch (tabControls.SelectedIndex)
            {
                case 0:
                    this.textContent = txt_content.Text;
                    break;
                case 1:
                    if (txt_url.Text!="http://") 
                        this.textContent = txt_url.Text;
                    break;
                case 2:
                    if (!String.IsNullOrEmpty(txt_email.Text))
                        this.textContent = String.Format("mailto:{0}",txt_email.Text);
                    break;
                case 3:
                     if (!String.IsNullOrEmpty(txt_tel.Text)) 
                        this.textContent = String.Format("TEL:+{0}",txt_tel.Text);
                    break;
                case 4:
                    this.textContent = CreateVCard();
                    break;
                case 5:
                    if (!String.IsNullOrEmpty(txt_smsMen.Text) || !String.IsNullOrEmpty(txt_smsTel.Text)) 
                        this.textContent = String.Format("SMSTO:+{0}:{1}",txt_smsTel.Text,txt_smsMen.Text);
                    break;
            }
            if (imageRender != null)
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
            }
            if (this.IsLoaded)
                Start();
           

        }
      
        #endregion
        /// /////////////////////////////////////
        #region não usado
        //private void CreateVetorLocal()
        //{
        //    this.app.ActiveDocument.Unit = this.app.ActiveDocument.Rulers.HUnits;
        //    // Double.TryParse(txt_dot.Text, out this.dotSize);
        //    black.CMYKAssign(0, 0, 0, 100);
        //    white.CMYKAssign(0, 0, 0, 0);
        //    Layer qrCodeLayer = this.app.ActiveDocument.ActivePage.CreateLayer("temp_qrCode");
        //    qrCodeLayer.Activate();
        //    //DrawQuietZone(graphics, matrix.Width, offset);
        //    // Size paddingOffset = new Size(m_Padding, m_Padding) + new Size(offset.X, offset.Y);
        //    // Size moduleSize = new Size(m_ModuleSize, m_ModuleSize);
        //    Size m_Size = Measure(this.bitMatrix.Width);
        //    ShapeRange shapeRange = new ShapeRange();

        //    for (int j = 0; j < bitMatrix.Width; j++)
        //    {
        //        for (int i = 0; i < bitMatrix.Width; i++)
        //        {
        //            //Point moduleRelativePosition = new Point(i * m_ModuleSize, j * m_ModuleSize);
        //            //Rectangle moduleAbsoluteArea = new Rectangle(moduleRelativePosition + paddingOffset, moduleSize);
        //            //Corel.Interop.VGCore.Color color = bitMatrix[i, j] ? black : white;
        //            //Corel.Interop.VGCore.Color color = white;
        //            //graphics.FillRectangle(bush, moduleAbsoluteArea);
        //            if (bitMatrix[i, j])
        //            {
        //                Corel.Interop.VGCore.Shape ret = this.app.ActiveDocument.ActiveLayer.CreateRectangle2(i * this.dotSize + m_Padding, j * this.dotSize + m_Padding, this.dotSize, this.dotSize);
        //                ret.Fill.ApplyUniformFill(black);
        //                ret.Outline.Width = 0;
        //                //ret.AddToSelection();
        //                shapeRange.Add(ret);
        //            }
        //        }
        //    }
        //    //foreach(Corel.Interop.VGCore.Shape shape in qrCodeLayer.Shapes)
        //    // {
        //    //      shape.AddToSelection();
        //    //  }
        //    //border.AddToSelection();


        //    shapeRange.CreateSelection();
        //    Corel.Interop.VGCore.Shape qrCodeShape = this.app.ActiveSelection;
        //    this.app.ActiveSelection.Weld(qrCodeShape);
        //    Corel.Interop.VGCore.Shape border = qrCodeLayer.CreateRectangle2(0, 0, m_Size.Width, m_Size.Height);
        //    border.Fill.ApplyUniformFill(white);
        //    border.Outline.Width = 0;
        //    qrCodeShape.Flip(cdrFlipAxes.cdrFlipVertical);
        //    border.OrderToBack();
        //    foreach (Corel.Interop.VGCore.Shape shape in qrCodeLayer.Shapes)
        //    {
        //        shape.AddToSelection();
        //    }
        //    this.app.ActiveSelection.Group();
        //    qrCodeLayer.Name = "QrCode";
        //}
        #endregion
        /// /////////////////////////////////////
        #region Eventos Do CorelDraw App
        void app_WindowActivate(Document Doc, Corel.Interop.VGCore.Window Window)
        {
            this.Doc = Doc;
            Start();
        }

        void app_DocumentOpen(Document Doc, string FileName)
        {
            this.Doc = Doc;
            Start();
        }

        void app_DocumentNew(Document Doc, bool FromTemplate, string Template, bool IncludeGraphics)
        {
            this.Doc = Doc;
            Start();
        }
        #endregion
        /// /////////////////////////////////////
        #region Eventos Do WPF
        private void btn_gerar_Click(object sender, RoutedEventArgs e)
        {
            if (this.app.ActiveDocument == null)
            {
                global::System.Windows.MessageBox.Show("Abra um novo documento para gerar");
                return;
            }
                CreateVetorLocal2();
            
        }

        

        private void btn_gerarBitmap_Click(object sender, RoutedEventArgs e)
        {
            if (this.app.ActiveDocument == null)
            {
                global::System.Windows.MessageBox.Show("Abra um novo documento para gerar");
                return;
            }
            CreateBitmapLocal();
            this.app.ActiveLayer.Name = "QrCode JPG";
            this.app.ActiveDocument.Unit = this.app.ActiveDocument.Rulers.HUnits;
            Corel.Interop.VGCore.Shape q = this.app.ActiveLayer.FindShape("temp_qrcode.jpg");
            int strSize = 221;

            if(Int32.TryParse(txt_size.Text, out strSize))
                  q.SetSize(strSize, strSize);
            else
                System.Windows.MessageBox.Show("Formato de tamanho inválido", "Aviso para Você!");
       
         
        }
        
        private void txt_content_TextChanged(object sender, TextChangedEventArgs e)
        {
            lba_cont.Content = String.Format("Restam: {0}", 300 - txt_content.Text.Length);
            

            if (txt_content.IsFocused)
            {

                setContent();
           
            }
    
        }
       

        private void txt_size_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txt_size.IsFocused)
                setContent();
          
        }
        private void txt_content_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (txt_content.Text.Length >= 300)
                e.Handled = true;
        }

        private void Image_MouseUp(object sender, MouseButtonEventArgs e)
        {

            System.Diagnostics.Process.Start("http://bonus630.tk");
        }

        private void radioButton_gma_Checked(object sender, RoutedEventArgs e)
        {
            imageRender = new GmaImageRender();

        }

        private void radioButton_zxing_Checked(object sender, RoutedEventArgs e)
        {
            imageRender = new ZXingImageRender();

        }

        private void radioButton_zxing_Click(object sender, RoutedEventArgs e)
        {
            setContent();
        }

        private void btn_colar_Click(object sender, RoutedEventArgs e)
        {
            if (System.Windows.Clipboard.ContainsText())
                txt_url.Text = System.Windows.Clipboard.GetText();
        }

        private void txt_url_TextChanged(object sender, TextChangedEventArgs e)
        {
            setContent();
        
        }

        private void tabControls_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            setContent();
        }


        private void txt_email_TextChanged(object sender, TextChangedEventArgs e)
        {
            setContent();
        }
        private void btn_colar2_Click(object sender, RoutedEventArgs e)
        {
            if (System.Windows.Clipboard.ContainsText())
                txt_email.Text = System.Windows.Clipboard.GetText();
        }

        private void txt_vcard(object sender, TextChangedEventArgs e)
        {
            setContent();
        }

        private void txt_tel_TextChanged(object sender, TextChangedEventArgs e)
        {
            setContent();
        }

        private void txt_smsTel_TextChanged(object sender, TextChangedEventArgs e)
        {
            setContent();
        }
        private void txt_smsMen_TextChanged(object sender, TextChangedEventArgs e)
        {
            setContent();
        }

        private void txt_tel_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            int res;
            if (!Int32.TryParse(e.Text, out res))
                e.Handled = true;
        }

        private void txt_smsTel_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            int res;
            if (!Int32.TryParse(e.Text, out res))
                e.Handled = true;
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

        
    }
}
