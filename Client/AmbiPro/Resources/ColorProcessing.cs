using System.Diagnostics;
using static AmbiPro.AppClasses;

namespace AmbiPro.Resources
{
    public class ColorProcessing
    {
        //Get the color of a pixel
        public static unsafe ColorRGBA GetPixelColor(byte* bitmapData, int screenWidth, int screenHeight, int pixelHor, int pixelVer)
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
                byte b = bitmapData[Pixel++];
                byte g = bitmapData[Pixel++];
                byte r = bitmapData[Pixel++];
                byte a = bitmapData[Pixel];
                return new ColorRGBA { R = r, G = g, B = b, A = a };
            }
            catch
            {
                Debug.WriteLine("Failed to get pixel color from bitmap data.");
                return null;
            }
        }

        //Set the color of a pixel
        public static unsafe bool SetPixelColor(byte* bitmapData, int screenWidth, int screenHeight, int pixelHor, int pixelVer, ColorRGBA newColor)
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
                bitmapData[Pixel++] = newColor.B;
                bitmapData[Pixel++] = newColor.G;
                bitmapData[Pixel++] = newColor.R;
                bitmapData[Pixel] = newColor.A;
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