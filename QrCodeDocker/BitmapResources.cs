using System;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace br.corp.bonus630.QrCodeDocker
{

    public static class BitmapResources
        {
          
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

            public static BitmapSource Bonus630
            {
                get
                {
                    return genereteBitmapSource(QrCodeDocker.Properties.Resources.bonus630);
                }
            }
            public static BitmapSource CorelNaVeia2015
            {
                get
                {
                    return genereteBitmapSource(QrCodeDocker.Properties.Resources.CorelNaVeia2015);
                }
            }
        }
    

}
