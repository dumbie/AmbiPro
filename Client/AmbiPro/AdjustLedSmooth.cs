using System;
using System.Diagnostics;
using static AmbiPro.AppClasses;
using static AmbiPro.AppVariables;
using static AmbiPro.PreloadSettings;
using static ArnoldVinkCode.AVArrayFunctions;

namespace AmbiPro
{
    public partial class SerialMonitor
    {
        //Adjust leds to smooth frame transition
        /// <summary>
        /// Note: this reduces flickering when scenes change quickly or camera is pointed to flames or helicopter blades
        /// </summary>
        private static void AdjustLedSmoothFrame(ColorRGBA[] colorArray)
        {
            try
            {
                if (setLedSmoothFrame > 0)
                {
                    //Make copy of current colors
                    ColorRGBA[] colorArrayCopy = CloneObjectArray(colorArray);

                    //Blend current with history led colors
                    for (int ledCount = 0; ledCount < colorArray.Length; ledCount++)
                    {
                        int colorCount = 1;
                        int colorAverageR = colorArray[ledCount].R;
                        int colorAverageG = colorArray[ledCount].G;
                        int colorAverageB = colorArray[ledCount].B;
                        //Debug.WriteLine("Frame smoothing old: R" + colorArray[ledCount].R + "/G" + colorArray[ledCount].G + "/B" + colorArray[ledCount].B);

                        for (int smoothCount = 0; smoothCount < setLedSmoothFrame; smoothCount++)
                        {
                            if (vCaptureHistoryArray[smoothCount] != null)
                            {
                                ColorRGBA colorHistory = vCaptureHistoryArray[smoothCount][ledCount];
                                colorAverageR += colorHistory.R;
                                colorAverageG += colorHistory.G;
                                colorAverageB += colorHistory.B;
                                colorCount++;
                            }
                            else
                            {
                                break;
                            }
                        }

                        colorArray[ledCount].R = (byte)(colorAverageR / colorCount);
                        colorArray[ledCount].G = (byte)(colorAverageG / colorCount);
                        colorArray[ledCount].B = (byte)(colorAverageB / colorCount);
                        //Debug.WriteLine("Frame smoothing new: R" + colorArray[ledCount].R + "/G" + colorArray[ledCount].G + "/B" + colorArray[ledCount].B + "/C" + colorCount);
                    }

                    //Update capture color history
                    InsertObjectBegin(vCaptureHistoryArray, colorArrayCopy);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to smooth frame: " + ex.Message);
            }
        }

        //Adjust leds to smooth object movement
        /// <summary>
        /// Note: this reduces flashing when objects are moving between leds like subtitle scrolling
        /// </summary>
        private static void AdjustLedSmoothObject(ColorRGBA[] colorArray)
        {
            try
            {
                if (setLedSmoothObject > 0)
                {
                    //Blend neighboring led colors together
                    int colorAverageR = 0;
                    int colorAverageG = 0;
                    int colorAverageB = 0;
                    int colorBlendCount = 0;
                    int colorSmoothCount = setLedSmoothObject + 1;
                    for (int ledCount = 0; ledCount < colorArray.Length; ledCount++)
                    {
                        //Accumulate colors
                        colorAverageR += colorArray[ledCount].R;
                        colorAverageG += colorArray[ledCount].G;
                        colorAverageB += colorArray[ledCount].B;

                        //Update variables
                        colorBlendCount++;

                        if (colorBlendCount == colorSmoothCount)
                        {
                            //Blend colors
                            byte colorBlendR = (byte)(colorAverageR / colorBlendCount);
                            byte colorBlendG = (byte)(colorAverageG / colorBlendCount);
                            byte colorBlendB = (byte)(colorAverageB / colorBlendCount);
                            for (int blendIndex = 0; blendIndex < colorSmoothCount; blendIndex++)
                            {
                                colorArray[ledCount - blendIndex].R = colorBlendR;
                                colorArray[ledCount - blendIndex].G = colorBlendG;
                                colorArray[ledCount - blendIndex].B = colorBlendB;
                            }

                            //Update variables
                            colorAverageR = 0;
                            colorAverageG = 0;
                            colorAverageB = 0;
                            colorBlendCount = 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to smooth object: " + ex.Message);
            }
        }
    }
}