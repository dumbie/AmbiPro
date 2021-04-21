using ArnoldVinkCode;
using System;
using System.Diagnostics;
using System.Drawing;
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
                //Reset side margins
                vMarginBottom = vMarginOffset;
                vMarginTop = vMarginOffset;
                vMarginLeft = vMarginOffset;
                vMarginRight = vMarginOffset;

                //Initialize Screen Capturer
                bool CapturerInitialized = await InitializeScreenCapturer();
                if (!CapturerInitialized)
                {
                    Debug.WriteLine("Failed to initialize the capturer.");
                    ShowCaptureMessage();
                    return;
                }

                //Update the tray icon
                AppTray.NotifyIcon.Icon = new Icon(Assembly.GetEntryAssembly().GetManifestResourceStream("AmbiPro.Assets.ApplicationIcon.ico"));

                //Start updating the leds
                while (!vTask_LedUpdate.TaskStopRequest)
                {
                    //Capture screenshot
                    IntPtr IntPtrBitmap = IntPtr.Zero;
                    Bitmap BitmapImage = null;
                    try
                    {
                        try
                        {
                            IntPtrBitmap = AppImport.CaptureScreenshot(out vScreenWidth, out vScreenHeight, out vScreenOutputSize);
                        }
                        catch { }
                        if (IntPtrBitmap == IntPtr.Zero)
                        {
                            Debug.WriteLine("Failed capturing screenshot.");
                            await InitializeScreenCapturer();
                            continue;
                        }

                        //Current byte information
                        int CurrentSerialByte = InitByteSize;

                        unsafe
                        {
                            //Convert IntPtr to bitmap
                            BitmapImage = ConvertDataToBitmap((byte*)IntPtrBitmap, vScreenWidth, vScreenHeight, vScreenOutputSize, false);

                            //Calculate resize ratio
                            double ratioX = (double)200 / (double)vScreenWidth;
                            double ratioY = (double)200 / (double)vScreenHeight;
                            double ratio = Math.Max(ratioX, ratioY);

                            //Resize the bitmap image
                            vScreenWidth = (int)(vScreenWidth * ratio);
                            vScreenHeight = (int)(vScreenHeight * ratio);
                            vScreenCapturePixels = (setLedCaptureRange * vScreenHeight) / 100 / 2;
                            ResizeBitmap(ref BitmapImage, vScreenWidth, vScreenHeight);

                            Debug.WriteLine("Screen width: " + vScreenWidth + " / Screen height: " + vScreenHeight + " / Capture range: " + vScreenCapturePixels);
                            byte* BitmapData = GetDataFromBitmap(BitmapImage);

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
                            for (Int32 RotateCount = 0; RotateCount < setLedRotate; RotateCount++)
                            {
                                AVFunctions.MoveByteInArrayLeft(SerialBytes, 3, SerialBytes.Length - 1);
                                AVFunctions.MoveByteInArrayLeft(SerialBytes, 3, SerialBytes.Length - 1);
                                AVFunctions.MoveByteInArrayLeft(SerialBytes, 3, SerialBytes.Length - 1);
                            }
                        }
                        else if (setLedRotate < 0)
                        {
                            Int32 LedRotateAbs = Math.Abs(setLedRotate);
                            for (Int32 RotateCount = 0; RotateCount < LedRotateAbs; RotateCount++)
                            {
                                AVFunctions.MoveByteInArrayRight(SerialBytes, SerialBytes.Length - 1, 3);
                                AVFunctions.MoveByteInArrayRight(SerialBytes, SerialBytes.Length - 1, 3);
                                AVFunctions.MoveByteInArrayRight(SerialBytes, SerialBytes.Length - 1, 3);
                            }
                        }

                        //Send the serial bytes to device
                        //Debug.WriteLine("Serial bytes sended: " + SerialBytes.Length);
                        vSerialComPort.Write(SerialBytes, 0, SerialBytes.Length);
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

                        //Delay the loop task
                        TaskDelayMs((uint)setUpdateRate);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Screen capture task failed: " + ex.Message);
            }
        }
    }
}