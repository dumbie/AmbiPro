using ArnoldVinkCode;
using ScreenCapture;
using System;
using System.Configuration;
using System.Diagnostics;
using System.Threading.Tasks;
using static AmbiPro.AppClasses;
using static AmbiPro.AppEnums;
using static AmbiPro.AppTasks;
using static AmbiPro.AppVariables;
using static AmbiPro.PreloadSettings;
using static ArnoldVinkCode.AVActions;
using static ArnoldVinkCode.AVClassConverters;

namespace AmbiPro
{
    partial class SerialMonitor
    {
        //Initialize Screen Capture
        private static async Task<bool> InitializeScreenCapture(int delayTime)
        {
            try
            {
                Debug.WriteLine("Initializing screen capture: " + DateTime.Now);
                int captureMonitor = Convert.ToInt32(ConfigurationManager.AppSettings["MonitorCapture"]);

                //Set capture settings
                vCaptureSettings = new CaptureSettings
                {
                    MonitorId = 0,
                    MaxPixelDimension = 480
                };

                //Initialize screen capture
                bool captureInitialized = CaptureImport.CaptureInitialize(vCaptureSettings, out vCaptureDetails);

                //Update capture variables
                UpdateCaptureVariables();

                //Update screen information
                ActionDispatcherInvoke(delegate
                {
                    App.vFormSettings.UpdateScreenInformation();
                });

                return captureInitialized;
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
                if (vCaptureDetails.Height < vCaptureDetails.Width)
                {
                    vCaptureRange = (setLedCaptureRange * vCaptureDetails.Height) / 100 / 2;
                }
                else
                {
                    vCaptureRange = (setLedCaptureRange * vCaptureDetails.Width) / 100 / 2;
                }
                //Debug.WriteLine("Capture range set to: " + vCaptureRange);

                //Set blackbar range
                vBlackbarRangeVertical = (setAdjustBlackbarRange * vCaptureDetails.Height) / 100;
                vBlackbarRangeHorizontal = (setAdjustBlackbarRange * vCaptureDetails.Width) / 100;
                //Debug.WriteLine("Blackbar range set to: V" + vBlackBarRangeVertical + "/H" + vBlackBarRangeHorizontal);
            }
            catch { }
        }

        //Reset Screen Capture
        private static bool ResetScreenCapture()
        {
            try
            {
                Debug.WriteLine("Resetting screen capture: " + DateTime.Now);
                return CaptureImport.CaptureReset();
            }
            catch
            {
                Debug.WriteLine("Failed to reset screen capture.");
                return false;
            }
        }

        //Loop cature the screen
        private static async Task ModeScreenCapture(int initByteSize, byte[] serialBytes)
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
                while (!vTask_UpdateLed.TaskStopRequest)
                {
                    int currentSerialByte = initByteSize;
                    IntPtr bitmapIntPtr = IntPtr.Zero;
                    try
                    {
                        //Capture screenshot
                        try
                        {
                            bitmapIntPtr = CaptureImport.CaptureScreenshot();
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

                        //Adjust the black bars range
                        if (setAdjustBlackBars && (Environment.TickCount - vBlackbarLastUpdate) > 100)
                        {
                            AdjustBlackBars(setLedSideFirst, bitmapByteArray);
                            AdjustBlackBars(setLedSideSecond, bitmapByteArray);
                            AdjustBlackBars(setLedSideThird, bitmapByteArray);
                            AdjustBlackBars(setLedSideFourth, bitmapByteArray);
                            vBlackbarLastUpdate = Environment.TickCount;
                        }

                        //Check led capture sides color
                        ScreenColors(setLedSideFirst, setLedCountFirst, serialBytes, bitmapByteArray, ref currentSerialByte);
                        ScreenColors(setLedSideSecond, setLedCountSecond, serialBytes, bitmapByteArray, ref currentSerialByte);
                        ScreenColors(setLedSideThird, setLedCountThird, serialBytes, bitmapByteArray, ref currentSerialByte);
                        ScreenColors(setLedSideFourth, setLedCountFourth, serialBytes, bitmapByteArray, ref currentSerialByte);

                        //Update debug capture preview
                        if (vDebugCaptureAllowed)
                        {
                            UpdateCaptureDebugPreview(bitmapIntPtr, bitmapByteArray);
                        }

                        //Smooth led frame transition
                        if (setLedSmoothing > 0)
                        {
                            //Make copy of current color bytes
                            byte[] CaptureByteCurrent = CloneByteArray(serialBytes);

                            //Merge current colors with history
                            for (int ledCount = initByteSize; ledCount < (serialBytes.Length - initByteSize); ledCount++)
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
                                AVFunctions.MoveByteInArrayLeft(serialBytes, 3, serialBytes.Length - 1);
                                AVFunctions.MoveByteInArrayLeft(serialBytes, 3, serialBytes.Length - 1);
                                AVFunctions.MoveByteInArrayLeft(serialBytes, 3, serialBytes.Length - 1);
                            }
                        }
                        else if (setLedRotate < 0)
                        {
                            for (int RotateCount = 0; RotateCount < Math.Abs(setLedRotate); RotateCount++)
                            {
                                AVFunctions.MoveByteInArrayRight(serialBytes, serialBytes.Length - 1, 3);
                                AVFunctions.MoveByteInArrayRight(serialBytes, serialBytes.Length - 1, 3);
                                AVFunctions.MoveByteInArrayRight(serialBytes, serialBytes.Length - 1, 3);
                            }
                        }

                        //Send the serial bytes to device
                        if (!SerialComPortWrite(serialBytes))
                        {
                            ConnectionFailed = true;
                            break;
                        }

                        //Delay the loop task
                        await TaskDelayLoop(setUpdateRate, vTask_UpdateLed);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("Screen capture loop failed: " + ex.Message);
                    }
                    finally
                    {
                        //Clear screen capture resources
                        if (bitmapIntPtr != IntPtr.Zero)
                        {
                            CaptureImport.CaptureFreeMemory(bitmapIntPtr);
                        }
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

        private static void UpdateCaptureDebugPreview(IntPtr bitmapIntPtr, byte[] bitmapByteArray)
        {
            try
            {
                //Update debug screen capture preview
                ActionDispatcherInvoke(delegate
                {
                    try
                    {
                        App.vFormSettings.image_DebugPreview.Source = CaptureBitmap.BitmapByteArrayToBitmapSource(bitmapByteArray, vCaptureDetails, vCaptureSettings);
                    }
                    catch { }
                });

                //Debug save screen capture as image
                if (setDebugSave)
                {
                    CaptureImport.CaptureSaveFilePng(bitmapIntPtr, "Debug\\" + Environment.TickCount + ".png");
                }
            }
            catch { }
        }

        private static void UpdateLedColorsPreview(LedSideTypes ledSide, int currentLed, ColorRGBA colorRGBA)
        {
            try
            {
                ActionDispatcherInvoke(delegate
                {
                    if (ledSide == LedSideTypes.LeftBottomToTop)
                    {
                        int countLed = App.vFormSettings.listbox_LedPreviewLeft.Items.Count - 1;
                        App.vFormSettings.listbox_LedPreviewLeft.Items[countLed - currentLed] = colorRGBA.AsSolidColorBrush();
                    }
                    else if (ledSide == LedSideTypes.LeftTopToBottom)
                    {
                        int countLed = App.vFormSettings.listbox_LedPreviewLeft.Items.Count - 1;
                        App.vFormSettings.listbox_LedPreviewLeft.Items[currentLed] = colorRGBA.AsSolidColorBrush();
                    }
                    else if (ledSide == LedSideTypes.TopRightToLeft)
                    {
                        int countLed = App.vFormSettings.listbox_LedPreviewTop.Items.Count - 1;
                        App.vFormSettings.listbox_LedPreviewTop.Items[countLed - currentLed] = colorRGBA.AsSolidColorBrush();
                    }
                    else if (ledSide == LedSideTypes.TopLeftToRight)
                    {
                        int countLed = App.vFormSettings.listbox_LedPreviewTop.Items.Count - 1;
                        App.vFormSettings.listbox_LedPreviewTop.Items[currentLed] = colorRGBA.AsSolidColorBrush();
                    }
                    else if (ledSide == LedSideTypes.RightBottomToTop)
                    {
                        int countLed = App.vFormSettings.listbox_LedPreviewRight.Items.Count - 1;
                        App.vFormSettings.listbox_LedPreviewRight.Items[countLed - currentLed] = colorRGBA.AsSolidColorBrush();
                    }
                    else if (ledSide == LedSideTypes.RightTopToBottom)
                    {
                        int countLed = App.vFormSettings.listbox_LedPreviewRight.Items.Count - 1;
                        App.vFormSettings.listbox_LedPreviewRight.Items[currentLed] = colorRGBA.AsSolidColorBrush();
                    }
                    else if (ledSide == LedSideTypes.BottomRightToLeft)
                    {
                        int countLed = App.vFormSettings.listbox_LedPreviewBottom.Items.Count - 1;
                        App.vFormSettings.listbox_LedPreviewBottom.Items[countLed - currentLed] = colorRGBA.AsSolidColorBrush();
                    }
                    else if (ledSide == LedSideTypes.BottomLeftToRight)
                    {
                        int countLed = App.vFormSettings.listbox_LedPreviewBottom.Items.Count - 1;
                        App.vFormSettings.listbox_LedPreviewBottom.Items[currentLed] = colorRGBA.AsSolidColorBrush();
                    }
                });
            }
            catch { }
        }
    }
}