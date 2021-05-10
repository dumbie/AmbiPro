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
        //Detect black bars around screen and adjust the margin accordingly
        private static unsafe void AdjustBlackBars(LedSideTypes SideType, byte* BitmapData)
        {
            try
            {
                if (SideType == LedSideTypes.BottomLeftToRight || SideType == LedSideTypes.BottomRightToLeft) { AdjustBlackbarBottom(BitmapData, ref vMarginBottom); }
                else if (SideType == LedSideTypes.TopLeftToRight || SideType == LedSideTypes.TopRightToLeft) { AdjustBlackbarTop(BitmapData, ref vMarginTop); }
                else if (SideType == LedSideTypes.LeftTopToBottom || SideType == LedSideTypes.LeftBottomToTop) { AdjustBlackbarLeft(BitmapData, ref vMarginLeft); }
                else if (SideType == LedSideTypes.RightTopToBottom || SideType == LedSideTypes.RightBottomToTop) { AdjustBlackbarRight(BitmapData, ref vMarginRight); }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to adjust the black bars: " + ex.Message);
            }
        }

        //Adjust black bars to horizontal top
        static unsafe void AdjustBlackbarTop(byte* bitmapData, ref int targetMargin)
        {
            try
            {
                int captureStepMaximum = vScreenHeight / 3;
                int captureRangeMaximum = vScreenWidth - vMarginMinimumOffset;
                for (int captureStep = vMarginMinimumOffset; captureStep < captureStepMaximum; captureStep += vMarginBlackAccuracy)
                {
                    int CaptureZoneVer = vScreenHeight - captureStep;
                    for (int captureRange = vMarginMinimumOffset; captureRange < captureRangeMaximum; captureRange += vMarginBlackAccuracy)
                    {
                        int CaptureZoneHor = captureRange;
                        Color ColorPixel = ColorProcessing.GetPixelColor(bitmapData, vScreenWidth, vScreenHeight, CaptureZoneHor, CaptureZoneVer);
                        if (setDebugMode && setDebugBlackBar)
                        {
                            ColorProcessing.SetPixelColor(bitmapData, vScreenWidth, vScreenHeight, CaptureZoneHor, CaptureZoneVer, Color.Orange);
                        }
                        if (ColorPixel.R > setAdjustBlackBarBrightness || ColorPixel.G > setAdjustBlackBarBrightness || ColorPixel.B > setAdjustBlackBarBrightness)
                        {
                            targetMargin = captureStep;
                            //Debug.WriteLine("Adjusting black bar margin to: " + captureStep);
                            return;
                        }
                    }
                }
                targetMargin = captureStepMaximum;
            }
            catch { }
        }

        //Adjust black bars to horizontal bottom
        static unsafe void AdjustBlackbarBottom(byte* bitmapData, ref int targetMargin)
        {
            try
            {
                int captureStepMaximum = vScreenHeight / 3;
                int captureRangeMaximum = vScreenWidth - vMarginMinimumOffset;
                for (int captureStep = vMarginMinimumOffset; captureStep < captureStepMaximum; captureStep += vMarginBlackAccuracy)
                {
                    int CaptureZoneVer = captureStep;
                    for (int captureRange = vMarginMinimumOffset; captureRange < captureRangeMaximum; captureRange += vMarginBlackAccuracy)
                    {
                        int CaptureZoneHor = captureRange;
                        Color ColorPixel = ColorProcessing.GetPixelColor(bitmapData, vScreenWidth, vScreenHeight, CaptureZoneHor, CaptureZoneVer);
                        if (setDebugMode && setDebugBlackBar)
                        {
                            ColorProcessing.SetPixelColor(bitmapData, vScreenWidth, vScreenHeight, CaptureZoneHor, CaptureZoneVer, Color.Orange);
                        }
                        if (ColorPixel.R > setAdjustBlackBarBrightness || ColorPixel.G > setAdjustBlackBarBrightness || ColorPixel.B > setAdjustBlackBarBrightness)
                        {
                            targetMargin = captureStep;
                            //Debug.WriteLine("Adjusting black bar margin to: " + captureStep);
                            return;
                        }
                    }
                }
                targetMargin = captureStepMaximum;
            }
            catch { }
        }

        //Adjust black bars to vertical right
        static unsafe void AdjustBlackbarRight(byte* bitmapData, ref int targetMargin)
        {
            try
            {
                int captureStepMaximum = vScreenWidth / 3;
                int captureRangeMaximum = vScreenHeight - vMarginMinimumOffset;
                for (int captureStep = vMarginMinimumOffset; captureStep < captureStepMaximum; captureStep += vMarginBlackAccuracy)
                {
                    int CaptureZoneHor = vScreenWidth - captureStep;
                    for (int captureRange = vMarginMinimumOffset; captureRange < captureRangeMaximum; captureRange += vMarginBlackAccuracy)
                    {
                        int CaptureZoneVer = captureRange;
                        Color ColorPixel = ColorProcessing.GetPixelColor(bitmapData, vScreenWidth, vScreenHeight, CaptureZoneHor, CaptureZoneVer);
                        if (setDebugMode && setDebugBlackBar)
                        {
                            ColorProcessing.SetPixelColor(bitmapData, vScreenWidth, vScreenHeight, CaptureZoneHor, CaptureZoneVer, Color.Orange);
                        }
                        if (ColorPixel.R > setAdjustBlackBarBrightness || ColorPixel.G > setAdjustBlackBarBrightness || ColorPixel.B > setAdjustBlackBarBrightness)
                        {
                            targetMargin = captureStep;
                            //Debug.WriteLine("Adjusting black bar margin to: " + captureStep);
                            return;
                        }
                    }
                }
                targetMargin = captureStepMaximum;
            }
            catch { }
        }

        //Adjust black bars to vertical left
        static unsafe void AdjustBlackbarLeft(byte* bitmapData, ref int targetMargin)
        {
            try
            {
                int captureStepMaximum = vScreenWidth / 3;
                int captureRangeMaximum = vScreenHeight - vMarginMinimumOffset;
                for (int captureStep = vMarginMinimumOffset; captureStep < captureStepMaximum; captureStep += vMarginBlackAccuracy)
                {
                    int CaptureZoneHor = captureStep;
                    for (int captureRange = vMarginMinimumOffset; captureRange < captureRangeMaximum; captureRange += vMarginBlackAccuracy)
                    {
                        int CaptureZoneVer = captureRange;
                        Color ColorPixel = ColorProcessing.GetPixelColor(bitmapData, vScreenWidth, vScreenHeight, CaptureZoneHor, CaptureZoneVer);
                        if (setDebugMode && setDebugBlackBar)
                        {
                            ColorProcessing.SetPixelColor(bitmapData, vScreenWidth, vScreenHeight, CaptureZoneHor, CaptureZoneVer, Color.Orange);
                        }
                        if (ColorPixel.R > setAdjustBlackBarBrightness || ColorPixel.G > setAdjustBlackBarBrightness || ColorPixel.B > setAdjustBlackBarBrightness)
                        {
                            targetMargin = captureStep;
                            //Debug.WriteLine("Adjusting black bar margin to: " + captureStep);
                            return;
                        }
                    }
                }
                targetMargin = captureStepMaximum;
            }
            catch { }
        }
    }
}