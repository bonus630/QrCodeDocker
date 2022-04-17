using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Corel.Interop.VGCore;
using System.IO;
using br.corp.bonus630.PluginLoader;
using br.corp.bonus630.QrCodeDocker;
using br.corp.bonus630.QrCodeDocker.Enums;
using System.Text.RegularExpressions;
using System.Windows.Media;
using System.Windows.Input;
using System.ComponentModel;
using System.Reflection;
using br.corp.bonus630.plugin.ZxingQrCodeConfigurator.Lang;

namespace br.corp.bonus630.plugin.ZxingQrCodeConfigurator
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class ZxingQrCodeConfiguratorUI : UserControl, IPluginUI, IPluginConfig, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public Ilang Lang { get; set; }

        public string PluginDisplayName { get { return Core.PluginDisplayName; } }

        public ZxingQrCodeConfiguratorUI()
        {
            InitializeComponent();
          

        }
        public void ChangeLang(LangTagsEnum langTag)
        {
            Lang = LangController.CreateInstance(Assembly.GetAssembly(typeof(br.corp.bonus630.plugin.ZxingQrCodeConfigurator.ZxingQrCodeConfiguratorUI)), langTag) as Ilang;
            this.DataContext = this;
            (Lang as LangController).AutoUpdateProperties();
        }
        private ColorSystem selectedBorderColor;
        public ColorSystem SelectedBorderColor
        {
            get { return selectedBorderColor; }
            set
            {
                selectedBorderColor = value;
                NotifyPropertyChanged("SelectedBorderColor");
            }
        }
        private ColorSystem selectedDotColor;
        public ColorSystem SelectedDotColor
        {
            get { return selectedDotColor; }
            set
            {
                selectedDotColor = value;
                NotifyPropertyChanged("SelectedDotColor");
            }
        }
        private ColorSystem selectedDotBorderColor;
        public ColorSystem SelectedDotBorderColor
        {
            get { return selectedDotBorderColor; }
            set
            {
                selectedDotBorderColor = value;
                NotifyPropertyChanged("SelectedDotBorderColor");
            }
        }

        private Corel.Interop.VGCore.Application app;
        public Corel.Interop.VGCore.Application App { set { this.app = value; } }

        private ICodeGenerator codeGenerator;
        public ICodeGenerator CodeGenerator
        {
            get { return codeGenerator; }
            set { codeGenerator = value; }
        }
        public int Index { get; set; }

        public event Action<object> FinishJob;
        public event Action<int> ProgressChange;
        public event Action<object, Type> GetCodeGenerator;
        public event Action<string> AnyTextChanged;
        public event Action UpdatePreview;

        public void NotifyPropertyChanged(string propertyName = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        public void OnFinishJob(object obj)
        {
            throw new NotImplementedException();
        }

        public void OnGetCodeGenerator(object sender, Type type)
        {
            if (CodeGenerator == null)
            {
                if (GetCodeGenerator != null)
                    GetCodeGenerator(sender, type);
            }
        }

        public void OnProgressChange(int progress)
        {
            throw new NotImplementedException();
        }

        private void ck_weld_Click(object sender, RoutedEventArgs e)
        {

            OnGetCodeGenerator(this, typeof(QrCodeGenerator));
            (CodeGenerator as QrCodeGenerator).Weld = (bool)ck_weld.IsChecked;
            OnUpdatePreview();

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            OnGetCodeGenerator(this, typeof(QrCodeGenerator));
            LoadConfigToCode();
            ck_weld.IsChecked = (CodeGenerator as QrCodeGenerator).Weld;
       
        }

  

        private void btn_validate_Click(object sender, RoutedEventArgs e)
        {
            if ((this.app.ActiveSelection != null && this.app.ActiveSelection.Shapes.Count > 0) || this.app.ActiveShape != null)
            {
                string filePath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "qrcode");
                if (!System.IO.Directory.Exists(filePath))
                {
                    System.IO.Directory.CreateDirectory(filePath);
                }
                filePath = System.IO.Path.Combine(filePath, "temp_decode.jpg");

                ExportFilter ex = this.app.ActiveDocument.ExportBitmap(filePath, Corel.Interop.VGCore.cdrFilter.cdrJPEG, Corel.Interop.VGCore.cdrExportRange.cdrSelection, Corel.Interop.VGCore.cdrImageType.cdrRGBColorImage);
                ex.Finish();
                try
                {
                    string text = CodeGenerator.DecodeImage(filePath);
                   
                    app.MsgShow(text, Lang.MBOX_QrMessage);
                   
                }
                catch (NotImplementedException ex1)
                {
                    app.MsgShow(Lang.MBOX_QrCodingWarning, Lang.Warning, QrCodeDocker.MessageBox.DialogButtons.Ok);
                }
                catch (Exception ex2)
                {
                    app.MsgShow(Lang.MBOX_ERRO_QRInvalid);
                }
                finally
                {
                    if (System.IO.File.Exists(filePath))
                        System.IO.File.Delete(filePath);
                }
            }
        }

        private void txt_dotBorderSize_PreviewKeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            double size = 0;
            if (Double.TryParse(txt_dotBorderSize.Text, out size))
            {
                (CodeGenerator as QrCodeGenerator).DotBorderSize = size;
                OnUpdatePreview();

            }
        }

        private void cb_dotShape_DropDownClosed(object sender, EventArgs e)
        {
            if (cb_dotShape.SelectedIndex != -1)
            {
                (CodeGenerator as QrCodeGenerator).DotShapeType = (DotShape)cb_dotShape.SelectedIndex;
                OnUpdatePreview();
            }

        }

        public void restoreDefault()
        {
            this.txt_dotBorderSize.Text = "0";
        }
        private void checkTextFormat(object sender, TextCompositionEventArgs e)
        {
            string pattern = @"^(-)?(\d+)?([\.|,]{1})?([\d]{0,2})?$";



            Regex rg = new Regex(pattern);
            string text = (e.Source as TextBox).Text;
            string prevText = e.Text;

            bool stop = false;
            stop = !rg.IsMatch(prevText);

            if (prevText == "," || prevText == ".")
            {

                stop = (text.Contains(",") || text.Contains("."));

                if (prevText == "." && !stop)
                {

                    TextBox tb = sender as TextBox;
                    tb.AppendText(",");
                    tb.CaretIndex = tb.Text.Length;
                    e.Handled = true;
                    return;
                }
            }


            e.Handled = stop;

        }



        private void txt_content_GotFocus(object sender, RoutedEventArgs e)
        {
            if ((sender as TextBox).Text == "0")
                (sender as TextBox).Text = "";

        }

        private void txt_content_LostFocus(object sender, RoutedEventArgs e)
        {
            if ((sender as TextBox).Text == "")
                (sender as TextBox).Text = "0";
        }

        private void btn_BorderColor_Click(object sender, EventArgs e)
        {
            ColorPicker c = GetColorPicker(sender);
            if ((bool)c.ShowDialog())
            {
                SelectedBorderColor = c.SelectedColor;
                if (SelectedBorderColor != null)
                {
                    (CodeGenerator as QrCodeGenerator).BorderColor = SelectedBorderColor.CorelColor; OnUpdatePreview();
                }
            }
        }

        private void btn_DotColor_Click(object sender, EventArgs e)
        {
            ColorPicker c =  GetColorPicker(sender);
            if ((bool)c.ShowDialog())
            {
                SelectedDotColor = c.SelectedColor;
                if (SelectedDotColor != null)
                {
                    (CodeGenerator as QrCodeGenerator).DotFillColor = SelectedDotColor.CorelColor; OnUpdatePreview();
                }
            }
        }

        private void btn_DotBorderColor_Click(object sender, EventArgs e)
        {
            ColorPicker c =  GetColorPicker(sender);
            if ((bool)c.ShowDialog())
            {
                SelectedDotBorderColor = c.SelectedColor;
                if (SelectedDotBorderColor != null)
                {
                    (CodeGenerator as QrCodeGenerator).DotOutlineColor = SelectedDotBorderColor.CorelColor;
                    OnUpdatePreview();
                }
            }
        }
        private ColorPicker GetColorPicker(object sender)
        {
            int x, y, w, h = 0;
            app.FrameWork.Automation.GetItemScreenRect("68622454-ABAA-4099-976F-E620DCF8C89B", "efc02df4-8eb5-44b5-8016-1c495af1504e", out x, out y, out w, out h);
            ColorPicker colorPicker = new ColorPicker(app.ActivePalette);
            colorPicker.WindowStartupLocation = WindowStartupLocation.Manual;
            x = (int)(x - (colorPicker.Width + 16));
            if (x < 0)
                x = 0;
                colorPicker.Left = x;
            
            colorPicker.Top = y + 32;
            return colorPicker;
        }
        private void ck_noBorder_Checked(object sender, RoutedEventArgs e)
        {
            (CodeGenerator as QrCodeGenerator).NoBorder = (bool)ck_noBorder.IsChecked;
            btn_BorderColor.IsEnabled = !(bool)ck_noBorder.IsChecked;
            OnUpdatePreview();
        }

        private void OnUpdatePreview()
        {
            if (UpdatePreview != null)
                UpdatePreview();
        }

        public void SaveConfig()
        {
            QrCodeGenerator code = (CodeGenerator as QrCodeGenerator);
            Properties.Settings.Default.Weld = code.Weld;
            Properties.Settings.Default.NoBorder = code.NoBorder;
            Properties.Settings.Default.DotShape = (ushort)code.DotShapeType;
            Properties.Settings.Default.DotBordeSize = code.DotBorderSize;
            if (SelectedBorderColor != null)
                Properties.Settings.Default.BorderColor = SelectedBorderColor.CorelColorName;
            if (SelectedDotColor != null)
                Properties.Settings.Default.DotFillColor = SelectedDotColor.CorelColorName;
            if (SelectedDotBorderColor != null)
                Properties.Settings.Default.DotBorderColor = SelectedDotBorderColor.CorelColorName;
            Properties.Settings.Default.PaletteIndentifier = app.ActivePalette.Name;
            Properties.Settings.Default.Save();
        }

        public void LoadConfig()
        {

            ck_noBorder.IsChecked = Properties.Settings.Default.NoBorder;
            ck_weld.IsChecked = Properties.Settings.Default.Weld;
            txt_dotBorderSize.Text = Properties.Settings.Default.DotBordeSize.ToString();
            cb_dotShape.SelectedIndex = Properties.Settings.Default.DotShape;
            //O codegenerator está nulo nesse momento teremos que modificar o modo de carga de plugin para corrigir?
            // code.NoBorder = Properties.Settings.Default.Weld;
            string paletter = Properties.Settings.Default.PaletteIndentifier;
            try
            {
                Palette palette = app.PaletteManager.GetPalette(paletter);
                int index = palette.FindColor(Properties.Settings.Default.BorderColor);
                Corel.Interop.VGCore.Color bColor = palette.Color[index];
                SelectedBorderColor = new ColorSystem(bColor.HexValue, bColor.Name, bColor);
                bColor = palette.Color[palette.FindColor(Properties.Settings.Default.DotFillColor)];
                SelectedDotColor = new ColorSystem(bColor.HexValue, bColor.Name, bColor);
                bColor = palette.Color[palette.FindColor(Properties.Settings.Default.DotBorderColor)];
                SelectedDotBorderColor = new ColorSystem(bColor.HexValue, bColor.Name, bColor);
            }
            catch { }

        }
        private void LoadConfigToCode()
        {
            QrCodeGenerator code = (CodeGenerator as QrCodeGenerator);
            code.NoBorder = Properties.Settings.Default.NoBorder;
            code.Weld = Properties.Settings.Default.Weld;
            code.DotShapeType = (DotShape)Properties.Settings.Default.DotShape;
            code.DotBorderSize = Properties.Settings.Default.DotBordeSize;
            if(selectedBorderColor!=null)
                code.BorderColor = selectedBorderColor.CorelColor;
            if (selectedDotColor != null)
                code.DotFillColor = selectedDotColor.CorelColor;
            if (selectedDotBorderColor != null)
                code.DotOutlineColor = selectedDotBorderColor.CorelColor;
        }

        public void DeleteConfig()
        {
            Properties.Settings.Default.Reset();
        }

    }
   
}
