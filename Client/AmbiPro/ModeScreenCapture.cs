using System;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Threading.Tasks;
using static AmbiPro.AppTasks;
using static AmbiPro.Resources.BitmapProcessing;
using static ArnoldVinkCode.AVActions;

namespace AmbiPro
{
    partial class SerialMonitor
    {
        //Screen size
        private static int vScreenWidth = 0;
        private static int vScreenHeight = 0;
        private static int vOutputSize = 0;

        //Loop cature the screen
        private static async Task ModeScreenCapture(int InitByteSize, byte[] SerialBytes)
        {
            try
            {
                //Update side margins (Atleast 1)
                int OffsetMargin = 5;
                int HorizontalMargin0 = 5;
                int HorizontalMargin2 = 5;
                int VerticalMargin1 = 5;
                int VerticalMargin3 = 5;

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
                    try
                    {
                        IntPtrBitmap = AppImport.CaptureScreenshot(out vScreenWidth, out vScreenHeight, out vOutputSize);
                    }
                    catch { }

                    if (IntPtrBitmap == IntPtr.Zero)
                    {
                        Debug.WriteLine("Failed capturing screenshot.");
                        await InitializeScreenCapturer();
                        continue;
                    }

                    //Led capture calculations
                    double WidthPercentage = ((double)vScreenHeight / (double)vScreenWidth) * 100;
                    double HeightPercentage = 100 - WidthPercentage;

                    int HorizontalLedCount = Convert.ToInt32(((WidthPercentage / 100) * setLedCount) / 2);
                    int VerticalLedCount = Convert.ToInt32(((HeightPercentage / 100) * setLedCount) / 2);

                    //Rotate the leds as calibrated for current ratio
                    if (setLedRotate >= 0)
                    {
                        HorizontalLedCount += Math.Abs(setLedRotate);
                        VerticalLedCount -= Math.Abs(setLedRotate);
                    }
                    else
                    {
                        HorizontalLedCount -= Math.Abs(setLedRotate);
                        VerticalLedCount += Math.Abs(setLedRotate);
                    }

                    int HorizontalStep = (vScreenWidth / HorizontalLedCount);
                    int VerticalStep = (vScreenHeight / VerticalLedCount);

                    //Current byte information
                    int CurrentSerialByte = InitByteSize;

                    unsafe
                    {
                        //Convert IntPtr to bitmap bytes
                        byte* BitmapData = (byte*)IntPtrBitmap;

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
                            ScreenColors(1, OffsetMargin, VerticalMargin1, SerialBytes, BitmapData, VerticalLedCount, VerticalStep, ref CurrentSerialByte);
                            ScreenColors(3, OffsetMargin, VerticalMargin3, SerialBytes, BitmapData, VerticalLedCount, VerticalStep, ref CurrentSerialByte);
                        }
                        else if (setLedSides == 1)
                        {
                            ScreenColors(1, OffsetMargin, VerticalMargin1, SerialBytes, BitmapData, VerticalLedCount, VerticalStep, ref CurrentSerialByte);
                            ScreenColors(2, OffsetMargin, HorizontalMargin2, SerialBytes, BitmapData, HorizontalLedCount, HorizontalStep, ref CurrentSerialByte);
                            ScreenColors(3, OffsetMargin, VerticalMargin3, SerialBytes, BitmapData, VerticalLedCount, VerticalStep, ref CurrentSerialByte);
                        }
                        else if (setLedSides == 2)
                        {
                            ScreenColors(0, OffsetMargin, HorizontalMargin0, SerialBytes, BitmapData, HorizontalLedCount, HorizontalStep, ref CurrentSerialByte);
                            ScreenColors(1, OffsetMargin, VerticalMargin1, SerialBytes, BitmapData, VerticalLedCount, VerticalStep, ref CurrentSerialByte);
                            ScreenColors(2, OffsetMargin, HorizontalMargin2, SerialBytes, BitmapData, HorizontalLedCount, HorizontalStep, ref CurrentSerialByte);
                            ScreenColors(3, OffsetMargin, VerticalMargin3, SerialBytes, BitmapData, VerticalLedCount, VerticalStep, ref CurrentSerialByte);
                        }

                        //Check if debug mode is enabled
                        if (setDebugMode)
                        {
                            ConvertDataToBitmap(BitmapData, vScreenWidth, vScreenHeight, vOutputSize, out Bitmap bitmapImage, out byte* bitmapDataX);

                            //Debug update screen capture preview
                            ActionDispatcherInvoke(delegate
                            {
                                try
                                {
                                    App.vFormSettings.image_DebugPreview.Source = BitmapToBitmapImage(bitmapImage);
                                }
                                catch { }
                            });

                            //Debug save screen capture as image
                            if (setDebugSave)
                            {
                                SaveScreenCaptureBitmap(bitmapImage);
                            }
                        }
                    }

                    //Send the serial bytes to device
                    //Debug.WriteLine("Serial bytes sended: " + SerialBytes.Length);
                    vSerialComPort.Write(SerialBytes, 0, SerialBytes.Length);

                    //Clear screen capture resources
                    AppImport.CaptureFreeMemory(IntPtrBitmap);

                    //Delay the loop task
                    TaskDelayMs((uint)setUpdateRate);
                }
            }
            catch { }
        }
    }
}