﻿using br.corp.bonus630.QrCodeDocker;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace br.corp.bonus630.plugin.MediaSchema
{
    public class BitmapResources
    {
        public static BitmapSource generateBitmapSource(Bitmap resource)
        {
           
            var bitmap = new System.Drawing.Bitmap(resource);
            switch (StyleController.ThemeShortName)
            {
                case "Black":
                    bitmap = Transform(bitmap);
                    break;
                case "DarkGrey":
                    bitmap = Transform(bitmap);
                    break;
            }


            var bitmapSource = Imaging.CreateBitmapSourceFromHBitmap(bitmap.GetHbitmap(),
                                                                                  IntPtr.Zero,
                                                                                  Int32Rect.Empty,
                                                                                  BitmapSizeOptions.FromEmptyOptions()
                  );

            bitmap.Dispose();

            return bitmapSource;
        }

        public static Bitmap Transform(Bitmap source)
        {
            //create a blank bitmap the same size as original
            Bitmap newBitmap = new Bitmap(source.Width, source.Height);

            //get a graphics object from the new image
            Graphics g = Graphics.FromImage(newBitmap);
            
            // create the negative color matrix
            ColorMatrix colorMatrix = new ColorMatrix(new float[][]
            {
        new float[] {-1, 0, 0, 0, 0},
        new float[] {0, -1, 0, 0, 0},
        new float[] {0, 0, -1, 0, 0},
        new float[] {0, 0, 0, 1, 0},
        new float[] {1, 1, 1, 0, 1}
            });

            // create some image attributes
            ImageAttributes attributes = new ImageAttributes();

            attributes.SetColorMatrix(colorMatrix);

            g.DrawImage(source, new Rectangle(0, 0, source.Width, source.Height),
                        0, 0, source.Width, source.Height, GraphicsUnit.Pixel, attributes);

            //dispose the Graphics object
            g.Dispose();

            return newBitmap;
        }
        private System.Drawing.Bitmap InvertBlacks(System.Drawing.Bitmap bitmap)
        {
            for (int x = 0; x < bitmap.HorizontalResolution - 1; x++)
            {
                for (int y = 0; y < bitmap.VerticalResolution - 1; y++)
                {
                    var color = bitmap.GetPixel(x, y);
                    float b = color.GetBrightness();
                    if (b < 0.2)
                    {
                        bitmap.SetPixel(x, y, System.Drawing.Color.White);
                    }
                }
            }
            return bitmap;
        }
     
        private static System.Drawing.Bitmap GetBitmap(string ResourceName)
        {
            ResourceManager rm = new ResourceManager("br.corp.bonus630.plugin.MediaSchema.Properties.Resources",
            Assembly.GetExecutingAssembly());
            try
            {
                return (System.Drawing.Bitmap)rm.GetObject(ResourceName);
            }
            catch (ArgumentNullException ex)

            { throw ex; }

        }
    }
}
