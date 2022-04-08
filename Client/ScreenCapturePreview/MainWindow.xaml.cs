using AmbiPro.Resources;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows;

namespace ScreenCapturePreview
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        //Handle window initialized event
        protected override async void OnSourceInitialized(EventArgs e)
        {
            try
            {
                Debug.WriteLine("Initializing screen capture: " + DateTime.Now);
                AppImport.CaptureInitialize(0, out int vScreenOutputWidth, out int vScreenOutputHeight, out int vScreenOutputSize, out bool vScreenOutputHDREnabled, 800);
                Debug.WriteLine("ScreenOutputWidth: " + vScreenOutputWidth);
                Debug.WriteLine("ScreenOutputHeight: " + vScreenOutputHeight);
                Debug.WriteLine("ScreenOutputSize: " + vScreenOutputSize);
                Debug.WriteLine("HDR enabled: " + vScreenOutputHDREnabled);
                await Task.Delay(500);

                while (true)
                {
                    IntPtr BitmapIntPtr = IntPtr.Zero;
                    Bitmap BitmapImage = null;
                    try
                    {
                        //Capture screenshot
                        try
                        {
                            BitmapIntPtr = AppImport.CaptureScreenshot();
                        }
                        catch { }

                        //Check screenshot
                        if (BitmapIntPtr == IntPtr.Zero)
                        {
                            Debug.WriteLine("Screenshot is corrupted, restarting capture: " + DateTime.Now);
                            AppImport.CaptureInitialize(0, out vScreenOutputWidth, out vScreenOutputHeight, out vScreenOutputSize, out vScreenOutputHDREnabled, 800);
                            Debug.WriteLine("ScreenOutputWidth: " + vScreenOutputWidth);
                            Debug.WriteLine("ScreenOutputHeight: " + vScreenOutputHeight);
                            Debug.WriteLine("ScreenOutputSize: " + vScreenOutputSize);
                            Debug.WriteLine("HDR enabled: " + vScreenOutputHDREnabled);
                            await Task.Delay(500);
                            continue;
                        }

                        unsafe
                        {
                            //Convert pointer to bytes
                            byte* BitmapData = (byte*)BitmapIntPtr;

                            //Convert data to bitmap
                            BitmapImage = BitmapProcessing.BitmapConvertData(BitmapData, vScreenOutputWidth, vScreenOutputHeight, vScreenOutputSize, false);
                        }

                        //Save screenshot directly
                        //AppImport.CaptureSaveBitmap(BitmapIntPtr, "E:\\Screenshot " + DateTime.Now.ToString("hh.mm (MM-dd-yyyy)") + ".bmp");

                        //Update screen capture preview
                        image_DebugPreview.Source = BitmapProcessing.BitmapToBitmapImage(BitmapImage);
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

                        await Task.Delay(500);
                    }
                }
            }
            catch { }
        }
    }
}