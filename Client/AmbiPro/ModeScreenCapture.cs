using ArnoldVinkCode;
using ScreenCapture;
using System;
using System.Configuration;
using System.Diagnostics;
using System.Threading.Tasks;
using static AmbiPro.AppTasks;
using static AmbiPro.AppVariables;
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
                    MaxPixelDimension = 500
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

        private static void UpdateCaptureVariables()
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
                vBlackBarStepVertical = (setAdjustBlackbarRange * vCaptureDetails.Height) / 100;
                vBlackBarRangeVertical = vCaptureDetails.Width - vMarginMinimumOffset;
                vBlackBarStepHorizontal = (setAdjustBlackbarRange * vCaptureDetails.Width) / 100;
                vBlackBarRangeHorizontal = vCaptureDetails.Height - vMarginMinimumOffset;
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
        private static async Task ModeScreenCapture(int InitByteSize, byte[] SerialBytes)
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

                //Update led status icons
                UpdateLedStatusIcons(true);

                //Start updating the leds
                while (!vTask_UpdateLed.TaskStopRequest)
                {
                    int currentSerialByte = InitByteSize;
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

                        //Adjust the black bars margin
                        if (setAdjustBlackBars)
                        {
                            if (setAdjustBlackbarRate == 0 || (Environment.TickCount - vMarginBlackLastUpdate) > setAdjustBlackbarRate)
                            {
                                //Set blackbar range
                                AdjustBlackBars(setLedSideFirst, bitmapByteArray);
                                AdjustBlackBars(setLedSideSecond, bitmapByteArray);
                                AdjustBlackBars(setLedSideThird, bitmapByteArray);
                                AdjustBlackBars(setLedSideFourth, bitmapByteArray);
                                vMarginBlackLastUpdate = Environment.TickCount;
                            }
                        }

                        //Check led capture sides color
                        ScreenColors(setLedSideFirst, setLedCountFirst, SerialBytes, bitmapByteArray, ref currentSerialByte);
                        ScreenColors(setLedSideSecond, setLedCountSecond, SerialBytes, bitmapByteArray, ref currentSerialByte);
                        ScreenColors(setLedSideThird, setLedCountThird, SerialBytes, bitmapByteArray, ref currentSerialByte);
                        ScreenColors(setLedSideFourth, setLedCountFourth, SerialBytes, bitmapByteArray, ref currentSerialByte);

                        //Check if debug mode is enabled
                        if (vDebugCaptureAllowed)
                        {
                            //Update debug screen capture preview
                            ActionDispatcherInvoke(delegate
                                {
                                    try
                                    {
                                        App.vFormSettings.image_DebugPreview.Source = CaptureBitmap.BitmapIntPtrToBitmapSource(bitmapIntPtr, vCaptureDetails, vCaptureSettings);
                                    }
                                    catch { }
                                });

                            //Debug save screen capture as image
                            if (setDebugSave)
                            {
                                CaptureImport.CaptureSaveFilePng(bitmapIntPtr, "Debug\\" + Environment.TickCount + ".png");
                            }
                        }

                        //Smooth led frame transition
                        if (setLedSmoothing > 0)
                        {
                            //Make copy of current color bytes
                            byte[] CaptureByteCurrent = CloneByteArray(SerialBytes);

                            //Merge current colors with history
                            for (int ledCount = InitByteSize; ledCount < (SerialBytes.Length - InitByteSize); ledCount++)
                            {
                                //Debug.WriteLine("Led smoothing old: " + SerialBytes[ledCount]);

                                int ColorCount = 1;
                                int ColorAverage = SerialBytes[ledCount];
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

                                SerialBytes[ledCount] = (byte)(ColorAverage / ColorCount);
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
                                AVFunctions.MoveByteInArrayLeft(SerialBytes, 3, SerialBytes.Length - 1);
                                AVFunctions.MoveByteInArrayLeft(SerialBytes, 3, SerialBytes.Length - 1);
                                AVFunctions.MoveByteInArrayLeft(SerialBytes, 3, SerialBytes.Length - 1);
                            }
                        }
                        else if (setLedRotate < 0)
                        {
                            for (int RotateCount = 0; RotateCount < Math.Abs(setLedRotate); RotateCount++)
                            {
                                AVFunctions.MoveByteInArrayRight(SerialBytes, SerialBytes.Length - 1, 3);
                                AVFunctions.MoveByteInArrayRight(SerialBytes, SerialBytes.Length - 1, 3);
                                AVFunctions.MoveByteInArrayRight(SerialBytes, SerialBytes.Length - 1, 3);
                            }
                        }

                        //Send the serial bytes to device
                        if (!SerialComPortWrite(SerialBytes))
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
    }
}