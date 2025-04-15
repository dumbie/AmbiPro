using ArnoldVinkCode;
using ScreenCaptureImport;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using static AmbiPro.AppClasses;
using static AmbiPro.AppTasks;
using static AmbiPro.AppVariables;
using static AmbiPro.PreloadSettings;
using static ArnoldVinkCode.AVActions;
using static ArnoldVinkCode.AVArrayFunctions;
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
                    DrawMouseCursor = false,
                    DrawBorder = false,
                    HDRtoSDR = true,
                    HDRPaperWhite = captureHDRPaperWhite,
                    HDRMaximumNits = captureHDRMaximumNits
                };

                Debug.WriteLine("Set capture settings variable.");
            }
            catch { }
        }

        //Screen capture events
        public static async void CaptureEventDeviceChangeDetected()
        {
            try
            {
                Debug.WriteLine("Device change event triggered, restarting capture.");

                //Initialize Screen Capture
                await InitializeScreenCapture();
            }
            catch { }
        }

        //Initialize screen capture
        private static async Task<CaptureResult> InitializeScreenCapture()
        {
            try
            {
                Debug.WriteLine("Initializing screen capture: " + DateTime.Now);

                //Set capture settings
                SetCaptureSettings();

                //Register capture events
                CaptureImport.CaptureEventDeviceChangeDetected(CaptureEventDeviceChangeDetected);

                //Initialize screen capture
                CaptureResult captureResult = CaptureImport.CaptureInitialize(vCaptureSettings, true);
                Debug.WriteLine("Capture result: " + captureResult.Status + " / " + captureResult.Message);
                if (captureResult.Status == CaptureStatus.Success)
                {
                    //Update capture details
                    vCaptureDetails = CaptureImport.CaptureGetDetails();
                }
                else
                {
                    //Show failed capture message
                    await ShowFailedCaptureMessage(captureResult.Message);
                }

                //Update capture variables
                UpdateCaptureVariables();

                //Update screen information
                DispatcherInvoke(delegate
                {
                    vFormSettings.UpdateScreenInformation();
                });

                //Return capture result
                return captureResult;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed initializing screen capturer: " + ex.Message);

                //Show failed capture message
                await ShowFailedCaptureMessage(ex.Message);

                //Return capture result
                return new CaptureResult() { Status = CaptureStatus.Failed, Message = ex.Message };
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
        private static CaptureResult ResetScreenCapture()
        {
            try
            {
                Debug.WriteLine("Resetting screen capture: " + DateTime.Now);
                return CaptureImport.CaptureReset();
            }
            catch
            {
                Debug.WriteLine("Failed to reset screen capture.");
                return new CaptureResult { Status = CaptureStatus.Failed };
            }
        }

        //Loop capture the screen
        private static async Task ModeScreenCapture()
        {
            try
            {
                //Loop mode variables
                bool ConnectionFailed = false;
                int LoopDelayMs = 0;

                //Create led ColorRGBA array
                ColorRGBA[] colorArray = CreateArray(setLedCountTotal, ColorRGBA.Black);

                //Initialize Screen Capture
                CaptureResult initializeResult = await InitializeScreenCapture();
                if (initializeResult.Status != CaptureStatus.Success)
                {
                    return;
                }

                //Start updating leds
                while (await TaskCheckLoop(vTask_UpdateLed, LoopDelayMs))
                {
                    int colorCurrentIndex = 0;
                    IntPtr bitmapIntPtr = IntPtr.Zero;
                    try
                    {
                        //Check monitor sleeping and send black leds update
                        if (setLedOffMonitorSleep && vMonitorSleeping)
                        {
                            //Reset color array
                            ResetArray(colorArray, ColorRGBA.Black);

                            //Set loop delay time
                            LoopDelayMs = 500;
                        }
                        else
                        {
                            //Capture screenshot
                            try
                            {
                                bitmapIntPtr = CaptureImport.CaptureScreenBytes();
                            }
                            catch { }

                            //Check screenshot
                            if (bitmapIntPtr == IntPtr.Zero)
                            {
                                continue;
                            }

                            //Convert BitmapIntPtr to BitmapByteArray
                            byte[] bitmapByteArray = CaptureBitmap.BitmapIntPtrToBitmapByteArray(bitmapIntPtr, vCaptureDetails);

                            //Get led capture sides color
                            ScreenColors(setLedSideFirst, setLedCountFirst, bitmapByteArray, colorArray, ref colorCurrentIndex);
                            ScreenColors(setLedSideSecond, setLedCountSecond, bitmapByteArray, colorArray, ref colorCurrentIndex);
                            ScreenColors(setLedSideThird, setLedCountThird, bitmapByteArray, colorArray, ref colorCurrentIndex);
                            ScreenColors(setLedSideFourth, setLedCountFourth, bitmapByteArray, colorArray, ref colorCurrentIndex);

                            //Update debug screen capture preview
                            DebugUpdateCapturePreview(bitmapByteArray);

                            //Smooth object movement
                            AdjustLedSmoothObject(colorArray);

                            //Adjust leds color to settings
                            AdjustLedColors(colorArray);

                            //Smooth frame transition
                            //AdjustLedSmoothFrameOpacity(colorArray);
                            AdjustLedSmoothFrameMerge(colorArray);

                            //Rotate leds as calibrated
                            AdjustLedRotate(colorArray);

                            //Adjust leds to energy mode
                            AdjustLedEnergyMode(colorArray);

                            //Set loop delay time
                            LoopDelayMs = setUpdateRate;
                        }

                        //Send serial bytes to device
                        if (!SerialComPortWrite(colorArray))
                        {
                            ConnectionFailed = true;
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("Screen capture loop failed: " + ex.Message);
                    }
                }

                //Show failed connection message
                if (ConnectionFailed)
                {
                    await ShowFailedConnectionMessage();
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
                CaptureResult captureResult = CaptureImport.CaptureUpdateSettings(vCaptureSettings);
                Debug.WriteLine("Capture settings updated: " + captureResult.Status);
            }
            catch { }
        }
    }
}