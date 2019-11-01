using AmbiPro.Resources;
using System;
using System.Diagnostics;
using System.Drawing;

namespace AmbiPro
{
    public partial class SerialMonitor
    {
        //Adjust the led color values to settings
        private static Color AdjustLedColors(Color Adjust)
        {
            try
            {
                //Convert Color to HSL
                HslColor.HSL ConvertedHslColor = HslColor.ToHsl(Adjust);

                //Adjust the screen capture saturation / vibrance
                if (setLedVibrance != 1.00) { ConvertedHslColor = HslColor.ModifySaturation(ConvertedHslColor, setLedVibrance); }

                //Adjust the screen capture hue / shade
                if (setLedHue != 1.00) { ConvertedHslColor = HslColor.ModifyHue(ConvertedHslColor, setLedHue); }

                //Adjust the screen capture color brightness
                if (setLedBrightness != 1.00) { ConvertedHslColor = HslColor.ModifyBrightness(ConvertedHslColor, setLedBrightness); }

                //Convert HSL to Color
                Adjust = HslColor.ToColor(ConvertedHslColor);

                //Adjust the screen capture gamma level
                if (setLedGamma != 1.00)
                {
                    Int32 RedGamma = Convert.ToInt32((255.0 * Math.Pow(Adjust.R / 255.0, 1.0 / setLedGamma)));
                    Int32 GreenGamma = Convert.ToInt32((255.0 * Math.Pow(Adjust.G / 255.0, 1.0 / setLedGamma)));
                    Int32 BlueGamma = Convert.ToInt32((255.0 * Math.Pow(Adjust.B / 255.0, 1.0 / setLedGamma)));
                    Adjust = Color.FromArgb(0, RedGamma, GreenGamma, BlueGamma);
                }

                //Adjust specific color values
                if (setLedColorRed != 1.00 || setLedColorGreen != 1.00 || setLedColorBlue != 1.00)
                {
                    Int32 AdjustedRed = Convert.ToInt32(Adjust.R * setLedColorRed); if (AdjustedRed > 255) { AdjustedRed = 255; }
                    Int32 AdjustedGreen = Convert.ToInt32(Adjust.G * setLedColorGreen); if (AdjustedGreen > 255) { AdjustedGreen = 255; }
                    Int32 AdjustedBlue = Convert.ToInt32(Adjust.B * setLedColorBlue); if (AdjustedBlue > 255) { AdjustedBlue = 255; }
                    Adjust = Color.FromArgb(0, AdjustedRed, AdjustedGreen, AdjustedBlue);
                }

                //Adjust color to the color cut off setting
                if (Adjust.R < setLedColorCut && Adjust.G < setLedColorCut && Adjust.B < setLedColorCut) { Adjust = Color.FromArgb(0, 0, 0, 0); }

                //Adjust the minimum brightness to settings
                if (Adjust.R < setLedMinBrightness && Adjust.G < setLedMinBrightness && Adjust.B < setLedMinBrightness) { Adjust = Color.FromArgb(0, setLedMinBrightness, setLedMinBrightness, setLedMinBrightness); }

                //Set the color output according to setting
                //Green, blue and red
                if (setLedOutput == 1) { Adjust = Color.FromArgb(0, Adjust.G, Adjust.B, Adjust.R); }
                //Blue, red and green
                else if (setLedOutput == 2) { Adjust = Color.FromArgb(0, Adjust.B, Adjust.R, Adjust.G); }
                //Red, blue and green
                else if (setLedOutput == 3) { Adjust = Color.FromArgb(0, Adjust.R, Adjust.B, Adjust.G); }
                //Green, red and blue
                else if (setLedOutput == 4) { Adjust = Color.FromArgb(0, Adjust.G, Adjust.R, Adjust.B); }
                //Blue, green and red
                else if (setLedOutput == 5) { Adjust = Color.FromArgb(0, Adjust.B, Adjust.G, Adjust.R); }
            }
            catch (Exception ex) { Debug.WriteLine("Failed to adjust screen colors: " + ex.Message); }
            return Adjust;
        }
    }
}