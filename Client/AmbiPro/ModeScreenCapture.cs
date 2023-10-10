using ArnoldVinkCode;
using ScreenCapture;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using static AmbiPro.AppClasses;
using static AmbiPro.AppEnums;
using static AmbiPro.AppTasks;
using static AmbiPro.AppVariables;
using static AmbiPro.PreloadSettings;
using static ArnoldVinkCode.AVActions;
using static ArnoldVinkCode.AVClassConverters;
using static ArnoldVinkCode.AVSettings;

namespace AmbiPro
{
    partial class SerialMonitor
    {
        //Set capture settings variable
        private static void SetCaptureSettings()
        {
            try
            {
                //Load capture settings
                int captureMonitor = SettingLoad(vConfiguration, "MonitorCapture", typeof(int));
                float captureBlur = SettingLoad(vConfiguration, "CaptureBlur", typeof(float));
                float captureHDRPaperWhite = SettingLoad(vConfiguration, "CaptureHDRPaperWhite", typeof(float));
                float captureHDRMaximumNits = SettingLoad(vConfiguration, "CaptureHDRMaximumNits", typeof(float));

                //Calculate maximum pixel dimension
                int maximumPixelDimension = AVFunctions.MathMaxMulti(setLedCountFirst, setLedCountSecond, setLedCountThird, setLedCountFourth) * 2;
                if (maximumPixelDimension < 180) { maximumPixelDimension = 180; }
                Debug.WriteLine("Capture requested maximum pixel dimension: " + maximumPixelDimension);

                //Set capture settings variable
                vCaptureSettings = new CaptureSettings
                {
                    MonitorId = captureMonitor,
                    MaxPixelDimension = maximumPixelDimension,
                    MouseDrawCursor = false,
                    Blur = captureBlur,
                    HDRtoSDR = true,
                    HDRPaperWhite = captureHDRPaperWhite,
                    HDRMaximumNits = captureHDRMaximumNits
                };

                Debug.WriteLine("Set capture settings variable.");
            }
            catch { }
        }

        //Initialize Screen Capture
        private static async Task<bool> InitializeScreenCapture(int delayTime)
        {
            try
            {
                Debug.WriteLine("Initializing screen capture: " + DateTime.Now);

                //Set capture settings
                SetCaptureSettings();

                //Reset screen capture
                bool captureReset = CaptureImport.CaptureReset(0);

                //Initialize screen capture
                bool captureInitialized = CaptureImport.CaptureInitialize(0, vCaptureSettings, out vCaptureDetails);

                //Update capture variables
                UpdateCaptureVariables();

                //Update screen information
                DispatcherInvoke(delegate
                {
                    vFormSettings.UpdateScreenInformation();
                });

                return captureReset && captureInitialized;
            }
            catch
            {
                Debug.WriteLine("Failed to initialize screen capture.");
                return false;
            }
            finally
            {
                await Task.Delay(delayTime);
            }
        }

        public static void UpdateCaptureVariables()
        {
            try
            {
                //Set capture range
                vCaptureRangeVertical = (setLedCaptureRange * vCaptureDetails.OutputHeight) / 100 / 2;
                vCaptureRangeHorizontal = (setLedCaptureRange * vCaptureDetails.OutputWidth) / 100 / 2;
                //Debug.WriteLine("Capture range set to: V" + vCaptureRangeVertical + "/H" + vCaptureRangeHorizontal);

                //Set blackbar range
                vBlackbarRangeVertical = (setAdjustBlackbarRange * vCaptureDetails.OutputHeight) / 100;
                vBlackbarRangeHorizontal = (setAdjustBlackbarRange * vCaptureDetails.OutputWidth) / 100;
                //Debug.WriteLine("Blackbar range set to: V" + vBlackbarRangeVertical + "/H" + vBlackbarRangeHorizontal);

                ////Set blackbar step
                //vBlackbarAdjustStepVertical = (1 * vCaptureDetails.OutputHeight) / 100;
                //if (vBlackbarAdjustStepVertical < 1) { vBlackbarAdjustStepVertical = 1; }
                //vBlackbarAdjustStepHorizontal = (1 * vCaptureDetails.OutputWidth) / 100;
                //if (vBlackbarAdjustStepHorizontal < 1) { vBlackbarAdjustStepHorizontal = 1; }
                //Debug.WriteLine("Blackbar step set to: V" + vBlackbarAdjustStepVertical + "/H" + vBlackbarAdjustStepHorizontal);
            }
            catch { }
        }

        //Reset Screen Capture
        private static bool ResetScreenCapture()
        {
            try
            {
                Debug.WriteLine("Resetting screen capture: " + DateTime.Now);
                return CaptureImport.CaptureReset(0);
            }
            catch
            {
                Debug.WriteLine("Failed to reset screen capture.");
                return false;
            }
        }

        //Loop cature the screen
        private static async Task ModeScreenCapture(int initByteSize, int totalByteSize, byte[] serialBytes)
        {
            try
            {
                //Loop mode variables
                bool ConnectionFailed = false;

                //Initialize Screen Capturer
                if (!await InitializeScreenCapture(200))
                {
                    Debug.WriteLine("Failed to initialize the screen capturer.");
                    ShowFailedCaptureMessage();
                    return;
                }

                //Start updating the leds
                while (!vTask_UpdateLed.TaskStopRequested)
                {
                    int currentSerialByte = initByteSize;
                    IntPtr bitmapIntPtr = IntPtr.Zero;
                    try
                    {
                        //Capture screenshot
                        try
                        {
                            bitmapIntPtr = CaptureImport.CaptureScreenBytes(0);
                        }
                        catch { }

                        //Check screenshot
                        if (bitmapIntPtr == IntPtr.Zero)
                        {
                            Debug.WriteLine("Screenshot is corrupted, restarting capture.");
                            await InitializeScreenCapture(200);
                            continue;
                        }

                        //Convert BitmapIntPtr to BitmapByteArray
                        byte[] bitmapByteArray = CaptureBitmap.BitmapIntPtrToBitmapByteArray(bitmapIntPtr, vCaptureDetails);

                        //Check led capture sides color
                        ScreenColors(setLedSideFirst, setLedCountFirst, serialBytes, bitmapByteArray, ref currentSerialByte);
                        ScreenColors(setLedSideSecond, setLedCountSecond, serialBytes, bitmapByteArray, ref currentSerialByte);
                        ScreenColors(setLedSideThird, setLedCountThird, serialBytes, bitmapByteArray, ref currentSerialByte);
                        ScreenColors(setLedSideFourth, setLedCountFourth, serialBytes, bitmapByteArray, ref currentSerialByte);

                        //Update debug capture preview
                        if (vDebugCaptureAllowed)
                        {
                            UpdateCaptureDebugPreview(bitmapByteArray);
                        }

                        //Smooth led frame transition
                        if (setLedSmoothing > 0)
                        {
                            //Make copy of current color bytes
                            byte[] CaptureByteCurrent = CloneByteArray(serialBytes);

                            //Merge current colors with history
                            for (int ledCount = initByteSize; ledCount < (totalByteSize - initByteSize); ledCount++)
                            {
                                //Debug.WriteLine("Led smoothing old: " + SerialBytes[ledCount]);

                                int ColorCount = 1;
                                int ColorAverage = serialBytes[ledCount];
                                for (int smoothCount = 0; smoothCount < setLedSmoothing; smoothCount++)
                                {
                                    if (vCaptureByteHistoryArray[smoothCount] != null)
                                    {
                                        ColorAverage += vCaptureByteHistoryArray[smoothCount][ledCount];
                                        ColorCount++;
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }

                                serialBytes[ledCount] = (byte)(ColorAverage / ColorCount);
                                //Debug.WriteLine("Led smoothing new: " + SerialBytes[ledCount]);
                            }

                            //Update capture color bytes history
                            Array.Copy(vCaptureByteHistoryArray, 0, vCaptureByteHistoryArray, 1, vCaptureByteHistoryArray.Length - 1);
                            vCaptureByteHistoryArray[0] = CaptureByteCurrent;
                        }

                        //Rotate the leds as calibrated
                        if (setLedRotate > 0)
                        {
                            for (int RotateCount = 0; RotateCount < setLedRotate; RotateCount++)
                            {
                                AVFunctions.MoveByteInArrayLeft(totalByteSize, serialBytes, 3, totalByteSize - 1);
                                AVFunctions.MoveByteInArrayLeft(totalByteSize, serialBytes, 3, totalByteSize - 1);
                                AVFunctions.MoveByteInArrayLeft(totalByteSize, serialBytes, 3, totalByteSize - 1);
                            }
                        }
                        else if (setLedRotate < 0)
                        {
                            for (int RotateCount = 0; RotateCount < Math.Abs(setLedRotate); RotateCount++)
                            {
                                AVFunctions.MoveByteInArrayRight(totalByteSize, serialBytes, totalByteSize - 1, 3);
                                AVFunctions.MoveByteInArrayRight(totalByteSize, serialBytes, totalByteSize - 1, 3);
                                AVFunctions.MoveByteInArrayRight(totalByteSize, serialBytes, totalByteSize - 1, 3);
                            }
                        }

                        //Send the serial bytes to device
                        if (!SerialComPortWrite(totalByteSize, serialBytes))
                        {
                            ConnectionFailed = true;
                            break;
                        }

                        //Delay the loop task
                        await TaskDelay(setUpdateRate, vTask_UpdateLed);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("Screen capture loop failed: " + ex.Message);
                    }
                }

                //Show failed connection message
                if (ConnectionFailed)
                {
                    ShowFailedConnectionMessage();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Screen capture task failed: " + ex.Message);
            }
            finally
            {
                ResetScreenCapture();
            }
        }

        public static void UpdateCaptureSettings()
        {
            try
            {
                //Set capture settings
                SetCaptureSettings();

                //Update capture settings
                bool settingsUpdated = CaptureImport.CaptureUpdateSettings(0, vCaptureSettings);
                Debug.WriteLine("Capture settings updated: " + settingsUpdated);
            }
            catch { }
        }

        private static void UpdateCaptureDebugPreview(byte[] bitmapByteArray)
        {
            try
            {
                //Update debug screen capture preview
                DispatcherInvoke(delegate
                {
                    try
                    {
                        vFormSettings.image_DebugPreview.Source = CaptureBitmap.BitmapByteArrayToBitmapSource(bitmapByteArray, vCaptureDetails, vCaptureSettings);
                    }
                    catch { }
                });
            }
            catch { }
        }

        private static void UpdateLedColorsPreview(LedSideTypes ledSide, int currentLed, ColorRGBA colorRGBA)
        {
            try
            {
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
    }
}