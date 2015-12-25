using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace br.corp.bonus630.QrCodeDocker
{
    
        public class BitmapResources
        {
          
            private BitmapSource genereteBitmapSource(System.Drawing.Bitmap resource)
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

            public BitmapSource Bonus630
            {
                get
                {
                    return genereteBitmapSource(QrCodeDocker.Properties.Resources.bonus630);
                }
            }
            public BitmapSource CorelNaVeia2015
            {
                get
                {
                    return genereteBitmapSource(QrCodeDocker.Properties.Resources.CorelNaVeia2015);
                }
            }
        }
    

}
