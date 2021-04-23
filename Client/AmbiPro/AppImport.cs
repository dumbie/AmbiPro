using System;
using System.Runtime.InteropServices;

namespace AmbiPro
{
    class AppImport
    {
        [DllImport("Resources\\ScreenCapture.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool CaptureFreeMemory(IntPtr FreeMemory);

        [DllImport("Resources\\ScreenCapture.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool CaptureInitialize(int CaptureMonitor);

        [DllImport("Resources\\ScreenCapture.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool CaptureReset();

        [DllImport("Resources\\ScreenCapture.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr CaptureScreenshot(out int OutputWidth, out int OutputHeight, out int OutputSize);

        [DllImport("Resources\\ScreenCapture.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr CaptureResize(IntPtr CaptureBytes, int ResizeWidth, int ResizeHeight, out int OutputSize);
    }
}