using System;
using System.Runtime.InteropServices;

namespace ScreenCapturePreview
{
    class AppImport
    {
        [DllImport("Resources\\ScreenCapture.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool CaptureInitialize(int CaptureMonitorId, out int OutputWidth, out int OutputHeight, out int OutputTotalByteSize, out bool OutputHDREnabled, int MaxPixelDimension);

        [DllImport("Resources\\ScreenCapture.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool CaptureReset();

        [DllImport("Resources\\ScreenCapture.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool CaptureFreeMemory(IntPtr BitmapData);

        [DllImport("Resources\\ScreenCapture.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool CaptureSaveFileBmp(IntPtr BitmapData, [MarshalAs(UnmanagedType.LPWStr)] string FilePath);

        [DllImport("Resources\\ScreenCapture.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool CaptureSaveFileJpg(IntPtr BitmapData, [MarshalAs(UnmanagedType.LPWStr)] string FilePath, int SaveQuality);

        [DllImport("Resources\\ScreenCapture.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool CaptureSaveFilePng(IntPtr BitmapData, [MarshalAs(UnmanagedType.LPWStr)] string FilePath);

        [DllImport("Resources\\ScreenCapture.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr CaptureScreenshot();
    }
}