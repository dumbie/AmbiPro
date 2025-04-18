using System;
using System.Collections.Generic;
using System.Diagnostics;
using static AmbiPro.AdjustColorMerge;
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
        /// Opacity method is more cpu efficient but it darkens color.
        /// </summary>
        private static void AdjustLedSmoothFrameOpacity(ColorRGBA[] colorArray)
        {
            try
            {
                if (setLedSmoothFrame <= 0) { return; }
                float setLedSmoothFrameTemp = setLedSmoothFrame / 100;

                //Merge current with previous color
                if (vCaptureColorHistoryOpacity != null)
                {
                    for (int ledIndex = 0; ledIndex < colorArray.Length; ledIndex++)
                    {
                        //Merge colors
                        colorArray[ledIndex] = ColorMergeSqrt(colorArray[ledIndex], vCaptureColorHistoryOpacity[ledIndex], setLedSmoothFrameTemp);
                    }
                }

                //Update previous capture color
                vCaptureColorHistoryOpacity = CloneObjectArray(colorArray);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to smooth frame: " + ex.Message);
            }
        }

        private static void AdjustLedSmoothFrameMerge(ColorRGBA[] colorArray)
        {
            try
            {
                if (setLedSmoothFrame <= 0) { return; }

                //Make copy of current colors
                ColorRGBA[] colorArrayCopy = CloneObjectArray(colorArray);

                //Blend current with history led colors
                List<ColorRGBA> colorMergeList = [];
                for (int ledIndex = 0; ledIndex < colorArray.Length; ledIndex++)
                {
                    //Add current color
                    colorMergeList.Add(colorArray[ledIndex]);

                    //Add colors from history
                    for (int smoothCount = 0; smoothCount < setLedSmoothFrame; smoothCount++)
                    {
                        if (vCaptureColorHistoryMerge[smoothCount] != null)
                        {
                            colorMergeList.Add(vCaptureColorHistoryMerge[smoothCount][ledIndex]);
                        }
                        else
                        {
                            break;
                        }
                    }

                    //Merge colors
                    colorArray[ledIndex] = ColorMergeSqrt(colorMergeList);

                    //Update variables
                    colorMergeList.Clear();
                }

                //Update capture color history
                InsertObjectBegin(vCaptureColorHistoryMerge, colorArrayCopy);
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
                if (setLedSmoothObject <= 0) { return; }

                //Merge neighboring led colors together
                int colorMergeCount = 0;
                List<ColorRGBA> colorMergeList = [];
                int colorSmoothCount = setLedSmoothObject + 1;
                for (int ledIndex = 0; ledIndex < colorArray.Length; ledIndex++)
                {
                    //Accumulate colors
                    colorMergeList.Add(colorArray[ledIndex]);

                    //Update merge count
                    colorMergeCount++;

                    //Check merge count
                    if (colorMergeCount == colorSmoothCount)
                    {
                        //Merge colors
                        ColorRGBA colorMerge = ColorMergeSqrt(colorMergeList);
                        for (int mergeIndex = 0; mergeIndex < colorSmoothCount; mergeIndex++)
                        {
                            colorArray[ledIndex - mergeIndex] = ColorRGBA.Clone(colorMerge);
                        }

                        //Update variables
                        colorMergeList.Clear();
                        colorMergeCount = 0;
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