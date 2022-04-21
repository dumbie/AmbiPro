namespace ScreenCapture
{
    public struct CaptureSettings
    {
        public CaptureSettings()
        {
            MonitorId = 0;
            MaxPixelDimension = 0;
            HDRtoSDR = true;
            HDRBrightness = 70.0F;
            Saturation = 1.0F;
            RedChannel = 1.0F;
            GreenChannel = 1.0F;
            BlueChannel = 1.0F;
            Brightness = 1.0F;
            Contrast = 0.0F;
            Gamma = 1.1F;
        }
        public int MonitorId { get; set; }
        public int MaxPixelDimension { get; set; }
        public bool HDRtoSDR { get; set; }
        public float HDRBrightness { get; set; }
        public float Saturation { get; set; }
        public float RedChannel { get; set; }
        public float GreenChannel { get; set; }
        public float BlueChannel { get; set; }
        public float Brightness { get; set; }
        public float Contrast { get; set; }
        public float Gamma { get; set; }
    }

    public struct CaptureDetails
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public bool HDREnabled { get; set; }
        public float SDRWhiteLevel { get; set; }
        public int PixelByteSize { get; set; }
        public int WidthByteSize { get; set; }
        public int TotalByteSize { get; set; }
    }
}