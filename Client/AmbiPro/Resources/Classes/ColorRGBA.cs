using System;
using System.Globalization;

namespace AmbiPro
{
    public partial class AppClasses
    {
        public class ColorRGBA
        {
            public byte R = 0;
            public byte G = 0;
            public byte B = 0;
            public byte A = 0;
            public static ColorRGBA Red = new ColorRGBA() { R = 255 };
            public static ColorRGBA Green = new ColorRGBA() { G = 255 };
            public static ColorRGBA Blue = new ColorRGBA() { B = 255 };
            public static ColorRGBA Purple = new ColorRGBA() { R = 128, B = 128 };
            public static ColorRGBA Yellow = new ColorRGBA() { R = 255, G = 255 };
            public static ColorRGBA Orange = new ColorRGBA() { R = 255, G = 165 };
            public static ColorRGBA Black = new ColorRGBA() { R = 0, G = 0, B = 0 };
            public static ColorRGBA White = new ColorRGBA() { R = 255, G = 255, B = 255 };

            public static ColorRGBA HexToRGBA(string hexString)
            {
                try
                {
                    hexString = hexString.Replace("#", string.Empty);
                    if (hexString.Length == 6)
                    {
                        byte rHex = byte.Parse(hexString.Substring(0, 2), NumberStyles.AllowHexSpecifier);
                        byte gHex = byte.Parse(hexString.Substring(2, 2), NumberStyles.AllowHexSpecifier);
                        byte bHex = byte.Parse(hexString.Substring(4, 2), NumberStyles.AllowHexSpecifier);
                        return new ColorRGBA() { R = rHex, G = gHex, B = bHex };
                    }
                    else
                    {
                        byte aHex = byte.Parse(hexString.Substring(0, 2), NumberStyles.AllowHexSpecifier);
                        byte rHex = byte.Parse(hexString.Substring(2, 2), NumberStyles.AllowHexSpecifier);
                        byte gHex = byte.Parse(hexString.Substring(4, 2), NumberStyles.AllowHexSpecifier);
                        byte bHex = byte.Parse(hexString.Substring(6, 2), NumberStyles.AllowHexSpecifier);
                        return new ColorRGBA() { A = aHex, R = rHex, G = gHex, B = bHex };
                    }
                }
                catch
                {
                    return null;
                }
            }

            public static ColorRGBA Clone(ColorRGBA cloneSource)
            {
                try
                {
                    return new ColorRGBA() { R = cloneSource.R, G = cloneSource.G, B = cloneSource.B, A = cloneSource.A };
                }
                catch
                {
                    return null;
                }
            }

            public void AdjustGamma(double targetAdjust)
            {
                try
                {
                    if (targetAdjust != 1.00)
                    {
                        double rAdjusted = 255 * Math.Pow(R / 255.0, 1.0 / targetAdjust);
                        if (rAdjusted > 255) { rAdjusted = 255; }
                        if (rAdjusted < 0) { rAdjusted = 0; }

                        double gAdjusted = 255 * Math.Pow(G / 255.0, 1.0 / targetAdjust);
                        if (gAdjusted > 255) { gAdjusted = 255; }
                        if (gAdjusted < 0) { gAdjusted = 0; }

                        double bAdjusted = 255 * Math.Pow(B / 255.0, 1.0 / targetAdjust);
                        if (bAdjusted > 255) { bAdjusted = 255; }
                        if (bAdjusted < 0) { bAdjusted = 0; }

                        R = (byte)rAdjusted;
                        G = (byte)gAdjusted;
                        B = (byte)bAdjusted;
                        //Debug.WriteLine("Gamma adjusted: R" + R + "/G" + G + "/B" + B + "/T" + targetAdjust);
                    }
                }
                catch { }
            }

            public void AdjustBrightness(double targetAdjust)
            {
                try
                {
                    if (targetAdjust != 1.00)
                    {
                        double rAdjusted = R * targetAdjust;
                        if (rAdjusted > 255) { rAdjusted = 255; }
                        if (rAdjusted < 0) { rAdjusted = 0; }

                        double gAdjusted = G * targetAdjust;
                        if (gAdjusted > 255) { gAdjusted = 255; }
                        if (gAdjusted < 0) { gAdjusted = 0; }

                        double bAdjusted = B * targetAdjust;
                        if (bAdjusted > 255) { bAdjusted = 255; }
                        if (bAdjusted < 0) { bAdjusted = 0; }

                        R = (byte)rAdjusted;
                        G = (byte)gAdjusted;
                        B = (byte)bAdjusted;
                        //Debug.WriteLine("Brightness adjusted: R" + R + "/G" + G + "/B" + B + "/T" + targetAdjust);
                    }
                }
                catch { }
            }

            public void AdjustSaturation(double targetAdjust)
            {
                try
                {
                    if (targetAdjust != 1.00)
                    {
                        double rAdjusted = (0.213 + 0.787 * targetAdjust) * R + (0.715 - 0.715 * targetAdjust) * G + (0.072 - 0.072 * targetAdjust) * B;
                        if (rAdjusted > 255) { rAdjusted = 255; }
                        if (rAdjusted < 0) { rAdjusted = 0; }

                        double gAdjusted = (0.213 - 0.213 * targetAdjust) * R + (0.715 + 0.285 * targetAdjust) * G + (0.072 - 0.072 * targetAdjust) * B;
                        if (gAdjusted > 255) { gAdjusted = 255; }
                        if (gAdjusted < 0) { gAdjusted = 0; }

                        double bAdjusted = (0.213 - 0.213 * targetAdjust) * R + (0.715 - 0.715 * targetAdjust) * G + (0.072 + 0.928 * targetAdjust) * B;
                        if (bAdjusted > 255) { bAdjusted = 255; }
                        if (bAdjusted < 0) { bAdjusted = 0; }

                        R = (byte)rAdjusted;
                        G = (byte)gAdjusted;
                        B = (byte)bAdjusted;
                        //Debug.WriteLine("Saturation adjusted: R" + R + "/G" + G + "/B" + B + "/T" + targetAdjust);
                    }
                }
                catch { }
            }

            public void AdjustHue(double targetAdjust)
            {
                try
                {
                    if (targetAdjust != 0 && targetAdjust != 360)
                    {
                        double radian = Math.PI * targetAdjust / 180;
                        double cosine = Math.Cos(radian);
                        double sine = Math.Sin(radian);

                        double a00 = 0.213 + cosine * 0.787 - sine * 0.213;
                        double a01 = 0.213 - cosine * 0.213 + sine * 0.143;
                        double a02 = 0.213 - cosine * 0.213 - sine * 0.787;
                        double a10 = 0.715 - cosine * 0.715 - sine * 0.715;
                        double a11 = 0.715 + cosine * 0.285 + sine * 0.140;
                        double a12 = 0.715 - cosine * 0.715 + sine * 0.715;
                        double a20 = 0.072 - cosine * 0.072 + sine * 0.928;
                        double a21 = 0.072 - cosine * 0.072 - sine * 0.283;
                        double a22 = 0.072 + cosine * 0.928 + sine * 0.072;

                        double rAdjusted = R * a00 + G * a10 + B * a20;
                        if (rAdjusted > 255) { rAdjusted = 255; }
                        if (rAdjusted < 0) { rAdjusted = 0; }

                        double gAdjusted = R * a01 + G * a11 + B * a21;
                        if (gAdjusted > 255) { gAdjusted = 255; }
                        if (gAdjusted < 0) { gAdjusted = 0; }

                        double bAdjusted = R * a02 + G * a12 + B * a22;
                        if (bAdjusted > 255) { bAdjusted = 255; }
                        if (bAdjusted < 0) { bAdjusted = 0; }

                        R = (byte)rAdjusted;
                        G = (byte)gAdjusted;
                        B = (byte)bAdjusted;
                        //Debug.WriteLine("Hue adjusted: R" + R + "/G" + G + "/B" + B + "/T" + targetAdjust);
                    }
                }
                catch { }
            }

            public void AdjustColorRed(double targetAdjust)
            {
                try
                {
                    if (targetAdjust != 1.00)
                    {
                        double rAdjusted = (0.213 + 0.787 * targetAdjust) * R + (0.715 - 0.715 * targetAdjust) * G + (0.072 - 0.072 * targetAdjust) * B;
                        if (rAdjusted > 255) { rAdjusted = 255; }
                        if (rAdjusted < 0) { rAdjusted = 0; }

                        R = (byte)rAdjusted;
                        //Debug.WriteLine("Red adjusted to: " + R + "/T" + targetAdjust);
                    }
                }
                catch { }
            }

            public void AdjustColorGreen(double targetAdjust)
            {
                try
                {
                    if (targetAdjust != 1.00)
                    {
                        double gAdjusted = (0.213 - 0.213 * targetAdjust) * R + (0.715 + 0.285 * targetAdjust) * G + (0.072 - 0.072 * targetAdjust) * B;
                        if (gAdjusted > 255) { gAdjusted = 255; }
                        if (gAdjusted < 0) { gAdjusted = 0; }

                        G = (byte)gAdjusted;
                        //Debug.WriteLine("Green adjusted to: " + G + "/T" + targetAdjust);
                    }
                }
                catch { }
            }

            public void AdjustColorBlue(double targetAdjust)
            {
                try
                {
                    if (targetAdjust != 1.00)
                    {
                        double bAdjusted = (0.213 - 0.213 * targetAdjust) * R + (0.715 - 0.715 * targetAdjust) * G + (0.072 + 0.928 * targetAdjust) * B;
                        if (bAdjusted > 255) { bAdjusted = 255; }
                        if (bAdjusted < 0) { bAdjusted = 0; }

                        B = (byte)bAdjusted;
                        //Debug.WriteLine("Blue adjusted to: " + B + "/T" + targetAdjust);
                    }
                }
                catch { }
            }
        }
    }
}