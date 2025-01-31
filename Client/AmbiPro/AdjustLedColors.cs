using System;
using System.Diagnostics;
using static AmbiPro.AppClasses;
using static AmbiPro.AppEnums;
using static AmbiPro.PreloadSettings;

namespace AmbiPro
{
    public partial class SerialMonitor
    {
        //Adjust led color values to settings
        private static void AdjustLedColors(ColorRGBA[] colorArray)
        {
            try
            {
                foreach (ColorRGBA adjustColor in colorArray)
                {
                    //Adjust specific color values
                    adjustColor.AdjustColorChannels(setLedColorRed, setLedColorGreen, setLedColorBlue);

                    //Adjust led saturation
                    adjustColor.AdjustSaturation(setLedSaturation);

                    //Adjust led brightness
                    adjustColor.AdjustBrightness(setLedBrightness);

                    //Adjust led contrast
                    adjustColor.AdjustContrast(setLedContrast);

                    //Adjust led gamma
                    adjustColor.AdjustGamma(setLedGamma);

                    //Check current led mode
                    if (setLedMode == 0)
                    {
                        //Calculate color luminance
                        int colorLuminance = adjustColor.CalculateLuminance();

                        //Adjust color to the minimum color setting
                        if (colorLuminance < setLedMinColor)
                        {
                            //Set color to black
                            adjustColor.R = 0;
                            adjustColor.G = 0;
                            adjustColor.B = 0;

                            //Update color luminance
                            colorLuminance = 0;
                        }

                        //Adjust led to the minimum brightness setting
                        if (colorLuminance < setLedMinBrightness)
                        {
                            adjustColor.R = setLedMinBrightness;
                            adjustColor.G = setLedMinBrightness;
                            adjustColor.B = setLedMinBrightness;
                            return;
                        }
                    }

                    //Set color output according to setting
                    if (setLedOutput == LedOutputTypes.GBR)
                    {
                        byte currentRed = adjustColor.R;
                        byte currentGreen = adjustColor.G;
                        byte currentBlue = adjustColor.B;
                        adjustColor.R = currentGreen;
                        adjustColor.G = currentBlue;
                        adjustColor.B = currentRed;
                    }
                    else if (setLedOutput == LedOutputTypes.BRG)
                    {
                        byte currentRed = adjustColor.R;
                        byte currentGreen = adjustColor.G;
                        byte currentBlue = adjustColor.B;
                        adjustColor.R = currentBlue;
                        adjustColor.G = currentRed;
                        adjustColor.B = currentGreen;
                    }
                    else if (setLedOutput == LedOutputTypes.RBG)
                    {
                        byte currentRed = adjustColor.R;
                        byte currentGreen = adjustColor.G;
                        byte currentBlue = adjustColor.B;
                        adjustColor.R = currentRed;
                        adjustColor.G = currentBlue;
                        adjustColor.B = currentGreen;
                    }
                    else if (setLedOutput == LedOutputTypes.GRB)
                    {
                        byte currentRed = adjustColor.R;
                        byte currentGreen = adjustColor.G;
                        byte currentBlue = adjustColor.B;
                        adjustColor.R = currentGreen;
                        adjustColor.G = currentRed;
                        adjustColor.B = currentBlue;
                    }
                    else if (setLedOutput == LedOutputTypes.BGR)
                    {
                        byte currentRed = adjustColor.R;
                        byte currentGreen = adjustColor.G;
                        byte currentBlue = adjustColor.B;
                        adjustColor.R = currentBlue;
                        adjustColor.G = currentGreen;
                        adjustColor.B = currentRed;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to adjust led colors: " + ex.Message);
            }
        }
    }
}