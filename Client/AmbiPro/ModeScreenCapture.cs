using ArnoldVinkCode;
using System;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Threading.Tasks;
using static AmbiPro.AppTasks;
using static AmbiPro.AppVariables;
using static AmbiPro.Resources.BitmapProcessing;
using static ArnoldVinkCode.AVActions;

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
                return AppImport.CaptureInitialize(Convert.ToInt32(ConfigurationManager.AppSettings["MonitorCapture"]));
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

        //Reset Screen Capture
        private static bool ResetScreenCapture()
        {
            try
            {
                Debug.WriteLine("Resetting screen capture: " + DateTime.Now);
                return AppImport.CaptureReset();
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
                while (!vTask_LedUpdate.TaskStopRequest)
                {
                    int CurrentSerialByte = InitByteSize;
                    IntPtr BitmapIntPtr = IntPtr.Zero;
                    Bitmap BitmapImage = null;
                    try
                    {
                        //Capture screenshot
                        try
                        {
                            BitmapIntPtr = AppImport.CaptureScreenshot(out vScreenWidth, out vScreenHeight, out vScreenOutputSize, 2);
                        }
                        catch { }

                        //Check screenshot
                        if (BitmapIntPtr == IntPtr.Zero)
                        {
                            Debug.WriteLine("Screenshot is corrupted, restarting capture.");
                            await InitializeScreenCapture(500);
                            continue;
                        }

                        //Set capture range
                        if (vScreenHeight < vScreenWidth)
                        {
                            vCaptureRange = (setLedCaptureRange * vScreenHeight) / 100 / 2;
                        }
                        else
                        {
                            vCaptureRange = (setLedCaptureRange * vScreenWidth) / 100 / 2;
                        }
                        //Debug.WriteLine("Screen width: " + vScreenWidth + " / Screen height: " + vScreenHeight + " / Capture range: " + vCaptureZoneRange + " / Resize: " + resizeScreenshot);

                        unsafe
                        {
                            byte* BitmapData = (byte*)BitmapIntPtr;

                            //Adjust the black bars margin
                            if (setAdjustBlackBars)
                            {
                                if (setAdjustBlackbarRate == 0 || (Environment.TickCount - vMarginBlackLastUpdate) > setAdjustBlackbarRate)
                                {
                                    //Set blackbar range
                                    vBlackBarStepVertical = (setAdjustBlackbarRange * vScreenHeight) / 100;
                                    vBlackBarRangeVertical = vScreenWidth - vMarginMinimumOffset;
                                    vBlackBarStepHorizontal = (setAdjustBlackbarRange * vScreenWidth) / 100;
                                    vBlackBarRangeHorizontal = vScreenHeight - vMarginMinimumOffset;
                                    AdjustBlackBars(setLedSideFirst, BitmapData);
                                    AdjustBlackBars(setLedSideSecond, BitmapData);
                                    AdjustBlackBars(setLedSideThird, BitmapData);
                                    AdjustBlackBars(setLedSideFourth, BitmapData);
                                    vMarginBlackLastUpdate = Environment.TickCount;
                                }
                            }

                            //Check led capture sides color
                            ScreenColors(setLedSideFirst, setLedCountFirst, SerialBytes, BitmapData, ref CurrentSerialByte);
                            ScreenColors(setLedSideSecond, setLedCountSecond, SerialBytes, BitmapData, ref CurrentSerialByte);
                            ScreenColors(setLedSideThird, setLedCountThird, SerialBytes, BitmapData, ref CurrentSerialByte);
                            ScreenColors(setLedSideFourth, setLedCountFourth, SerialBytes, BitmapData, ref CurrentSerialByte);

                            //Check if debug mode is enabled
                            if (setDebugMode)
                            {
                                //Convert IntPtr to bitmap image
                                BitmapImage = ConvertDataToBitmap(BitmapData, vScreenWidth, vScreenHeight, vScreenOutputSize, false);

                                //Debug update screen capture preview
                                ActionDispatcherInvoke(delegate
                                {
                                    try
                                    {
                                        App.vFormSettings.image_DebugPreview.Source = BitmapToBitmapImage(BitmapImage);
                                    }
                                    catch { }
                                });

                                //Debug save screen capture as image
                                if (setDebugSave)
                                {
                                    SaveScreenCaptureBitmap(BitmapImage);
                                }
                            }
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
                            int LedRotateAbs = Math.Abs(setLedRotate);
                            for (int RotateCount = 0; RotateCount < LedRotateAbs; RotateCount++)
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
                        await TaskDelayLoop(setUpdateRate, vTask_LedUpdate);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("Screen capture loop failed: " + ex.Message);
                    }
                    finally
                    {
                        //Clear screen capture resources
                        if (BitmapImage != null)
                        {
                            BitmapImage.Dispose();
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