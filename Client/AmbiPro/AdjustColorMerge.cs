using System;
using System.Collections.Generic;
using System.Diagnostics;
using static AmbiPro.AppClasses;

namespace AmbiPro
{
    public partial class AdjustColorMerge
    {
        //Merge two colors (Clamp)
        public static ColorRGBA ColorMergeClamp(ColorRGBA colorBase, ColorRGBA colorAdd)
        {
            try
            {
                int mergeAlpha = Math.Min(colorBase.A + colorAdd.A, 255);
                int mergeRed = Math.Min(colorBase.R + colorAdd.R, 255);
                int mergeGreen = Math.Min(colorBase.G + colorAdd.G, 255);
                int mergeBlue = Math.Min(colorBase.B + colorAdd.B, 255);
                return new ColorRGBA() { R = (byte)mergeRed, G = (byte)mergeGreen, B = (byte)mergeBlue, A = (byte)mergeAlpha };
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to merge colors: " + ex.Message);
                return null;
            }
        }

        //Merge two colors (Linear interpolation)
        public static ColorRGBA ColorMergeLerp(ColorRGBA colorBase, ColorRGBA colorAdd, float amountAdd)
        {
            try
            {
                if (amountAdd > 1.0F) { amountAdd = 1.0F; }
                float mergeAlpha = Single.Lerp(colorBase.A, colorAdd.A, amountAdd);
                float mergeRed = Single.Lerp(colorBase.R, colorAdd.R, amountAdd);
                float mergeGreen = Single.Lerp(colorBase.G, colorAdd.G, amountAdd);
                float mergeBlue = Single.Lerp(colorBase.B, colorAdd.B, amountAdd);
                return new ColorRGBA() { R = (byte)mergeRed, G = (byte)mergeGreen, B = (byte)mergeBlue, A = (byte)mergeAlpha };
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to merge colors: " + ex.Message);
                return null;
            }
        }

        //Merge two colors (Square root)
        public static ColorRGBA ColorMergeSqrt(ColorRGBA colorBase, ColorRGBA colorAdd, float amountAdd)
        {
            try
            {
                if (amountAdd > 1.0F) { amountAdd = 1.0F; }
                float amountWeight = 1.0F - amountAdd;
                double mergeAlpha = Math.Sqrt(amountWeight * Math.Pow(colorBase.A, 2) + amountAdd * Math.Pow(colorAdd.A, 2));
                double mergeRed = Math.Sqrt(amountWeight * Math.Pow(colorBase.R, 2) + amountAdd * Math.Pow(colorAdd.R, 2));
                double mergeGreen = Math.Sqrt(amountWeight * Math.Pow(colorBase.G, 2) + amountAdd * Math.Pow(colorAdd.G, 2));
                double mergeBlue = Math.Sqrt(amountWeight * Math.Pow(colorBase.B, 2) + amountAdd * Math.Pow(colorAdd.B, 2));
                return new ColorRGBA() { R = (byte)mergeRed, G = (byte)mergeGreen, B = (byte)mergeBlue, A = (byte)mergeAlpha };
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to merge colors: " + ex.Message);
                return null;
            }
        }

        //Merge two colors (Square root)
        public static ColorRGBA ColorMergeSqrt(ColorRGBA colorBase, ColorRGBA colorAdd)
        {
            try
            {
                double mergeAlpha = Math.Sqrt((Math.Pow(colorBase.A, 2) + Math.Pow(colorAdd.A, 2)) / 2);
                double mergeRed = Math.Sqrt((Math.Pow(colorBase.R, 2) + Math.Pow(colorAdd.R, 2)) / 2);
                double mergeGreen = Math.Sqrt((Math.Pow(colorBase.G, 2) + Math.Pow(colorAdd.G, 2)) / 2);
                double mergeBlue = Math.Sqrt((Math.Pow(colorBase.B, 2) + Math.Pow(colorAdd.B, 2)) / 2);
                return new ColorRGBA() { R = (byte)mergeRed, G = (byte)mergeGreen, B = (byte)mergeBlue, A = (byte)mergeAlpha };
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to merge colors: " + ex.Message);
                return null;
            }
        }

        //Merge multiple colors (Square root)
        public static ColorRGBA ColorMergeSqrt(List<ColorRGBA> colorArray)
        {
            try
            {
                int colorCount = colorArray.Count;
                double mergeAlpha = 0;
                double mergeRed = 0;
                double mergeGreen = 0;
                double mergeBlue = 0;
                foreach (ColorRGBA color in colorArray)
                {
                    mergeAlpha += Math.Pow(color.A, 2);
                    mergeRed += Math.Pow(color.R, 2);
                    mergeGreen += Math.Pow(color.G, 2);
                    mergeBlue += Math.Pow(color.B, 2);
                }
                mergeAlpha = Math.Sqrt(mergeAlpha / colorCount);
                mergeRed = Math.Sqrt(mergeRed / colorCount);
                mergeGreen = Math.Sqrt(mergeGreen / colorCount);
                mergeBlue = Math.Sqrt(mergeBlue / colorCount);
                return new ColorRGBA() { R = (byte)mergeRed, G = (byte)mergeGreen, B = (byte)mergeBlue, A = (byte)mergeAlpha };
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to merge colors: " + ex.Message);
                return null;
            }
        }

        //Merge two colors (Average)
        public static ColorRGBA ColorMergeAverage(ColorRGBA colorBase, ColorRGBA colorAdd)
        {
            try
            {
                int mergeAlpha = (colorBase.A + colorAdd.A) / 2;
                int mergeRed = (colorBase.R + colorAdd.R) / 2;
                int mergeGreen = (colorBase.G + colorAdd.G) / 2;
                int mergeBlue = (colorBase.B + colorAdd.B) / 2;
                return new ColorRGBA() { R = (byte)mergeRed, G = (byte)mergeGreen, B = (byte)mergeBlue, A = (byte)mergeAlpha };
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to merge colors: " + ex.Message);
                return null;
            }
        }

        //Merge multiple colors (Average)
        public static ColorRGBA ColorMergeAverage(List<ColorRGBA> colorArray)
        {
            try
            {
                int colorCount = colorArray.Count;
                int mergeAlpha = 0;
                int mergeRed = 0;
                int mergeGreen = 0;
                int mergeBlue = 0;
                foreach (ColorRGBA color in colorArray)
                {
                    mergeAlpha += color.A;
                    mergeRed += color.R;
                    mergeGreen += color.G;
                    mergeBlue += color.B;
                }
                mergeAlpha /= colorCount;
                mergeRed /= colorCount;
                mergeGreen /= colorCount;
                mergeBlue /= colorCount;
                return new ColorRGBA() { R = (byte)mergeRed, G = (byte)mergeGreen, B = (byte)mergeBlue, A = (byte)mergeAlpha };
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to merge colors: " + ex.Message);
                return null;
            }
        }
    }
}