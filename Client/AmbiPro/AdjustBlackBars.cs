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
        private static void AdjustBlackBars(LedSideTypes sideType, byte[] bitmapByteArray)
        {
            try
            {
                if (sideType == LedSideTypes.BottomLeftToRight || sideType == LedSideTypes.BottomRightToLeft) { AdjustBlackbarBottom(bitmapByteArray, ref vMarginBottom); }
                else if (sideType == LedSideTypes.TopLeftToRight || sideType == LedSideTypes.TopRightToLeft) { AdjustBlackbarTop(bitmapByteArray, ref vMarginTop); }
                else if (sideType == LedSideTypes.LeftTopToBottom || sideType == LedSideTypes.LeftBottomToTop) { AdjustBlackbarLeft(bitmapByteArray, ref vMarginLeft); }
                else if (sideType == LedSideTypes.RightTopToBottom || sideType == LedSideTypes.RightBottomToTop) { AdjustBlackbarRight(bitmapByteArray, ref vMarginRight); }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to adjust the black bars: " + ex.Message);
            }
        }

        //Adjust black bars to horizontal top
        private static void AdjustBlackbarTop(byte[] bitmapByteArray, ref int targetMargin)
        {
            try
            {
                for (int captureStep = vMarginMinimumOffset; captureStep < vBlackBarStepVertical; captureStep += vMarginBlackAccuracy)
                {
                    int CaptureZoneVer = vCaptureDetails.Height - captureStep;
                    for (int captureRange = vMarginMinimumOffset; captureRange < vBlackBarRangeVertical; captureRange += vMarginBlackAccuracy)
                    {
                        int CaptureZoneHor = captureRange;
                        ColorRGBA ColorPixel = ColorProcessing.GetPixelColor(bitmapByteArray, vCaptureDetails.Width, vCaptureDetails.Height, CaptureZoneHor, CaptureZoneVer);
                        if (ColorPixel != null)
                        {
                            if (vDebugCaptureAllowed && setDebugBlackBar)
                            {
                                ColorProcessing.SetPixelColor(bitmapByteArray, vCaptureDetails.Width, vCaptureDetails.Height, CaptureZoneHor, CaptureZoneVer, ColorRGBA.Orange);
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
        private static void AdjustBlackbarBottom(byte[] bitmapByteArray, ref int targetMargin)
        {
            try
            {
                for (int captureStep = vMarginMinimumOffset; captureStep < vBlackBarStepVertical; captureStep += vMarginBlackAccuracy)
                {
                    int CaptureZoneVer = captureStep;
                    for (int captureRange = vMarginMinimumOffset; captureRange < vBlackBarRangeVertical; captureRange += vMarginBlackAccuracy)
                    {
                        int CaptureZoneHor = captureRange;
                        ColorRGBA ColorPixel = ColorProcessing.GetPixelColor(bitmapByteArray, vCaptureDetails.Width, vCaptureDetails.Height, CaptureZoneHor, CaptureZoneVer);
                        if (ColorPixel != null)
                        {
                            if (vDebugCaptureAllowed && setDebugBlackBar)
                            {
                                ColorProcessing.SetPixelColor(bitmapByteArray, vCaptureDetails.Width, vCaptureDetails.Height, CaptureZoneHor, CaptureZoneVer, ColorRGBA.Orange);
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
        private static void AdjustBlackbarRight(byte[] bitmapByteArray, ref int targetMargin)
        {
            try
            {
                for (int captureStep = vMarginMinimumOffset; captureStep < vBlackBarStepHorizontal; captureStep += vMarginBlackAccuracy)
                {
                    int CaptureZoneHor = vCaptureDetails.Width - captureStep;
                    for (int captureRange = vMarginMinimumOffset; captureRange < vBlackBarRangeHorizontal; captureRange += vMarginBlackAccuracy)
                    {
                        int CaptureZoneVer = captureRange;
                        ColorRGBA ColorPixel = ColorProcessing.GetPixelColor(bitmapByteArray, vCaptureDetails.Width, vCaptureDetails.Height, CaptureZoneHor, CaptureZoneVer);
                        if (ColorPixel != null)
                        {
                            if (vDebugCaptureAllowed && setDebugBlackBar)
                            {
                                ColorProcessing.SetPixelColor(bitmapByteArray, vCaptureDetails.Width, vCaptureDetails.Height, CaptureZoneHor, CaptureZoneVer, ColorRGBA.Orange);
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
        private static void AdjustBlackbarLeft(byte[] bitmapByteArray, ref int targetMargin)
        {
            try
            {
                for (int captureStep = vMarginMinimumOffset; captureStep < vBlackBarStepHorizontal; captureStep += vMarginBlackAccuracy)
                {
                    int CaptureZoneHor = captureStep;
                    for (int captureRange = vMarginMinimumOffset; captureRange < vBlackBarRangeHorizontal; captureRange += vMarginBlackAccuracy)
                    {
                        int CaptureZoneVer = captureRange;
                        ColorRGBA ColorPixel = ColorProcessing.GetPixelColor(bitmapByteArray, vCaptureDetails.Width, vCaptureDetails.Height, CaptureZoneHor, CaptureZoneVer);
                        if (ColorPixel != null)
                        {
                            if (vDebugCaptureAllowed && setDebugBlackBar)
                            {
                                ColorProcessing.SetPixelColor(bitmapByteArray, vCaptureDetails.Width, vCaptureDetails.Height, CaptureZoneHor, CaptureZoneVer, ColorRGBA.Orange);
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