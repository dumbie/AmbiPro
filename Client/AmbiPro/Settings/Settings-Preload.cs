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
        public static int setAdjustBlackbarRange = 0;
        public static int setAdjustBlackBarBrightness = 0;
        public static int setAdjustBlackBarUpdateRate = 0;
        public static int setUpdateRate = 0;
        public static int setLedSmoothing = 0;
        public static int setLedBottomGap = 0;
        public static bool setLedEnergyMode = true;

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
        public static bool setDebugBlackBar = false;
        public static bool setDebugLedPreview = false;
        public static bool setDebugColor = true;

        //Update led setting variables
        public static void UpdateSettingsPreload()
        {
            try
            {
                //Debug.WriteLine("Updating preload settings.");

                //Device settings
                setSerialPortName = "COM" + SettingLoad(vConfiguration, "ComPort", typeof(string));
                setSerialBaudRate = SettingLoad(vConfiguration, "BaudRate", typeof(int));

                //Led settings
                setAdjustBlackBars = SettingLoad(vConfiguration, "AdjustBlackBars", typeof(bool));
                setAdjustBlackbarRange = SettingLoad(vConfiguration, "AdjustBlackbarRange", typeof(int));
                setAdjustBlackBarBrightness = SettingLoad(vConfiguration, "AdjustBlackBarBrightness", typeof(int));
                setAdjustBlackBarUpdateRate = SettingLoad(vConfiguration, "AdjustBlackbarUpdateRate", typeof(int));
                setUpdateRate = SettingLoad(vConfiguration, "UpdateRate", typeof(int));
                setLedSmoothing = SettingLoad(vConfiguration, "LedSmoothing", typeof(int));
                setLedBottomGap = SettingLoad(vConfiguration, "LedBottomGap", typeof(int));
                setLedEnergyMode = SettingLoad(vConfiguration, "LedEnergyMode", typeof(bool));

                setLedSaturation = SettingLoad(vConfiguration, "LedSaturation", typeof(float)) / 100;
                setLedColorRed = SettingLoad(vConfiguration, "LedColorRed", typeof(float)) / 100;
                setLedColorGreen = SettingLoad(vConfiguration, "LedColorGreen", typeof(float)) / 100;
                setLedColorBlue = SettingLoad(vConfiguration, "LedColorBlue", typeof(float)) / 100;
                setLedContrast = SettingLoad(vConfiguration, "LedContrast", typeof(float)) / 100;
                setLedBrightness = SettingLoad(vConfiguration, "LedBrightness", typeof(float)) / 100;
                setLedGamma = SettingLoad(vConfiguration, "LedGamma3", typeof(float));

                setLedMinBrightness = SettingLoad(vConfiguration, "LedMinBrightness", typeof(byte));
                setLedMinColor = SettingLoad(vConfiguration, "LedMinColor2", typeof(int));
                setColorLoopSpeed = SettingLoad(vConfiguration, "ColorLoopSpeed", typeof(int));
                setSpectrumRotationSpeed = SettingLoad(vConfiguration, "SpectrumRotationSpeed", typeof(int));
                setSolidLedColor = SettingLoad(vConfiguration, "SolidLedColor", typeof(string));
                setLedCaptureRange = SettingLoad(vConfiguration, "LedCaptureRange", typeof(int));
                setLedMode = SettingLoad(vConfiguration, "LedMode", typeof(int));
                setLedOutput = (LedOutputTypes)SettingLoad(vConfiguration, "LedOutput", typeof(int));
                setLedSideFirst = (LedSideTypes)SettingLoad(vConfiguration, "LedSideFirst", typeof(int));
                setLedSideSecond = (LedSideTypes)SettingLoad(vConfiguration, "LedSideSecond", typeof(int));
                setLedSideThird = (LedSideTypes)SettingLoad(vConfiguration, "LedSideThird", typeof(int));
                setLedSideFourth = (LedSideTypes)SettingLoad(vConfiguration, "LedSideFourth", typeof(int));
                setLedCountFirst = SettingLoad(vConfiguration, "LedCountFirst", typeof(int));
                setLedCountSecond = SettingLoad(vConfiguration, "LedCountSecond", typeof(int));
                setLedCountThird = SettingLoad(vConfiguration, "LedCountThird", typeof(int));
                setLedCountFourth = SettingLoad(vConfiguration, "LedCountFourth", typeof(int));
                setLedCountTotal = setLedCountFirst + setLedCountSecond + setLedCountThird + setLedCountFourth;

                //Debug settings
                setDebugBlackBar = SettingLoad(vConfiguration, "DebugBlackBar", typeof(bool));
                setDebugLedPreview = SettingLoad(vConfiguration, "DebugLedPreview", typeof(bool));
                setDebugColor = SettingLoad(vConfiguration, "DebugColor", typeof(bool));

                //Update capture variables
                UpdateCaptureVariables();

                //Update the rotation based on ratio
                if (SettingCheck(vConfiguration, "LedRotate" + vCurrentRatio))
                {
                    setLedRotate = SettingLoad(vConfiguration, "LedRotate" + vCurrentRatio, typeof(int));
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