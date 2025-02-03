using ScreenCaptureImport;
using System;
using static AmbiPro.AppClasses;
using static AmbiPro.AppEnums;
using static AmbiPro.AppVariables;
using static AmbiPro.PreloadSettings;
using static ArnoldVinkCode.AVActions;

namespace AmbiPro
{
    public partial class SerialMonitor
    {
        //Update debug screen capture preview
        private static void DebugUpdateCapturePreview(byte[] bitmapByteArray)
        {
            try
            {
                if (!vDebugCaptureAllowed)
                {
                    return;
                }

                DispatcherInvoke(delegate
                {
                    //Update framerate
                    int currentTickCount = Environment.TickCount;
                    int currentLatencyMs = currentTickCount - vCaptureTickCount;
                    if (currentLatencyMs > 0)
                    {
                        int currentFramesPerSecond = Convert.ToInt32(1000 / currentLatencyMs);
                        vFormSettings.textblock_DebugFramerate.Text = "Capture framerate: " + currentFramesPerSecond + "FPS (" + currentLatencyMs + "ms)";
                    }
                    vCaptureTickCount = currentTickCount;

                    //Update preview image
                    vFormSettings.image_DebugPreview.Source = CaptureBitmap.BitmapByteArrayToBitmapSource(bitmapByteArray, vCaptureDetails);
                });
            }
            catch { }
        }

        //Update debug led strip preview
        private static void DebugUpdateLedStripPreview(LedSideTypes ledSide, int currentLed, ColorRGBA colorRGBA)
        {
            try
            {
                if (!vDebugCaptureAllowed && !setDebugLedPreview)
                {
                    return;
                }

                DispatcherInvoke(delegate
                {
                    if (ledSide == LedSideTypes.LeftBottomToTop)
                    {
                        int countLed = vFormSettings.listbox_LedPreviewLeft.Items.Count - 1;
                        vFormSettings.listbox_LedPreviewLeft.Items[countLed - currentLed] = colorRGBA.AsSolidColorBrush();
                    }
                    else if (ledSide == LedSideTypes.LeftTopToBottom)
                    {
                        int countLed = vFormSettings.listbox_LedPreviewLeft.Items.Count - 1;
                        vFormSettings.listbox_LedPreviewLeft.Items[currentLed] = colorRGBA.AsSolidColorBrush();
                    }
                    else if (ledSide == LedSideTypes.TopRightToLeft)
                    {
                        int countLed = vFormSettings.listbox_LedPreviewTop.Items.Count - 1;
                        vFormSettings.listbox_LedPreviewTop.Items[countLed - currentLed] = colorRGBA.AsSolidColorBrush();
                    }
                    else if (ledSide == LedSideTypes.TopLeftToRight)
                    {
                        int countLed = vFormSettings.listbox_LedPreviewTop.Items.Count - 1;
                        vFormSettings.listbox_LedPreviewTop.Items[currentLed] = colorRGBA.AsSolidColorBrush();
                    }
                    else if (ledSide == LedSideTypes.RightBottomToTop)
                    {
                        int countLed = vFormSettings.listbox_LedPreviewRight.Items.Count - 1;
                        vFormSettings.listbox_LedPreviewRight.Items[countLed - currentLed] = colorRGBA.AsSolidColorBrush();
                    }
                    else if (ledSide == LedSideTypes.RightTopToBottom)
                    {
                        int countLed = vFormSettings.listbox_LedPreviewRight.Items.Count - 1;
                        vFormSettings.listbox_LedPreviewRight.Items[currentLed] = colorRGBA.AsSolidColorBrush();
                    }
                    else if (ledSide == LedSideTypes.BottomRightToLeft)
                    {
                        int countLed = vFormSettings.listbox_LedPreviewBottom.Items.Count - 1;
                        vFormSettings.listbox_LedPreviewBottom.Items[countLed - currentLed] = colorRGBA.AsSolidColorBrush();
                    }
                    else if (ledSide == LedSideTypes.BottomLeftToRight)
                    {
                        int countLed = vFormSettings.listbox_LedPreviewBottom.Items.Count - 1;
                        vFormSettings.listbox_LedPreviewBottom.Items[currentLed] = colorRGBA.AsSolidColorBrush();
                    }
                });
            }
            catch { }
        }

        //Draw debug pixel colors
        private static void DebugDrawPixelColors(byte[] bitmapByteArray, int captureZoneHor, int captureZoneVer, LedSideTypes ledSideType, int captureZoneHorRange, int captureZoneVerRange, bool captureInBlackbarRange)
        {
            try
            {
                if (!vDebugCaptureAllowed)
                {
                    return;
                }

                if (setDebugColorLeftRight)
                {
                    if (ledSideType == LedSideTypes.LeftBottomToTop || ledSideType == LedSideTypes.LeftTopToBottom)
                    {
                        if (captureInBlackbarRange)
                        {
                            ScreenColorProcessing.SetPixelColor(bitmapByteArray, vCaptureDetails.OutputWidth, vCaptureDetails.OutputHeight, captureZoneHor + captureZoneHorRange, captureZoneVer + captureZoneVerRange, ColorRGBA.Purple);
                        }
                        else
                        {
                            ScreenColorProcessing.SetPixelColor(bitmapByteArray, vCaptureDetails.OutputWidth, vCaptureDetails.OutputHeight, captureZoneHor + captureZoneHorRange, captureZoneVer + captureZoneVerRange, ColorRGBA.Red);
                        }
                    }
                    if (ledSideType == LedSideTypes.RightBottomToTop || ledSideType == LedSideTypes.RightTopToBottom)
                    {
                        if (captureInBlackbarRange)
                        {
                            ScreenColorProcessing.SetPixelColor(bitmapByteArray, vCaptureDetails.OutputWidth, vCaptureDetails.OutputHeight, captureZoneHor + captureZoneHorRange, captureZoneVer + captureZoneVerRange, ColorRGBA.Purple);
                        }
                        else
                        {
                            ScreenColorProcessing.SetPixelColor(bitmapByteArray, vCaptureDetails.OutputWidth, vCaptureDetails.OutputHeight, captureZoneHor + captureZoneHorRange, captureZoneVer + captureZoneVerRange, ColorRGBA.Green);
                        }
                    }
                }
                if (setDebugColorTopBottom)
                {
                    if (ledSideType == LedSideTypes.TopLeftToRight || ledSideType == LedSideTypes.TopRightToLeft)
                    {
                        if (captureInBlackbarRange)
                        {
                            ScreenColorProcessing.SetPixelColor(bitmapByteArray, vCaptureDetails.OutputWidth, vCaptureDetails.OutputHeight, captureZoneHor + captureZoneHorRange, captureZoneVer + captureZoneVerRange, ColorRGBA.Purple);
                        }
                        else
                        {
                            ScreenColorProcessing.SetPixelColor(bitmapByteArray, vCaptureDetails.OutputWidth, vCaptureDetails.OutputHeight, captureZoneHor + captureZoneHorRange, captureZoneVer + captureZoneVerRange, ColorRGBA.Blue);
                        }
                    }
                    if (ledSideType == LedSideTypes.BottomLeftToRight || ledSideType == LedSideTypes.BottomRightToLeft)
                    {
                        if (captureInBlackbarRange)
                        {
                            ScreenColorProcessing.SetPixelColor(bitmapByteArray, vCaptureDetails.OutputWidth, vCaptureDetails.OutputHeight, captureZoneHor + captureZoneHorRange, captureZoneVer + captureZoneVerRange, ColorRGBA.Purple);
                        }
                        else
                        {
                            ScreenColorProcessing.SetPixelColor(bitmapByteArray, vCaptureDetails.OutputWidth, vCaptureDetails.OutputHeight, captureZoneHor + captureZoneHorRange, captureZoneVer + captureZoneVerRange, ColorRGBA.Yellow);
                        }
                    }
                }
            }
            catch { }
        }
    }
}