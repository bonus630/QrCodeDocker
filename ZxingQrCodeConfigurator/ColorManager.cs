using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using Corel.Interop.VGCore;


namespace br.corp.bonus630.plugin.ZxingQrCodeConfigurator
{
    public class ColorManager : NotifyPropertyBase
    {
        private ColorSystem[] colorArray;
        public ColorSystem[] ColorArray { get { return this.colorArray; } }
        public RoutedCommand<ColorSystem> ColorSelected { get { return new RoutedCommand<ColorSystem>(SetSelectedColor); } }
        public RoutedCommand<object> OpenEyeDropper { get { return new RoutedCommand<object>(OpenEyedropper); } }
        public bool InEyedropper { get; set; }
        private ColorSystem selectedColor;
        private Corel.Interop.VGCore.Application application;
        public Corel.Interop.VGCore.Application App { get { return application; } }
        
        private Eyedropper eyedropper;
        public event Action ExitEyedropper;
        public ColorSystem SelectedColor
        {
            get
            {
                return selectedColor;
            }
            set
            {
                selectedColor = value;
                NotifyPropertyChanged("SelectedColor");
            }
        }
        public string PaletteName { get; set; }
        private void SetSelectedColor(ColorSystem color)
        {
            SelectedColor = color;
        }
        public BitmapSource EyedropperIcon { get { return genereteBitmapSource(Properties.Resources.icons8_color_dropper_48); } }
        public static BitmapSource genereteBitmapSource(System.Drawing.Bitmap resource)
        {
            var image = resource;
            var bitmap = new System.Drawing.Bitmap(image);
            var bitmapSource = Imaging.CreateBitmapSourceFromHBitmap(bitmap.GetHbitmap(),
                                                                                  IntPtr.Zero,
                                                                                  Int32Rect.Empty,
                                                                                  BitmapSizeOptions.FromEmptyOptions()
                  );

            bitmap.Dispose();
            return bitmapSource;
        }
        //public static ColorSystem CreateColorSystemFromHex (string hexValue,Palette palette)
        //{
        //    int colorValue = Convert.ToInt32(hexValue.Substring(1),16);
        //    System.Drawing.Color color = System.Drawing.Color.FromArgb(colorValue);
        //    for (int i = 1; i < Palette palette; i++)
        //    {

        //    }
        //}
        public void Close()
        {
            eyedropper.Close();
        }
        public void OpenEyedropper(object o)
        {
            InEyedropper = true;
            double x = 0;
            double y = 0;
            int a = 0;
            eyedropper = new Eyedropper(application);
            eyedropper.Show();
            this.application.ActiveDocument.GetUserClick(out x, out y, out a, 0, false, cdrCursorShape.cdrCursorEyeDrop);
            Shape s = this.application.ActivePage.FindShapeAtPoint(x, y, false);
            if (s != null)
            {
                Color color = s.Fill.UniformColor;
                SetSelectedColor(new ColorSystem(color.HexValue, color.Name, color));
            }
            Debug.WriteLine("{0}-{1} getuserclick", x, y);
            //  System.Windows.MessageBox.Show(string.Format("{0} - {1}",x,y));
            eyedropper.Close();
            //colorPicker.ShowDialog();
            InEyedropper = false;
            //if (ExitEyedropper != null)
            //    ExitEyedropper();
        }
        private ColorPicker colorPicker;
        public ColorManager(Palette palette,ColorPicker colorPicker)
        {
            this.colorPicker = colorPicker;
            if (palette == null)
                return;
            this.application = palette.Application as Corel.Interop.VGCore.Application;
            PaletteName = palette.Name;
            colorArray = new ColorSystem[palette.ColorCount];
            for (int i = 1; i < palette.ColorCount; i++)
            {
                Color color = palette.Color[i];
                colorArray[i - 1] = new ColorSystem(color.HexValue, color.Name, color);
                Debug.WriteLine(color.HexValue);
            }

        }


       
    }
    public class RoutedCommand<T> : ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public Action<T> action;


        public RoutedCommand(Action<T> action)
        {
            this.action = action;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            action.Invoke((T)parameter);
        }
  
    }
    public interface IColorSystem
    {
        string ColorHexValue { get; set; }
        string CorelColorName { get; set; }
        Color CorelColor { get; set; }
    }
    public class ColorSystem:IColorSystem
    {
        public ColorSystem(string colorHexValue, string corelColorName, Color corelColor)
        {
            this.colorHexValue = colorHexValue;
            this.corelColorName = corelColorName;
            this.corelColor = corelColor;
        }


        private string colorHexValue;

        public string ColorHexValue
        {
            get { return colorHexValue; }
            set { colorHexValue = value; }
        }
        private string corelColorName;

        public string CorelColorName
        {
            get { return corelColorName; }
            set { corelColorName = value; }
        }
        private Color corelColor;
        public Color CorelColor
        {
            get { return corelColor; }
            set { corelColor = value; }
        }
    }


}
