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
                AppImport.CaptureInitialize(0, out int vScreenOutputWidth, out int vScreenOutputHeight, out int vScreenOutputSize, out bool vScreenOutputHDR, 0);
                Debug.WriteLine("ScreenOutputWidth: " + vScreenOutputWidth);
                Debug.WriteLine("ScreenOutputHeight: " + vScreenOutputHeight);
                Debug.WriteLine("ScreenOutputSize: " + vScreenOutputSize);
                Debug.WriteLine("HDR enabled: " + vScreenOutputHDR);
                await Task.Delay(500);

                while (true)
                {
                    unsafe
                    {
                        //Capture screenshot
                        IntPtr BitmapIntPtr = AppImport.CaptureScreenshot();
                        byte* BitmapData = (byte*)BitmapIntPtr;

                        //Convert data to bitmap
                        Bitmap Bitmap = BitmapProcessing.BitmapConvertData(BitmapData, vScreenOutputWidth, vScreenOutputHeight, vScreenOutputSize, false);

                        //Capture free memory
                        AppImport.CaptureFreeMemory(BitmapIntPtr);

                        //Update screen capture preview
                        image_DebugPreview.Source = BitmapProcessing.BitmapToBitmapImage(Bitmap);

                        //Dispose the bitmap
                        Bitmap.Dispose();
                    }

                    await Task.Delay(100);
                }
            }
            catch { }
        }
    }
}