using AmbiPro.Resources;
using ArnoldVinkCode;
using System;
using System.Diagnostics;
using static AmbiPro.AppClasses;
using static AmbiPro.AppEnums;
using static AmbiPro.AppVariables;
using static AmbiPro.PreloadSettings;

namespace AmbiPro
{
    public partial class SerialMonitor
    {
        //Get screen shot colors and set it to byte array
        private static void ScreenColors(LedSideTypes sideType, int directionLedCount, byte[] serialBytes, byte[] bitmapByteArray, ref int currentSerialByte)
        {
            try
            {
                //Check the led side
                if (directionLedCount == 0 || sideType == LedSideTypes.None)
                {
                    return;
                }

                //Calculate led color direction and bottom gap
                int captureZoneHor = 0;
                int captureZoneVer = 0;
                int captureZoneSize = 0;
                int captureDiffMax = 0;
                int captureDiffMin = 0;
                int bottomGapMax = 0;
                int bottomGapMin = 0;
                if (sideType == LedSideTypes.TopLeftToRight)
                {
                    captureZoneHor = 0;
                    captureZoneVer = vCaptureDetails.Height - vMarginTop;
                    captureZoneSize = vCaptureDetails.Width / directionLedCount;
                    captureDiffMax = (vCaptureDetails.Width - (captureZoneSize * directionLedCount)) / 2;
                    captureDiffMin = directionLedCount - captureDiffMax;
                }
                else if (sideType == LedSideTypes.TopRightToLeft)
                {
                    captureZoneHor = vCaptureDetails.Width - 1;
                    captureZoneVer = vCaptureDetails.Height - vMarginTop;
                    captureZoneSize = vCaptureDetails.Width / directionLedCount;
                    captureDiffMax = (vCaptureDetails.Width - (captureZoneSize * directionLedCount)) / 2;
                    captureDiffMin = directionLedCount - captureDiffMax;
                }
                else if (sideType == LedSideTypes.BottomLeftToRight)
                {
                    directionLedCount += setLedBottomGap;
                    captureZoneHor = 0;
                    captureZoneVer = vMarginBottom;
                    captureZoneSize = vCaptureDetails.Width / directionLedCount;
                    captureDiffMax = (vCaptureDetails.Width - (captureZoneSize * directionLedCount)) / 2;
                    captureDiffMin = directionLedCount - captureDiffMax;
                    int BottomGapDiff = setLedBottomGap / 2;
                    int BottomGapTarget = directionLedCount / 2;
                    bottomGapMax = BottomGapTarget + BottomGapDiff;
                    bottomGapMin = BottomGapTarget - BottomGapDiff - 1;
                }
                else if (sideType == LedSideTypes.BottomRightToLeft)
                {
                    directionLedCount += setLedBottomGap;
                    captureZoneHor = vCaptureDetails.Width - 1;
                    captureZoneVer = vMarginBottom;
                    captureZoneSize = vCaptureDetails.Width / directionLedCount;
                    captureDiffMax = (vCaptureDetails.Width - (captureZoneSize * directionLedCount)) / 2;
                    captureDiffMin = directionLedCount - captureDiffMax;
                    int bottomGapDiff = setLedBottomGap / 2;
                    int bottomGapTarget = directionLedCount / 2;
                    bottomGapMax = bottomGapTarget + bottomGapDiff;
                    bottomGapMin = bottomGapTarget - bottomGapDiff - 1;
                }
                else if (sideType == LedSideTypes.LeftBottomToTop)
                {
                    captureZoneHor = vMarginLeft;
                    captureZoneVer = 1;
                    captureZoneSize = vCaptureDetails.Height / directionLedCount;
                    captureDiffMax = (vCaptureDetails.Height - (captureZoneSize * directionLedCount)) / 2;
                    captureDiffMin = directionLedCount - captureDiffMax;
                }
                else if (sideType == LedSideTypes.LeftTopToBottom)
                {
                    captureZoneHor = vMarginLeft;
                    captureZoneVer = vCaptureDetails.Height;
                    captureZoneSize = vCaptureDetails.Height / directionLedCount;
                    captureDiffMax = (vCaptureDetails.Height - (captureZoneSize * directionLedCount)) / 2;
                    captureDiffMin = directionLedCount - captureDiffMax;
                }
                else if (sideType == LedSideTypes.RightBottomToTop)
                {
                    captureZoneHor = vCaptureDetails.Width - vMarginRight;
                    captureZoneVer = 1;
                    captureZoneSize = vCaptureDetails.Height / directionLedCount;
                    captureDiffMax = (vCaptureDetails.Height - (captureZoneSize * directionLedCount)) / 2;
                    captureDiffMin = directionLedCount - captureDiffMax;
                }
                else if (sideType == LedSideTypes.RightTopToBottom)
                {
                    captureZoneHor = vCaptureDetails.Width - vMarginRight;
                    captureZoneVer = vCaptureDetails.Height;
                    captureZoneSize = vCaptureDetails.Height / directionLedCount;
                    captureDiffMax = (vCaptureDetails.Height - (captureZoneSize * directionLedCount)) / 2;
                    captureDiffMin = directionLedCount - captureDiffMax;
                }
                //Debug.WriteLine("Zone range: " + vCaptureZoneRange + " / Zone size: " + CaptureZoneSize + " / Zone difference: " + CaptureZoneDiff + " / Leds: " + DirectionLedCount);

                //Get colors from the bitmap
                for (int currentLed = 0; currentLed < directionLedCount; currentLed++)
                {
                    //Set led off color byte array
                    if (sideType == LedSideTypes.LedsOff)
                    {
                        serialBytes[currentSerialByte] = 0;
                        currentSerialByte++;

                        serialBytes[currentSerialByte] = 0;
                        currentSerialByte++;

                        serialBytes[currentSerialByte] = 0;
                        currentSerialByte++;
                        continue;
                    }

                    //Skip bottom gap leds
                    if (bottomGapMax != 0 && AVFunctions.BetweenNumbers(currentLed, bottomGapMin, bottomGapMax, false))
                    {
                        //Debug.WriteLine("Led: " + currentLed + " / GapMin: " + BottomGapMin + " / GapMax: " + BottomGapMax + " / GapLeds: " + setLedBottomGap);
                    }
                    else
                    {
                        //Capture the average color from the bitmap screen capture
                        CaptureColorAlgorithm(bitmapByteArray, out ColorRGBA captureColor, captureZoneHor, captureZoneVer, captureZoneSize, sideType);
                        //Debug.WriteLine("Captured average color R" + CaptureColor.R + "G" + CaptureColor.G + "B" + CaptureColor.B + " from the bitmap data.");

                        //Update debug led colors preview
                        if (vDebugCaptureAllowed)
                        {
                            UpdateLedColorsPreview(sideType, currentLed, captureColor);
                        }

                        //Check if average color is black
                        if (captureColor != null && (captureColor.R > 0 || captureColor.G > 0 || captureColor.B > 0))
                        {
                            //Adjust the colors to settings
                            AdjustLedColors(ref captureColor);

                            //Set the color to color byte array
                            serialBytes[currentSerialByte] = captureColor.R;
                            currentSerialByte++;

                            serialBytes[currentSerialByte] = captureColor.G;
                            currentSerialByte++;

                            serialBytes[currentSerialByte] = captureColor.B;
                            currentSerialByte++;
                        }
                        else
                        {
                            //Set the color to color byte array
                            serialBytes[currentSerialByte] = 0;
                            currentSerialByte++;

                            serialBytes[currentSerialByte] = 0;
                            currentSerialByte++;

                            serialBytes[currentSerialByte] = 0;
                            currentSerialByte++;
                        }
                    }

                    //Zone difference correction
                    if (captureDiffMax != 0 && AVFunctions.BetweenNumbersOr(currentLed, captureDiffMin, captureDiffMax, true))
                    {
                        if (sideType == LedSideTypes.TopLeftToRight) { captureZoneHor++; }
                        else if (sideType == LedSideTypes.TopRightToLeft) { captureZoneHor--; }
                        else if (sideType == LedSideTypes.BottomLeftToRight) { captureZoneHor++; }
                        else if (sideType == LedSideTypes.BottomRightToLeft) { captureZoneHor--; }
                        else if (sideType == LedSideTypes.LeftBottomToTop) { captureZoneVer++; }
                        else if (sideType == LedSideTypes.LeftTopToBottom) { captureZoneVer--; }
                        else if (sideType == LedSideTypes.RightBottomToTop) { captureZoneVer++; }
                        else if (sideType == LedSideTypes.RightTopToBottom) { captureZoneVer--; }
                    }

                    //Skip to the next capture zone
                    if (sideType == LedSideTypes.TopLeftToRight) { captureZoneHor += captureZoneSize; }
                    else if (sideType == LedSideTypes.TopRightToLeft) { captureZoneHor -= captureZoneSize; }
                    else if (sideType == LedSideTypes.BottomLeftToRight) { captureZoneHor += captureZoneSize; }
                    else if (sideType == LedSideTypes.BottomRightToLeft) { captureZoneHor -= captureZoneSize; }
                    else if (sideType == LedSideTypes.LeftBottomToTop) { captureZoneVer += captureZoneSize; }
                    else if (sideType == LedSideTypes.LeftTopToBottom) { captureZoneVer -= captureZoneSize; }
                    else if (sideType == LedSideTypes.RightBottomToTop) { captureZoneVer += captureZoneSize; }
                    else if (sideType == LedSideTypes.RightTopToBottom) { captureZoneVer -= captureZoneSize; }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to get colors for leds: " + ex.Message);
            }
        }

        //Capture the color pixels
        private static void CaptureColorAlgorithm(byte[] bitmapByteArray, out ColorRGBA averageColor, int captureZoneHor, int captureZoneVer, int captureZoneSize, LedSideTypes sideType)
        {
            try
            {
                int ColorCount = 0;
                int ColorAverageR = 0;
                int ColorAverageG = 0;
                int ColorAverageB = 0;
                int CaptureEvenStep = 1;
                int CaptureZoneHorRange = 0;
                int CaptureZoneVerRange = 0;
                for (int captureStep = 0; captureStep < captureZoneSize; captureStep++)
                {
                    if (CaptureEvenStep == 1) { CaptureEvenStep = 0; } else { CaptureEvenStep = 1; }
                    for (int captureRange = 0; captureRange < vCaptureRange; captureRange += 2)
                    {
                        if (sideType == LedSideTypes.TopLeftToRight)
                        {
                            CaptureZoneHorRange = captureStep;
                            CaptureZoneVerRange = -captureRange - CaptureEvenStep;
                        }
                        else if (sideType == LedSideTypes.TopRightToLeft)
                        {
                            CaptureZoneHorRange = -captureStep;
                            CaptureZoneVerRange = -captureRange - CaptureEvenStep;
                        }
                        else if (sideType == LedSideTypes.BottomLeftToRight)
                        {
                            CaptureZoneHorRange = captureStep;
                            CaptureZoneVerRange = captureRange + CaptureEvenStep;
                        }
                        else if (sideType == LedSideTypes.BottomRightToLeft)
                        {
                            CaptureZoneHorRange = -captureStep;
                            CaptureZoneVerRange = captureRange + CaptureEvenStep;
                        }
                        else if (sideType == LedSideTypes.LeftBottomToTop)
                        {
                            CaptureZoneHorRange = captureRange + CaptureEvenStep;
                            CaptureZoneVerRange = captureStep;
                        }
                        else if (sideType == LedSideTypes.LeftTopToBottom)
                        {
                            CaptureZoneHorRange = captureRange + CaptureEvenStep;
                            CaptureZoneVerRange = -captureStep;
                        }
                        else if (sideType == LedSideTypes.RightBottomToTop)
                        {
                            CaptureZoneHorRange = -captureRange - CaptureEvenStep;
                            CaptureZoneVerRange = captureStep;
                        }
                        else if (sideType == LedSideTypes.RightTopToBottom)
                        {
                            CaptureZoneHorRange = -captureRange - CaptureEvenStep;
                            CaptureZoneVerRange = -captureStep;
                        }

                        ColorRGBA ColorPixel = ColorProcessing.GetPixelColor(bitmapByteArray, vCaptureDetails.Width, vCaptureDetails.Height, captureZoneHor + CaptureZoneHorRange, captureZoneVer + CaptureZoneVerRange);
                        if (ColorPixel != null)
                        {
                            if (vDebugCaptureAllowed && setDebugColor)
                            {
                                ColorProcessing.SetPixelColor(bitmapByteArray, vCaptureDetails.Width, vCaptureDetails.Height, captureZoneHor + CaptureZoneHorRange, captureZoneVer + CaptureZoneVerRange, ColorRGBA.Purple);
                            }

                            //Add pixel color to average color
                            ColorAverageR += ColorPixel.R;
                            ColorAverageG += ColorPixel.G;
                            ColorAverageB += ColorPixel.B;
                            ColorCount++;
                        }
                    }
                }

                //Calculate the average color
                averageColor = new ColorRGBA()
                {
                    R = (byte)(ColorAverageR / ColorCount),
                    G = (byte)(ColorAverageG / ColorCount),
                    B = (byte)(ColorAverageB / ColorCount)
                };
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to capture pixel colors: " + ex.Message);
                averageColor = null;
            }
        }
    }
}