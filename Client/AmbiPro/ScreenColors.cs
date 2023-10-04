using ArnoldVinkCode;
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
        //Get screen shot colors and set it to byte array
        private static void ScreenColors(LedSideTypes ledSideType, int directionLedCount, byte[] serialBytes, byte[] bitmapByteArray, ref int currentSerialByte)
        {
            try
            {
                //Check the led side
                if (directionLedCount == 0 || ledSideType == LedSideTypes.None)
                {
                    return;
                }

                //Calculate led color direction and bottom gap
                int captureRange = 0;
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
                    captureZoneHor = vCaptureDetails.OutputWidth;
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
                    captureZoneHor = vCaptureDetails.OutputWidth;
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
                    captureZoneVer = 0;
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
                    captureZoneVer = 0;
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
                //Debug.WriteLine("Zone range: " + vCaptureZoneRange + " / Zone size: " + CaptureZoneSize + " / Zone difference: " + CaptureZoneDiff + " / Leds: " + DirectionLedCount);

                //Get colors from the bitmap
                for (int ledCurrentIndex = 0; ledCurrentIndex < directionLedCount; ledCurrentIndex++)
                {
                    //Set led off color byte array
                    if (ledSideType == LedSideTypes.LedsOff)
                    {
                        serialBytes[currentSerialByte] = 0;
                        currentSerialByte++;

                        serialBytes[currentSerialByte] = 0;
                        currentSerialByte++;

                        serialBytes[currentSerialByte] = 0;
                        currentSerialByte++;
                        continue;
                    }

                    //Skip bottom gap leds
                    if (bottomGapMax != 0 && AVFunctions.BetweenNumbers(ledCurrentIndex, bottomGapMin, bottomGapMax, false))
                    {
                        //Debug.WriteLine("Led: " + currentLed + " / GapMin: " + BottomGapMin + " / GapMax: " + BottomGapMax + " / GapLeds: " + setLedBottomGap);
                    }
                    else
                    {
                        if (ledSideType == LedSideTypes.LeftBottomToTop || ledSideType == LedSideTypes.LeftTopToBottom)
                        {
                            //Detect led blackbar range
                            if (vBlackbarRunUpdate)
                            {
                                int colorMargin = BlackbarColorAlgorithm(bitmapByteArray, captureZoneHor, captureZoneVer, captureZoneSize, vBlackbarRangeHorizontal, ledSideType);
                                UpdateBlackBarMargin(colorMargin, ref vBlackbarRangesLeft[ledCurrentIndex]);
                            }

                            //Set capture range
                            captureRange = vCaptureRange + vBlackbarRangesLeft[ledCurrentIndex];
                        }
                        else if (ledSideType == LedSideTypes.RightBottomToTop || ledSideType == LedSideTypes.RightTopToBottom)
                        {
                            //Detect led blackbar range
                            if (vBlackbarRunUpdate)
                            {
                                int colorMargin = BlackbarColorAlgorithm(bitmapByteArray, captureZoneHor, captureZoneVer, captureZoneSize, vBlackbarRangeHorizontal, ledSideType);
                                UpdateBlackBarMargin(colorMargin, ref vBlackbarRangesRight[ledCurrentIndex]);
                            }

                            //Set capture range
                            captureRange = vCaptureRange + vBlackbarRangesRight[ledCurrentIndex];
                        }
                        else if (ledSideType == LedSideTypes.TopLeftToRight || ledSideType == LedSideTypes.TopRightToLeft)
                        {
                            //Detect led blackbar range
                            if (vBlackbarRunUpdate)
                            {
                                int colorMargin = BlackbarColorAlgorithm(bitmapByteArray, captureZoneHor, captureZoneVer, captureZoneSize, vBlackbarRangeVertical, ledSideType);
                                UpdateBlackBarMargin(colorMargin, ref vBlackbarRangesTop[ledCurrentIndex]);
                            }

                            //Set capture range
                            captureRange = vCaptureRange + vBlackbarRangesTop[ledCurrentIndex];
                        }
                        else if (ledSideType == LedSideTypes.BottomLeftToRight || ledSideType == LedSideTypes.BottomRightToLeft)
                        {
                            //Detect led blackbar range
                            if (vBlackbarRunUpdate)
                            {
                                int colorMargin = BlackbarColorAlgorithm(bitmapByteArray, captureZoneHor, captureZoneVer, captureZoneSize, vBlackbarRangeVertical, ledSideType);
                                UpdateBlackBarMargin(colorMargin, ref vBlackbarRangesBottom[ledCurrentIndex]);
                            }

                            //Set capture range
                            captureRange = vCaptureRange + vBlackbarRangesBottom[ledCurrentIndex];
                        }

                        //Capture the average color
                        CaptureScreenColorAlgorithm(bitmapByteArray, out ColorRGBA captureColor, captureZoneHor, captureZoneVer, captureZoneSize, captureRange, ledSideType, ledCurrentIndex);
                        //Debug.WriteLine("Captured average color R" + captureColor.R + "G" + captureColor.G + "B" + captureColor.B + " from the bitmap data.");

                        //Update debug led colors preview
                        if (vDebugCaptureAllowed && setDebugLedPreview)
                        {
                            UpdateLedColorsPreview(ledSideType, ledCurrentIndex, captureColor);
                        }

                        //Adjust the colors to settings
                        AdjustLedColors(ref captureColor);

                        //Set the color to color byte array
                        serialBytes[currentSerialByte] = captureColor.R;
                        currentSerialByte++;

                        serialBytes[currentSerialByte] = captureColor.G;
                        currentSerialByte++;

                        serialBytes[currentSerialByte] = captureColor.B;
                        currentSerialByte++;
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

        //Check color for blackbars
        private static int BlackbarColorAlgorithm(byte[] bitmapByteArray, int captureZoneHor, int captureZoneVer, int captureZoneSize, int captureZoneRange, LedSideTypes sideType)
        {
            int captureSizeStep = 0;
            int captureRangeStep = 0;
            try
            {
                int CaptureZoneHorRange = 0;
                int CaptureZoneVerRange = 0;
                for (captureRangeStep = 0; captureRangeStep < captureZoneRange; captureRangeStep += vBlackbarDetectAccuracy)
                {
                    for (captureSizeStep = 0; captureSizeStep < captureZoneSize; captureSizeStep += vBlackbarDetectAccuracy)
                    {
                        if (sideType == LedSideTypes.TopLeftToRight)
                        {
                            CaptureZoneHorRange = captureSizeStep;
                            CaptureZoneVerRange = -captureRangeStep;
                        }
                        else if (sideType == LedSideTypes.TopRightToLeft)
                        {
                            CaptureZoneHorRange = -captureSizeStep;
                            CaptureZoneVerRange = -captureRangeStep;
                        }
                        else if (sideType == LedSideTypes.BottomLeftToRight)
                        {
                            CaptureZoneHorRange = captureSizeStep;
                            CaptureZoneVerRange = captureRangeStep;
                        }
                        else if (sideType == LedSideTypes.BottomRightToLeft)
                        {
                            CaptureZoneHorRange = -captureSizeStep;
                            CaptureZoneVerRange = captureRangeStep;
                        }
                        else if (sideType == LedSideTypes.LeftBottomToTop)
                        {
                            CaptureZoneHorRange = captureRangeStep;
                            CaptureZoneVerRange = captureSizeStep;
                        }
                        else if (sideType == LedSideTypes.LeftTopToBottom)
                        {
                            CaptureZoneHorRange = captureRangeStep;
                            CaptureZoneVerRange = -captureSizeStep;
                        }
                        else if (sideType == LedSideTypes.RightBottomToTop)
                        {
                            CaptureZoneHorRange = -captureRangeStep;
                            CaptureZoneVerRange = captureSizeStep;
                        }
                        else if (sideType == LedSideTypes.RightTopToBottom)
                        {
                            CaptureZoneHorRange = -captureRangeStep;
                            CaptureZoneVerRange = -captureSizeStep;
                        }

                        ColorRGBA ColorPixel = ScreenColorProcessing.GetPixelColor(bitmapByteArray, vCaptureDetails.OutputWidth, vCaptureDetails.OutputHeight, captureZoneHor + CaptureZoneHorRange, captureZoneVer + CaptureZoneVerRange);
                        if (ColorPixel != null)
                        {
                            //Calculate color luminance
                            int colorLuminance = (ColorPixel.R + ColorPixel.G + ColorPixel.B) / 3;

                            //Check if color is detected
                            if (colorLuminance > setAdjustBlackBarBrightness)
                            {
                                //Debug.WriteLine("Color detected at: " + captureRangeStep);
                                return captureRangeStep;
                            }
                        }
                    }
                }
                return captureRangeStep;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to check colors for blackbar: " + ex.Message);
                return captureRangeStep;
            }
        }

        //Capture screen color
        private static void CaptureScreenColorAlgorithm(byte[] bitmapByteArray, out ColorRGBA averageColor, int captureZoneHor, int captureZoneVer, int captureZoneSize, int captureRangeSize, LedSideTypes ledSideType, int ledCurrentIndex)
        {
            try
            {
                int colorCount = 0;
                int colorAverageR = 0;
                int colorAverageG = 0;
                int colorAverageB = 0;
                int captureSizeStep = 0;
                int captureEvenStep = 1;
                int captureRangeStep = 0;
                int captureZoneHorRange = 0;
                int captureZoneVerRange = 0;
                for (captureSizeStep = 0; captureSizeStep < captureZoneSize; captureSizeStep += 1)
                {
                    if (captureEvenStep == 1) { captureEvenStep = 0; } else { captureEvenStep = 1; }
                    for (captureRangeStep = 0; captureRangeStep < captureRangeSize; captureRangeStep += 2)
                    {
                        if (ledSideType == LedSideTypes.TopLeftToRight)
                        {
                            captureZoneHorRange = captureSizeStep;
                            captureZoneVerRange = -captureRangeStep - captureEvenStep;
                        }
                        else if (ledSideType == LedSideTypes.TopRightToLeft)
                        {
                            captureZoneHorRange = -captureSizeStep;
                            captureZoneVerRange = -captureRangeStep - captureEvenStep;
                        }
                        else if (ledSideType == LedSideTypes.BottomLeftToRight)
                        {
                            captureZoneHorRange = captureSizeStep;
                            captureZoneVerRange = captureRangeStep + captureEvenStep;
                        }
                        else if (ledSideType == LedSideTypes.BottomRightToLeft)
                        {
                            captureZoneHorRange = -captureSizeStep;
                            captureZoneVerRange = captureRangeStep + captureEvenStep;
                        }
                        else if (ledSideType == LedSideTypes.LeftBottomToTop)
                        {
                            captureZoneHorRange = captureRangeStep + captureEvenStep;
                            captureZoneVerRange = captureSizeStep;
                        }
                        else if (ledSideType == LedSideTypes.LeftTopToBottom)
                        {
                            captureZoneHorRange = captureRangeStep + captureEvenStep;
                            captureZoneVerRange = -captureSizeStep;
                        }
                        else if (ledSideType == LedSideTypes.RightBottomToTop)
                        {
                            captureZoneHorRange = -captureRangeStep - captureEvenStep;
                            captureZoneVerRange = captureSizeStep;
                        }
                        else if (ledSideType == LedSideTypes.RightTopToBottom)
                        {
                            captureZoneHorRange = -captureRangeStep - captureEvenStep;
                            captureZoneVerRange = -captureSizeStep;
                        }

                        ColorRGBA ColorPixel = ScreenColorProcessing.GetPixelColor(bitmapByteArray, vCaptureDetails.OutputWidth, vCaptureDetails.OutputHeight, captureZoneHor + captureZoneHorRange, captureZoneVer + captureZoneVerRange);
                        if (ColorPixel != null)
                        {
                            if (vDebugCaptureAllowed && setDebugColor)
                            {
                                ColorRGBA debugSideColor = ColorRGBA.White;
                                if (ledSideType == LedSideTypes.TopLeftToRight || ledSideType == LedSideTypes.TopRightToLeft)
                                {
                                    debugSideColor = ColorRGBA.Blue;
                                }
                                else if (ledSideType == LedSideTypes.BottomLeftToRight || ledSideType == LedSideTypes.BottomRightToLeft)
                                {
                                    debugSideColor = ColorRGBA.Yellow;
                                }
                                else if (ledSideType == LedSideTypes.LeftBottomToTop || ledSideType == LedSideTypes.LeftTopToBottom)
                                {
                                    debugSideColor = ColorRGBA.Red;
                                }
                                else if (ledSideType == LedSideTypes.RightBottomToTop || ledSideType == LedSideTypes.RightTopToBottom)
                                {
                                    debugSideColor = ColorRGBA.Green;
                                }
                                ScreenColorProcessing.SetPixelColor(bitmapByteArray, vCaptureDetails.OutputWidth, vCaptureDetails.OutputHeight, captureZoneHor + captureZoneHorRange, captureZoneVer + captureZoneVerRange, debugSideColor);
                            }

                            //Add pixel color to average color
                            colorAverageR += ColorPixel.R;
                            colorAverageG += ColorPixel.G;
                            colorAverageB += ColorPixel.B;
                            colorCount++;
                        }
                    }
                }

                //Calculate the average color
                averageColor = new ColorRGBA()
                {
                    R = (byte)(colorAverageR / colorCount),
                    G = (byte)(colorAverageG / colorCount),
                    B = (byte)(colorAverageB / colorCount)
                };
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to capture screen colors: " + ex.Message);
                averageColor = ColorRGBA.Black;
            }
        }
    }
}