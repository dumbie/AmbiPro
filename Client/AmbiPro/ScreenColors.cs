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
                int CaptureZoneSizeOrg = 0;
                int CaptureZoneSizeAdj = 0;
                int CaptureZoneDiff = 0;
                if (SideType == LedSideTypes.BottomLeftToRight)
                {
                    CaptureZoneHor = 0;
                    CaptureZoneVer = vScreenHeight - vMarginBottom;
                    CaptureZoneSizeOrg = vScreenWidth / DirectionLedCount;
                    CaptureZoneDiff = vScreenWidth - (CaptureZoneSizeOrg * DirectionLedCount);
                }
                else if (SideType == LedSideTypes.BottomRightToLeft)
                {
                    CaptureZoneHor = vScreenWidth;
                    CaptureZoneVer = vScreenHeight - vMarginBottom;
                    CaptureZoneSizeOrg = vScreenWidth / DirectionLedCount;
                    CaptureZoneDiff = vScreenWidth - (CaptureZoneSizeOrg * DirectionLedCount);
                }
                else if (SideType == LedSideTypes.TopLeftToRight)
                {
                    CaptureZoneHor = 0;
                    CaptureZoneVer = vMarginTop;
                    CaptureZoneSizeOrg = vScreenWidth / DirectionLedCount;
                    CaptureZoneDiff = vScreenWidth - (CaptureZoneSizeOrg * DirectionLedCount);
                }
                else if (SideType == LedSideTypes.TopRightToLeft)
                {
                    CaptureZoneHor = vScreenWidth;
                    CaptureZoneVer = vMarginTop;
                    CaptureZoneSizeOrg = vScreenWidth / DirectionLedCount;
                    CaptureZoneDiff = vScreenWidth - (CaptureZoneSizeOrg * DirectionLedCount);
                }
                else if (SideType == LedSideTypes.LeftTopToBottom)
                {
                    CaptureZoneHor = vMarginLeft;
                    CaptureZoneVer = 0;
                    CaptureZoneSizeOrg = vScreenHeight / DirectionLedCount;
                    CaptureZoneDiff = vScreenHeight - (CaptureZoneSizeOrg * DirectionLedCount);
                }
                else if (SideType == LedSideTypes.LeftBottomToTop)
                {
                    CaptureZoneHor = vMarginLeft;
                    CaptureZoneVer = vScreenHeight;
                    CaptureZoneSizeOrg = vScreenHeight / DirectionLedCount;
                    CaptureZoneDiff = vScreenHeight - (CaptureZoneSizeOrg * DirectionLedCount);
                }
                else if (SideType == LedSideTypes.RightTopToBottom)
                {
                    CaptureZoneHor = vScreenWidth - vMarginRight;
                    CaptureZoneVer = 0;
                    CaptureZoneSizeOrg = vScreenHeight / DirectionLedCount;
                    CaptureZoneDiff = vScreenHeight - (CaptureZoneSizeOrg * DirectionLedCount);
                }
                else if (SideType == LedSideTypes.RightBottomToTop)
                {
                    CaptureZoneHor = vScreenWidth - vMarginRight;
                    CaptureZoneVer = vScreenHeight;
                    CaptureZoneSizeOrg = vScreenHeight / DirectionLedCount;
                    CaptureZoneDiff = vScreenHeight - (CaptureZoneSizeOrg * DirectionLedCount);
                }
                //Debug.WriteLine("Zone range: " + vCaptureZoneRange + " / Zone size: " + CaptureZoneSizeOrg + " / Zone difference: " + CaptureZoneDiff);

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

                    //Zone difference correction
                    if (CaptureZoneDiff > 0)
                    {
                        CaptureZoneSizeAdj = CaptureZoneSizeOrg + 1;
                        CaptureZoneDiff--;
                    }
                    else
                    {
                        CaptureZoneSizeAdj = CaptureZoneSizeOrg;
                    }

                    //Capture the colors from point
                    int CapturedColors = 0;
                    int AverageRed = 0;
                    int AverageGreen = 0;
                    int AverageBlue = 0;
                    CaptureColorAlgorithm(BitmapData, ref CapturedColors, ref AverageRed, ref AverageGreen, ref AverageBlue, CaptureZoneHor, CaptureZoneVer, CaptureZoneSizeAdj, SideType);

                    //Skip to the next capture point
                    if (SideType == LedSideTypes.BottomLeftToRight) { CaptureZoneHor += CaptureZoneSizeAdj; }
                    else if (SideType == LedSideTypes.BottomRightToLeft) { CaptureZoneHor -= CaptureZoneSizeAdj; }
                    else if (SideType == LedSideTypes.TopLeftToRight) { CaptureZoneHor += CaptureZoneSizeAdj; }
                    else if (SideType == LedSideTypes.TopRightToLeft) { CaptureZoneHor -= CaptureZoneSizeAdj; }
                    else if (SideType == LedSideTypes.LeftTopToBottom) { CaptureZoneVer += CaptureZoneSizeAdj; }
                    else if (SideType == LedSideTypes.LeftBottomToTop) { CaptureZoneVer -= CaptureZoneSizeAdj; }
                    else if (SideType == LedSideTypes.RightTopToBottom) { CaptureZoneVer += CaptureZoneSizeAdj; }
                    else if (SideType == LedSideTypes.RightBottomToTop) { CaptureZoneVer -= CaptureZoneSizeAdj; }

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
                        if (SideType == LedSideTypes.BottomLeftToRight)
                        {
                            CaptureZoneHorRange = captureStep;
                            CaptureZoneVerRange = -captureRange - CaptureEvenStep;
                        }
                        else if (SideType == LedSideTypes.BottomRightToLeft)
                        {
                            CaptureZoneHorRange = -captureStep;
                            CaptureZoneVerRange = -captureRange - CaptureEvenStep;
                        }
                        else if (SideType == LedSideTypes.TopLeftToRight)
                        {
                            CaptureZoneHorRange = captureStep;
                            CaptureZoneVerRange = captureRange + CaptureEvenStep;
                        }
                        else if (SideType == LedSideTypes.TopRightToLeft)
                        {
                            CaptureZoneHorRange = -captureStep;
                            CaptureZoneVerRange = captureRange + CaptureEvenStep;
                        }
                        else if (SideType == LedSideTypes.LeftTopToBottom)
                        {
                            CaptureZoneHorRange = captureRange + CaptureEvenStep;
                            CaptureZoneVerRange = captureStep;
                        }
                        else if (SideType == LedSideTypes.LeftBottomToTop)
                        {
                            CaptureZoneHorRange = captureRange + CaptureEvenStep;
                            CaptureZoneVerRange = -captureStep;
                        }
                        else if (SideType == LedSideTypes.RightTopToBottom)
                        {
                            CaptureZoneHorRange = -captureRange - CaptureEvenStep;
                            CaptureZoneVerRange = captureStep;
                        }
                        else if (SideType == LedSideTypes.RightBottomToTop)
                        {
                            CaptureZoneHorRange = -captureRange - CaptureEvenStep;
                            CaptureZoneVerRange = -captureStep;
                        }

                        Color currentColor = ColorProcessing.GetPixelColor(BitmapData, vScreenWidth, vScreenHeight, CaptureZoneHor + CaptureZoneHorRange, CaptureZoneVer + CaptureZoneVerRange);
                        if (currentColor != Color.Empty)
                        {
                            if (setDebugMode && setDebugColor)
                            {
                                ColorProcessing.SetPixelColor(BitmapData, vScreenWidth, vScreenHeight, CaptureZoneHor + CaptureZoneHorRange, CaptureZoneVer + CaptureZoneVerRange, Color.Red);
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