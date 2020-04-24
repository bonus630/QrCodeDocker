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
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace br.corp.bonus630.plugin.ZxingQrCodeConfigurator
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class ZxingQrCodeConfiguratorUI : UserControl,IPluginUI,IPluginConfig
    {
        

        public ZxingQrCodeConfiguratorUI()
        {
            InitializeComponent();
            
        }
        private Corel.Interop.VGCore.Application app;
        public Corel.Interop.VGCore.Application App { set { this.app = value; } }
        public ICodeGenerator CodeGenerator { get; set; }
        public int Index { get; set; }

        public event Action<object> FinishJob;
        public event Action<int> ProgressChange;
        public event Action<object, Type> GetCodeGenerator;
        public event Action<string> AnyTextChanged;

        public void OnFinishJob(object obj)
        {
            throw new NotImplementedException();
        }

        public void OnGetCodeGenerator(object sender, Type type)
        {
            if (CodeGenerator == null)
            {
                if(GetCodeGenerator!=null)
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
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            OnGetCodeGenerator(this, typeof(QrCodeGenerator));
            ck_weld.IsChecked = (CodeGenerator as QrCodeGenerator).Weld;
            if (this.app.ActivePalette != null)
            {
                ColorManager colorManager = new ColorManager(this.app.ActivePalette);
                cb_borderColor.ItemsSource = colorManager.ColorArray;
                cb_dotColor.ItemsSource = colorManager.ColorArray;
                cb_dotBorderColor.ItemsSource = colorManager.ColorArray;
            }
        }

        private void cb_borderColor_DropDownClosed(object sender, EventArgs e)
        {
            if(cb_borderColor.SelectedItem != null)
                (CodeGenerator as QrCodeGenerator).BorderColor = (cb_borderColor.SelectedItem as ColorSystem).CorelColor;
        }

        private void cb_dotColor_DropDownClosed(object sender, EventArgs e)
        {
            if(cb_dotColor.SelectedItem != null)
                (CodeGenerator as QrCodeGenerator).DotFillColor = (cb_dotColor.SelectedItem as ColorSystem).CorelColor;
        }
        private void cb_dotBorderColor_DropDownClosed(object sender, EventArgs e)
        {
            if (cb_dotBorderColor.SelectedItem != null)
                (CodeGenerator as QrCodeGenerator).DotOutlineColor = (cb_dotBorderColor.SelectedItem as ColorSystem).CorelColor;
        }

        private void btn_validate_Click(object sender, RoutedEventArgs e)
        {
            if((this.app.ActiveSelection != null && this.app.ActiveSelection.Shapes.Count > 0) || this.app.ActiveShape != null)
            {
                string filePath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),"qrcode");
                if(!System.IO.Directory.Exists(filePath))
                {
                    System.IO.Directory.CreateDirectory(filePath);
                }
                filePath = System.IO.Path.Combine(filePath, "temp_decode.jpg");
                
                ExportFilter ex = this.app.ActiveDocument.ExportBitmap(filePath,Corel.Interop.VGCore.cdrFilter.cdrJPEG,Corel.Interop.VGCore.cdrExportRange.cdrSelection,Corel.Interop.VGCore.cdrImageType.cdrRGBColorImage);
                ex.Finish();
                try
                {
                    string text = CodeGenerator.DecodeImage(filePath);
                    
                    app.MsgShow(text,"Your Qrcode contains the follow message:");
                    //ValidateWindow val = new ValidateWindow(text, filePath);
                    //val.Show();
                    //val.LostFocus += (s, ev) => { val.Close(); };
                }
                catch(NotImplementedException ex1)
                {
                    app.MsgShow("Change your encoding to Zxing to validate!","Warning",QrCodeDocker.MessageBox.DialogButtons.Ok);
                }
                catch(Exception ex2)
                {
                    app.MsgShow("Invalid qrcode");
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
            }
        }

        private void cb_dotShape_DropDownClosed(object sender, EventArgs e)
        {
            if(cb_dotShape.SelectedIndex != -1)
                (CodeGenerator as QrCodeGenerator).DotShapeType = (DotShape)cb_dotShape.SelectedIndex;
        }

        public void restoreDefault()
        {
            this.txt_dotBorderSize.Text = "0";
        }
        private void checkTextFormat(object sender, TextCompositionEventArgs e)
        {
            string pattern =  @"^(-)?(\d+)?([\.|,]{1})?([\d]{0,2})?$";
           


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
    }
}
