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
                Color CurrentColor = new Color();
                int DirectionSkipStep = 0;
                int DirectionStep = 0;

                //Check the led side
                if (DirectionLedCount == 0 || SideType == LedSideTypes.None)
                {
                    return;
                }

                //Check led color direction
                if (SideType == LedSideTypes.BottomLeftToRight)
                {
                    DirectionSkipStep = vMarginOffset;
                    DirectionStep = vScreenWidth / DirectionLedCount;
                }
                else if (SideType == LedSideTypes.BottomRightToLeft)
                {
                    DirectionSkipStep = vScreenWidth - vMarginOffset;
                    DirectionStep = vScreenWidth / DirectionLedCount;
                }
                else if (SideType == LedSideTypes.TopLeftToRight)
                {
                    DirectionSkipStep = vMarginOffset;
                    DirectionStep = vScreenWidth / DirectionLedCount;
                }
                else if (SideType == LedSideTypes.TopRightToLeft)
                {
                    DirectionSkipStep = vScreenWidth - vMarginOffset;
                    DirectionStep = vScreenWidth / DirectionLedCount;
                }
                else if (SideType == LedSideTypes.LeftTopToBottom)
                {
                    DirectionSkipStep = vMarginOffset;
                    DirectionStep = vScreenHeight / DirectionLedCount;
                }
                else if (SideType == LedSideTypes.LeftBottomToTop)
                {
                    DirectionSkipStep = vScreenHeight - vMarginOffset;
                    DirectionStep = vScreenHeight / DirectionLedCount;
                }
                else if (SideType == LedSideTypes.RightTopToBottom)
                {
                    DirectionSkipStep = vMarginOffset;
                    DirectionStep = vScreenHeight / DirectionLedCount;
                }
                else if (SideType == LedSideTypes.RightBottomToTop)
                {
                    DirectionSkipStep = vScreenHeight - vMarginOffset;
                    DirectionStep = vScreenHeight / DirectionLedCount;
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

                    int UsedColors = 0;
                    int AverageRed = 0;
                    int AverageGreen = 0;
                    int AverageBlue = 0;
                    int CaptureZoneStep = 0;

                    for (int lm = 0; lm < vScreenCapturePixels; lm++)
                    {
                        if (SideType == LedSideTypes.BottomLeftToRight)
                        {
                            int ZoneY = vScreenHeight - vMarginBottom - CaptureZoneStep;
                            for (int sk = vMarginOffset; sk < DirectionStep; sk += vCaptureZoneSize)
                            {
                                int ZoneX = DirectionSkipStep + sk;
                                if (ZoneX < vScreenWidth - vMarginOffset) { CurrentColor = CaptureColorAlgorithm(BitmapData, ref UsedColors, ref AverageRed, ref AverageGreen, ref AverageBlue, ZoneY, ZoneX); }
                            }
                        }
                        else if (SideType == LedSideTypes.BottomRightToLeft)
                        {
                            int ZoneY = vScreenHeight - vMarginBottom - CaptureZoneStep;
                            for (int sk = vMarginOffset; sk < DirectionStep; sk += vCaptureZoneSize)
                            {
                                int ZoneX = DirectionSkipStep - sk;
                                if (ZoneX > vMarginOffset) { CurrentColor = CaptureColorAlgorithm(BitmapData, ref UsedColors, ref AverageRed, ref AverageGreen, ref AverageBlue, ZoneY, ZoneX); }
                            }
                        }
                        else if (SideType == LedSideTypes.TopLeftToRight)
                        {
                            int ZoneY = vMarginTop + CaptureZoneStep;
                            for (int sk = vMarginOffset; sk < DirectionStep; sk += vCaptureZoneSize)
                            {
                                int ZoneX = DirectionSkipStep + sk;
                                if (ZoneX < vScreenWidth - vMarginOffset) { CurrentColor = CaptureColorAlgorithm(BitmapData, ref UsedColors, ref AverageRed, ref AverageGreen, ref AverageBlue, ZoneY, ZoneX); }
                            }
                        }
                        else if (SideType == LedSideTypes.TopRightToLeft)
                        {
                            int ZoneY = vMarginTop + CaptureZoneStep;
                            for (int sk = vMarginOffset; sk < DirectionStep; sk += vCaptureZoneSize)
                            {
                                int ZoneX = DirectionSkipStep - sk;
                                if (ZoneX > vMarginOffset) { CurrentColor = CaptureColorAlgorithm(BitmapData, ref UsedColors, ref AverageRed, ref AverageGreen, ref AverageBlue, ZoneY, ZoneX); }
                            }
                        }
                        else if (SideType == LedSideTypes.LeftTopToBottom)
                        {
                            int ZoneX = vMarginLeft + CaptureZoneStep;
                            for (int sk = vMarginOffset; sk < DirectionStep; sk += vCaptureZoneSize)
                            {
                                int ZoneY = DirectionSkipStep + sk;
                                if (ZoneY < vScreenHeight - vMarginOffset) { CurrentColor = CaptureColorAlgorithm(BitmapData, ref UsedColors, ref AverageRed, ref AverageGreen, ref AverageBlue, ZoneY, ZoneX); }
                            }
                        }
                        else if (SideType == LedSideTypes.LeftBottomToTop)
                        {
                            int ZoneX = vMarginRight + CaptureZoneStep;
                            for (int sk = vMarginOffset; sk < DirectionStep; sk += vCaptureZoneSize)
                            {
                                int ZoneY = DirectionSkipStep - sk;
                                if (ZoneY > vMarginOffset) { CurrentColor = CaptureColorAlgorithm(BitmapData, ref UsedColors, ref AverageRed, ref AverageGreen, ref AverageBlue, ZoneY, ZoneX); }
                            }
                        }
                        else if (SideType == LedSideTypes.RightTopToBottom)
                        {
                            int ZoneX = vScreenWidth - vMarginLeft - CaptureZoneStep;
                            for (int sk = vMarginOffset; sk < DirectionStep; sk += vCaptureZoneSize)
                            {
                                int ZoneY = DirectionSkipStep + sk;
                                if (ZoneY < vScreenHeight - vMarginOffset) { CurrentColor = CaptureColorAlgorithm(BitmapData, ref UsedColors, ref AverageRed, ref AverageGreen, ref AverageBlue, ZoneY, ZoneX); }
                            }
                        }
                        else if (SideType == LedSideTypes.RightBottomToTop)
                        {
                            int ZoneX = vScreenWidth - vMarginRight - CaptureZoneStep;
                            for (int sk = vMarginOffset; sk < DirectionStep; sk += vCaptureZoneSize)
                            {
                                int ZoneY = DirectionSkipStep - sk;
                                if (ZoneY > vMarginOffset) { CurrentColor = CaptureColorAlgorithm(BitmapData, ref UsedColors, ref AverageRed, ref AverageGreen, ref AverageBlue, ZoneY, ZoneX); }
                            }
                        }
                        CaptureZoneStep += vCaptureZoneSize;
                    }

                    //Skip to the next capture point
                    if (SideType == LedSideTypes.BottomLeftToRight) { DirectionSkipStep += DirectionStep; }
                    else if (SideType == LedSideTypes.BottomRightToLeft) { DirectionSkipStep -= DirectionStep; }
                    else if (SideType == LedSideTypes.TopLeftToRight) { DirectionSkipStep += DirectionStep; }
                    else if (SideType == LedSideTypes.TopRightToLeft) { DirectionSkipStep -= DirectionStep; }
                    else if (SideType == LedSideTypes.LeftTopToBottom) { DirectionSkipStep += DirectionStep; }
                    else if (SideType == LedSideTypes.LeftBottomToTop) { DirectionSkipStep -= DirectionStep; }
                    else if (SideType == LedSideTypes.RightTopToBottom) { DirectionSkipStep += DirectionStep; }
                    else if (SideType == LedSideTypes.RightBottomToTop) { DirectionSkipStep -= DirectionStep; }

                    //Calculate average color from the bitmap screen capture
                    //Debug.WriteLine("Captured " + UsedColors + " pixel colors from the bitmap data.");
                    CurrentColor = Color.FromArgb(0, AverageRed / UsedColors, AverageGreen / UsedColors, AverageBlue / UsedColors);
                    CurrentColor = AdjustLedColors(CurrentColor);

                    //Set the color to color byte array
                    SerialBytes[CurrentSerialByte] = CurrentColor.R;
                    CurrentSerialByte++;

                    SerialBytes[CurrentSerialByte] = CurrentColor.G;
                    CurrentSerialByte++;

                    SerialBytes[CurrentSerialByte] = CurrentColor.B;
                    CurrentSerialByte++;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to get colors for leds: " + ex.Message);
            }
        }

        //Capture the color pixels
        private static unsafe Color CaptureColorAlgorithm(byte* BitmapData, ref int UsedColors, ref int AverageRed, ref int AverageGreen, ref int AverageBlue, int ZoneY, int ZoneX)
        {
            try
            {
                for (int currentCaptureZone = 0; currentCaptureZone < vCaptureZoneSize; currentCaptureZone++)
                {
                    CurrentColor = ColorProcessing.GetPixelColor(BitmapData, vScreenWidth, vScreenHeight, (ZoneX - currentCaptureZone), (ZoneY - currentCaptureZone));
                    if (setDebugMode && setDebugColor)
                    {
                        ColorProcessing.SetPixelColor(BitmapData, vScreenWidth, vScreenHeight, (ZoneX - currentCaptureZone), (ZoneY - currentCaptureZone), Color.Red);
                    }

                    AverageRed += CurrentColor.R;
                    AverageGreen += CurrentColor.G;
                    AverageBlue += CurrentColor.B;
                    UsedColors += vCaptureZoneSize;
                }
                return CurrentColor;
            }
            catch
            {
                return new Color();
            }
        }
    }
}