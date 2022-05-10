using AmbiPro.Resources;
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
        private static void ScreenColors(LedSideTypes sideType, int directionLedCount, byte[] serialBytes, byte[] bitmapByteArray, ref int currentSerialByte)
        {
            try
            {
                //Check the led side
                if (directionLedCount == 0 || sideType == LedSideTypes.None)
                {
                    return;
                }

                //Calculate led color direction and bottom gap
                int blackbarZoneHor = 0;
                int blackbarZoneVer = 0;
                int captureZoneHor = 0;
                int captureZoneVer = 0;
                int captureZoneSize = 0;
                int captureDiffMax = 0;
                int captureDiffMin = 0;
                int bottomGapMax = 0;
                int bottomGapMin = 0;
                if (sideType == LedSideTypes.TopLeftToRight)
                {
                    captureZoneHor = 0;
                    captureZoneVer = vCaptureDetails.Height;
                    captureZoneSize = vCaptureDetails.Width / directionLedCount;
                    captureDiffMax = (vCaptureDetails.Width - (captureZoneSize * directionLedCount)) / 2;
                    captureDiffMin = directionLedCount - captureDiffMax;
                }
                else if (sideType == LedSideTypes.TopRightToLeft)
                {
                    captureZoneHor = vCaptureDetails.Width;
                    captureZoneVer = vCaptureDetails.Height;
                    captureZoneSize = vCaptureDetails.Width / directionLedCount;
                    captureDiffMax = (vCaptureDetails.Width - (captureZoneSize * directionLedCount)) / 2;
                    captureDiffMin = directionLedCount - captureDiffMax;
                }
                else if (sideType == LedSideTypes.BottomLeftToRight)
                {
                    directionLedCount += setLedBottomGap;
                    captureZoneHor = 0;
                    captureZoneVer = 0;
                    captureZoneSize = vCaptureDetails.Width / directionLedCount;
                    captureDiffMax = (vCaptureDetails.Width - (captureZoneSize * directionLedCount)) / 2;
                    captureDiffMin = directionLedCount - captureDiffMax;
                    int BottomGapDiff = setLedBottomGap / 2;
                    int BottomGapTarget = directionLedCount / 2;
                    bottomGapMax = BottomGapTarget + BottomGapDiff;
                    bottomGapMin = BottomGapTarget - BottomGapDiff - 1;
                }
                else if (sideType == LedSideTypes.BottomRightToLeft)
                {
                    directionLedCount += setLedBottomGap;
                    captureZoneHor = vCaptureDetails.Width;
                    captureZoneVer = 0;
                    captureZoneSize = vCaptureDetails.Width / directionLedCount;
                    captureDiffMax = (vCaptureDetails.Width - (captureZoneSize * directionLedCount)) / 2;
                    captureDiffMin = directionLedCount - captureDiffMax;
                    int bottomGapDiff = setLedBottomGap / 2;
                    int bottomGapTarget = directionLedCount / 2;
                    bottomGapMax = bottomGapTarget + bottomGapDiff;
                    bottomGapMin = bottomGapTarget - bottomGapDiff - 1;
                }
                else if (sideType == LedSideTypes.LeftBottomToTop)
                {
                    captureZoneHor = 0;
                    captureZoneVer = 0;
                    captureZoneSize = vCaptureDetails.Height / directionLedCount;
                    captureDiffMax = (vCaptureDetails.Height - (captureZoneSize * directionLedCount)) / 2;
                    captureDiffMin = directionLedCount - captureDiffMax;
                }
                else if (sideType == LedSideTypes.LeftTopToBottom)
                {
                    captureZoneHor = 0;
                    captureZoneVer = vCaptureDetails.Height;
                    captureZoneSize = vCaptureDetails.Height / directionLedCount;
                    captureDiffMax = (vCaptureDetails.Height - (captureZoneSize * directionLedCount)) / 2;
                    captureDiffMin = directionLedCount - captureDiffMax;
                }
                else if (sideType == LedSideTypes.RightBottomToTop)
                {
                    captureZoneHor = vCaptureDetails.Width;
                    captureZoneVer = 0;
                    captureZoneSize = vCaptureDetails.Height / directionLedCount;
                    captureDiffMax = (vCaptureDetails.Height - (captureZoneSize * directionLedCount)) / 2;
                    captureDiffMin = directionLedCount - captureDiffMax;
                }
                else if (sideType == LedSideTypes.RightTopToBottom)
                {
                    captureZoneHor = vCaptureDetails.Width;
                    captureZoneVer = vCaptureDetails.Height;
                    captureZoneSize = vCaptureDetails.Height / directionLedCount;
                    captureDiffMax = (vCaptureDetails.Height - (captureZoneSize * directionLedCount)) / 2;
                    captureDiffMin = directionLedCount - captureDiffMax;
                }
                //Debug.WriteLine("Zone range: " + vCaptureZoneRange + " / Zone size: " + CaptureZoneSize + " / Zone difference: " + CaptureZoneDiff + " / Leds: " + DirectionLedCount);

                //Get colors from the bitmap
                for (int currentLed = 0; currentLed < directionLedCount; currentLed++)
                {
                    //Set led off color byte array
                    if (sideType == LedSideTypes.LedsOff)
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
                    if (bottomGapMax != 0 && AVFunctions.BetweenNumbers(currentLed, bottomGapMin, bottomGapMax, false))
                    {
                        //Debug.WriteLine("Led: " + currentLed + " / GapMin: " + BottomGapMin + " / GapMax: " + BottomGapMax + " / GapLeds: " + setLedBottomGap);
                    }
                    else
                    {
                        //Detect led blackbar margin
                        if (sideType == LedSideTypes.LeftBottomToTop || sideType == LedSideTypes.LeftTopToBottom)
                        {
                            if (vBlackbarRunUpdate)
                            {
                                int colorMargin = BlackbarColorAlgorithm(bitmapByteArray, captureZoneHor, captureZoneVer, captureZoneSize, vBlackbarRangeHorizontal, sideType);
                                UpdateBlackBarMargin(colorMargin, ref vBlackbarLedMarginLeft[currentLed]);
                            }
                            blackbarZoneHor = vBlackbarLedMarginLeft[currentLed];
                        }
                        else if (sideType == LedSideTypes.RightBottomToTop || sideType == LedSideTypes.RightTopToBottom)
                        {
                            if (vBlackbarRunUpdate)
                            {
                                int colorMargin = BlackbarColorAlgorithm(bitmapByteArray, captureZoneHor, captureZoneVer, captureZoneSize, vBlackbarRangeHorizontal, sideType);
                                UpdateBlackBarMargin(colorMargin, ref vBlackbarLedMarginRight[currentLed]);
                            }
                            blackbarZoneHor = -vBlackbarLedMarginRight[currentLed];
                        }
                        else if (sideType == LedSideTypes.TopLeftToRight || sideType == LedSideTypes.TopRightToLeft)
                        {
                            if (vBlackbarRunUpdate)
                            {
                                int colorMargin = BlackbarColorAlgorithm(bitmapByteArray, captureZoneHor, captureZoneVer, captureZoneSize, vBlackbarRangeVertical, sideType);
                                UpdateBlackBarMargin(colorMargin, ref vBlackbarLedMarginTop[currentLed]);
                            }
                            blackbarZoneVer = -vBlackbarLedMarginTop[currentLed];
                        }
                        else if (sideType == LedSideTypes.BottomLeftToRight || sideType == LedSideTypes.BottomRightToLeft)
                        {
                            if (vBlackbarRunUpdate)
                            {
                                int colorMargin = BlackbarColorAlgorithm(bitmapByteArray, captureZoneHor, captureZoneVer, captureZoneSize, vBlackbarRangeVertical, sideType);
                                UpdateBlackBarMargin(colorMargin, ref vBlackbarLedMarginBottom[currentLed]);
                            }
                            blackbarZoneVer = vBlackbarLedMarginBottom[currentLed];
                        }

                        //Capture the average color
                        CaptureColorAlgorithm(bitmapByteArray, out ColorRGBA captureColor, captureZoneHor + blackbarZoneHor, captureZoneVer + blackbarZoneVer, captureZoneSize, vCaptureRange, sideType);
                        //Debug.WriteLine("Captured average color R" + captureColor.R + "G" + captureColor.G + "B" + captureColor.B + " from the bitmap data.");

                        //Check if color is captured
                        if (captureColor == null)
                        {
                            captureColor = ColorRGBA.Black;
                        }

                        //Update debug led colors preview
                        if (vDebugCaptureAllowed && setDebugLedPreview)
                        {
                            UpdateLedColorsPreview(sideType, currentLed, captureColor);
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
                    if (captureDiffMax != 0 && AVFunctions.BetweenNumbersOr(currentLed, captureDiffMin, captureDiffMax, true))
                    {
                        if (sideType == LedSideTypes.TopLeftToRight) { captureZoneHor++; }
                        else if (sideType == LedSideTypes.TopRightToLeft) { captureZoneHor--; }
                        else if (sideType == LedSideTypes.BottomLeftToRight) { captureZoneHor++; }
                        else if (sideType == LedSideTypes.BottomRightToLeft) { captureZoneHor--; }
                        else if (sideType == LedSideTypes.LeftBottomToTop) { captureZoneVer++; }
                        else if (sideType == LedSideTypes.LeftTopToBottom) { captureZoneVer--; }
                        else if (sideType == LedSideTypes.RightBottomToTop) { captureZoneVer++; }
                        else if (sideType == LedSideTypes.RightTopToBottom) { captureZoneVer--; }
                    }

                    //Skip to the next capture zone
                    if (sideType == LedSideTypes.TopLeftToRight) { captureZoneHor += captureZoneSize; }
                    else if (sideType == LedSideTypes.TopRightToLeft) { captureZoneHor -= captureZoneSize; }
                    else if (sideType == LedSideTypes.BottomLeftToRight) { captureZoneHor += captureZoneSize; }
                    else if (sideType == LedSideTypes.BottomRightToLeft) { captureZoneHor -= captureZoneSize; }
                    else if (sideType == LedSideTypes.LeftBottomToTop) { captureZoneVer += captureZoneSize; }
                    else if (sideType == LedSideTypes.LeftTopToBottom) { captureZoneVer -= captureZoneSize; }
                    else if (sideType == LedSideTypes.RightBottomToTop) { captureZoneVer += captureZoneSize; }
                    else if (sideType == LedSideTypes.RightTopToBottom) { captureZoneVer -= captureZoneSize; }
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
            int captureRangeStep = 0;
            try
            {
                int CaptureZoneHorRange = 0;
                int CaptureZoneVerRange = 0;
                for (captureRangeStep = 0; captureRangeStep < captureZoneRange; captureRangeStep += vBlackbarDetectAccuracy)
                {
                    for (int captureSizeStep = 0; captureSizeStep < captureZoneSize; captureSizeStep += vBlackbarDetectAccuracy)
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

                        ColorRGBA ColorPixel = ColorProcessing.GetPixelColor(bitmapByteArray, vCaptureDetails.Width, vCaptureDetails.Height, captureZoneHor + CaptureZoneHorRange, captureZoneVer + CaptureZoneVerRange);
                        if (ColorPixel != null)
                        {
                            if (vDebugCaptureAllowed && setDebugBlackBar)
                            {
                                ColorProcessing.SetPixelColor(bitmapByteArray, vCaptureDetails.Width, vCaptureDetails.Height, captureZoneHor + CaptureZoneHorRange, captureZoneVer + CaptureZoneVerRange, ColorRGBA.Orange);
                            }

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

        //Capture color
        private static void CaptureColorAlgorithm(byte[] bitmapByteArray, out ColorRGBA averageColor, int captureZoneHor, int captureZoneVer, int captureZoneSize, int captureRangeSize, LedSideTypes sideType)
        {
            try
            {
                int ColorCount = 0;
                int ColorAverageR = 0;
                int ColorAverageG = 0;
                int ColorAverageB = 0;
                int CaptureEvenStep = 1;
                int CaptureZoneHorRange = 0;
                int CaptureZoneVerRange = 0;
                for (int captureSizeStep = 0; captureSizeStep < captureZoneSize; captureSizeStep += 1)
                {
                    if (CaptureEvenStep == 1) { CaptureEvenStep = 0; } else { CaptureEvenStep = 1; }
                    for (int captureRangeStep = 0; captureRangeStep < captureRangeSize; captureRangeStep += 2)
                    {
                        if (sideType == LedSideTypes.TopLeftToRight)
                        {
                            CaptureZoneHorRange = captureSizeStep;
                            CaptureZoneVerRange = -captureRangeStep - CaptureEvenStep;
                        }
                        else if (sideType == LedSideTypes.TopRightToLeft)
                        {
                            CaptureZoneHorRange = -captureSizeStep;
                            CaptureZoneVerRange = -captureRangeStep - CaptureEvenStep;
                        }
                        else if (sideType == LedSideTypes.BottomLeftToRight)
                        {
                            CaptureZoneHorRange = captureSizeStep;
                            CaptureZoneVerRange = captureRangeStep + CaptureEvenStep;
                        }
                        else if (sideType == LedSideTypes.BottomRightToLeft)
                        {
                            CaptureZoneHorRange = -captureSizeStep;
                            CaptureZoneVerRange = captureRangeStep + CaptureEvenStep;
                        }
                        else if (sideType == LedSideTypes.LeftBottomToTop)
                        {
                            CaptureZoneHorRange = captureRangeStep + CaptureEvenStep;
                            CaptureZoneVerRange = captureSizeStep;
                        }
                        else if (sideType == LedSideTypes.LeftTopToBottom)
                        {
                            CaptureZoneHorRange = captureRangeStep + CaptureEvenStep;
                            CaptureZoneVerRange = -captureSizeStep;
                        }
                        else if (sideType == LedSideTypes.RightBottomToTop)
                        {
                            CaptureZoneHorRange = -captureRangeStep - CaptureEvenStep;
                            CaptureZoneVerRange = captureSizeStep;
                        }
                        else if (sideType == LedSideTypes.RightTopToBottom)
                        {
                            CaptureZoneHorRange = -captureRangeStep - CaptureEvenStep;
                            CaptureZoneVerRange = -captureSizeStep;
                        }

                        ColorRGBA ColorPixel = ColorProcessing.GetPixelColor(bitmapByteArray, vCaptureDetails.Width, vCaptureDetails.Height, captureZoneHor + CaptureZoneHorRange, captureZoneVer + CaptureZoneVerRange);
                        if (ColorPixel != null)
                        {
                            if (vDebugCaptureAllowed && setDebugColor)
                            {
                                ColorProcessing.SetPixelColor(bitmapByteArray, vCaptureDetails.Width, vCaptureDetails.Height, captureZoneHor + CaptureZoneHorRange, captureZoneVer + CaptureZoneVerRange, ColorRGBA.Purple);
                            }

                            //Add pixel color to average color
                            ColorAverageR += ColorPixel.R;
                            ColorAverageG += ColorPixel.G;
                            ColorAverageB += ColorPixel.B;
                            ColorCount++;
                        }
                    }
                }

                //Calculate the average color
                averageColor = new ColorRGBA()
                {
                    R = (byte)(ColorAverageR / ColorCount),
                    G = (byte)(ColorAverageG / ColorCount),
                    B = (byte)(ColorAverageB / ColorCount)
                };
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to capture colors: " + ex.Message);
                averageColor = null;
            }
        }
    }
}