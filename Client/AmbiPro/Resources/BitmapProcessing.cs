using System;
using System.Diagnostics;
using System.Drawing;

namespace AmbiPro.Resources
{
    public class BitmapProcessing
    {
        //Get the color of a pixel
        public static unsafe Color GetPixelColor(byte* BitmapData, Int32 Width, Int32 Height, Int32 HorX, Int32 VerY)
        {
            Color PixelColor = Color.Empty;
            try
            {
                //Get start of the pixel
                Int32 i = (((Height - VerY) * Width) + HorX) * 4;

                //Get the color from pixel
                Byte b = BitmapData[i];
                Byte g = BitmapData[i + 1];
                Byte r = BitmapData[i + 2];
                Byte a = BitmapData[i + 3];
                PixelColor = Color.FromArgb(a, r, g, b);
            }
            catch { Debug.WriteLine("Failed to get pixel color from bitmap data."); }
            return PixelColor;
        }

        //Set the color of a pixel
        public static unsafe void SetPixelColor(byte* BitmapData, Int32 Width, Int32 Height, Int32 HorX, Int32 VerY, Color NewColor)
        {
            try
            {
                //Get start of the pixel
                Int32 i = (((Height - VerY) * Width) + HorX) * 4;

                //Set the color to pixel
                BitmapData[i] = NewColor.B;
                BitmapData[i + 1] = NewColor.G;
                BitmapData[i + 2] = NewColor.R;
                BitmapData[i + 3] = NewColor.A;
            }
            catch { Debug.WriteLine("Failed to set pixel color in bitmap data."); }
        }
    }
}