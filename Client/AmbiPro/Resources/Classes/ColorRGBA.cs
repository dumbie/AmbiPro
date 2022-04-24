using System;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Globalization;
using System.Windows.Media;

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

            public SolidColorBrush AsSolidColorBrush()
            {
                try
                {
                    return new SolidColorBrush(Color.FromRgb(R, G, B));
                }
                catch
                {
                    return null;
                }
            }

            public void AdjustSaturation(float targetAdjust)
            {
                try
                {
                    if (targetAdjust == 1.0F) { return; }
                    float colorLuminance = (R + G + B) / 3.0F;

                    float rAdjusted;
                    float gAdjusted;
                    float bAdjusted;
                    if (targetAdjust > 1.0F)
                    {
                        rAdjusted = Math.Max(colorLuminance + (R - colorLuminance) * targetAdjust, R);
                        gAdjusted = Math.Max(colorLuminance + (G - colorLuminance) * targetAdjust, G);
                        bAdjusted = Math.Max(colorLuminance + (B - colorLuminance) * targetAdjust, B);
                    }
                    else
                    {
                        rAdjusted = colorLuminance + (R - colorLuminance) * targetAdjust;
                        gAdjusted = colorLuminance + (G - colorLuminance) * targetAdjust;
                        bAdjusted = colorLuminance + (B - colorLuminance) * targetAdjust;
                    }

                    R = ValidateColor(rAdjusted);
                    G = ValidateColor(gAdjusted);
                    B = ValidateColor(bAdjusted);
                    //Debug.WriteLine("Saturation adjusted: R" + R + "/G" + G + "/B" + B + "/T" + targetAdjust);
                }
                catch { }
            }

            public void AdjustColorChannels(float targetRed, float targetGreen, float targetBlue)
            {
                try
                {
                    if (targetRed == 1.0F && targetGreen == 1.0F && targetBlue == 1.0F) { return; }
                    ColorMatrix colorMatrix = new ColorMatrix()
                    {
                        Matrix00 = targetRed,
                        Matrix11 = targetGreen,
                        Matrix22 = targetBlue,
                    };
                    ApplyColorMatrix(colorMatrix);
                    //Debug.WriteLine("Color channels adjusted to: R" + R + "/G" + G + "/B" + B);
                }
                catch { }
            }

            public void AdjustBrightness(float targetAdjust)
            {
                try
                {
                    if (targetAdjust == 1.0F) { return; }
                    ColorMatrix colorMatrix = new ColorMatrix()
                    {
                        Matrix00 = targetAdjust,
                        Matrix11 = targetAdjust,
                        Matrix22 = targetAdjust,
                    };
                    ApplyColorMatrix(colorMatrix);
                    //Debug.WriteLine("Brightness adjusted: R" + R + "/G" + G + "/B" + B + "/T" + targetAdjust);
                }
                catch { }
            }

            public void AdjustContrast(float targetAdjust)
            {
                try
                {
                    if (targetAdjust == 0.0F) { return; }
                    ColorMatrix colorMatrix = new ColorMatrix()
                    {
                        Matrix40 = targetAdjust,
                        Matrix41 = targetAdjust,
                        Matrix42 = targetAdjust,
                    };
                    ApplyColorMatrix(colorMatrix);
                    //Debug.WriteLine("Contrast adjusted: R" + R + "/G" + G + "/B" + B + "/T" + targetAdjust);
                }
                catch { }
            }

            public void AdjustGamma(float targetAdjust)
            {
                try
                {
                    if (targetAdjust == 1.0F) { return; }

                    float rFloat = R / 255.0F;
                    float gFloat = G / 255.0F;
                    float bFloat = B / 255.0F;

                    float adjustGamma = targetAdjust / 1.0F;
                    float rAdjusted = (float)Math.Pow(rFloat, adjustGamma);
                    float gAdjusted = (float)Math.Pow(gFloat, adjustGamma);
                    float bAdjusted = (float)Math.Pow(bFloat, adjustGamma);

                    R = ValidateColor(255.0F * rAdjusted);
                    G = ValidateColor(255.0F * gAdjusted);
                    B = ValidateColor(255.0F * bAdjusted);

                    //Debug.WriteLine("Gamma adjusted: R" + R + "/G" + G + "/B" + B + "/T" + targetAdjust);

                }
                catch { }
            }

            public void AdjustLedColorCorrection(bool ledCorrection)
            {
                try
                {
                    //Check if led correction is enabled
                    if (!ledCorrection) { return; }

                    //Check color difference
                    int differenceBlueGreen = B - G;
                    if (differenceBlueGreen > 0 && R <= 10)
                    {
                        float adjustDifference = differenceBlueGreen / 255.0F;
                        float adjustRed = 1.0F - (0.3086F - adjustDifference);
                        float adjustGreen = 1.0F - (0.6094F - adjustDifference);
                        float adjustBlue = 1.0F - (0.0820F - adjustDifference);
                        AdjustColorChannels(adjustRed, adjustGreen, adjustBlue);
                    }
                }
                catch { }
            }

            public void ApplyColorMatrix(ColorMatrix colorMatrix)
            {
                try
                {
                    //Adjust color matrix rgb
                    float rAdjusted = (R * colorMatrix[0, 0]) + (G * colorMatrix[1, 0]) + (B * colorMatrix[2, 0]) + (A * colorMatrix[3, 0]) + (255.0F * colorMatrix[4, 0]);
                    float gAdjusted = (R * colorMatrix[0, 1]) + (G * colorMatrix[1, 1]) + (B * colorMatrix[2, 1]) + (A * colorMatrix[3, 1]) + (255.0F * colorMatrix[4, 1]);
                    float bAdjusted = (R * colorMatrix[0, 2]) + (G * colorMatrix[1, 2]) + (B * colorMatrix[2, 2]) + (A * colorMatrix[3, 2]) + (255.0F * colorMatrix[4, 2]);
                    float aAdjusted = (R * colorMatrix[0, 3]) + (G * colorMatrix[1, 3]) + (B * colorMatrix[2, 3]) + (A * colorMatrix[3, 3]) + (255.0F * colorMatrix[4, 3]);

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

            public byte ValidateColor(float checkColor)
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