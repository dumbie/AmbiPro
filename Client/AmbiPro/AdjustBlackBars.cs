using AmbiPro.Resources;
using System;
using System.Diagnostics;
using static AmbiPro.AppClasses;
using static AmbiPro.AppEnums;
using static AmbiPro.AppVariables;
using static AmbiPro.PreloadSettings;

namespace AmbiPro
{
    public partial class SerialMonitor
    {
        //Detect and adjust blackbars
        private static void AdjustBlackBars(LedSideTypes sideType, byte[] bitmapByteArray)
        {
            try
            {
                if (sideType == LedSideTypes.BottomLeftToRight || sideType == LedSideTypes.BottomRightToLeft)
                {
                    UpdateBlackBarMargin(DetectBlackbarBottom(bitmapByteArray), ref vMarginBottom);
                }
                else if (sideType == LedSideTypes.TopLeftToRight || sideType == LedSideTypes.TopRightToLeft)
                {
                    UpdateBlackBarMargin(DetectBlackbarTop(bitmapByteArray), ref vMarginTop);
                }
                else if (sideType == LedSideTypes.LeftTopToBottom || sideType == LedSideTypes.LeftBottomToTop)
                {
                    UpdateBlackBarMargin(DetectBlackbarLeft(bitmapByteArray), ref vMarginLeft);
                }
                else if (sideType == LedSideTypes.RightTopToBottom || sideType == LedSideTypes.RightBottomToTop)
                {
                    UpdateBlackBarMargin(DetectBlackbarRight(bitmapByteArray), ref vMarginRight);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to adjust black bars: " + ex.Message);
            }
        }

        //Update blackbar margin
        private static void UpdateBlackBarMargin(int colorMargin, ref int captureMargin)
        {
            try
            {
                //Debug.WriteLine("Color detected at: " + colorMargin);
                //Debug.WriteLine("Current margin at: " + targetMargin);
                if (captureMargin < colorMargin)
                {
                    int newMargin = captureMargin + vBlackBarAdjustStep;
                    if (newMargin < colorMargin)
                    {
                        captureMargin = newMargin;
                    }
                    else
                    {
                        captureMargin = colorMargin;
                    }
                }
                else if (captureMargin > colorMargin)
                {
                    int newMargin = captureMargin - vBlackBarAdjustStep;
                    if (newMargin > colorMargin)
                    {
                        captureMargin = newMargin;
                    }
                    else
                    {
                        captureMargin = colorMargin;
                    }
                }
            }
            catch { }
        }

        //Detect blackbar top
        private static int DetectBlackbarTop(byte[] bitmapByteArray)
        {
            int captureStep = vBlackBarMinimumMargin;
            try
            {
                while (captureStep < vBlackBarVerticalMaximumMargin)
                {
                    captureStep += vBlackBarDetectStep;
                    int CaptureZoneVer = vCaptureDetails.Height - captureStep;
                    for (int captureRange = vBlackBarMinimumMargin; captureRange < vBlackBarVerticalDetectRange; captureRange += vBlackBarDetectStep)
                    {
                        int CaptureZoneHor = captureRange;
                        ColorRGBA ColorPixel = ColorProcessing.GetPixelColor(bitmapByteArray, vCaptureDetails.Width, vCaptureDetails.Height, CaptureZoneHor, CaptureZoneVer);
                        if (ColorPixel != null)
                        {
                            if (vDebugCaptureAllowed && setDebugBlackBar)
                            {
                                ColorProcessing.SetPixelColor(bitmapByteArray, vCaptureDetails.Width, vCaptureDetails.Height, CaptureZoneHor, CaptureZoneVer, ColorRGBA.Orange);
                            }

                            //Calculate color luminance
                            int colorLuminance = (ColorPixel.R + ColorPixel.G + ColorPixel.B) / 3;

                            //Check if color is detected
                            if (colorLuminance > setAdjustBlackBarBrightness)
                            {
                                return captureStep;
                            }
                        }
                    }
                }
            }
            catch { }
            return captureStep;
        }

        //Detect blackbar bottom
        private static int DetectBlackbarBottom(byte[] bitmapByteArray)
        {
            int captureStep = vBlackBarMinimumMargin;
            try
            {
                while (captureStep < vBlackBarVerticalMaximumMargin)
                {
                    captureStep += vBlackBarDetectStep;
                    int CaptureZoneVer = captureStep;
                    for (int captureRange = vBlackBarMinimumMargin; captureRange < vBlackBarVerticalDetectRange; captureRange += vBlackBarDetectStep)
                    {
                        int CaptureZoneHor = captureRange;
                        ColorRGBA ColorPixel = ColorProcessing.GetPixelColor(bitmapByteArray, vCaptureDetails.Width, vCaptureDetails.Height, CaptureZoneHor, CaptureZoneVer);
                        if (ColorPixel != null)
                        {
                            if (vDebugCaptureAllowed && setDebugBlackBar)
                            {
                                ColorProcessing.SetPixelColor(bitmapByteArray, vCaptureDetails.Width, vCaptureDetails.Height, CaptureZoneHor, CaptureZoneVer, ColorRGBA.Orange);
                            }

                            //Calculate color luminance
                            int colorLuminance = (ColorPixel.R + ColorPixel.G + ColorPixel.B) / 3;

                            //Check if color is detected
                            if (colorLuminance > setAdjustBlackBarBrightness)
                            {
                                return captureStep;
                            }
                        }
                    }
                }
            }
            catch { }
            return captureStep;
        }

        //Detect blackbar right
        private static int DetectBlackbarRight(byte[] bitmapByteArray)
        {
            int captureStep = vBlackBarMinimumMargin;
            try
            {
                while (captureStep < vBlackBarHorizontalMaximumMargin)
                {
                    captureStep += vBlackBarDetectStep;
                    int CaptureZoneHor = vCaptureDetails.Width - captureStep;
                    for (int captureRange = vBlackBarMinimumMargin; captureRange < vBlackBarHorizontalDetectRange; captureRange += vBlackBarDetectStep)
                    {
                        int CaptureZoneVer = captureRange;
                        ColorRGBA ColorPixel = ColorProcessing.GetPixelColor(bitmapByteArray, vCaptureDetails.Width, vCaptureDetails.Height, CaptureZoneHor, CaptureZoneVer);
                        if (ColorPixel != null)
                        {
                            if (vDebugCaptureAllowed && setDebugBlackBar)
                            {
                                ColorProcessing.SetPixelColor(bitmapByteArray, vCaptureDetails.Width, vCaptureDetails.Height, CaptureZoneHor, CaptureZoneVer, ColorRGBA.Orange);
                            }

                            //Calculate color luminance
                            int colorLuminance = (ColorPixel.R + ColorPixel.G + ColorPixel.B) / 3;

                            //Check if color is detected
                            if (colorLuminance > setAdjustBlackBarBrightness)
                            {
                                return captureStep;
                            }
                        }
                    }
                }
            }
            catch { }
            return captureStep;
        }

        //Detect blackbar left
        private static int DetectBlackbarLeft(byte[] bitmapByteArray)
        {
            int captureStep = vBlackBarMinimumMargin;
            try
            {
                while (captureStep < vBlackBarHorizontalMaximumMargin)
                {
                    captureStep += vBlackBarDetectStep;
                    int CaptureZoneHor = captureStep;
                    for (int captureRange = vBlackBarMinimumMargin; captureRange < vBlackBarHorizontalDetectRange; captureRange += vBlackBarDetectStep)
                    {
                        int CaptureZoneVer = captureRange;
                        ColorRGBA ColorPixel = ColorProcessing.GetPixelColor(bitmapByteArray, vCaptureDetails.Width, vCaptureDetails.Height, CaptureZoneHor, CaptureZoneVer);
                        if (ColorPixel != null)
                        {
                            if (vDebugCaptureAllowed && setDebugBlackBar)
                            {
                                ColorProcessing.SetPixelColor(bitmapByteArray, vCaptureDetails.Width, vCaptureDetails.Height, CaptureZoneHor, CaptureZoneVer, ColorRGBA.Orange);
                            }

                            //Calculate color luminance
                            int colorLuminance = (ColorPixel.R + ColorPixel.G + ColorPixel.B) / 3;

                            //Check if color is detected
                            if (colorLuminance > setAdjustBlackBarBrightness)
                            {
                                return captureStep;
                            }
                        }
                    }
                }
            }
            catch { }
            return captureStep;
        }
    }
}