using System.Diagnostics;
using static AmbiPro.AppClasses;

namespace AmbiPro.Resources
{
    public class ColorProcessing
    {
        //Get the color of a pixel
        public static ColorRGBA GetPixelColor(byte[] bitmapByteArray, int screenWidth, int screenHeight, int pixelHor, int pixelVer)
        {
            try
            {
                //Check width and height
                if (pixelHor > screenWidth || pixelHor < 0) { return null; }
                if (pixelVer > screenHeight || pixelVer < 0) { return null; }

                //Capture error correction
                if (pixelHor == screenWidth) { pixelHor--; }
                if (pixelVer == 0) { pixelVer++; }

                //Get start of the pixel
                int PixelSize = 4;
                int Pixel = PixelSize * ((screenHeight - pixelVer) * screenWidth + pixelHor);

                //Get the color from pixel
                byte b = bitmapByteArray[Pixel++];
                byte g = bitmapByteArray[Pixel++];
                byte r = bitmapByteArray[Pixel++];
                byte a = bitmapByteArray[Pixel];
                return new ColorRGBA { R = r, G = g, B = b, A = a };
            }
            catch
            {
                Debug.WriteLine("Failed to get pixel color from bitmap data.");
                return null;
            }
        }

        //Set the color of a pixel
        public static bool SetPixelColor(byte[] bitmapByteArray, int screenWidth, int screenHeight, int pixelHor, int pixelVer, ColorRGBA newColor)
        {
            try
            {
                //Check width and height
                if (pixelHor > screenWidth || pixelHor < 0) { return false; }
                if (pixelVer > screenHeight || pixelVer < 0) { return false; }

                //Capture error correction
                if (pixelHor == screenWidth) { pixelHor--; }
                if (pixelVer == 0) { pixelVer++; }

                //Get start of the pixel
                int PixelSize = 4;
                int Pixel = PixelSize * ((screenHeight - pixelVer) * screenWidth + pixelHor);

                //Set the color to pixel
                bitmapByteArray[Pixel++] = newColor.B;
                bitmapByteArray[Pixel++] = newColor.G;
                bitmapByteArray[Pixel++] = newColor.R;
                bitmapByteArray[Pixel] = newColor.A;
                return true;
            }
            catch
            {
                Debug.WriteLine("Failed to set pixel color in bitmap data.");
                return false;
            }
        }
    }
}