using System;
using System.Runtime.InteropServices;

namespace ScreenCapture
{
    class CaptureImport
    {
        [DllImport("Resources\\ScreenCapture.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool CaptureInitialize(CaptureSettings captureSettings, out CaptureDetails captureDetails);

        [DllImport("Resources\\ScreenCapture.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool CaptureUpdateSettings(CaptureSettings captureSettings);

        [DllImport("Resources\\ScreenCapture.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool CaptureUpdateSDRWhiteLevel();

        [DllImport("Resources\\ScreenCapture.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool CaptureReset();

        [DllImport("Resources\\ScreenCapture.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool CaptureFreeMemory(IntPtr bitmapData);

        [DllImport("Resources\\ScreenCapture.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr CaptureScreenshot();

        [DllImport("Resources\\ScreenCapture.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool CaptureSaveFileBmp(IntPtr bitmapData, [MarshalAs(UnmanagedType.LPWStr)] string filePath);

        [DllImport("Resources\\ScreenCapture.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool CaptureSaveFileJpg(IntPtr bitmapData, [MarshalAs(UnmanagedType.LPWStr)] string filePath, int imageQualityPercentage);

        [DllImport("Resources\\ScreenCapture.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool CaptureSaveFilePng(IntPtr bitmapData, [MarshalAs(UnmanagedType.LPWStr)] string filePath);

        [DllImport("Resources\\ScreenCapture.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool CaptureSaveFileJxr(IntPtr bitmapData, [MarshalAs(UnmanagedType.LPWStr)] string filePath);
    }
}