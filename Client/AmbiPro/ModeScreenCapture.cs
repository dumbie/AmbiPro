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
                vMarginBottom = vMarginMinimumOffset;
                vMarginTop = vMarginMinimumOffset;
                vMarginLeft = vMarginMinimumOffset;
                vMarginRight = vMarginMinimumOffset;

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
                    IntPtr IntPtrBitmap = IntPtr.Zero;
                    Bitmap BitmapImage = null;
                    try
                    {
                        //Capture screenshot
                        try
                        {
                            IntPtrBitmap = AppImport.CaptureScreenshot(out vScreenWidth, out vScreenHeight, out vScreenOutputSize);
                        }
                        catch
                        {
                            Debug.WriteLine("Failed capturing screenshot, restarting capture.");
                            await InitializeScreenCapturer();
                            continue;
                        }

                        //Check screenshot
                        if (IntPtrBitmap == IntPtr.Zero)
                        {
                            Debug.WriteLine("Screenshot is corrupted, restarting capture.");
                            await InitializeScreenCapturer();
                            continue;
                        }

                        //Calculate resize ratio
                        int[] ledCounts = { setLedCountFirst, setLedCountSecond, setLedCountThird, setLedCountFourth };
                        int targetSize = ledCounts.Max();
                        if (targetSize < 200) { targetSize = 200; }
                        double ratioHor = (double)targetSize / (double)vScreenWidth;
                        double ratioVer = (double)targetSize / (double)vScreenHeight;
                        double ratio = Math.Max(ratioHor, ratioVer);

                        //Resize the bitmap image
                        vScreenWidth = (int)(vScreenWidth * ratio);
                        vScreenHeight = (int)(vScreenHeight * ratio);
                        vCaptureZoneRange = (setLedCaptureRange * vScreenHeight) / 100 / 2;
                        IntPtrBitmap = AppImport.CaptureResize(IntPtrBitmap, vScreenWidth, vScreenHeight, out vScreenOutputSize);
                        Debug.WriteLine("Screen width: " + vScreenWidth + " / Screen height: " + vScreenHeight);

                        //Current byte information
                        int CurrentSerialByte = InitByteSize;

                        unsafe
                        {
                            byte* BitmapData = (byte*)IntPtrBitmap;

                            ////Adjust the black bars margin
                            //if (setAdjustBlackBars)
                            //{
                            //    AdjustBlackBars(0, OffsetMargin, ref HorizontalMargin0, BitmapData);
                            //    AdjustBlackBars(1, OffsetMargin, ref VerticalMargin1, BitmapData);
                            //    AdjustBlackBars(2, OffsetMargin, ref HorizontalMargin2, BitmapData);
                            //    AdjustBlackBars(3, OffsetMargin, ref VerticalMargin3, BitmapData);
                            //}

                            //Check led capture sides color
                            ScreenColors(setLedSideFirst, setLedCountFirst, SerialBytes, BitmapData, ref CurrentSerialByte);
                            ScreenColors(setLedSideSecond, setLedCountSecond, SerialBytes, BitmapData, ref CurrentSerialByte);
                            ScreenColors(setLedSideThird, setLedCountThird, SerialBytes, BitmapData, ref CurrentSerialByte);
                            ScreenColors(setLedSideFourth, setLedCountFourth, SerialBytes, BitmapData, ref CurrentSerialByte);

                            //Check if debug mode is enabled
                            if (setDebugMode)
                            {
                                //Convert IntPtr to bitmap image
                                BitmapImage = ConvertDataToBitmap((byte*)IntPtrBitmap, vScreenWidth, vScreenHeight, vScreenOutputSize, true);

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
                        TaskDelayMs((uint)setUpdateRate);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("Screen capture loop failed: " + ex.Message);
                    }
                    finally
                    {
                        //Clear screen capture resources
                        if (IntPtrBitmap != IntPtr.Zero)
                        {
                            AppImport.CaptureFreeMemory(IntPtrBitmap);
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