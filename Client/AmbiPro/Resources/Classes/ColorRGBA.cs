using System;
using System.Drawing.Imaging;
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
                        double gAdjusted = 255 * Math.Pow(G / 255.0, 1.0 / targetAdjust);
                        double bAdjusted = 255 * Math.Pow(B / 255.0, 1.0 / targetAdjust);

                        R = ValidateColor(rAdjusted);
                        G = ValidateColor(gAdjusted);
                        B = ValidateColor(bAdjusted);
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
                        ColorMatrix colorMatrix = new ColorMatrix()
                        {
                            Matrix00 = (float)targetAdjust,
                            Matrix11 = (float)targetAdjust,
                            Matrix22 = (float)targetAdjust,
                        };
                        ApplyColorMatrix(colorMatrix);
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
                        ColorMatrix colorMatrix = new ColorMatrix()
                        {
                            Matrix00 = (float)(0.213 + 0.787 * targetAdjust),
                            Matrix01 = (float)(0.715 - 0.715 * targetAdjust),
                            Matrix02 = (float)(0.072 - 0.072 * targetAdjust),
                            Matrix10 = (float)(0.213 - 0.213 * targetAdjust),
                            Matrix11 = (float)(0.715 + 0.285 * targetAdjust),
                            Matrix12 = (float)(0.072 - 0.072 * targetAdjust),
                            Matrix20 = (float)(0.213 - 0.213 * targetAdjust),
                            Matrix21 = (float)(0.715 - 0.715 * targetAdjust),
                            Matrix22 = (float)(0.072 + 0.928 * targetAdjust),
                        };
                        ApplyColorMatrix(colorMatrix);
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

                        ColorMatrix colorMatrix = new ColorMatrix()
                        {
                            Matrix00 = (float)(0.213 + cosine * 0.787 - sine * 0.213),
                            Matrix01 = (float)(0.715 - cosine * 0.715 - sine * 0.715),
                            Matrix02 = (float)(0.072 - cosine * 0.072 + sine * 0.928),
                            Matrix10 = (float)(0.213 - cosine * 0.213 + sine * 0.143),
                            Matrix11 = (float)(0.715 + cosine * 0.285 + sine * 0.140),
                            Matrix12 = (float)(0.072 - cosine * 0.072 - sine * 0.283),
                            Matrix20 = (float)(0.213 - cosine * 0.213 - sine * 0.787),
                            Matrix21 = (float)(0.715 - cosine * 0.715 + sine * 0.715),
                            Matrix22 = (float)(0.072 + cosine * 0.928 + sine * 0.072),
                        };
                        ApplyColorMatrix(colorMatrix);
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
                        ColorMatrix colorMatrix = new ColorMatrix()
                        {
                            Matrix00 = (float)(0.213 + 0.787 * targetAdjust),
                            Matrix01 = (float)(0.715 - 0.715 * targetAdjust),
                            Matrix02 = (float)(0.072 - 0.072 * targetAdjust),
                        };
                        ApplyColorMatrix(colorMatrix);
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
                        ColorMatrix colorMatrix = new ColorMatrix()
                        {
                            Matrix10 = (float)(0.213 - 0.213 * targetAdjust),
                            Matrix11 = (float)(0.715 + 0.285 * targetAdjust),
                            Matrix12 = (float)(0.072 - 0.072 * targetAdjust),
                        };
                        ApplyColorMatrix(colorMatrix);
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
                        ColorMatrix colorMatrix = new ColorMatrix()
                        {
                            Matrix20 = (float)(0.213 - 0.213 * targetAdjust),
                            Matrix21 = (float)(0.715 - 0.715 * targetAdjust),
                            Matrix22 = (float)(0.072 + 0.928 * targetAdjust),
                        };
                        ApplyColorMatrix(colorMatrix);
                        //Debug.WriteLine("Blue adjusted to: " + B + "/T" + targetAdjust);
                    }
                }
                catch { }
            }

            public void AdjustHdrReinhard()
            {
                try
                {
                    float rReinhard = (R / 1 + R);
                    float gReinhard = (G / 1 + G);
                    float bReinhard = (B / 1 + B);
                    R = ValidateColor(rReinhard);
                    G = ValidateColor(gReinhard);
                    B = ValidateColor(bReinhard);
                    //Debug.WriteLine("HDR Reinhard adjusted: " + R + " / G" + G + " / B" + B);
                }
                catch { }
            }

            public void ApplyColorMatrix(ColorMatrix colorMatrix)
            {
                try
                {
                    double rAdjusted = (R * colorMatrix.Matrix00) + (G * colorMatrix.Matrix01) + (B * colorMatrix.Matrix02) + (A * colorMatrix.Matrix03) + (255 * colorMatrix.Matrix40);
                    double gAdjusted = (R * colorMatrix.Matrix10) + (G * colorMatrix.Matrix11) + (B * colorMatrix.Matrix12) + (A * colorMatrix.Matrix13) + (255 * colorMatrix.Matrix41);
                    double bAdjusted = (R * colorMatrix.Matrix20) + (G * colorMatrix.Matrix21) + (B * colorMatrix.Matrix22) + (A * colorMatrix.Matrix23) + (255 * colorMatrix.Matrix42);
                    double aAdjusted = (R * colorMatrix.Matrix30) + (G * colorMatrix.Matrix31) + (B * colorMatrix.Matrix32) + (A * colorMatrix.Matrix33) + (255 * colorMatrix.Matrix43);
                    R = ValidateColor(rAdjusted);
                    G = ValidateColor(gAdjusted);
                    B = ValidateColor(bAdjusted);
                    A = ValidateColor(aAdjusted);
                }
                catch { }
            }

            public byte ValidateColor(double checkColor)
            {
                try
                {
                    if (checkColor < 0) { return 0; } else if (checkColor > 255) { return 255; }
                }
                catch { }
                return (byte)checkColor;
            }
        }
    }
}