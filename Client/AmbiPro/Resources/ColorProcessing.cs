using System.Diagnostics;
using System.Drawing;

namespace AmbiPro.Resources
{
    public class ColorProcessing
    {
        //Get the color of a pixel
        public static unsafe Color GetPixelColor(byte* bitmapData, int screenWidth, int screenHeight, int pixelHor, int pixelVer)
        {
            try
            {
                //Check width and height
                if (pixelHor > screenWidth || pixelHor < 0) { return Color.Empty; }
                if (pixelVer > screenHeight || pixelVer < 0) { return Color.Empty; }

                //Capture error correction
                if (pixelHor == screenWidth) { pixelHor = screenWidth - 1; }
                if (pixelVer == 0) { pixelVer = 1; }

                //Get start of the pixel
                int PixelSize = 4;
                int Pixel = PixelSize * ((screenHeight - pixelVer) * screenWidth + pixelHor);

                //Get the color from pixel
                byte b = bitmapData[Pixel++];
                byte g = bitmapData[Pixel++];
                byte r = bitmapData[Pixel++];
                byte a = bitmapData[Pixel];
                return Color.FromArgb(a, r, g, b);
            }
            catch
            {
                Debug.WriteLine("Failed to get pixel color from bitmap data.");
                return Color.Empty;
            }
        }

        //Set the color of a pixel
        public static unsafe bool SetPixelColor(byte* bitmapData, int screenWidth, int screenHeight, int pixelHor, int pixelVer, Color newColor)
        {
            try
            {
                //Check width and height
                if (pixelHor > screenWidth || pixelHor < 0) { return false; }
                if (pixelVer > screenHeight || pixelVer < 0) { return false; }

                //Capture error correction
                if (pixelHor == screenWidth) { pixelHor = screenWidth - 1; }
                if (pixelVer == 0) { pixelVer = 1; }

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