using System;
using System.Drawing;

namespace AmbiPro.Resources
{
    public static class HslColor
    {
        public class HSL
        {
            public HSL()
            {
                _h = 0;
                _s = 0;
                _l = 0;
            }

            double _h;
            double _s;
            double _l;

            public double H
            {
                get { return _h; }
                set
                {
                    _h = value;
                    _h = _h > 1 ? 1 : _h < 0 ? 0 : _h;
                }
            }

            public double S
            {
                get { return _s; }
                set
                {
                    _s = value;
                    _s = _s > 1 ? 1 : _s < 0 ? 0 : _s;
                }
            }

            public double L
            {
                get { return _l; }
                set
                {
                    _l = value;
                    _l = _l > 1 ? 1 : _l < 0 ? 0 : _l;
                }
            }
        }

        /// <summary>
        /// Modifies an existing brightness level
        /// </summary>
        /// <remarks>
        /// To reduce brightness use a number smaller than 1. To increase brightness use a number larger than 1
        /// </remarks>
        /// <param name="Brightness">The luminance delta</param>
        /// <returns>An adjusted colour</returns>
        public static HSL ModifyBrightness(HSL hsl, double Brightness)
        {
            hsl.L *= Brightness;
            return hsl;
        }

        /// <summary>
        /// Modifies an existing Saturation level
        /// </summary>
        /// <remarks>
        /// To reduce Saturation use a number smaller than 1. To increase Saturation use a number larger than 1
        /// </remarks>
        /// <param name="Saturation">The saturation delta</param>
        /// <returns>An adjusted colour</returns>
        public static HSL ModifySaturation(HSL hsl, double Saturation)
        {
            hsl.S *= Saturation;
            return hsl;
        }

        /// <summary>
        /// Modifies an existing Hue level
        /// </summary>
        /// <remarks>
        /// To reduce Hue use a number smaller than 1. To increase Hue use a number larger than 1
        /// </remarks>
        /// <param name="c">The original colour</param>
        /// <param name="Hue">The Hue delta</param>
        /// <returns>An adjusted colour</returns>
        public static HSL ModifyHue(HSL hsl, double Hue)
        {
            hsl.H *= Hue;
            return hsl;
        }

        /// <summary>
        /// Converts a colour from HSL to RGB
        /// </summary>
        /// <remarks>Adapted from the algoritm in Foley and Van-Dam</remarks>
        /// <returns>A Color structure containing the equivalent RGB values</returns>
        public static Color ToColor(HSL hsl)
        {
            double r = 0, g = 0, b = 0;
            double temp1, temp2;

            if (hsl.L == 0) { r = g = b = 0; }
            else
            {
                if (hsl.S == 0) { r = g = b = hsl.L; }
                else
                {
                    temp2 = ((hsl.L <= 0.5) ? hsl.L * (1.0 + hsl.S) : hsl.L + hsl.S - (hsl.L * hsl.S));
                    temp1 = 2.0 * hsl.L - temp2;

                    double[] t3 = new double[] { hsl.H + 1.0 / 3.0, hsl.H, hsl.H - 1.0 / 3.0 };
                    double[] clr = new double[] { 0, 0, 0 };
                    for (Int32 i = 0; i < 3; i++)
                    {
                        if (t3[i] < 0) { t3[i] += 1.0; }
                        if (t3[i] > 1) { t3[i] -= 1.0; }

                        if (6.0 * t3[i] < 1.0) { clr[i] = temp1 + (temp2 - temp1) * t3[i] * 6.0; }
                        else if (2.0 * t3[i] < 1.0) { clr[i] = temp2; }
                        else if (3.0 * t3[i] < 2.0) { clr[i] = (temp1 + (temp2 - temp1) * ((2.0 / 3.0) - t3[i]) * 6.0); }
                        else { clr[i] = temp1; }
                    }
                    r = clr[0];
                    g = clr[1];
                    b = clr[2];
                }
            }

            return Color.FromArgb(0, (byte)(255 * r), (byte)(255 * g), (byte)(255 * b));
        }

        /// <summary>
        /// Converts RGB to HSL
        /// </summary>
        /// <returns>An HSL value</returns>
        public static HSL ToHsl(Color c)
        {
            HSL hsl = new HSL();

            hsl.H = c.GetHue() / 360.0;
            hsl.L = c.GetBrightness();
            hsl.S = c.GetSaturation();

            return hsl;
        }
    }
}