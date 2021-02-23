using AmbiPro.Resources;
using System;
using System.Diagnostics;
using System.Drawing;

namespace AmbiPro
{
    public partial class SerialMonitor
    {
        //Get screen shot colors and set it to byte array
        private static unsafe void ScreenColors(Int32 SideType, Int32 OffsetMargin, Int32 AdjustedMargin, byte[] SerialBytes, byte* BitmapData, Int32 DirectionLedCount, Int32 DirectionStep, ref Int32 CurrentSerialByte)
        {
            try
            {
                Color CurrentColor = new Color();
                Int32 CaptureRange = Convert.ToInt32(((vScreenHeight / 100) * setLedCaptureRange) / 2 / setLedSmoothing);
                Int32 DirectionSkipStep = 0;

                //Check led color direction
                if (setLedDirection == 0)
                {
                    if (SideType == 0) { DirectionSkipStep = OffsetMargin; }
                    else if (SideType == 1) { DirectionSkipStep = vScreenHeight - OffsetMargin; }
                    else if (SideType == 2) { DirectionSkipStep = vScreenWidth - OffsetMargin; }
                    else if (SideType == 3) { DirectionSkipStep = OffsetMargin; }
                }
                else
                {
                    if (SideType == 0) { DirectionSkipStep = vScreenWidth - OffsetMargin; }
                    else if (SideType == 1) { DirectionSkipStep = vScreenHeight - OffsetMargin; }
                    else if (SideType == 2) { DirectionSkipStep = OffsetMargin; }
                    else if (SideType == 3) { DirectionSkipStep = OffsetMargin; }
                }

                //Get colors from the bitmap
                for (Int32 dl = 0; dl < DirectionLedCount; dl++)
                {
                    Int32 UsedColors = 0;
                    Int32 AverageRed = 0;
                    Int32 AverageGreen = 0;
                    Int32 AverageBlue = 0;
                    Int32 ZoneSkipStep = 0;

                    for (Int32 lm = 0; lm < CaptureRange; lm++)
                    {
                        if (SideType == 0)
                        {
                            if (setLedDirection == 0)
                            {
                                Int32 ZoneY = vScreenHeight - AdjustedMargin - ZoneSkipStep;
                                for (Int32 sk = setLedSmoothing; sk < DirectionStep; sk += setLedSmoothing)
                                {
                                    Int32 ZoneX = DirectionSkipStep + sk;
                                    if (ZoneX < vScreenWidth - OffsetMargin) { CurrentColor = CaptureColorAlgorithm(BitmapData, ref UsedColors, ref AverageRed, ref AverageGreen, ref AverageBlue, ZoneY, ZoneX); }
                                }
                            }
                            else
                            {
                                Int32 ZoneY = vScreenHeight - AdjustedMargin - ZoneSkipStep;
                                for (Int32 sk = setLedSmoothing; sk < DirectionStep; sk += setLedSmoothing)
                                {
                                    Int32 ZoneX = DirectionSkipStep - sk;
                                    if (ZoneX > OffsetMargin) { CurrentColor = CaptureColorAlgorithm(BitmapData, ref UsedColors, ref AverageRed, ref AverageGreen, ref AverageBlue, ZoneY, ZoneX); }
                                }
                            }
                        }
                        else if (SideType == 1)
                        {
                            if (setLedDirection == 0)
                            {
                                Int32 ZoneX = vScreenWidth - AdjustedMargin - ZoneSkipStep;
                                for (Int32 sk = setLedSmoothing; sk < DirectionStep; sk += setLedSmoothing)
                                {
                                    Int32 ZoneY = DirectionSkipStep - sk;
                                    if (ZoneY > OffsetMargin) { CurrentColor = CaptureColorAlgorithm(BitmapData, ref UsedColors, ref AverageRed, ref AverageGreen, ref AverageBlue, ZoneY, ZoneX); }
                                }
                            }
                            else
                            {
                                Int32 ZoneX = AdjustedMargin + ZoneSkipStep;
                                for (Int32 sk = setLedSmoothing; sk < DirectionStep; sk += setLedSmoothing)
                                {
                                    Int32 ZoneY = DirectionSkipStep - sk;
                                    if (ZoneY > OffsetMargin) { CurrentColor = CaptureColorAlgorithm(BitmapData, ref UsedColors, ref AverageRed, ref AverageGreen, ref AverageBlue, ZoneY, ZoneX); }
                                }
                            }
                        }
                        else if (SideType == 2)
                        {
                            if (setLedDirection == 0)
                            {
                                Int32 ZoneY = AdjustedMargin + ZoneSkipStep;
                                for (Int32 sk = setLedSmoothing; sk < DirectionStep; sk += setLedSmoothing)
                                {
                                    Int32 ZoneX = DirectionSkipStep - sk;
                                    if (ZoneX > OffsetMargin) { CurrentColor = CaptureColorAlgorithm(BitmapData, ref UsedColors, ref AverageRed, ref AverageGreen, ref AverageBlue, ZoneY, ZoneX); }
                                }
                            }
                            else
                            {
                                Int32 ZoneY = AdjustedMargin + ZoneSkipStep;
                                for (Int32 sk = setLedSmoothing; sk < DirectionStep; sk += setLedSmoothing)
                                {
                                    Int32 ZoneX = DirectionSkipStep + sk;
                                    if (ZoneX < vScreenWidth - OffsetMargin) { CurrentColor = CaptureColorAlgorithm(BitmapData, ref UsedColors, ref AverageRed, ref AverageGreen, ref AverageBlue, ZoneY, ZoneX); }
                                }
                            }
                        }
                        else if (SideType == 3)
                        {
                            if (setLedDirection == 0)
                            {
                                Int32 ZoneX = AdjustedMargin + ZoneSkipStep;
                                for (Int32 sk = setLedSmoothing; sk < DirectionStep; sk += setLedSmoothing)
                                {
                                    Int32 ZoneY = DirectionSkipStep + sk;
                                    if (ZoneY < vScreenHeight - OffsetMargin) { CurrentColor = CaptureColorAlgorithm(BitmapData, ref UsedColors, ref AverageRed, ref AverageGreen, ref AverageBlue, ZoneY, ZoneX); }
                                }
                            }
                            else
                            {
                                Int32 ZoneX = vScreenWidth - AdjustedMargin - ZoneSkipStep;
                                for (Int32 sk = setLedSmoothing; sk < DirectionStep; sk += setLedSmoothing)
                                {
                                    Int32 ZoneY = DirectionSkipStep + sk;
                                    if (ZoneY < vScreenHeight - OffsetMargin) { CurrentColor = CaptureColorAlgorithm(BitmapData, ref UsedColors, ref AverageRed, ref AverageGreen, ref AverageBlue, ZoneY, ZoneX); }
                                }
                            }
                        }
                        ZoneSkipStep += setLedSmoothing;
                    }

                    //Skip to the next capture point
                    if (setLedDirection == 0)
                    {
                        if (SideType == 0) { DirectionSkipStep += DirectionStep; }
                        else if (SideType == 1) { DirectionSkipStep -= DirectionStep; }
                        else if (SideType == 2) { DirectionSkipStep -= DirectionStep; }
                        else if (SideType == 3) { DirectionSkipStep += DirectionStep; }
                    }
                    else
                    {
                        if (SideType == 0) { DirectionSkipStep -= DirectionStep; }
                        else if (SideType == 1) { DirectionSkipStep -= DirectionStep; }
                        else if (SideType == 2) { DirectionSkipStep += DirectionStep; }
                        else if (SideType == 3) { DirectionSkipStep += DirectionStep; }
                    }

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
            catch (Exception ex) { Debug.WriteLine("Failed to get colors for leds: " + ex.Message); }
        }

        //Capture the color pixels
        private static unsafe Color CaptureColorAlgorithm(byte* BitmapData, ref int UsedColors, ref int AverageRed, ref int AverageGreen, ref int AverageBlue, int ZoneY, int ZoneX)
        {
            try
            {
                Color CurrentColor = ColorProcessing.GetPixelColor(BitmapData, vScreenWidth, vScreenHeight, ZoneX, ZoneY);
                if (setDebugMode && setDebugColor)
                {
                    ColorProcessing.SetPixelColor(BitmapData, vScreenWidth, vScreenHeight, ZoneX, ZoneY, Color.Red);
                }
                AverageRed += CurrentColor.R; AverageGreen += CurrentColor.G; AverageBlue += CurrentColor.B;

                CurrentColor = ColorProcessing.GetPixelColor(BitmapData, vScreenWidth, vScreenHeight, (ZoneX - 1), (ZoneY - 1));
                if (setDebugMode && setDebugColor)
                {
                    ColorProcessing.SetPixelColor(BitmapData, vScreenWidth, vScreenHeight, (ZoneX - 1), (ZoneY - 1), Color.Green);
                }
                AverageRed += CurrentColor.R; AverageGreen += CurrentColor.G; AverageBlue += CurrentColor.B;

                CurrentColor = ColorProcessing.GetPixelColor(BitmapData, vScreenWidth, vScreenHeight, (ZoneX - 2), (ZoneY - 2));
                if (setDebugMode && setDebugColor)
                {
                    ColorProcessing.SetPixelColor(BitmapData, vScreenWidth, vScreenHeight, (ZoneX - 2), (ZoneY - 2), Color.Blue);
                }
                AverageRed += CurrentColor.R; AverageGreen += CurrentColor.G; AverageBlue += CurrentColor.B;

                UsedColors += 3;
                return CurrentColor;
            }
            catch { return new Color(); }
        }
    }
}