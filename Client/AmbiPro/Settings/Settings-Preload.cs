using ArnoldVinkCode;
using System;
using System.Configuration;
using System.Diagnostics;
using static AmbiPro.AppEnums;
using static AmbiPro.AppVariables;
using static AmbiPro.SerialMonitor;

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
        public static int setLedSmoothing = 0;
        public static int setLedBottomGap = 0;

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
        public static bool setLedStripCorrection = true;
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
        public static bool setDebugSave = false;

        //Update led setting variables
        public static void UpdateSettingsPreload()
        {
            try
            {
                //Device settings
                setSerialPortName = "COM" + ConfigurationManager.AppSettings["ComPort"].ToString();
                setSerialBaudRate = Convert.ToInt32(ConfigurationManager.AppSettings["BaudRate"]);

                //Led settings
                setAdjustBlackBars = Convert.ToBoolean(ConfigurationManager.AppSettings["AdjustBlackBars"]);
                setAdjustBlackbarRate = Convert.ToInt32(ConfigurationManager.AppSettings["AdjustBlackbarRate"]);
                setAdjustBlackbarRange = Convert.ToInt32(ConfigurationManager.AppSettings["AdjustBlackbarRange"]);
                setAdjustBlackBarBrightness = Convert.ToInt32(ConfigurationManager.AppSettings["AdjustBlackBarBrightness"]);
                setUpdateRate = Convert.ToInt32(ConfigurationManager.AppSettings["UpdateRate"]);
                setLedSmoothing = Convert.ToInt32(ConfigurationManager.AppSettings["LedSmoothing"]);
                setLedBottomGap = Convert.ToInt32(ConfigurationManager.AppSettings["LedBottomGap"]);

                setLedSaturation = AVSettings.Load(vConfiguration, "LedSaturation", typeof(float)) / 100;
                setLedColorRed = AVSettings.Load(vConfiguration, "LedColorRed", typeof(float)) / 100;
                setLedColorGreen = AVSettings.Load(vConfiguration, "LedColorGreen", typeof(float)) / 100;
                setLedColorBlue = AVSettings.Load(vConfiguration, "LedColorBlue", typeof(float)) / 100;
                setLedContrast = AVSettings.Load(vConfiguration, "LedContrast", typeof(float)) / 100;
                setLedBrightness = AVSettings.Load(vConfiguration, "LedBrightness", typeof(float)) / 100;
                setLedGamma = AVSettings.Load(vConfiguration, "LedGamma3", typeof(float));

                setLedMinBrightness = Convert.ToByte(ConfigurationManager.AppSettings["LedMinBrightness"]);
                setLedMinColor = Convert.ToInt32(ConfigurationManager.AppSettings["LedMinColor2"]);
                setColorLoopSpeed = Convert.ToInt32(ConfigurationManager.AppSettings["ColorLoopSpeed"]);
                setSpectrumRotationSpeed = Convert.ToInt32(ConfigurationManager.AppSettings["SpectrumRotationSpeed"]);
                setSolidLedColor = ConfigurationManager.AppSettings["SolidLedColor"].ToString();
                setLedStripCorrection = AVSettings.Load(vConfiguration, "LedStripCorrection", typeof(bool));
                setLedCaptureRange = Convert.ToInt32(ConfigurationManager.AppSettings["LedCaptureRange"]);
                setLedOutput = (LedOutputTypes)Convert.ToInt32(ConfigurationManager.AppSettings["LedOutput"]);
                setLedMode = Convert.ToInt32(ConfigurationManager.AppSettings["LedMode"]);
                setLedSideFirst = (LedSideTypes)Convert.ToInt32(ConfigurationManager.AppSettings["LedSideFirst"]);
                setLedSideSecond = (LedSideTypes)Convert.ToInt32(ConfigurationManager.AppSettings["LedSideSecond"]);
                setLedSideThird = (LedSideTypes)Convert.ToInt32(ConfigurationManager.AppSettings["LedSideThird"]);
                setLedSideFourth = (LedSideTypes)Convert.ToInt32(ConfigurationManager.AppSettings["LedSideFourth"]);
                setLedCountFirst = Convert.ToInt32(ConfigurationManager.AppSettings["LedCountFirst"]);
                setLedCountSecond = Convert.ToInt32(ConfigurationManager.AppSettings["LedCountSecond"]);
                setLedCountThird = Convert.ToInt32(ConfigurationManager.AppSettings["LedCountThird"]);
                setLedCountFourth = Convert.ToInt32(ConfigurationManager.AppSettings["LedCountFourth"]);
                setLedCountTotal = setLedCountFirst + setLedCountSecond + setLedCountThird + setLedCountFourth;

                //Debug settings
                setDebugBlackBar = Convert.ToBoolean(ConfigurationManager.AppSettings["DebugBlackBar"]);
                setDebugLedPreview = Convert.ToBoolean(ConfigurationManager.AppSettings["DebugLedPreview"]);
                setDebugColor = Convert.ToBoolean(ConfigurationManager.AppSettings["DebugColor"]);
                setDebugSave = Convert.ToBoolean(ConfigurationManager.AppSettings["DebugSave"]);

                //Update capture variables
                UpdateCaptureVariables();

                //Update the rotation based on ratio
                if (AVSettings.Check(vConfiguration, "LedRotate" + vCurrentRatio))
                {
                    setLedRotate = Convert.ToInt32(ConfigurationManager.AppSettings["LedRotate" + vCurrentRatio]);
                }
                else
                {
                    setLedRotate = 0;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed updating the settings: " + ex.Message);
            }
        }
    }
}