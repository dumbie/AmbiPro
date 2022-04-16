using System;
using System.Diagnostics;
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
                        double preCalc = 1.0 / targetAdjust;
                        double rAdjusted = 255 * Math.Pow(R / 255.0, preCalc);
                        double gAdjusted = 255 * Math.Pow(G / 255.0, preCalc);
                        double bAdjusted = 255 * Math.Pow(B / 255.0, preCalc);

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

            public void AdjustContrast(double targetAdjust)
            {
                try
                {
                    if (targetAdjust != 0)
                    {
                        ColorMatrix colorMatrix = new ColorMatrix()
                        {
                            Matrix40 = (float)targetAdjust,
                            Matrix41 = (float)targetAdjust,
                            Matrix42 = (float)targetAdjust,
                        };
                        ApplyColorMatrix(colorMatrix);
                        //Debug.WriteLine("Contrast adjusted: R" + R + "/G" + G + "/B" + B + "/T" + targetAdjust);
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
                        double preCalc = 1.0 - targetAdjust;
                        double redSaturation = 0.3086;
                        double greenSaturation = 0.6094;
                        double blueSaturation = 0.0820;
                        ColorMatrix colorMatrix = new ColorMatrix()
                        {
                            Matrix00 = (float)(preCalc * redSaturation + targetAdjust),
                            Matrix01 = (float)(preCalc * redSaturation),
                            Matrix02 = (float)(preCalc * redSaturation),
                            Matrix10 = (float)(preCalc * greenSaturation),
                            Matrix11 = (float)(preCalc * greenSaturation + targetAdjust),
                            Matrix12 = (float)(preCalc * greenSaturation),
                            Matrix20 = (float)(preCalc * blueSaturation),
                            Matrix21 = (float)(preCalc * blueSaturation),
                            Matrix22 = (float)(preCalc * blueSaturation + targetAdjust),
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

            public void AdjustTemperature(double targetAdjust)
            {
                try
                {
                    if (targetAdjust != 0)
                    {
                        double redTemp = 255;
                        if (targetAdjust > 66)
                        {
                            redTemp = targetAdjust - 60;
                            redTemp = 329.698727466 * Math.Pow(redTemp, -0.1332047592);
                            redTemp = ValidateColor(redTemp);
                        }

                        double greenTemp = 0;
                        if (targetAdjust <= 66)
                        {
                            greenTemp = targetAdjust;
                            greenTemp = (99.4708025861 * Math.Log(greenTemp)) - 161.1195681661;
                        }
                        else
                        {
                            greenTemp = targetAdjust - 60;
                            greenTemp = 288.1221695283 * Math.Pow(greenTemp, -0.0755148492);
                        }
                        greenTemp = ValidateColor(greenTemp);

                        double blueTemp = 255;
                        if (targetAdjust < 66)
                        {
                            if (targetAdjust <= 19)
                            {
                                blueTemp = 0;
                            }
                            else
                            {
                                blueTemp = targetAdjust - 10;
                                blueTemp = (138.5177312231 * Math.Log(blueTemp)) - 305.0447927307;
                                blueTemp = ValidateColor(blueTemp);
                            }
                        }

                        ColorMatrix colorMatrix = new ColorMatrix()
                        {
                            Matrix00 = (float)(redTemp / 255),
                            Matrix11 = (float)(greenTemp / 255),
                            Matrix22 = (float)(blueTemp / 255),
                        };
                        ApplyColorMatrix(colorMatrix);
                        //Debug.WriteLine("Temperature adjusted: R" + R + "/G" + G + "/B" + B + "/T" + targetAdjust);
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
                            Matrix00 = (float)targetAdjust,
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
                            Matrix11 = (float)targetAdjust,
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
                            Matrix22 = (float)targetAdjust,
                        };
                        ApplyColorMatrix(colorMatrix);
                        //Debug.WriteLine("Blue adjusted to: " + B + "/T" + targetAdjust);
                    }
                }
                catch { }
            }

            public void ApplyColorMatrix(ColorMatrix colorMatrix)
            {
                try
                {
                    //Adjust color matrix rgb
                    float rAdjusted = (R * colorMatrix[0, 0]) + (G * colorMatrix[1, 0]) + (B * colorMatrix[2, 0]) + (A * colorMatrix[3, 0]) + (255 * colorMatrix[4, 0]);
                    float gAdjusted = (R * colorMatrix[0, 1]) + (G * colorMatrix[1, 1]) + (B * colorMatrix[2, 1]) + (A * colorMatrix[3, 1]) + (255 * colorMatrix[4, 1]);
                    float bAdjusted = (R * colorMatrix[0, 2]) + (G * colorMatrix[1, 2]) + (B * colorMatrix[2, 2]) + (A * colorMatrix[3, 2]) + (255 * colorMatrix[4, 2]);
                    float aAdjusted = (R * colorMatrix[0, 3]) + (G * colorMatrix[1, 3]) + (B * colorMatrix[2, 3]) + (A * colorMatrix[3, 3]) + (255 * colorMatrix[4, 3]);

                    //Validate color matrix rgb
                    R = ValidateColor(rAdjusted);
                    G = ValidateColor(gAdjusted);
                    B = ValidateColor(bAdjusted);
                    A = ValidateColor(aAdjusted);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Failed to apply color matrix: " + ex.Message);
                }
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