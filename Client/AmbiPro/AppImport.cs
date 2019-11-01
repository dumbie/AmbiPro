using System;
using System.Runtime.InteropServices;

namespace AmbiPro
{
    class AppImport
    {
        [DllImport("Resources\\ScreenCapture.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool CaptureFreeMemory(IntPtr FreeMemory);

        [DllImport("Resources\\ScreenCapture.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool CaptureInitialize(Int32 CaptureMonitor);

        [DllImport("Resources\\ScreenCapture.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool CaptureReset();

        [DllImport("Resources\\ScreenCapture.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr CaptureScreenshot(out Int32 OutputWidth, out Int32 OutputHeight, out Int32 OutputSize);
    }
}