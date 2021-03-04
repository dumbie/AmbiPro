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
                //Update side margins
                int OffsetMargin = 2;
                int HorizontalMargin0 = OffsetMargin;
                int HorizontalMargin2 = OffsetMargin;
                int VerticalMargin1 = OffsetMargin;
                int VerticalMargin3 = OffsetMargin;

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

                            //Calculate image resize
                            double ratioX = (double)setLedCount * vCaptureZoneSize / (double)vScreenWidth;
                            double ratioY = (double)setLedCount * vCaptureZoneSize / (double)vScreenHeight;
                            double ratio = Math.Min(ratioX, ratioY);
                            vScreenWidth = (int)(vScreenWidth * ratio);
                            vScreenHeight = (int)(vScreenHeight * ratio);
                            Debug.WriteLine("Screen width: " + vScreenWidth + "/Screen height: " + vScreenHeight);

                            ResizeBitmap(ref BitmapImage, vScreenWidth, vScreenHeight);
                            byte* BitmapData = GetDataFromBitmap(BitmapImage);

                            //Calculate led capture
                            double WidthPercentage = ((double)vScreenHeight / (double)vScreenWidth) * 100;
                            double HeightPercentage = 100 - WidthPercentage;
                            Debug.WriteLine("Percentage width: " + WidthPercentage + "/Percentage height: " + HeightPercentage);

                            int HorizontalLedCount = Convert.ToInt32(((WidthPercentage / 100) * setLedCount) / 2);
                            int VerticalLedCount = Convert.ToInt32(((HeightPercentage / 100) * setLedCount) / 2);
                            Debug.WriteLine("Horizontal led count: " + HorizontalLedCount + "/Vertical led count: " + VerticalLedCount);

                            int HorizontalLedStep = vScreenWidth / HorizontalLedCount;
                            int VerticalLedStep = vScreenHeight / VerticalLedCount;
                            Debug.WriteLine("Horizontal led step: " + HorizontalLedStep + "/Vertical led step: " + VerticalLedStep);

                            //Side0=Left and Right
                            //Side1=Left, Right and Top
                            //Side2=Left, Right, Top and Bottom

                            //Direction=0 Left to Right
                            //SideType0=Horizontal Bottom >
                            //SideType1=Vertical Right ^
                            //SideType2=Horizontal Top <
                            //SideType3=Vertical Left v

                            //Direction=1 Right to Left
                            //SideType0=Horizontal Bottom <
                            //SideType1=Vertical Left ^
                            //SideType2=Horizontal Top >
                            //SideType3=Vertical Right v

                            //Adjust the black bars margin
                            if (setAdjustBlackBars)
                            {
                                AdjustBlackBars(0, OffsetMargin, ref HorizontalMargin0, BitmapData);
                                AdjustBlackBars(1, OffsetMargin, ref VerticalMargin1, BitmapData);
                                AdjustBlackBars(2, OffsetMargin, ref HorizontalMargin2, BitmapData);
                                AdjustBlackBars(3, OffsetMargin, ref VerticalMargin3, BitmapData);
                            }

                            //Check led capture sides color
                            if (setLedSides == 0)
                            {
                                ScreenColors(1, OffsetMargin, VerticalMargin1, SerialBytes, BitmapData, VerticalLedCount, VerticalLedStep, ref CurrentSerialByte);
                                ScreenColors(3, OffsetMargin, VerticalMargin3, SerialBytes, BitmapData, VerticalLedCount, VerticalLedStep, ref CurrentSerialByte);
                            }
                            else if (setLedSides == 1)
                            {
                                ScreenColors(1, OffsetMargin, VerticalMargin1, SerialBytes, BitmapData, VerticalLedCount, VerticalLedStep, ref CurrentSerialByte);
                                ScreenColors(2, OffsetMargin, HorizontalMargin2, SerialBytes, BitmapData, HorizontalLedCount, HorizontalLedStep, ref CurrentSerialByte);
                                ScreenColors(3, OffsetMargin, VerticalMargin3, SerialBytes, BitmapData, VerticalLedCount, VerticalLedStep, ref CurrentSerialByte);
                            }
                            else if (setLedSides == 2)
                            {
                                ScreenColors(0, OffsetMargin, HorizontalMargin0, SerialBytes, BitmapData, HorizontalLedCount, HorizontalLedStep, ref CurrentSerialByte);
                                ScreenColors(1, OffsetMargin, VerticalMargin1, SerialBytes, BitmapData, VerticalLedCount, VerticalLedStep, ref CurrentSerialByte);
                                ScreenColors(2, OffsetMargin, HorizontalMargin2, SerialBytes, BitmapData, HorizontalLedCount, HorizontalLedStep, ref CurrentSerialByte);
                                ScreenColors(3, OffsetMargin, VerticalMargin3, SerialBytes, BitmapData, VerticalLedCount, VerticalLedStep, ref CurrentSerialByte);
                            }

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