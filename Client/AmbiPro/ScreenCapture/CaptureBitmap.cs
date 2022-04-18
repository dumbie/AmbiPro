using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ScreenCapture
{
    class CaptureBitmap
    {
        //Convert BitmapIntPtr to BitmapByteArray
        public static byte[] BitmapIntPtrToBitmapByteArray(IntPtr bitmapIntPtr, CaptureDetails captureDetails)
        {
            try
            {
                byte[] bitmapByteArray = new byte[captureDetails.TotalByteSize];
                Marshal.Copy(bitmapIntPtr, bitmapByteArray, 0, captureDetails.TotalByteSize);
                return bitmapByteArray;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to convert BitmapIntPtr to BitmapByteArray: " + ex.Message);
                return null;
            }
        }

        //Convert BitmapByteArray to BitmapSource
        public static BitmapSource BitmapByteArrayToBitmapSource(byte[] bitmapByteArray, CaptureDetails captureDetails, CaptureSettings captureSettings)
        {
            try
            {
                PixelFormat bitmapPixelFormat = PixelFormats.Bgra32;
                if (captureDetails.HDREnabled && !captureSettings.HDRtoSDR)
                {
                    bitmapPixelFormat = PixelFormats.Rgba64; //Fix Rgba64Half support missing 
                }
                return BitmapSource.Create(captureDetails.Width, captureDetails.Height, 96, 96, bitmapPixelFormat, null, bitmapByteArray, captureDetails.WidthByteSize);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to convert BitmapByteArray to BitmapSource: " + ex.Message);
                return null;
            }
        }

        //Convert BitmapIntPtr to BitmapSource
        public static BitmapSource BitmapIntPtrToBitmapSource(IntPtr bitmapIntPtr, CaptureDetails captureDetails, CaptureSettings captureSettings)
        {
            try
            {
                byte[] bitmapByteArray = BitmapIntPtrToBitmapByteArray(bitmapIntPtr, captureDetails);
                return BitmapByteArrayToBitmapSource(bitmapByteArray, captureDetails, captureSettings);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to convert BitmapIntPtr to BitmapSource: " + ex.Message);
                return null;
            }
        }
    }
}