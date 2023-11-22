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

namespace br.corp.bonus630.plugin.ZxingQrCodeConfigurator
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class ZxingQrCodeConfiguratorUI : UserControl, IPluginMainUI
    {
        ZxingConfiguratorCore zcCore;
        Corel.Interop.VGCore.Application app;


        public ZxingQrCodeConfiguratorUI()
        {
            InitializeComponent();
            this.Loaded += ZxingQrCodeConfiguratorUI_Loaded;

        }

        private void ZxingQrCodeConfiguratorUI_Loaded(object sender, RoutedEventArgs e)
        {
            zcCore = Core as ZxingConfiguratorCore;
            zcCore.LoadConfigEvent += ZcCore_LoadConfigEvent;
            app = zcCore.App;
        }

        private void ZcCore_LoadConfigEvent()
        {
            LoadConfig();
        }

        public IPluginCore Core { get; set; }


        private void ck_weld_Click(object sender, RoutedEventArgs e)
        {
            zcCore.Weld = (bool)ck_weld.IsChecked;

        }
        private void btn_validate_Click(object sender, RoutedEventArgs e)
        {

            string text = "";
            
            TryGetValidText(out text);
            if (text == zcCore.GetLocalizedString("MBOX_ERRO_QRInvalid"))
                app.MsgShow(zcCore.GetLocalizedString("MBOX_ERRO_QRInvalid"));
            else if (text == zcCore.GetLocalizedString("MBOX_QrCodingWarning"))
                app.MsgShow(zcCore.GetLocalizedString("MBOX_QrCodingWarning"), zcCore.GetLocalizedString("Warning"), QrCodeDocker.MessageBox.DialogButtons.Ok);
            else
                app.MsgShow(text, zcCore.GetLocalizedString("MBOX_QrMessage"));
            
        }
        private bool TryGetValidText(out string result)
        {
            bool sucess = false;
            result = "";
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
                    result = zcCore.CodeGenerator.DecodeImage(filePath);
                    sucess = true;
                }
                catch (NotImplementedException ex1)
                {
                    result = zcCore.GetLocalizedString("MBOX_QrCodingWarning");
                }
                catch (Exception ex2)
                {
                    result = zcCore.GetLocalizedString("MBOX_ERRO_QRInvalid");
                }
                finally
                {
                    if (System.IO.File.Exists(filePath))
                        System.IO.File.Delete(filePath);
                }
            }
            return sucess;
        }

        private void txt_dotBorderSize_PreviewKeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            double size = 0;
            if (Double.TryParse(txt_dotBorderSize.Text, out size))
            {
                zcCore.DotBorderSize = size;
            }
        }

        private void cb_dotShape_DropDownClosed(object sender, EventArgs e)
        {
            if (cb_dotShape.SelectedIndex != -1)
            {
                zcCore.DotShapeType = (DotShape)cb_dotShape.SelectedIndex;

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
                zcCore.SelectedBorderColor = c.SelectedColor;

            }
        }

        private void btn_DotColor_Click(object sender, EventArgs e)
        {
            ColorPicker c = GetColorPicker(sender);
            if ((bool)c.ShowDialog())
            {
                zcCore.SelectedDotColor = c.SelectedColor;
            }
        }

        private void btn_DotBorderColor_Click(object sender, EventArgs e)
        {
            ColorPicker c = GetColorPicker(sender);
            if ((bool)c.ShowDialog())
            {
                zcCore.SelectedDotBorderColor = c.SelectedColor;

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
            zcCore.NoBorder = (bool)ck_noBorder.IsChecked;
            btn_BorderColor.IsEnabled = !(bool)ck_noBorder.IsChecked;
        }

        public void LoadConfig()
        {
            ck_noBorder.IsChecked = Properties.Settings.Default.NoBorder;
            ck_weld.IsChecked = Properties.Settings.Default.Weld;
            txt_dotBorderSize.Text = Properties.Settings.Default.DotBordeSize.ToString();
            cb_dotShape.SelectedIndex = Properties.Settings.Default.DotShape;
        }


        public void DeleteConfig()
        {
            Properties.Settings.Default.Reset();
        }

        private void btn_validateAndDraw_Click(object sender, RoutedEventArgs e)
        {
            string text = "";
            if(TryGetValidText(out text))
            {
                System.Windows.Clipboard.SetText(text);
            }
        }
    }

}
