using System;
using System.Diagnostics;
using static AmbiPro.AppEnums;
using static AmbiPro.AppVariables;
using static AmbiPro.SerialMonitor;
using static ArnoldVinkCode.AVSettings;

namespace AmbiPro
{
    public partial class PreloadSettings
    {
        //Device settings
        public static string setSerialPortName = string.Empty;
        public static int setSerialBaudRate = 0;

        //Led settings
        public static bool setAdjustBlackBars = true;
        public static int setAdjustBlackbarRate = 0;
        public static int setAdjustBlackbarRange = 0;
        public static int setAdjustBlackBarBrightness = 0;
        public static int setUpdateRate = 0;
        public static int setLedSmoothFrame = 0;
        public static int setLedSmoothObject = 0;
        public static int setLedBottomGap = 0;
        public static bool setLedEnergyMode = true;
        public static bool setLedOffMonitorSleep = true;

        public static float setLedSaturation = 0;
        public static float setLedColorRed = 0;
        public static float setLedColorGreen = 0;
        public static float setLedColorBlue = 0;
        public static float setLedContrast = 0;
        public static float setLedBrightness = 0;
        public static float setLedGamma = 0;

        public static int setColorLoopSpeed = 0;
        public static int setSpectrumRotationSpeed = 0;
        public static string setSolidLedColor = string.Empty;
        public static int setLedMinColor = 0;
        public static byte setLedMinBrightness = 0;
        public static LedOutputTypes setLedOutput = 0;
        public static int setLedCaptureRange = 0;
        public static int setLedRotate = 0;
        public static int setLedMode = 0;
        public static LedSideTypes setLedSideFirst = 0;
        public static LedSideTypes setLedSideSecond = 0;
        public static LedSideTypes setLedSideThird = 0;
        public static LedSideTypes setLedSideFourth = 0;
        public static int setLedCountFirst = 0;
        public static int setLedCountSecond = 0;
        public static int setLedCountThird = 0;
        public static int setLedCountFourth = 0;
        public static int setLedCountTotal = 0;

        //Debug settings
        public static bool setDebugLedPreview = false;
        public static bool setDebugColorLeftRight = true;
        public static bool setDebugColorTopBottom = true;

        //Update led setting variables
        public static void UpdateSettingsPreload()
        {
            try
            {
                //Debug.WriteLine("Updating preload settings.");

                //Device settings
                setSerialPortName = "COM" + vSettings.Load("ComPort", typeof(string));
                setSerialBaudRate = vSettings.Load("BaudRate", typeof(int));

                //Led settings
                setAdjustBlackBars = vSettings.Load("AdjustBlackBars", typeof(bool));
                setAdjustBlackbarRate = vSettings.Load("AdjustBlackbarRate2", typeof(int));
                setAdjustBlackbarRange = vSettings.Load("AdjustBlackbarRange", typeof(int));
                setAdjustBlackBarBrightness = vSettings.Load("AdjustBlackBarBrightness", typeof(int));
                setUpdateRate = vSettings.Load("UpdateRate", typeof(int));
                setLedSmoothFrame = vSettings.Load("LedSmoothFrame", typeof(int));
                setLedSmoothObject = vSettings.Load("LedSmoothObject", typeof(int));
                setLedBottomGap = vSettings.Load("LedBottomGap", typeof(int));
                setLedEnergyMode = vSettings.Load("LedEnergyMode", typeof(bool));
                setLedOffMonitorSleep = vSettings.Load("LedOffMonitorSleep", typeof(bool));

                setLedSaturation = vSettings.Load("LedSaturation", typeof(float)) / 100;
                setLedColorRed = vSettings.Load("LedColorRed", typeof(float)) / 100;
                setLedColorGreen = vSettings.Load("LedColorGreen", typeof(float)) / 100;
                setLedColorBlue = vSettings.Load("LedColorBlue", typeof(float)) / 100;
                setLedContrast = vSettings.Load("LedContrast", typeof(float)) / 100;
                setLedBrightness = vSettings.Load("LedBrightness", typeof(float)) / 100;
                setLedGamma = vSettings.Load("LedGamma4", typeof(float));

                setLedMinBrightness = vSettings.Load("LedMinBrightness", typeof(byte));
                setLedMinColor = vSettings.Load("LedMinColor2", typeof(int));
                setColorLoopSpeed = vSettings.Load("ColorLoopSpeed", typeof(int));
                setSpectrumRotationSpeed = vSettings.Load("SpectrumRotationSpeed", typeof(int));
                setSolidLedColor = vSettings.Load("SolidLedColor", typeof(string));
                setLedCaptureRange = vSettings.Load("LedCaptureRange", typeof(int));
                setLedMode = vSettings.Load("LedMode", typeof(int));
                setLedOutput = (LedOutputTypes)vSettings.Load("LedOutput", typeof(int));
                setLedSideFirst = (LedSideTypes)vSettings.Load("LedSideFirst", typeof(int));
                setLedSideSecond = (LedSideTypes)vSettings.Load("LedSideSecond", typeof(int));
                setLedSideThird = (LedSideTypes)vSettings.Load("LedSideThird", typeof(int));
                setLedSideFourth = (LedSideTypes)vSettings.Load("LedSideFourth", typeof(int));
                setLedCountFirst = vSettings.Load("LedCountFirst", typeof(int));
                setLedCountSecond = vSettings.Load("LedCountSecond", typeof(int));
                setLedCountThird = vSettings.Load("LedCountThird", typeof(int));
                setLedCountFourth = vSettings.Load("LedCountFourth", typeof(int));
                setLedCountTotal = setLedCountFirst + setLedCountSecond + setLedCountThird + setLedCountFourth;

                //Debug settings
                setDebugLedPreview = vSettings.Load("DebugLedPreview", typeof(bool));
                setDebugColorLeftRight = vSettings.Load("DebugColorLeftRight", typeof(bool));
                setDebugColorTopBottom = vSettings.Load("DebugColorTopBottom", typeof(bool));

                //Update capture variables
                UpdateCaptureVariables();

                //Update the rotation based on ratio
                if (vSettings.Check("LedRotate" + vCurrentRatio))
                {
                    setLedRotate = vSettings.Load("LedRotate" + vCurrentRatio, typeof(int));
                }
                else
                {
                    setLedRotate = 0;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed updating preload settings: " + ex.Message);
            }
        }
    }
}