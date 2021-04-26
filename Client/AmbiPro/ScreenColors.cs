using AmbiPro.Resources;
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

                //Check led color direction
                int CaptureZoneHor = 0;
                int CaptureZoneVer = 0;
                int CaptureZoneSize = 0;
                int CaptureRangeSize = (setLedCaptureRange * vScreenHeight) / 100 / vCaptureStepSize;
                if (SideType == LedSideTypes.BottomLeftToRight)
                {
                    CaptureZoneHor = 0;
                    CaptureZoneVer = vScreenHeight - vMarginBottom;
                    CaptureZoneSize = (int)Math.Ceiling((double)vScreenWidth / (double)DirectionLedCount);
                }
                else if (SideType == LedSideTypes.BottomRightToLeft)
                {
                    CaptureZoneHor = vScreenWidth;
                    CaptureZoneVer = vScreenHeight - vMarginBottom;
                    CaptureZoneSize = (int)Math.Ceiling((double)vScreenWidth / (double)DirectionLedCount);
                }
                else if (SideType == LedSideTypes.TopLeftToRight)
                {
                    CaptureZoneHor = 0;
                    CaptureZoneVer = vMarginTop;
                    CaptureZoneSize = (int)Math.Ceiling((double)vScreenWidth / (double)DirectionLedCount);
                }
                else if (SideType == LedSideTypes.TopRightToLeft)
                {
                    CaptureZoneHor = vScreenWidth;
                    CaptureZoneVer = vMarginTop;
                    CaptureZoneSize = (int)Math.Ceiling((double)vScreenWidth / (double)DirectionLedCount);
                }
                else if (SideType == LedSideTypes.LeftTopToBottom)
                {
                    CaptureZoneHor = vMarginLeft;
                    CaptureZoneVer = 0;
                    CaptureZoneSize = (int)Math.Ceiling((double)vScreenHeight / (double)DirectionLedCount);
                }
                else if (SideType == LedSideTypes.LeftBottomToTop)
                {
                    CaptureZoneHor = vMarginLeft;
                    CaptureZoneVer = vScreenHeight;
                    CaptureZoneSize = (int)Math.Ceiling((double)vScreenHeight / (double)DirectionLedCount);
                }
                else if (SideType == LedSideTypes.RightTopToBottom)
                {
                    CaptureZoneHor = vScreenWidth - vMarginRight;
                    CaptureZoneVer = 0;
                    CaptureZoneSize = (int)Math.Ceiling((double)vScreenHeight / (double)DirectionLedCount);
                }
                else if (SideType == LedSideTypes.RightBottomToTop)
                {
                    CaptureZoneHor = vScreenWidth - vMarginRight;
                    CaptureZoneVer = vScreenHeight;
                    CaptureZoneSize = (int)Math.Ceiling((double)vScreenHeight / (double)DirectionLedCount);
                }

                //Get colors from the bitmap
                for (int dl = 0; dl < DirectionLedCount; dl++)
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

                    int CaptureStepMargin = 0;
                    int CapturedColors = 0;
                    int AverageRed = 0;
                    int AverageGreen = 0;
                    int AverageBlue = 0;

                    for (int lm = 0; lm < CaptureRangeSize; lm++)
                    {
                        if (SideType == LedSideTypes.BottomLeftToRight)
                        {
                            int ZoneVer = CaptureZoneVer - CaptureStepMargin;
                            for (int sk = 0; sk < CaptureZoneSize; sk += vCaptureStepSize)
                            {
                                int ZoneHor = CaptureZoneHor + sk;
                                CaptureColorAlgorithm(BitmapData, ref CapturedColors, ref AverageRed, ref AverageGreen, ref AverageBlue, ZoneHor, ZoneVer);
                            }
                        }
                        else if (SideType == LedSideTypes.BottomRightToLeft)
                        {
                            int ZoneVer = CaptureZoneVer - CaptureStepMargin;
                            for (int sk = 0; sk < CaptureZoneSize; sk += vCaptureStepSize)
                            {
                                int ZoneHor = CaptureZoneHor - sk;
                                CaptureColorAlgorithm(BitmapData, ref CapturedColors, ref AverageRed, ref AverageGreen, ref AverageBlue, ZoneHor, ZoneVer);
                            }
                        }
                        else if (SideType == LedSideTypes.TopLeftToRight)
                        {
                            int ZoneVer = CaptureZoneVer + CaptureStepMargin;
                            for (int sk = 0; sk < CaptureZoneSize; sk += vCaptureStepSize)
                            {
                                int ZoneHor = CaptureZoneHor + sk;
                                CaptureColorAlgorithm(BitmapData, ref CapturedColors, ref AverageRed, ref AverageGreen, ref AverageBlue, ZoneHor, ZoneVer);
                            }
                        }
                        else if (SideType == LedSideTypes.TopRightToLeft)
                        {
                            int ZoneVer = CaptureZoneVer + CaptureStepMargin;
                            for (int sk = 0; sk < CaptureZoneSize; sk += vCaptureStepSize)
                            {
                                int ZoneHor = CaptureZoneHor - sk;
                                CaptureColorAlgorithm(BitmapData, ref CapturedColors, ref AverageRed, ref AverageGreen, ref AverageBlue, ZoneHor, ZoneVer);
                            }
                        }
                        else if (SideType == LedSideTypes.LeftTopToBottom)
                        {
                            int ZoneHor = CaptureZoneHor + CaptureStepMargin;
                            for (int sk = 0; sk < CaptureZoneSize; sk += vCaptureStepSize)
                            {
                                int ZoneVer = CaptureZoneVer + sk;
                                CaptureColorAlgorithm(BitmapData, ref CapturedColors, ref AverageRed, ref AverageGreen, ref AverageBlue, ZoneHor, ZoneVer);
                            }
                        }
                        else if (SideType == LedSideTypes.LeftBottomToTop)
                        {
                            int ZoneHor = CaptureZoneHor + CaptureStepMargin;
                            for (int sk = 0; sk < CaptureZoneSize; sk += vCaptureStepSize)
                            {
                                int ZoneVer = CaptureZoneVer - sk;
                                CaptureColorAlgorithm(BitmapData, ref CapturedColors, ref AverageRed, ref AverageGreen, ref AverageBlue, ZoneHor, ZoneVer);
                            }
                        }
                        else if (SideType == LedSideTypes.RightTopToBottom)
                        {
                            int ZoneHor = CaptureZoneHor - CaptureStepMargin;
                            for (int sk = 0; sk < CaptureZoneSize; sk += vCaptureStepSize)
                            {
                                int ZoneVer = CaptureZoneVer + sk;
                                CaptureColorAlgorithm(BitmapData, ref CapturedColors, ref AverageRed, ref AverageGreen, ref AverageBlue, ZoneHor, ZoneVer);
                            }
                        }
                        else if (SideType == LedSideTypes.RightBottomToTop)
                        {
                            int ZoneHor = CaptureZoneHor - CaptureStepMargin;
                            for (int sk = 0; sk < CaptureZoneSize; sk += vCaptureStepSize)
                            {
                                int ZoneVer = CaptureZoneVer - sk;
                                CaptureColorAlgorithm(BitmapData, ref CapturedColors, ref AverageRed, ref AverageGreen, ref AverageBlue, ZoneHor, ZoneVer);
                            }
                        }
                        CaptureStepMargin += vCaptureStepSize;
                    }

                    //Skip to the next capture point
                    if (SideType == LedSideTypes.BottomLeftToRight) { CaptureZoneHor += CaptureZoneSize; }
                    else if (SideType == LedSideTypes.BottomRightToLeft) { CaptureZoneHor -= CaptureZoneSize; }
                    else if (SideType == LedSideTypes.TopLeftToRight) { CaptureZoneHor += CaptureZoneSize; }
                    else if (SideType == LedSideTypes.TopRightToLeft) { CaptureZoneHor -= CaptureZoneSize; }
                    else if (SideType == LedSideTypes.LeftTopToBottom) { CaptureZoneVer += CaptureZoneSize; }
                    else if (SideType == LedSideTypes.LeftBottomToTop) { CaptureZoneVer -= CaptureZoneSize; }
                    else if (SideType == LedSideTypes.RightTopToBottom) { CaptureZoneVer += CaptureZoneSize; }
                    else if (SideType == LedSideTypes.RightBottomToTop) { CaptureZoneVer -= CaptureZoneSize; }

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
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to get colors for leds: " + ex.Message);
            }
        }

        //Capture the color pixels
        private static unsafe void CaptureColorAlgorithm(byte* BitmapData, ref int CapturedColors, ref int AverageRed, ref int AverageGreen, ref int AverageBlue, int ZoneHor, int ZoneVer)
        {
            try
            {
                if (ZoneHor > vScreenWidth || ZoneHor < vCaptureStepSize) { return; }
                if (ZoneVer > vScreenHeight || ZoneVer < vCaptureStepSize) { return; }

                for (int captureZone = 0; captureZone < vCaptureStepSize; captureZone++)
                {
                    Color CurrentColor = ColorProcessing.GetPixelColor(BitmapData, vScreenWidth, vScreenHeight, (ZoneHor - captureZone), (ZoneVer - captureZone));
                    if (setDebugMode && setDebugColor)
                    {
                        ColorProcessing.SetPixelColor(BitmapData, vScreenWidth, vScreenHeight, (ZoneHor - captureZone), (ZoneVer - captureZone), Color.Red);
                    }

                    AverageRed += CurrentColor.R;
                    AverageGreen += CurrentColor.G;
                    AverageBlue += CurrentColor.B;
                    CapturedColors++;
                }
            }
            catch { }
        }
    }
}