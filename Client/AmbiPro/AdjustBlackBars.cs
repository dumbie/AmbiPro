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
                    UpdateBlackBarMargin(DetectBlackbarBottom(bitmapByteArray), ref vCaptureMarginBottom);
                }
                else if (sideType == LedSideTypes.TopLeftToRight || sideType == LedSideTypes.TopRightToLeft)
                {
                    UpdateBlackBarMargin(DetectBlackbarTop(bitmapByteArray), ref vCaptureMarginTop);
                }
                else if (sideType == LedSideTypes.LeftTopToBottom || sideType == LedSideTypes.LeftBottomToTop)
                {
                    UpdateBlackBarMargin(DetectBlackbarLeft(bitmapByteArray), ref vCaptureMarginLeft);
                }
                else if (sideType == LedSideTypes.RightTopToBottom || sideType == LedSideTypes.RightBottomToTop)
                {
                    UpdateBlackBarMargin(DetectBlackbarRight(bitmapByteArray), ref vCaptureMarginRight);
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
                //Debug.WriteLine("Current margin at: " + captureMargin);
                if (captureMargin < colorMargin)
                {
                    int newMargin = captureMargin + vBlackbarAdjustStep;
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
                    int newMargin = captureMargin - vBlackbarAdjustStep;
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
            int captureStep = 0;
            try
            {
                for (captureStep = 0; captureStep < vBlackbarRangeVertical; captureStep += vBlackbarDetectStep)
                {
                    int CaptureZoneVer = vCaptureDetails.Height - captureStep;
                    for (int captureRange = 0; captureRange < vCaptureDetails.Width; captureRange += vBlackbarDetectStep)
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
            int captureStep = 0;
            try
            {
                for (captureStep = 0; captureStep < vBlackbarRangeVertical; captureStep += vBlackbarDetectStep)
                {
                    int CaptureZoneVer = captureStep;
                    for (int captureRange = 0; captureRange < vCaptureDetails.Width; captureRange += vBlackbarDetectStep)
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
            int captureStep = 0;
            try
            {
                for (captureStep = 0; captureStep < vBlackbarRangeHorizontal; captureStep += vBlackbarDetectStep)
                {
                    int CaptureZoneHor = vCaptureDetails.Width - captureStep;
                    for (int captureRange = 0; captureRange < vCaptureDetails.Height; captureRange += vBlackbarDetectStep)
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
            int captureStep = 0;
            try
            {
                for (captureStep = 0; captureStep < vBlackbarRangeHorizontal; captureStep += vBlackbarDetectStep)
                {
                    int CaptureZoneHor = captureStep;
                    for (int captureRange = 0; captureRange < vCaptureDetails.Height; captureRange += vBlackbarDetectStep)
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