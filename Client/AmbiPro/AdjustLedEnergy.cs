using System;
using System.Diagnostics;
using static AmbiPro.AppClasses;
using static AmbiPro.PreloadSettings;

namespace AmbiPro
{
    public partial class SerialMonitor
    {
        //Adjust to led energy mode
        private static void AdjustLedEnergyMode(ColorRGBA[] colorArray)
        {
            try
            {
                if (setLedEnergyMode)
                {
                    int currentLedSkip = 0;
                    foreach (ColorRGBA colorRGBA in colorArray)
                    {
                        if (currentLedSkip == 0)
                        {
                            currentLedSkip = 1;
                        }
                        else
                        {
                            colorRGBA.R = 0;
                            colorRGBA.G = 0;
                            colorRGBA.B = 0;
                            colorRGBA.A = 255;
                            currentLedSkip = 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to adjust led energy mode: " + ex.Message);
            }
        }
    }
}