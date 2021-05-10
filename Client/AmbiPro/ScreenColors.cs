﻿using AmbiPro.Resources;
using ArnoldVinkCode;
using System;
using System.Diagnostics;
using System.Drawing;
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
                int CaptureZoneDiff = 0;
                int BottomGapMin = 0;
                int BottomGapMax = 0;
                if (SideType == LedSideTypes.TopLeftToRight)
                {
                    CaptureZoneHor = 0;
                    CaptureZoneVer = vScreenHeight - vMarginTop;
                    CaptureZoneSize = vScreenWidth / DirectionLedCount;
                    CaptureZoneDiff = vScreenWidth - (CaptureZoneSize * DirectionLedCount);
                }
                else if (SideType == LedSideTypes.TopRightToLeft)
                {
                    CaptureZoneHor = vScreenWidth;
                    CaptureZoneVer = vScreenHeight - vMarginTop;
                    CaptureZoneSize = vScreenWidth / DirectionLedCount;
                    CaptureZoneDiff = vScreenWidth - (CaptureZoneSize * DirectionLedCount);
                }
                else if (SideType == LedSideTypes.BottomLeftToRight)
                {
                    DirectionLedCount += setLedBottomGap;
                    CaptureZoneHor = 0;
                    CaptureZoneVer = vMarginBottom;
                    CaptureZoneSize = vScreenWidth / DirectionLedCount;
                    CaptureZoneDiff = vScreenWidth - (CaptureZoneSize * DirectionLedCount);
                    int BottomGapDiff = setLedBottomGap / 2;
                    int BottomGapTarget = DirectionLedCount / 2;
                    BottomGapMin = BottomGapTarget - BottomGapDiff - 1;
                    BottomGapMax = BottomGapTarget + BottomGapDiff;
                }
                else if (SideType == LedSideTypes.BottomRightToLeft)
                {
                    DirectionLedCount += setLedBottomGap;
                    CaptureZoneHor = vScreenWidth;
                    CaptureZoneVer = vMarginBottom;
                    CaptureZoneSize = vScreenWidth / DirectionLedCount;
                    CaptureZoneDiff = vScreenWidth - (CaptureZoneSize * DirectionLedCount);
                    int BottomGapDiff = setLedBottomGap / 2;
                    int BottomGapTarget = DirectionLedCount / 2;
                    BottomGapMin = BottomGapTarget - BottomGapDiff - 1;
                    BottomGapMax = BottomGapTarget + BottomGapDiff;
                }
                else if (SideType == LedSideTypes.LeftBottomToTop)
                {
                    CaptureZoneHor = vMarginLeft;
                    CaptureZoneVer = 0;
                    CaptureZoneSize = vScreenHeight / DirectionLedCount;
                    CaptureZoneDiff = vScreenHeight - (CaptureZoneSize * DirectionLedCount);
                }
                else if (SideType == LedSideTypes.LeftTopToBottom)
                {
                    CaptureZoneHor = vMarginLeft;
                    CaptureZoneVer = vScreenHeight;
                    CaptureZoneSize = vScreenHeight / DirectionLedCount;
                    CaptureZoneDiff = vScreenHeight - (CaptureZoneSize * DirectionLedCount);
                }
                else if (SideType == LedSideTypes.RightBottomToTop)
                {
                    CaptureZoneHor = vScreenWidth - vMarginRight;
                    CaptureZoneVer = 0;
                    CaptureZoneSize = vScreenHeight / DirectionLedCount;
                    CaptureZoneDiff = vScreenHeight - (CaptureZoneSize * DirectionLedCount);
                }
                else if (SideType == LedSideTypes.RightTopToBottom)
                {
                    CaptureZoneHor = vScreenWidth - vMarginRight;
                    CaptureZoneVer = vScreenHeight;
                    CaptureZoneSize = vScreenHeight / DirectionLedCount;
                    CaptureZoneDiff = vScreenHeight - (CaptureZoneSize * DirectionLedCount);
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
                    bool skipCapture = false;
                    if (BottomGapMin != 0 && AVFunctions.BetweenNumbers(currentLed, BottomGapMin, BottomGapMax, false))
                    {
                        //Debug.WriteLine("Led: " + currentLed + " / GapMin: " + BottomGapMin + " / GapMax: " + BottomGapMax + " / GapLeds: " + setLedBottomGap);
                        skipCapture = true;
                    }

                    if (!skipCapture)
                    {
                        //Capture the colors
                        int CapturedColors = 0;
                        int AverageRed = 0;
                        int AverageGreen = 0;
                        int AverageBlue = 0;
                        CaptureColorAlgorithm(BitmapData, ref CapturedColors, ref AverageRed, ref AverageGreen, ref AverageBlue, CaptureZoneHor, CaptureZoneVer, CaptureZoneSize, SideType);

                        //Calculate average color from the bitmap screen capture
                        //Debug.WriteLine("Captured " + UsedColors + " pixel colors from the bitmap data.");
                        if (CapturedColors > 0)
                        {
                            Color CurrentColor = Color.FromArgb(0, AverageRed / CapturedColors, AverageGreen / CapturedColors, AverageBlue / CapturedColors);
                            CurrentColor = AdjustLedColors(CurrentColor);

                            //Set the color to color byte array
                            SerialBytes[CurrentSerialByte] = CurrentColor.R;
                            CurrentSerialByte++;

                            SerialBytes[CurrentSerialByte] = CurrentColor.G;
                            CurrentSerialByte++;

                            SerialBytes[CurrentSerialByte] = CurrentColor.B;
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
                    if (CaptureZoneDiff > 0)
                    {
                        if (SideType == LedSideTypes.TopLeftToRight) { CaptureZoneHor++; }
                        else if (SideType == LedSideTypes.TopRightToLeft) { CaptureZoneHor--; }
                        else if (SideType == LedSideTypes.BottomLeftToRight) { CaptureZoneHor++; }
                        else if (SideType == LedSideTypes.BottomRightToLeft) { CaptureZoneHor--; }
                        else if (SideType == LedSideTypes.LeftBottomToTop) { CaptureZoneVer++; }
                        else if (SideType == LedSideTypes.LeftTopToBottom) { CaptureZoneVer--; }
                        else if (SideType == LedSideTypes.RightBottomToTop) { CaptureZoneVer++; }
                        else if (SideType == LedSideTypes.RightTopToBottom) { CaptureZoneVer--; }
                        CaptureZoneDiff--;
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
        private static unsafe void CaptureColorAlgorithm(byte* BitmapData, ref int CapturedColors, ref int AverageRed, ref int AverageGreen, ref int AverageBlue, int CaptureZoneHor, int CaptureZoneVer, int CaptureZoneSize, LedSideTypes SideType)
        {
            try
            {
                int CaptureEvenStep = 1;
                int CaptureZoneHorRange = 0;
                int CaptureZoneVerRange = 0;
                for (int captureStep = 0; captureStep < CaptureZoneSize; captureStep++)
                {
                    if (CaptureEvenStep == 1) { CaptureEvenStep = 0; } else { CaptureEvenStep = 1; }
                    for (int captureRange = 0; captureRange < vCaptureZoneRange; captureRange += 2)
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

                        Color currentColor = ColorProcessing.GetPixelColor(BitmapData, vScreenWidth, vScreenHeight, CaptureZoneHor + CaptureZoneHorRange, CaptureZoneVer + CaptureZoneVerRange);
                        if (currentColor != Color.Empty)
                        {
                            if (setDebugMode && setDebugColor)
                            {
                                ColorProcessing.SetPixelColor(BitmapData, vScreenWidth, vScreenHeight, CaptureZoneHor + CaptureZoneHorRange, CaptureZoneVer + CaptureZoneVerRange, Color.Purple);
                            }

                            AverageRed += currentColor.R;
                            AverageGreen += currentColor.G;
                            AverageBlue += currentColor.B;
                            CapturedColors++;
                        }
                    }
                }
            }
            catch { }
        }
    }
}