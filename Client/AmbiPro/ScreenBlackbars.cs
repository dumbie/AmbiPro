using System;
using System.Diagnostics;
using static AmbiPro.AppEnums;
using static AmbiPro.AppVariables;
using static AmbiPro.PreloadSettings;

namespace AmbiPro
{
    public partial class SerialMonitor
    {
        //Get blackbar ranges
        private static int GetBlackbarRanges(LedSideTypes ledSideType, int ledCurrentIndex)
        {
            try
            {
                if (ledSideType == LedSideTypes.LeftBottomToTop || ledSideType == LedSideTypes.LeftTopToBottom)
                {
                    return vCaptureRangeHorizontal + vBlackbarRangesLeft[ledCurrentIndex];
                }
                else if (ledSideType == LedSideTypes.RightBottomToTop || ledSideType == LedSideTypes.RightTopToBottom)
                {
                    return vCaptureRangeHorizontal + vBlackbarRangesRight[ledCurrentIndex];
                }
                else if (ledSideType == LedSideTypes.TopLeftToRight || ledSideType == LedSideTypes.TopRightToLeft)
                {
                    return vCaptureRangeVertical + vBlackbarRangesTop[ledCurrentIndex];
                }
                else if (ledSideType == LedSideTypes.BottomLeftToRight || ledSideType == LedSideTypes.BottomRightToLeft)
                {
                    return vCaptureRangeVertical + vBlackbarRangesBottom[ledCurrentIndex];
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to get blackbar ranges: " + ex.Message);
                return 0;
            }
        }

        //Detect blackbar ranges
        private static void DetectBlackbarRanges(LedSideTypes ledSideType, int ledCurrentIndex, bool colorFound, int colorFirstRange)
        {
            try
            {
                //Check blackbar enabled setting
                if (!setAdjustBlackBars) { return; }

                //Check if color is detected
                if (colorFound)
                {
                    if (ledSideType == LedSideTypes.LeftBottomToTop || ledSideType == LedSideTypes.LeftTopToBottom)
                    {
                        int currentBlackbarRange = vBlackbarRangesLeft[ledCurrentIndex];
                        if (currentBlackbarRange < colorFirstRange && currentBlackbarRange < vBlackbarRangeHorizontal)
                        {
                            vBlackbarRangesLeft[ledCurrentIndex] += vBlackbarAdjustStepHorizontal;
                        }
                        else if (currentBlackbarRange > colorFirstRange)
                        {
                            vBlackbarRangesLeft[ledCurrentIndex] -= vBlackbarAdjustStepHorizontal;
                        }
                    }
                    else if (ledSideType == LedSideTypes.RightBottomToTop || ledSideType == LedSideTypes.RightTopToBottom)
                    {
                        int currentBlackbarRange = vBlackbarRangesRight[ledCurrentIndex];
                        if (currentBlackbarRange < colorFirstRange && currentBlackbarRange < vBlackbarRangeHorizontal)
                        {
                            vBlackbarRangesRight[ledCurrentIndex] += vBlackbarAdjustStepHorizontal;
                        }
                        else if (currentBlackbarRange > colorFirstRange)
                        {
                            vBlackbarRangesRight[ledCurrentIndex] -= vBlackbarAdjustStepHorizontal;
                        }
                    }
                    else if (ledSideType == LedSideTypes.TopLeftToRight || ledSideType == LedSideTypes.TopRightToLeft)
                    {
                        int currentBlackbarRange = vBlackbarRangesTop[ledCurrentIndex];
                        if (currentBlackbarRange < colorFirstRange && currentBlackbarRange < vBlackbarRangeVertical)
                        {
                            vBlackbarRangesTop[ledCurrentIndex] += vBlackbarAdjustStepVertical;
                        }
                        else if (currentBlackbarRange > colorFirstRange)
                        {
                            vBlackbarRangesTop[ledCurrentIndex] -= vBlackbarAdjustStepVertical;
                        }
                    }
                    else if (ledSideType == LedSideTypes.BottomLeftToRight || ledSideType == LedSideTypes.BottomRightToLeft)
                    {
                        int currentBlackbarRange = vBlackbarRangesBottom[ledCurrentIndex];
                        if (currentBlackbarRange < colorFirstRange && currentBlackbarRange < vBlackbarRangeVertical)
                        {
                            vBlackbarRangesBottom[ledCurrentIndex] += vBlackbarAdjustStepVertical;
                        }
                        else if (currentBlackbarRange > colorFirstRange)
                        {
                            vBlackbarRangesBottom[ledCurrentIndex] -= vBlackbarAdjustStepVertical;
                        }
                    }
                }
                else
                {
                    if (ledSideType == LedSideTypes.LeftBottomToTop || ledSideType == LedSideTypes.LeftTopToBottom)
                    {
                        int currentBlackbarRange = vBlackbarRangesLeft[ledCurrentIndex];
                        if (currentBlackbarRange < vBlackbarRangeHorizontal)
                        {
                            vBlackbarRangesLeft[ledCurrentIndex] += vBlackbarAdjustStepHorizontal;
                        }
                        else if (currentBlackbarRange > vBlackbarRangeHorizontal)
                        {
                            vBlackbarRangesLeft[ledCurrentIndex] -= vBlackbarAdjustStepHorizontal;
                        }
                    }
                    else if (ledSideType == LedSideTypes.RightBottomToTop || ledSideType == LedSideTypes.RightTopToBottom)
                    {
                        int currentBlackbarRange = vBlackbarRangesRight[ledCurrentIndex];
                        if (currentBlackbarRange < vBlackbarRangeHorizontal)
                        {
                            vBlackbarRangesRight[ledCurrentIndex] += vBlackbarAdjustStepHorizontal;
                        }
                        else if (currentBlackbarRange > vBlackbarRangeHorizontal)
                        {
                            vBlackbarRangesRight[ledCurrentIndex] -= vBlackbarAdjustStepHorizontal;
                        }
                    }
                    else if (ledSideType == LedSideTypes.TopLeftToRight || ledSideType == LedSideTypes.TopRightToLeft)
                    {
                        int currentBlackbarRange = vBlackbarRangesTop[ledCurrentIndex];
                        if (currentBlackbarRange < vBlackbarRangeVertical)
                        {
                            vBlackbarRangesTop[ledCurrentIndex] += vBlackbarAdjustStepVertical;
                        }
                        else if (currentBlackbarRange > vBlackbarRangeVertical)
                        {
                            vBlackbarRangesTop[ledCurrentIndex] -= vBlackbarAdjustStepVertical;
                        }
                    }
                    else if (ledSideType == LedSideTypes.BottomLeftToRight || ledSideType == LedSideTypes.BottomRightToLeft)
                    {
                        int currentBlackbarRange = vBlackbarRangesBottom[ledCurrentIndex];
                        if (currentBlackbarRange < vBlackbarRangeVertical)
                        {
                            vBlackbarRangesBottom[ledCurrentIndex] += vBlackbarAdjustStepVertical;
                        }
                        else if (currentBlackbarRange > vBlackbarRangeVertical)
                        {
                            vBlackbarRangesBottom[ledCurrentIndex] -= vBlackbarAdjustStepVertical;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to detect blackbar ranges: " + ex.Message);
            }
        }
    }
}