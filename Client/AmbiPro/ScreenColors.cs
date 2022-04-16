using AmbiPro.Resources;
using ArnoldVinkCode;
using System;
using System.Diagnostics;
using static AmbiPro.AppClasses;
using static AmbiPro.AppEnums;
using static AmbiPro.AppVariables;

namespace AmbiPro
{
    public partial class SerialMonitor
    {
        //Get screen shot colors and set it to byte array
        private static unsafe void ScreenColors(LedSideTypes SideType, int DirectionLedCount, byte[] SerialBytes, byte* BitmapData, ref int CurrentSerialByte)
        {
            try
            {
                //Check the led side
                if (DirectionLedCount == 0 || SideType == LedSideTypes.None)
                {
                    return;
                }

                //Calculate led color direction and bottom gap
                int CaptureZoneHor = 0;
                int CaptureZoneVer = 0;
                int CaptureZoneSize = 0;
                int CaptureDiffMax = 0;
                int CaptureDiffMin = 0;
                int BottomGapMax = 0;
                int BottomGapMin = 0;
                if (SideType == LedSideTypes.TopLeftToRight)
                {
                    CaptureZoneHor = 0;
                    CaptureZoneVer = vCaptureHeight - vMarginTop;
                    CaptureZoneSize = vCaptureWidth / DirectionLedCount;
                    CaptureDiffMax = (vCaptureWidth - (CaptureZoneSize * DirectionLedCount)) / 2;
                    CaptureDiffMin = DirectionLedCount - CaptureDiffMax;
                }
                else if (SideType == LedSideTypes.TopRightToLeft)
                {
                    CaptureZoneHor = vCaptureWidth - 1;
                    CaptureZoneVer = vCaptureHeight - vMarginTop;
                    CaptureZoneSize = vCaptureWidth / DirectionLedCount;
                    CaptureDiffMax = (vCaptureWidth - (CaptureZoneSize * DirectionLedCount)) / 2;
                    CaptureDiffMin = DirectionLedCount - CaptureDiffMax;
                }
                else if (SideType == LedSideTypes.BottomLeftToRight)
                {
                    DirectionLedCount += setLedBottomGap;
                    CaptureZoneHor = 0;
                    CaptureZoneVer = vMarginBottom;
                    CaptureZoneSize = vCaptureWidth / DirectionLedCount;
                    CaptureDiffMax = (vCaptureWidth - (CaptureZoneSize * DirectionLedCount)) / 2;
                    CaptureDiffMin = DirectionLedCount - CaptureDiffMax;
                    int BottomGapDiff = setLedBottomGap / 2;
                    int BottomGapTarget = DirectionLedCount / 2;
                    BottomGapMax = BottomGapTarget + BottomGapDiff;
                    BottomGapMin = BottomGapTarget - BottomGapDiff - 1;
                }
                else if (SideType == LedSideTypes.BottomRightToLeft)
                {
                    DirectionLedCount += setLedBottomGap;
                    CaptureZoneHor = vCaptureWidth - 1;
                    CaptureZoneVer = vMarginBottom;
                    CaptureZoneSize = vCaptureWidth / DirectionLedCount;
                    CaptureDiffMax = (vCaptureWidth - (CaptureZoneSize * DirectionLedCount)) / 2;
                    CaptureDiffMin = DirectionLedCount - CaptureDiffMax;
                    int BottomGapDiff = setLedBottomGap / 2;
                    int BottomGapTarget = DirectionLedCount / 2;
                    BottomGapMax = BottomGapTarget + BottomGapDiff;
                    BottomGapMin = BottomGapTarget - BottomGapDiff - 1;
                }
                else if (SideType == LedSideTypes.LeftBottomToTop)
                {
                    CaptureZoneHor = vMarginLeft;
                    CaptureZoneVer = 1;
                    CaptureZoneSize = vCaptureHeight / DirectionLedCount;
                    CaptureDiffMax = (vCaptureHeight - (CaptureZoneSize * DirectionLedCount)) / 2;
                    CaptureDiffMin = DirectionLedCount - CaptureDiffMax;
                }
                else if (SideType == LedSideTypes.LeftTopToBottom)
                {
                    CaptureZoneHor = vMarginLeft;
                    CaptureZoneVer = vCaptureHeight;
                    CaptureZoneSize = vCaptureHeight / DirectionLedCount;
                    CaptureDiffMax = (vCaptureHeight - (CaptureZoneSize * DirectionLedCount)) / 2;
                    CaptureDiffMin = DirectionLedCount - CaptureDiffMax;
                }
                else if (SideType == LedSideTypes.RightBottomToTop)
                {
                    CaptureZoneHor = vCaptureWidth - vMarginRight;
                    CaptureZoneVer = 1;
                    CaptureZoneSize = vCaptureHeight / DirectionLedCount;
                    CaptureDiffMax = (vCaptureHeight - (CaptureZoneSize * DirectionLedCount)) / 2;
                    CaptureDiffMin = DirectionLedCount - CaptureDiffMax;
                }
                else if (SideType == LedSideTypes.RightTopToBottom)
                {
                    CaptureZoneHor = vCaptureWidth - vMarginRight;
                    CaptureZoneVer = vCaptureHeight;
                    CaptureZoneSize = vCaptureHeight / DirectionLedCount;
                    CaptureDiffMax = (vCaptureHeight - (CaptureZoneSize * DirectionLedCount)) / 2;
                    CaptureDiffMin = DirectionLedCount - CaptureDiffMax;
                }
                //Debug.WriteLine("Zone range: " + vCaptureZoneRange + " / Zone size: " + CaptureZoneSize + " / Zone difference: " + CaptureZoneDiff + " / Leds: " + DirectionLedCount);

                //Get colors from the bitmap
                for (int currentLed = 0; currentLed < DirectionLedCount; currentLed++)
                {
                    //Set led off color byte array
                    if (SideType == LedSideTypes.LedsOff)
                    {
                        SerialBytes[CurrentSerialByte] = 0;
                        CurrentSerialByte++;

                        SerialBytes[CurrentSerialByte] = 0;
                        CurrentSerialByte++;

                        SerialBytes[CurrentSerialByte] = 0;
                        CurrentSerialByte++;
                        continue;
                    }

                    //Skip bottom gap leds
                    if (BottomGapMax != 0 && AVFunctions.BetweenNumbers(currentLed, BottomGapMin, BottomGapMax, false))
                    {
                        //Debug.WriteLine("Led: " + currentLed + " / GapMin: " + BottomGapMin + " / GapMax: " + BottomGapMax + " / GapLeds: " + setLedBottomGap);
                    }
                    else
                    {
                        //Capture the average color from the bitmap screen capture
                        CaptureColorAlgorithm(BitmapData, out ColorRGBA CaptureColor, CaptureZoneHor, CaptureZoneVer, CaptureZoneSize, SideType);
                        //Debug.WriteLine("Captured average color R" + CaptureColor.R + "G" + CaptureColor.G + "B" + CaptureColor.B + " from the bitmap data.");

                        //Check if average color is black
                        if (CaptureColor != null && (CaptureColor.R > 0 || CaptureColor.G > 0 || CaptureColor.B > 0))
                        {
                            //Adjust the colors to settings
                            AdjustLedColors(ref CaptureColor);

                            //Set the color to color byte array
                            SerialBytes[CurrentSerialByte] = CaptureColor.R;
                            CurrentSerialByte++;

                            SerialBytes[CurrentSerialByte] = CaptureColor.G;
                            CurrentSerialByte++;

                            SerialBytes[CurrentSerialByte] = CaptureColor.B;
                            CurrentSerialByte++;
                        }
                        else
                        {
                            //Set the color to color byte array
                            SerialBytes[CurrentSerialByte] = 0;
                            CurrentSerialByte++;

                            SerialBytes[CurrentSerialByte] = 0;
                            CurrentSerialByte++;

                            SerialBytes[CurrentSerialByte] = 0;
                            CurrentSerialByte++;
                        }
                    }

                    //Zone difference correction
                    if (CaptureDiffMax != 0 && AVFunctions.BetweenNumbersOr(currentLed, CaptureDiffMin, CaptureDiffMax, true))
                    {
                        if (SideType == LedSideTypes.TopLeftToRight) { CaptureZoneHor++; }
                        else if (SideType == LedSideTypes.TopRightToLeft) { CaptureZoneHor--; }
                        else if (SideType == LedSideTypes.BottomLeftToRight) { CaptureZoneHor++; }
                        else if (SideType == LedSideTypes.BottomRightToLeft) { CaptureZoneHor--; }
                        else if (SideType == LedSideTypes.LeftBottomToTop) { CaptureZoneVer++; }
                        else if (SideType == LedSideTypes.LeftTopToBottom) { CaptureZoneVer--; }
                        else if (SideType == LedSideTypes.RightBottomToTop) { CaptureZoneVer++; }
                        else if (SideType == LedSideTypes.RightTopToBottom) { CaptureZoneVer--; }
                    }

                    //Skip to the next capture zone
                    if (SideType == LedSideTypes.TopLeftToRight) { CaptureZoneHor += CaptureZoneSize; }
                    else if (SideType == LedSideTypes.TopRightToLeft) { CaptureZoneHor -= CaptureZoneSize; }
                    else if (SideType == LedSideTypes.BottomLeftToRight) { CaptureZoneHor += CaptureZoneSize; }
                    else if (SideType == LedSideTypes.BottomRightToLeft) { CaptureZoneHor -= CaptureZoneSize; }
                    else if (SideType == LedSideTypes.LeftBottomToTop) { CaptureZoneVer += CaptureZoneSize; }
                    else if (SideType == LedSideTypes.LeftTopToBottom) { CaptureZoneVer -= CaptureZoneSize; }
                    else if (SideType == LedSideTypes.RightBottomToTop) { CaptureZoneVer += CaptureZoneSize; }
                    else if (SideType == LedSideTypes.RightTopToBottom) { CaptureZoneVer -= CaptureZoneSize; }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to get colors for leds: " + ex.Message);
            }
        }

        //Capture the color pixels
        private static unsafe void CaptureColorAlgorithm(byte* BitmapData, out ColorRGBA AverageColor, int CaptureZoneHor, int CaptureZoneVer, int CaptureZoneSize, LedSideTypes SideType)
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
                for (int captureStep = 0; captureStep < CaptureZoneSize; captureStep++)
                {
                    if (CaptureEvenStep == 1) { CaptureEvenStep = 0; } else { CaptureEvenStep = 1; }
                    for (int captureRange = 0; captureRange < vCaptureRange; captureRange += 2)
                    {
                        if (SideType == LedSideTypes.TopLeftToRight)
                        {
                            CaptureZoneHorRange = captureStep;
                            CaptureZoneVerRange = -captureRange - CaptureEvenStep;
                        }
                        else if (SideType == LedSideTypes.TopRightToLeft)
                        {
                            CaptureZoneHorRange = -captureStep;
                            CaptureZoneVerRange = -captureRange - CaptureEvenStep;
                        }
                        else if (SideType == LedSideTypes.BottomLeftToRight)
                        {
                            CaptureZoneHorRange = captureStep;
                            CaptureZoneVerRange = captureRange + CaptureEvenStep;
                        }
                        else if (SideType == LedSideTypes.BottomRightToLeft)
                        {
                            CaptureZoneHorRange = -captureStep;
                            CaptureZoneVerRange = captureRange + CaptureEvenStep;
                        }
                        else if (SideType == LedSideTypes.LeftBottomToTop)
                        {
                            CaptureZoneHorRange = captureRange + CaptureEvenStep;
                            CaptureZoneVerRange = captureStep;
                        }
                        else if (SideType == LedSideTypes.LeftTopToBottom)
                        {
                            CaptureZoneHorRange = captureRange + CaptureEvenStep;
                            CaptureZoneVerRange = -captureStep;
                        }
                        else if (SideType == LedSideTypes.RightBottomToTop)
                        {
                            CaptureZoneHorRange = -captureRange - CaptureEvenStep;
                            CaptureZoneVerRange = captureStep;
                        }
                        else if (SideType == LedSideTypes.RightTopToBottom)
                        {
                            CaptureZoneHorRange = -captureRange - CaptureEvenStep;
                            CaptureZoneVerRange = -captureStep;
                        }

                        ColorRGBA ColorPixel = ColorProcessing.GetPixelColor(BitmapData, vCaptureWidth, vCaptureHeight, CaptureZoneHor + CaptureZoneHorRange, CaptureZoneVer + CaptureZoneVerRange);
                        if (ColorPixel != null)
                        {
                            if (vDebugCaptureAllowed && setDebugColor)
                            {
                                ColorProcessing.SetPixelColor(BitmapData, vCaptureWidth, vCaptureHeight, CaptureZoneHor + CaptureZoneHorRange, CaptureZoneVer + CaptureZoneVerRange, ColorRGBA.Purple);
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
                AverageColor = new ColorRGBA()
                {
                    R = (byte)(ColorAverageR / ColorCount),
                    G = (byte)(ColorAverageG / ColorCount),
                    B = (byte)(ColorAverageB / ColorCount)
                };
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to capture pixel colors: " + ex.Message);
                AverageColor = null;
            }
        }
    }
}