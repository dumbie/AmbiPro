using AmbiPro.Resources;
using System;
using System.Diagnostics;
using static AmbiPro.AppClasses;
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
                for (int captureStep = vMarginMinimumOffset; captureStep < vBlackBarStepVertical; captureStep += vMarginBlackAccuracy)
                {
                    int CaptureZoneVer = vCaptureHeight - captureStep;
                    for (int captureRange = vMarginMinimumOffset; captureRange < vBlackBarRangeVertical; captureRange += vMarginBlackAccuracy)
                    {
                        int CaptureZoneHor = captureRange;
                        ColorRGBA ColorPixel = ColorProcessing.GetPixelColor(bitmapData, vCaptureWidth, vCaptureHeight, CaptureZoneHor, CaptureZoneVer);
                        if (ColorPixel != null)
                        {
                            if (vDebugCaptureAllowed && setDebugBlackBar)
                            {
                                ColorProcessing.SetPixelColor(bitmapData, vCaptureWidth, vCaptureHeight, CaptureZoneHor, CaptureZoneVer, ColorRGBA.Orange);
                            }
                            if (ColorPixel.R > setAdjustBlackBarBrightness || ColorPixel.G > setAdjustBlackBarBrightness || ColorPixel.B > setAdjustBlackBarBrightness)
                            {
                                targetMargin = captureStep;
                                //Debug.WriteLine("Adjusting black bar margin to: " + captureStep);
                                return;
                            }
                        }
                    }
                }
                targetMargin = vBlackBarStepVertical;
            }
            catch { }
        }

        //Adjust black bars to horizontal bottom
        static unsafe void AdjustBlackbarBottom(byte* bitmapData, ref int targetMargin)
        {
            try
            {
                for (int captureStep = vMarginMinimumOffset; captureStep < vBlackBarStepVertical; captureStep += vMarginBlackAccuracy)
                {
                    int CaptureZoneVer = captureStep;
                    for (int captureRange = vMarginMinimumOffset; captureRange < vBlackBarRangeVertical; captureRange += vMarginBlackAccuracy)
                    {
                        int CaptureZoneHor = captureRange;
                        ColorRGBA ColorPixel = ColorProcessing.GetPixelColor(bitmapData, vCaptureWidth, vCaptureHeight, CaptureZoneHor, CaptureZoneVer);
                        if (ColorPixel != null)
                        {
                            if (vDebugCaptureAllowed && setDebugBlackBar)
                            {
                                ColorProcessing.SetPixelColor(bitmapData, vCaptureWidth, vCaptureHeight, CaptureZoneHor, CaptureZoneVer, ColorRGBA.Orange);
                            }
                            if (ColorPixel.R > setAdjustBlackBarBrightness || ColorPixel.G > setAdjustBlackBarBrightness || ColorPixel.B > setAdjustBlackBarBrightness)
                            {
                                targetMargin = captureStep;
                                //Debug.WriteLine("Adjusting black bar margin to: " + captureStep);
                                return;
                            }
                        }
                    }
                }
                targetMargin = vBlackBarStepVertical;
            }
            catch { }
        }

        //Adjust black bars to vertical right
        static unsafe void AdjustBlackbarRight(byte* bitmapData, ref int targetMargin)
        {
            try
            {
                for (int captureStep = vMarginMinimumOffset; captureStep < vBlackBarStepHorizontal; captureStep += vMarginBlackAccuracy)
                {
                    int CaptureZoneHor = vCaptureWidth - captureStep;
                    for (int captureRange = vMarginMinimumOffset; captureRange < vBlackBarRangeHorizontal; captureRange += vMarginBlackAccuracy)
                    {
                        int CaptureZoneVer = captureRange;
                        ColorRGBA ColorPixel = ColorProcessing.GetPixelColor(bitmapData, vCaptureWidth, vCaptureHeight, CaptureZoneHor, CaptureZoneVer);
                        if (ColorPixel != null)
                        {
                            if (vDebugCaptureAllowed && setDebugBlackBar)
                            {
                                ColorProcessing.SetPixelColor(bitmapData, vCaptureWidth, vCaptureHeight, CaptureZoneHor, CaptureZoneVer, ColorRGBA.Orange);
                            }
                            if (ColorPixel.R > setAdjustBlackBarBrightness || ColorPixel.G > setAdjustBlackBarBrightness || ColorPixel.B > setAdjustBlackBarBrightness)
                            {
                                targetMargin = captureStep;
                                //Debug.WriteLine("Adjusting black bar margin to: " + captureStep);
                                return;
                            }
                        }
                    }
                }
                targetMargin = vBlackBarStepHorizontal;
            }
            catch { }
        }

        //Adjust black bars to vertical left
        static unsafe void AdjustBlackbarLeft(byte* bitmapData, ref int targetMargin)
        {
            try
            {
                for (int captureStep = vMarginMinimumOffset; captureStep < vBlackBarStepHorizontal; captureStep += vMarginBlackAccuracy)
                {
                    int CaptureZoneHor = captureStep;
                    for (int captureRange = vMarginMinimumOffset; captureRange < vBlackBarRangeHorizontal; captureRange += vMarginBlackAccuracy)
                    {
                        int CaptureZoneVer = captureRange;
                        ColorRGBA ColorPixel = ColorProcessing.GetPixelColor(bitmapData, vCaptureWidth, vCaptureHeight, CaptureZoneHor, CaptureZoneVer);
                        if (ColorPixel != null)
                        {
                            if (vDebugCaptureAllowed && setDebugBlackBar)
                            {
                                ColorProcessing.SetPixelColor(bitmapData, vCaptureWidth, vCaptureHeight, CaptureZoneHor, CaptureZoneVer, ColorRGBA.Orange);
                            }
                            if (ColorPixel.R > setAdjustBlackBarBrightness || ColorPixel.G > setAdjustBlackBarBrightness || ColorPixel.B > setAdjustBlackBarBrightness)
                            {
                                targetMargin = captureStep;
                                //Debug.WriteLine("Adjusting black bar margin to: " + captureStep);
                                return;
                            }
                        }
                    }
                }
                targetMargin = vBlackBarStepHorizontal;
            }
            catch { }
        }
    }
}