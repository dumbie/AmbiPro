using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;

namespace AmbiPro.Resources
{
    public class BitmapProcessing
    {
        //Convert data to bitmap
        public static unsafe Bitmap BitmapConvertData(byte* bitmapData, int bitmapWidth, int bitmapHeight, int bitmapSize, bool flipImage)
        {
            try
            {
                Bitmap imageBitmap = new Bitmap(bitmapWidth, bitmapHeight);
                Rectangle ScreenRectangle = new Rectangle(0, 0, imageBitmap.Width, imageBitmap.Height);

                BitmapData ScreenBitmapData = imageBitmap.LockBits(ScreenRectangle, ImageLockMode.ReadWrite, imageBitmap.PixelFormat);
                byte* imageData = (byte*)ScreenBitmapData.Scan0;
                for (int y = 0; y < bitmapSize; y++) { imageData[y] = bitmapData[y]; }
                imageBitmap.UnlockBits(ScreenBitmapData);

                if (flipImage)
                {
                    imageBitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);
                }

                return imageBitmap;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to convert data to bitmap: " + ex.Message);
                return null;
            }
        }

        //Get data from bitmap
        public static unsafe byte* BitmapGetData(Bitmap imageBitmap)
        {
            try
            {
                Rectangle ScreenRectangle = new Rectangle(0, 0, imageBitmap.Width, imageBitmap.Height);

                BitmapData ScreenBitmapData = imageBitmap.LockBits(ScreenRectangle, ImageLockMode.ReadWrite, imageBitmap.PixelFormat);
                byte* imageData = (byte*)ScreenBitmapData.Scan0;
                imageBitmap.UnlockBits(ScreenBitmapData);

                return imageData;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to convert data to bitmap: " + ex.Message);
                return null;
            }
        }

        //Save screen capture as image file
        public static void BitmapSaveScreenCapture(Bitmap bitmapSave)
        {
            try
            {
                //Create directory
                if (!Directory.Exists("Debug"))
                {
                    Directory.CreateDirectory("Debug");
                }

                //Save image file
                bitmapSave.Save("Debug\\" + Environment.TickCount + ".bmp", ImageFormat.Bmp);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to save debug bitmap image: " + ex.Message);
            }
        }

        //Convert bitmap to bitmapimage
        public static BitmapImage BitmapToBitmapImage(Bitmap bitmap)
        {
            try
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    bitmap.Save(memoryStream, ImageFormat.Bmp);
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.StreamSource = memoryStream;
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.EndInit();
                    return bitmapImage;
                }
            }
            catch { }
            return null;
        }

        //Apply color matrix to bitmap
        public static void BitmapApplyColorMatrix(Bitmap imageBitmap, ColorMatrix colorMatrix)
        {
            try
            {
                //Get graphics from image
                using (Graphics imageGraphics = Graphics.FromImage(imageBitmap))
                {
                    //Create screen rectangle
                    Rectangle screenRectangle = new Rectangle(0, 0, imageBitmap.Width, imageBitmap.Height);

                    //Create image attributes
                    using (ImageAttributes imageAttributes = new ImageAttributes())
                    {
                        imageAttributes.SetColorMatrix(colorMatrix);
                        imageGraphics.DrawImage(imageBitmap, screenRectangle, 0, 0, imageBitmap.Width, imageBitmap.Height, GraphicsUnit.Pixel, imageAttributes);
                    }
                }
            }
            catch { }
        }
    }
}