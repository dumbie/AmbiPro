using ArnoldVinkCode;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using static AmbiPro.AppTasks;
using static AmbiPro.AppVariables;
using static AmbiPro.Resources.BitmapProcessing;
using static ArnoldVinkCode.AVActions;

namespace AmbiPro
{
    partial class SerialMonitor
    {
        //Loop cature the screen
        private static async Task ModeScreenCapture(int InitByteSize, byte[] SerialBytes)
        {
            try
            {
                //Loop mode variables
                bool ConnectionFailed = false;

                //Initialize Screen Capturer
                if (!await InitializeScreenCapturer())
                {
                    Debug.WriteLine("Failed to initialize the screen capturer.");
                    ShowFailedCaptureMessage();
                    return;
                }

                //Update the tray icon
                AppTray.NotifyIcon.Icon = new Icon(Assembly.GetEntryAssembly().GetManifestResourceStream("AmbiPro.Assets.ApplicationIcon.ico"));

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
                            BitmapIntPtr = AppImport.CaptureScreenshot(out vScreenWidth, out vScreenHeight, out vScreenOutputSize);
                        }
                        catch { }

                        //Check screenshot
                        if (BitmapIntPtr == IntPtr.Zero)
                        {
                            Debug.WriteLine("Screenshot is corrupted, restarting capture.");
                            await InitializeScreenCapturer();
                            await TaskDelayLoop(1000, vTask_LedUpdate);
                            continue;
                        }

                        //Resize screenshot
                        try
                        {
                            int[] ledCounts = { setLedCountFirst, setLedCountSecond, setLedCountThird, setLedCountFourth };
                            int minimumSize = ledCounts.Max() * 2;
                            int ignoreSize = 300; //Under 900p
                            bool resizeScreenshot = false;

                            int screenWidthNew = vScreenWidth / 3;
                            int screenHeightNew = vScreenHeight / 3;
                            if (screenWidthNew >= ignoreSize && screenHeightNew >= ignoreSize)
                            {
                                resizeScreenshot = true;
                            }
                            else
                            {
                                screenWidthNew = vScreenWidth;
                                screenHeightNew = vScreenHeight;
                            }

                            if (screenWidthNew < minimumSize || screenHeightNew < minimumSize)
                            {
                                if (ignoreSize < minimumSize) { ignoreSize = minimumSize; }
                                double ratioX = (double)ignoreSize / (double)vScreenWidth;
                                double ratioY = (double)ignoreSize / (double)vScreenHeight;
                                double ratio = Math.Max(ratioX, ratioY);
                                screenWidthNew = (int)(vScreenWidth * ratio);
                                screenHeightNew = (int)(vScreenHeight * ratio);
                                resizeScreenshot = true;
                            }

                            if (resizeScreenshot)
                            {
                                vScreenWidth = screenWidthNew;
                                vScreenHeight = screenHeightNew;
                                BitmapIntPtr = AppImport.CaptureResizeNearest(BitmapIntPtr, vScreenWidth, vScreenHeight, out vScreenOutputSize);
                            }

                            vCaptureZoneRange = (setLedCaptureRange * vScreenHeight) / 100 / 2;
                            Debug.WriteLine("Screen width: " + vScreenWidth + " / Screen height: " + vScreenHeight + " / Capture range: " + vCaptureZoneRange + " / Resize: " + resizeScreenshot);
                        }
                        catch { }

                        unsafe
                        {
                            byte* BitmapData = (byte*)BitmapIntPtr;

                            //Adjust the black bars margin
                            if (setAdjustBlackBars)
                            {
                                if (setAdjustBlackbarRate == 0 || (Environment.TickCount - vMarginBlackLastUpdate) > setAdjustBlackbarRate)
                                {
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
                                BitmapImage = ConvertDataToBitmap(BitmapData, vScreenWidth, vScreenHeight, vScreenOutputSize, true);

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
                        if (BitmapIntPtr != IntPtr.Zero)
                        {
                            AppImport.CaptureFreeMemory(BitmapIntPtr);
                        }
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
        }
    }
}