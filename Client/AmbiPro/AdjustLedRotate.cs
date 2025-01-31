using System;
using System.Diagnostics;
using static AmbiPro.AppClasses;
using static AmbiPro.PreloadSettings;
using static ArnoldVinkCode.AVArrayFunctions;

namespace AmbiPro
{
    public partial class SerialMonitor
    {
        //Rotate leds as calibrated
        private static void AdjustLedRotate(ColorRGBA[] colorArray)
        {
            try
            {
                int totalByteSize = colorArray.Length;
                if (setLedRotate > 0)
                {
                    for (int RotateCount = 0; RotateCount < setLedRotate; RotateCount++)
                    {
                        MoveObjectInArrayRight(colorArray, totalByteSize - 1, 0);
                    }
                }
                else if (setLedRotate < 0)
                {
                    for (int RotateCount = 0; RotateCount < Math.Abs(setLedRotate); RotateCount++)
                    {
                        MoveObjectInArrayLeft(colorArray, 0, totalByteSize - 1);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to rotate leds: " + ex.Message);
            }
        }
    }
}