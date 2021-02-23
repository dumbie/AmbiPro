using AmbiPro.Resources;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;

namespace AmbiPro
{
    public partial class SerialMonitor
    {
        //Detect black bars around screen and adjust the margin accordingly
        private static unsafe void AdjustBlackBars(Int32 SideType, Int32 OffsetMargin, ref Int32 AdjustedMargin, byte* BitmapData)
        {
            try
            {
                Int32 BlackAccuracy = 30;
                Int32 BlackMarginStep = 15;
                Int32 NewColorMargin = AdjustedMargin - BlackMarginStep;
                Int32 NewBlackMargin = AdjustedMargin + BlackMarginStep;

                if (SideType == 0)
                {
                    HorizontalBottom(OffsetMargin, ref AdjustedMargin, BitmapData, BlackAccuracy, NewColorMargin, NewBlackMargin);
                }
                else if (SideType == 1)
                {
                    if (setLedDirection == 0) { VerticalRight(OffsetMargin, ref AdjustedMargin, BitmapData, BlackAccuracy, NewColorMargin, NewBlackMargin); }
                    else { VerticalLeft(OffsetMargin, ref AdjustedMargin, BitmapData, BlackAccuracy, NewColorMargin, NewBlackMargin); }
                }
                else if (SideType == 2)
                {
                    HorizontalTop(OffsetMargin, ref AdjustedMargin, BitmapData, BlackAccuracy, NewColorMargin, NewBlackMargin);
                }
                else if (SideType == 3)
                {
                    if (setLedDirection == 0) { VerticalLeft(OffsetMargin, ref AdjustedMargin, BitmapData, BlackAccuracy, NewColorMargin, NewBlackMargin); }
                    else { VerticalRight(OffsetMargin, ref AdjustedMargin, BitmapData, BlackAccuracy, NewColorMargin, NewBlackMargin); }
                }
            }
            catch (Exception ex) { Debug.WriteLine("Failed to adjust the black bars: " + ex.Message); }
        }

        //Adjust the margin based on entire screen
        static unsafe bool AdjustScreenEntire(byte* BitmapData, ref int AdjustedMargin, Int32 NewColorMargin, Int32 ZoneX, Int32 ZoneY)
        {
            try
            {
                Color EntireScreen = ColorProcessing.GetPixelColor(BitmapData, vScreenWidth, vScreenHeight, ZoneX, ZoneY);
                if (setDebugMode && setDebugBlackBar)
                {
                    ColorProcessing.SetPixelColor(BitmapData, vScreenWidth, vScreenHeight, ZoneX, ZoneY, Color.Pink);
                }
                if (EntireScreen.R > setAdjustBlackBarLevel || EntireScreen.G > setAdjustBlackBarLevel || EntireScreen.B > setAdjustBlackBarLevel)
                {
                    AdjustedMargin = NewColorMargin;
                    //Debug.WriteLine("Color adjusting margin to: " + AdjustedMargin);
                    return true;
                }
            }
            catch { }
            return false;
        }

        //Adjust the margin based on screen lines
        static unsafe bool AdjustScreenLines(byte* BitmapData, ref int AdjustedMargin, Int32 NewColorMargin, bool Horizontal, Int32 ZoneX, Int32 ZoneY)
        {
            try
            {
                Int32 HorizontalMargin = 0;
                Int32 VerticalMargin = 0;
                if (Horizontal) { HorizontalMargin = vScreenWidth / 3; } else { VerticalMargin = vScreenHeight / 3; }

                Color ScreenLine1 = ColorProcessing.GetPixelColor(BitmapData, vScreenWidth, vScreenHeight, ZoneX, ZoneY);
                if (setDebugMode && setDebugBlackBar)
                {
                    ColorProcessing.SetPixelColor(BitmapData, vScreenWidth, vScreenHeight, ZoneX, ZoneY, Color.Purple);
                }
                if (ScreenLine1.R > setAdjustBlackBarLevel || ScreenLine1.G > setAdjustBlackBarLevel || ScreenLine1.B > setAdjustBlackBarLevel)
                {
                    AdjustedMargin = NewColorMargin;
                    //Debug.WriteLine("Color adjusting margin to: " + AdjustedMargin);
                    return true;
                }

                Color ScreenLine2 = ColorProcessing.GetPixelColor(BitmapData, vScreenWidth, vScreenHeight, ZoneX - HorizontalMargin, ZoneY - VerticalMargin);
                if (setDebugMode && setDebugBlackBar)
                {
                    ColorProcessing.SetPixelColor(BitmapData, vScreenWidth, vScreenHeight, ZoneX - HorizontalMargin, ZoneY - VerticalMargin, Color.Purple);
                }
                if (ScreenLine2.R > setAdjustBlackBarLevel || ScreenLine2.G > setAdjustBlackBarLevel || ScreenLine2.B > setAdjustBlackBarLevel)
                {
                    AdjustedMargin = NewColorMargin;
                    //Debug.WriteLine("Color adjusting margin to: " + AdjustedMargin);
                    return true;
                }

                Color ScreenLine3 = ColorProcessing.GetPixelColor(BitmapData, vScreenWidth, vScreenHeight, ZoneX + HorizontalMargin, ZoneY + VerticalMargin);
                if (setDebugMode && setDebugBlackBar)
                {
                    ColorProcessing.SetPixelColor(BitmapData, vScreenWidth, vScreenHeight, ZoneX + HorizontalMargin, ZoneY + VerticalMargin, Color.Purple);
                }
                if (ScreenLine3.R > setAdjustBlackBarLevel || ScreenLine3.G > setAdjustBlackBarLevel || ScreenLine3.B > setAdjustBlackBarLevel)
                {
                    AdjustedMargin = NewColorMargin;
                    //Debug.WriteLine("Color adjusting margin to: " + AdjustedMargin);
                    return true;
                }
            }
            catch { }
            return false;
        }

        //Adjust to horizontal bottom
        static unsafe void HorizontalBottom(int OffsetMargin, ref int AdjustedMargin, byte* BitmapData, int BlackAccuracy, int NewColorMargin, int NewBlackMargin)
        {
            try
            {
                if (NewColorMargin >= OffsetMargin)
                {
                    //Color check entire screen
                    for (Int32 sz = OffsetMargin; sz < vScreenWidth; sz += BlackAccuracy)
                    {
                        Int32 ZoneX = sz;
                        Int32 ZoneY = vScreenHeight - NewColorMargin;
                        if (AdjustScreenEntire(BitmapData, ref AdjustedMargin, NewColorMargin, ZoneX, ZoneY)) { return; }
                    }
                    //Color check three lines
                    for (Int32 sz = OffsetMargin; sz < NewColorMargin; sz += BlackAccuracy)
                    {
                        Int32 ZoneX = vScreenWidth / 2;
                        Int32 ZoneY = vScreenHeight - sz;
                        if (AdjustScreenLines(BitmapData, ref AdjustedMargin, NewColorMargin, true, ZoneX, ZoneY)) { return; }
                    }
                }

                if (NewBlackMargin < (vScreenHeight / 2))
                {
                    Color[] BlackPixelColors = new Color[vScreenWidth];
                    //Black check entire screen
                    for (Int32 sz = OffsetMargin; sz < vScreenWidth; sz += BlackAccuracy)
                    {
                        Int32 ZoneX = sz;
                        Int32 ZoneY = vScreenHeight - AdjustedMargin;
                        BlackPixelColors[sz] = ColorProcessing.GetPixelColor(BitmapData, vScreenWidth, vScreenHeight, ZoneX, ZoneY);
                        if (setDebugMode && setDebugBlackBar)
                        {
                            ColorProcessing.SetPixelColor(BitmapData, vScreenWidth, vScreenHeight, ZoneX, ZoneY, Color.Orange);
                        }
                    }

                    if (BlackPixelColors.All(ColorByte => ColorByte.R <= setAdjustBlackBarLevel && ColorByte.G <= setAdjustBlackBarLevel && ColorByte.B <= setAdjustBlackBarLevel))
                    {
                        AdjustedMargin = NewBlackMargin;
                        //Debug.WriteLine("Black adjusting margin to: " + AdjustedMargin);
                        return;
                    }
                }
            }
            catch { }
        }

        //Adjust to horizontal top
        static unsafe void HorizontalTop(int OffsetMargin, ref int AdjustedMargin, byte* BitmapData, int BlackAccuracy, int NewColorMargin, int NewBlackMargin)
        {
            try
            {
                if (NewColorMargin >= OffsetMargin)
                {
                    //Color check entire screen
                    for (Int32 sz = OffsetMargin; sz < vScreenWidth; sz += BlackAccuracy)
                    {
                        Int32 ZoneX = sz;
                        Int32 ZoneY = NewColorMargin;
                        if (AdjustScreenEntire(BitmapData, ref AdjustedMargin, NewColorMargin, ZoneX, ZoneY)) { return; }
                    }
                    //Color check three lines
                    for (Int32 sz = OffsetMargin; sz < NewColorMargin; sz += BlackAccuracy)
                    {
                        Int32 ZoneX = vScreenWidth / 2;
                        Int32 ZoneY = sz;
                        if (AdjustScreenLines(BitmapData, ref AdjustedMargin, NewColorMargin, true, ZoneX, ZoneY)) { return; }
                    }
                }

                if (NewBlackMargin < (vScreenHeight / 2))
                {
                    Color[] BlackPixelColors = new Color[vScreenWidth];
                    //Black check entire screen
                    for (Int32 sz = OffsetMargin; sz < vScreenWidth; sz += BlackAccuracy)
                    {
                        Int32 ZoneX = sz;
                        Int32 ZoneY = AdjustedMargin;
                        BlackPixelColors[sz] = ColorProcessing.GetPixelColor(BitmapData, vScreenWidth, vScreenHeight, ZoneX, ZoneY);
                        if (setDebugMode && setDebugBlackBar)
                        {
                            ColorProcessing.SetPixelColor(BitmapData, vScreenWidth, vScreenHeight, ZoneX, ZoneY, Color.Orange);
                        }
                    }

                    if (BlackPixelColors.All(ColorByte => ColorByte.R <= setAdjustBlackBarLevel && ColorByte.G <= setAdjustBlackBarLevel && ColorByte.B <= setAdjustBlackBarLevel))
                    {
                        AdjustedMargin = NewBlackMargin;
                        //Debug.WriteLine("Black adjusting margin to: " + AdjustedMargin);
                        return;
                    }
                }
            }
            catch { }
        }

        //Adjust to vertical right
        static unsafe void VerticalRight(int OffsetMargin, ref int AdjustedMargin, byte* BitmapData, int BlackAccuracy, int NewColorMargin, int NewBlackMargin)
        {
            try
            {
                if (NewColorMargin >= OffsetMargin)
                {
                    //Color check entire screen
                    for (Int32 sz = OffsetMargin; sz < vScreenHeight; sz += BlackAccuracy)
                    {
                        Int32 ZoneX = vScreenWidth - NewColorMargin;
                        Int32 ZoneY = sz;
                        if (AdjustScreenEntire(BitmapData, ref AdjustedMargin, NewColorMargin, ZoneX, ZoneY)) { return; }
                    }
                    //Color check three lines
                    for (Int32 sz = OffsetMargin; sz < NewColorMargin; sz += BlackAccuracy)
                    {
                        Int32 ZoneX = vScreenWidth - sz;
                        Int32 ZoneY = vScreenHeight / 2;
                        if (AdjustScreenLines(BitmapData, ref AdjustedMargin, NewColorMargin, false, ZoneX, ZoneY)) { return; }
                    }
                }

                if (NewBlackMargin < (vScreenWidth / 2))
                {
                    Color[] BlackPixelColors = new Color[vScreenHeight];
                    //Black check entire screen
                    for (Int32 sz = OffsetMargin; sz < vScreenHeight; sz += BlackAccuracy)
                    {
                        Int32 ZoneX = vScreenWidth - AdjustedMargin;
                        Int32 ZoneY = sz;
                        BlackPixelColors[sz] = ColorProcessing.GetPixelColor(BitmapData, vScreenWidth, vScreenHeight, ZoneX, ZoneY);
                        if (setDebugMode && setDebugBlackBar)
                        {
                            ColorProcessing.SetPixelColor(BitmapData, vScreenWidth, vScreenHeight, ZoneX, ZoneY, Color.Orange);
                        }
                    }

                    if (BlackPixelColors.All(ColorByte => ColorByte.R <= setAdjustBlackBarLevel && ColorByte.G <= setAdjustBlackBarLevel && ColorByte.B <= setAdjustBlackBarLevel))
                    {
                        AdjustedMargin = NewBlackMargin;
                        //Debug.WriteLine("Black adjusting margin to: " + AdjustedMargin);
                        return;
                    }
                }
            }
            catch { }
        }

        //Adjust to vertical left
        static unsafe void VerticalLeft(int OffsetMargin, ref int AdjustedMargin, byte* BitmapData, int BlackAccuracy, int NewColorMargin, int NewBlackMargin)
        {
            try
            {
                if (NewColorMargin >= OffsetMargin)
                {
                    //Color check entire screen
                    for (Int32 sz = OffsetMargin; sz < vScreenHeight; sz += BlackAccuracy)
                    {
                        Int32 ZoneX = NewColorMargin;
                        Int32 ZoneY = sz;
                        if (AdjustScreenEntire(BitmapData, ref AdjustedMargin, NewColorMargin, ZoneX, ZoneY)) { return; }
                    }
                    //Color check three lines
                    for (Int32 sz = OffsetMargin; sz < NewColorMargin; sz += BlackAccuracy)
                    {
                        Int32 ZoneX = sz;
                        Int32 ZoneY = vScreenHeight / 2;
                        if (AdjustScreenLines(BitmapData, ref AdjustedMargin, NewColorMargin, false, ZoneX, ZoneY)) { return; }
                    }
                }

                if (NewBlackMargin < (vScreenWidth / 2))
                {
                    Color[] BlackPixelColors = new Color[vScreenHeight];
                    //Black check entire screen
                    for (Int32 sz = OffsetMargin; sz < vScreenHeight; sz += BlackAccuracy)
                    {
                        Int32 ZoneX = AdjustedMargin;
                        Int32 ZoneY = sz;
                        BlackPixelColors[sz] = ColorProcessing.GetPixelColor(BitmapData, vScreenWidth, vScreenHeight, ZoneX, ZoneY);
                        if (setDebugMode && setDebugBlackBar)
                        {
                            ColorProcessing.SetPixelColor(BitmapData, vScreenWidth, vScreenHeight, ZoneX, ZoneY, Color.Orange);
                        }
                    }

                    if (BlackPixelColors.All(ColorByte => ColorByte.R <= setAdjustBlackBarLevel && ColorByte.G <= setAdjustBlackBarLevel && ColorByte.B <= setAdjustBlackBarLevel))
                    {
                        AdjustedMargin = NewBlackMargin;
                        //Debug.WriteLine("Black adjusting margin to: " + AdjustedMargin);
                        return;
                    }
                }
            }
            catch { }
        }
    }
}