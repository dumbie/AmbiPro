using System;
using System.Diagnostics;
using static AmbiPro.AppClasses;
using static AmbiPro.AppEnums;

namespace AmbiPro
{
    public partial class SerialMonitor
    {
        //Adjust the led color values to settings
        private static void AdjustLedColors(ref ColorRGBA adjustColor)
        {
            try
            {
                //Adjust the screen capture hue
                adjustColor.AdjustHue(setLedHue);

                //Adjust specific color values
                adjustColor.AdjustColorRed(setLedColorRed);
                adjustColor.AdjustColorGreen(setLedColorGreen);
                adjustColor.AdjustColorBlue(setLedColorBlue);

                //Adjust the screen capture saturation
                adjustColor.AdjustSaturation(setLedSaturation);

                //Adjust the screen capture gamma
                adjustColor.AdjustGamma(setLedGamma);

                //Adjust the screen capture brightness
                adjustColor.AdjustBrightness(setLedBrightness);

                //Check current led mode
                if (setLedMode == 0)
                {
                    //Fix Check if HDR is enabled and apply tonemapping

                    //Adjust color to the color cut off setting
                    if (adjustColor.R < setLedColorCut && adjustColor.G < setLedColorCut && adjustColor.B < setLedColorCut)
                    {
                        adjustColor.R = 0;
                        adjustColor.G = 0;
                        adjustColor.B = 0;
                    }

                    //Adjust the minimum brightness to setting
                    if (adjustColor.R < setLedMinBrightness && adjustColor.G < setLedMinBrightness && adjustColor.B < setLedMinBrightness)
                    {
                        adjustColor.R = setLedMinBrightness;
                        adjustColor.G = setLedMinBrightness;
                        adjustColor.B = setLedMinBrightness;
                        return;
                    }
                }

                //Set the color output according to setting
                if (setLedOutput == LedOutputTypes.GBR)
                {
                    adjustColor = new ColorRGBA()
                    {
                        R = adjustColor.G,
                        G = adjustColor.B,
                        B = adjustColor.R
                    };
                }
                else if (setLedOutput == LedOutputTypes.BRG)
                {
                    adjustColor = new ColorRGBA()
                    {
                        R = adjustColor.B,
                        G = adjustColor.R,
                        B = adjustColor.G
                    };
                }
                else if (setLedOutput == LedOutputTypes.RBG)
                {
                    adjustColor = new ColorRGBA()
                    {
                        R = adjustColor.R,
                        G = adjustColor.B,
                        B = adjustColor.G
                    };
                }
                else if (setLedOutput == LedOutputTypes.GRB)
                {
                    adjustColor = new ColorRGBA()
                    {
                        R = adjustColor.G,
                        G = adjustColor.R,
                        B = adjustColor.B
                    };
                }
                else if (setLedOutput == LedOutputTypes.BGR)
                {
                    adjustColor = new ColorRGBA()
                    {
                        R = adjustColor.B,
                        G = adjustColor.G,
                        B = adjustColor.R
                    };
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to adjust screen colors: " + ex.Message);
            }
        }
    }
}