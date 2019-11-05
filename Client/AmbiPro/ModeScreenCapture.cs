using ArnoldVinkCode;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Threading.Tasks;

namespace AmbiPro
{
    partial class SerialMonitor
    {
        //Screen size
        private static Int32 vScreenWidth = 0;
        private static Int32 vScreenHeight = 0;
        private static Int32 vOutputSize = 0;

        //Loop cature the screen
        private static async Task ModeScreenCapture(Int32 InitByteSize, byte[] SerialBytes)
        {
            try
            {
                //Update side margins (Atleast 1)
                Int32 OffsetMargin = 5;
                Int32 HorizontalMargin0 = 5;
                Int32 HorizontalMargin2 = 5;
                Int32 VerticalMargin1 = 5;
                Int32 VerticalMargin3 = 5;

                //Initialize Screen Capturer
                bool CapturerInitialized = await InitializeScreenCapturer();
                if (!CapturerInitialized)
                {
                    Debug.WriteLine("Failed to initialize the capturer.");
                    ShowCaptureMessage();
                    return;
                }

                //Update the tray icon
                AppTray.NotifyIcon.Icon = new Icon(Assembly.GetExecutingAssembly().GetManifestResourceStream("AmbiPro.Assets.ApplicationIcon.ico"));

                //Start updating the leds
                while (AVActions.TaskRunningCheck(AppTasks.LedToken))
                {
                    IntPtr IntPtrBitmap = IntPtr.Zero;
                    try { IntPtrBitmap = AppImport.CaptureScreenshot(out vScreenWidth, out vScreenHeight, out vOutputSize); } catch { }
                    if (IntPtrBitmap == IntPtr.Zero)
                    {
                        Debug.WriteLine("Failed capturing screenshot.");
                        await InitializeScreenCapturer();
                        continue;
                    }

                    //Led capture calculations
                    double WidthPercentage = ((double)vScreenHeight / (double)vScreenWidth) * 100;
                    double HeightPercentage = 100 - WidthPercentage;

                    Int32 HorizontalLeds = Convert.ToInt32(((WidthPercentage / 100) * setLedCount) / 2);
                    Int32 VerticalLeds = Convert.ToInt32(((HeightPercentage / 100) * setLedCount) / 2);

                    //Rotate the leds as calibrated for current ratio
                    if (setLedRotate >= 0)
                    {
                        HorizontalLeds += Math.Abs(setLedRotate);
                        VerticalLeds -= Math.Abs(setLedRotate);
                    }
                    else
                    {
                        HorizontalLeds -= Math.Abs(setLedRotate);
                        VerticalLeds += Math.Abs(setLedRotate);
                    }

                    Int32 HorizontalStep = (vScreenWidth / HorizontalLeds);
                    Int32 VerticalStep = (vScreenHeight / VerticalLeds);

                    //Current byte information
                    Int32 CurrentSerialByte = InitByteSize;

                    unsafe
                    {
                        //Convert IntPtr to bitmap bytes
                        byte* BitmapData = (byte*)IntPtrBitmap;

                        //Side0=Left and Right
                        //Side1=Left, Right and Up
                        //Side2=Left, Right, Up and Bottom

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
                            ScreenColors(1, OffsetMargin, VerticalMargin1, SerialBytes, BitmapData, VerticalLeds, VerticalStep, ref CurrentSerialByte);
                            ScreenColors(3, OffsetMargin, VerticalMargin3, SerialBytes, BitmapData, VerticalLeds, VerticalStep, ref CurrentSerialByte);
                        }
                        else if (setLedSides == 1)
                        {
                            ScreenColors(1, OffsetMargin, VerticalMargin1, SerialBytes, BitmapData, VerticalLeds, VerticalStep, ref CurrentSerialByte);
                            ScreenColors(2, OffsetMargin, HorizontalMargin2, SerialBytes, BitmapData, HorizontalLeds, HorizontalStep, ref CurrentSerialByte);
                            ScreenColors(3, OffsetMargin, VerticalMargin3, SerialBytes, BitmapData, VerticalLeds, VerticalStep, ref CurrentSerialByte);
                        }
                        else if (setLedSides == 2)
                        {
                            ScreenColors(0, OffsetMargin, HorizontalMargin0, SerialBytes, BitmapData, HorizontalLeds, HorizontalStep, ref CurrentSerialByte);
                            ScreenColors(1, OffsetMargin, VerticalMargin1, SerialBytes, BitmapData, VerticalLeds, VerticalStep, ref CurrentSerialByte);
                            ScreenColors(2, OffsetMargin, HorizontalMargin2, SerialBytes, BitmapData, HorizontalLeds, HorizontalStep, ref CurrentSerialByte);
                            ScreenColors(3, OffsetMargin, VerticalMargin3, SerialBytes, BitmapData, VerticalLeds, VerticalStep, ref CurrentSerialByte);
                        }

                        //Debug save screen capture as image
                        //SaveBitmapFromData(BitmapData);
                    }

                    //Send the serial bytes to device
                    //Debug.WriteLine("Serial bytes sended: " + SerialBytes.Length);
                    vSerialComPort.Write(SerialBytes, 0, SerialBytes.Length);

                    //Clear screen capture resources
                    AppImport.CaptureFreeMemory(IntPtrBitmap);

                    await Task.Delay(setUpdateRate);
                }
            }
            catch { }
        }
    }
}