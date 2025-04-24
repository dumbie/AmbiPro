using ArnoldVinkCode;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using static AmbiPro.AppClasses;
using static AmbiPro.AppEnums;
using static AmbiPro.AppVariables;
using static AmbiPro.PreloadSettings;

namespace AmbiPro
{
    public partial class SerialMonitor
    {
        //Get screenshot colors and set it to byte array
        private static void ScreenColors(LedSideTypes ledSideType, int directionLedCount, byte[] bitmapByteArray, ColorRGBA[] colorArray, ref int colorCurrentIndex)
        {
            try
            {
                //Check the led side
                if (directionLedCount == 0 || ledSideType == LedSideTypes.None)
                {
                    return;
                }

                //Calculate led color direction and bottom gap
                int captureZoneHor = 0;
                int captureZoneVer = 0;
                int captureZoneSize = 0;
                int captureDiffMax = 0;
                int captureDiffMin = 0;
                int bottomGapMax = 0;
                int bottomGapMin = 0;
                if (ledSideType == LedSideTypes.TopLeftToRight)
                {
                    captureZoneHor = 0;
                    captureZoneVer = vCaptureDetails.OutputHeight;
                    captureZoneSize = vCaptureDetails.OutputWidth / directionLedCount;
                    captureDiffMax = (vCaptureDetails.OutputWidth - (captureZoneSize * directionLedCount)) / 2;
                    captureDiffMin = directionLedCount - captureDiffMax;
                }
                else if (ledSideType == LedSideTypes.TopRightToLeft)
                {
                    captureZoneHor = vCaptureDetails.OutputWidth - 1;
                    captureZoneVer = vCaptureDetails.OutputHeight;
                    captureZoneSize = vCaptureDetails.OutputWidth / directionLedCount;
                    captureDiffMax = (vCaptureDetails.OutputWidth - (captureZoneSize * directionLedCount)) / 2;
                    captureDiffMin = directionLedCount - captureDiffMax;
                }
                else if (ledSideType == LedSideTypes.BottomLeftToRight)
                {
                    directionLedCount += setLedBottomGap;
                    captureZoneHor = 0;
                    captureZoneVer = 0;
                    captureZoneSize = vCaptureDetails.OutputWidth / directionLedCount;
                    captureDiffMax = (vCaptureDetails.OutputWidth - (captureZoneSize * directionLedCount)) / 2;
                    captureDiffMin = directionLedCount - captureDiffMax;
                    int BottomGapDiff = setLedBottomGap / 2;
                    int BottomGapTarget = directionLedCount / 2;
                    bottomGapMax = BottomGapTarget + BottomGapDiff;
                    bottomGapMin = BottomGapTarget - BottomGapDiff - 1;
                }
                else if (ledSideType == LedSideTypes.BottomRightToLeft)
                {
                    directionLedCount += setLedBottomGap;
                    captureZoneHor = vCaptureDetails.OutputWidth - 1;
                    captureZoneVer = 0;
                    captureZoneSize = vCaptureDetails.OutputWidth / directionLedCount;
                    captureDiffMax = (vCaptureDetails.OutputWidth - (captureZoneSize * directionLedCount)) / 2;
                    captureDiffMin = directionLedCount - captureDiffMax;
                    int bottomGapDiff = setLedBottomGap / 2;
                    int bottomGapTarget = directionLedCount / 2;
                    bottomGapMax = bottomGapTarget + bottomGapDiff;
                    bottomGapMin = bottomGapTarget - bottomGapDiff - 1;
                }
                else if (ledSideType == LedSideTypes.LeftBottomToTop)
                {
                    captureZoneHor = 0;
                    captureZoneVer = 1;
                    captureZoneSize = vCaptureDetails.OutputHeight / directionLedCount;
                    captureDiffMax = (vCaptureDetails.OutputHeight - (captureZoneSize * directionLedCount)) / 2;
                    captureDiffMin = directionLedCount - captureDiffMax;
                }
                else if (ledSideType == LedSideTypes.LeftTopToBottom)
                {
                    captureZoneHor = 0;
                    captureZoneVer = vCaptureDetails.OutputHeight;
                    captureZoneSize = vCaptureDetails.OutputHeight / directionLedCount;
                    captureDiffMax = (vCaptureDetails.OutputHeight - (captureZoneSize * directionLedCount)) / 2;
                    captureDiffMin = directionLedCount - captureDiffMax;
                }
                else if (ledSideType == LedSideTypes.RightBottomToTop)
                {
                    captureZoneHor = vCaptureDetails.OutputWidth;
                    captureZoneVer = 1;
                    captureZoneSize = vCaptureDetails.OutputHeight / directionLedCount;
                    captureDiffMax = (vCaptureDetails.OutputHeight - (captureZoneSize * directionLedCount)) / 2;
                    captureDiffMin = directionLedCount - captureDiffMax;
                }
                else if (ledSideType == LedSideTypes.RightTopToBottom)
                {
                    captureZoneHor = vCaptureDetails.OutputWidth;
                    captureZoneVer = vCaptureDetails.OutputHeight;
                    captureZoneSize = vCaptureDetails.OutputHeight / directionLedCount;
                    captureDiffMax = (vCaptureDetails.OutputHeight - (captureZoneSize * directionLedCount)) / 2;
                    captureDiffMin = directionLedCount - captureDiffMax;
                }
                //Debug.WriteLine("ZoneHor: " + captureZoneHor + " / ZoneVer: " + captureZoneVer + " / ZoneSize: " + captureZoneSize + " / ZoneDiff: " + captureDiffMin + " / Leds: " + directionLedCount);

                //Get colors from the bitmap
                for (int ledCurrentIndex = 0; ledCurrentIndex < directionLedCount; ledCurrentIndex++)
                {
                    //Set led off color byte array
                    if (ledSideType == LedSideTypes.LedsOff)
                    {
                        //Set color to array
                        colorArray[colorCurrentIndex] = ColorRGBA.Black;
                        colorCurrentIndex++;
                        continue;
                    }

                    //Skip bottom gap leds
                    if (bottomGapMax != 0 && AVFunctions.BetweenNumbers(ledCurrentIndex, bottomGapMin, bottomGapMax, false))
                    {
                        //Debug.WriteLine("Led: " + ledCurrentIndex + " / GapMin: " + bottomGapMin + " / GapMax: " + bottomGapMax + " / GapLeds: " + setLedBottomGap);
                    }
                    else
                    {
                        //Get blackbar range
                        int captureRange = GetBlackbarRanges(ledSideType, ledCurrentIndex);

                        //Capture average color
                        CaptureScreenColorAlgorithm(bitmapByteArray, out ColorRGBA colorCapture, out bool colorFound, out int colorFirstRange, captureZoneHor, captureZoneVer, captureZoneSize, captureRange, ledSideType, ledCurrentIndex);
                        //Debug.WriteLine("Captured average color R" + colorCapture.R + "G" + colorCapture.G + "B" + colorCapture.B + " from the bitmap data.");

                        //Detect blackbar ranges
                        DetectBlackbarRanges(ledSideType, ledCurrentIndex, colorFound, colorFirstRange);

                        //Update debug led strip preview
                        DebugUpdateLedStripPreview(ledSideType, ledCurrentIndex, colorCapture);

                        //Set color to array
                        colorArray[colorCurrentIndex] = colorCapture;
                        colorCurrentIndex++;
                    }

                    //Zone difference correction
                    if (captureDiffMax != 0 && AVFunctions.BetweenNumbersOr(ledCurrentIndex, captureDiffMin, captureDiffMax, true))
                    {
                        if (ledSideType == LedSideTypes.TopLeftToRight) { captureZoneHor++; }
                        else if (ledSideType == LedSideTypes.TopRightToLeft) { captureZoneHor--; }
                        else if (ledSideType == LedSideTypes.BottomLeftToRight) { captureZoneHor++; }
                        else if (ledSideType == LedSideTypes.BottomRightToLeft) { captureZoneHor--; }
                        else if (ledSideType == LedSideTypes.LeftBottomToTop) { captureZoneVer++; }
                        else if (ledSideType == LedSideTypes.LeftTopToBottom) { captureZoneVer--; }
                        else if (ledSideType == LedSideTypes.RightBottomToTop) { captureZoneVer++; }
                        else if (ledSideType == LedSideTypes.RightTopToBottom) { captureZoneVer--; }
                    }

                    //Skip to the next capture zone
                    if (ledSideType == LedSideTypes.TopLeftToRight) { captureZoneHor += captureZoneSize; }
                    else if (ledSideType == LedSideTypes.TopRightToLeft) { captureZoneHor -= captureZoneSize; }
                    else if (ledSideType == LedSideTypes.BottomLeftToRight) { captureZoneHor += captureZoneSize; }
                    else if (ledSideType == LedSideTypes.BottomRightToLeft) { captureZoneHor -= captureZoneSize; }
                    else if (ledSideType == LedSideTypes.LeftBottomToTop) { captureZoneVer += captureZoneSize; }
                    else if (ledSideType == LedSideTypes.LeftTopToBottom) { captureZoneVer -= captureZoneSize; }
                    else if (ledSideType == LedSideTypes.RightBottomToTop) { captureZoneVer += captureZoneSize; }
                    else if (ledSideType == LedSideTypes.RightTopToBottom) { captureZoneVer -= captureZoneSize; }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to get colors for leds: " + ex.Message);
            }
        }

        //Capture screen color
        private static void CaptureScreenColorAlgorithm(byte[] bitmapByteArray, out ColorRGBA colorCapture, out bool colorFound, out int colorFirstRange, int captureZoneHor, int captureZoneVer, int captureZoneSize, int captureRangeSize, LedSideTypes ledSideType, int ledCurrentIndex)
        {
            colorCapture = ColorRGBA.Black;
            colorFound = false;
            colorFirstRange = 0;
            try
            {
                List<ColorRGBA> captureColors = [];
                int captureSizeStep = 0;
                int captureEvenStep = 1;
                int captureRangeStep = 0;
                int captureZoneHorRange = 0;
                int captureZoneVerRange = 0;
                bool captureInBlackbarRange = false;
                for (captureSizeStep = 0; captureSizeStep < captureZoneSize; captureSizeStep += 1)
                {
                    if (captureEvenStep == 1) { captureEvenStep = 0; } else { captureEvenStep = 1; }
                    for (captureRangeStep = 0; captureRangeStep < captureRangeSize; captureRangeStep += 2)
                    {
                        if (ledSideType == LedSideTypes.TopLeftToRight)
                        {
                            captureInBlackbarRange = captureRangeStep < vBlackbarRangesTop[ledCurrentIndex];
                            captureZoneHorRange = captureSizeStep;
                            captureZoneVerRange = -captureRangeStep - captureEvenStep;
                        }
                        else if (ledSideType == LedSideTypes.TopRightToLeft)
                        {
                            captureInBlackbarRange = captureRangeStep < vBlackbarRangesTop[ledCurrentIndex];
                            captureZoneHorRange = -captureSizeStep;
                            captureZoneVerRange = -captureRangeStep - captureEvenStep;
                        }
                        else if (ledSideType == LedSideTypes.BottomLeftToRight)
                        {
                            captureInBlackbarRange = captureRangeStep < vBlackbarRangesBottom[ledCurrentIndex];
                            captureZoneHorRange = captureSizeStep;
                            captureZoneVerRange = captureRangeStep + captureEvenStep;
                        }
                        else if (ledSideType == LedSideTypes.BottomRightToLeft)
                        {
                            captureInBlackbarRange = captureRangeStep < vBlackbarRangesBottom[ledCurrentIndex];
                            captureZoneHorRange = -captureSizeStep;
                            captureZoneVerRange = captureRangeStep + captureEvenStep;
                        }
                        else if (ledSideType == LedSideTypes.LeftBottomToTop)
                        {
                            captureInBlackbarRange = captureRangeStep < vBlackbarRangesLeft[ledCurrentIndex];
                            captureZoneHorRange = captureRangeStep + captureEvenStep;
                            captureZoneVerRange = captureSizeStep;
                        }
                        else if (ledSideType == LedSideTypes.LeftTopToBottom)
                        {
                            captureInBlackbarRange = captureRangeStep < vBlackbarRangesLeft[ledCurrentIndex];
                            captureZoneHorRange = captureRangeStep + captureEvenStep;
                            captureZoneVerRange = -captureSizeStep;
                        }
                        else if (ledSideType == LedSideTypes.RightBottomToTop)
                        {
                            captureInBlackbarRange = captureRangeStep < vBlackbarRangesRight[ledCurrentIndex];
                            captureZoneHorRange = -captureRangeStep - captureEvenStep;
                            captureZoneVerRange = captureSizeStep;
                        }
                        else if (ledSideType == LedSideTypes.RightTopToBottom)
                        {
                            captureInBlackbarRange = captureRangeStep < vBlackbarRangesRight[ledCurrentIndex];
                            captureZoneHorRange = -captureRangeStep - captureEvenStep;
                            captureZoneVerRange = -captureSizeStep;
                        }

                        ColorRGBA colorPixel = ScreenColorProcessing.GetPixelColor(bitmapByteArray, vCaptureDetails.OutputWidth, vCaptureDetails.OutputHeight, captureZoneHor + captureZoneHorRange, captureZoneVer + captureZoneVerRange);
                        if (colorPixel != null)
                        {
                            //Draw debug pixel colors
                            DebugDrawPixelColors(bitmapByteArray, captureZoneHor, captureZoneVer, ledSideType, captureZoneHorRange, captureZoneVerRange, captureInBlackbarRange);

                            //Check blackbar range and add captured pixel color
                            if (!captureInBlackbarRange)
                            {
                                captureColors.Add(colorPixel);
                            }

                            //Calculate color luminance
                            int colorLuminance = colorPixel.CalculateLuminance();
                            bool colorIsNotBlack = colorLuminance > setAdjustBlackBarBrightness;

                            //Check if color found and set first found range
                            if (!colorFound && colorIsNotBlack)
                            {
                                colorFound = true;
                                colorFirstRange = captureRangeStep;
                                //Debug.WriteLine("Blackbar color found at range: " + captureRangeStep);
                            }
                        }
                    }
                }

                //Merge captured pixel colors
                if (captureColors.Any())
                {
                    colorCapture = AdjustColorMerge.ColorMergeSqrt(captureColors);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to capture screen colors: " + ex.Message);
            }
        }
    }
}