using ArnoldVinkCode;
using System.Configuration;
using System.Diagnostics;
using static AmbiPro.AppVariables;

namespace AmbiPro.Settings
{
    public partial class SettingsFunction
    {
        //Check - Application Settings
        public static void SettingsCheck()
        {
            try
            {
                Debug.WriteLine("Checking application settings...");

                //Check - First launch
                if (ConfigurationManager.AppSettings["FirstLaunch2"] == null)
                {
                    AVSettings.Save(vConfiguration, "FirstLaunch2", "True");
                }

                //Check - Com Port
                if (ConfigurationManager.AppSettings["ComPort"] == null)
                {
                    AVSettings.Save(vConfiguration, "ComPort", "1");
                }

                //Check - Baud Rate
                if (ConfigurationManager.AppSettings["BaudRate"] == null)
                {
                    AVSettings.Save(vConfiguration, "BaudRate", "115200");
                }

                //Check - Enable or Disable Led Automatic
                if (ConfigurationManager.AppSettings["LedAutoOnOffBefore"] == null)
                {
                    AVSettings.Save(vConfiguration, "LedAutoOnOffBefore", "False");
                }
                if (ConfigurationManager.AppSettings["LedAutoTimeBefore"] == null)
                {
                    AVSettings.Save(vConfiguration, "LedAutoTimeBefore", "01/01/1970 9:00:00");
                }
                if (ConfigurationManager.AppSettings["LedAutoOnOffAfter"] == null)
                {
                    AVSettings.Save(vConfiguration, "LedAutoOnOffAfter", "False");
                }
                if (ConfigurationManager.AppSettings["LedAutoTimeAfter"] == null)
                {
                    AVSettings.Save(vConfiguration, "LedAutoTimeAfter", "01/01/1970 17:00:00");
                }

                //Check - Server Port
                if (ConfigurationManager.AppSettings["ServerPort"] == null)
                {
                    AVSettings.Save(vConfiguration, "ServerPort", "1020");
                }

                //Check - Monitor Capture
                if (ConfigurationManager.AppSettings["MonitorCapture"] == null)
                {
                    AVSettings.Save(vConfiguration, "MonitorCapture", "0");
                }

                //Check - Led Mode
                if (ConfigurationManager.AppSettings["LedMode"] == null)
                {
                    AVSettings.Save(vConfiguration, "LedMode", "0");
                }

                //Check - Adjust to Blackbars
                if (ConfigurationManager.AppSettings["AdjustBlackBars"] == null)
                {
                    AVSettings.Save(vConfiguration, "AdjustBlackBars", "True");
                }

                //Check - Blackbar Update Range
                if (ConfigurationManager.AppSettings["AdjustBlackbarRange"] == null)
                {
                    AVSettings.Save(vConfiguration, "AdjustBlackbarRange", "20");
                }

                //Check - Adjust Blackbar Brightness
                if (ConfigurationManager.AppSettings["AdjustBlackBarBrightness"] == null)
                {
                    AVSettings.Save(vConfiguration, "AdjustBlackBarBrightness", "1");
                }

                //Check - Led Bottom Gap
                if (ConfigurationManager.AppSettings["LedBottomGap"] == null)
                {
                    AVSettings.Save(vConfiguration, "LedBottomGap", "0");
                }

                //Check - Led Contrast Level
                if (ConfigurationManager.AppSettings["LedContrast"] == null)
                {
                    AVSettings.Save(vConfiguration, "LedContrast", "0");
                }

                //Check - Led Brightness Level
                if (ConfigurationManager.AppSettings["LedBrightness"] == null)
                {
                    AVSettings.Save(vConfiguration, "LedBrightness", "90");
                }

                //Check - Led Gamma
                if (ConfigurationManager.AppSettings["LedGamma3"] == null)
                {
                    AVSettings.Save(vConfiguration, "LedGamma3", "2.00");
                }

                //Check - Led Saturation
                if (ConfigurationManager.AppSettings["LedSaturation"] == null)
                {
                    AVSettings.Save(vConfiguration, "LedSaturation", "110");
                }

                //Check - Color Loop Speed
                if (ConfigurationManager.AppSettings["ColorLoopSpeed"] == null)
                {
                    AVSettings.Save(vConfiguration, "ColorLoopSpeed", "75");
                }

                //Check - Spectrum Rotation Speed
                if (ConfigurationManager.AppSettings["SpectrumRotationSpeed"] == null)
                {
                    AVSettings.Save(vConfiguration, "SpectrumRotationSpeed", "20");
                }

                //Check - Solid Led Color
                if (ConfigurationManager.AppSettings["SolidLedColor"] == null)
                {
                    AVSettings.Save(vConfiguration, "SolidLedColor", "#FFA500");
                }

                //Check - Led Minimum Brightness
                if (ConfigurationManager.AppSettings["LedMinBrightness"] == null)
                {
                    AVSettings.Save(vConfiguration, "LedMinBrightness", "0");
                }

                //Check - Led Minimum Color
                if (ConfigurationManager.AppSettings["LedMinColor2"] == null)
                {
                    AVSettings.Save(vConfiguration, "LedMinColor2", "2");
                }

                //Check - Led Color Red
                if (ConfigurationManager.AppSettings["LedColorRed"] == null)
                {
                    AVSettings.Save(vConfiguration, "LedColorRed", "100");
                }

                //Check - Led Color Green
                if (ConfigurationManager.AppSettings["LedColorGreen"] == null)
                {
                    AVSettings.Save(vConfiguration, "LedColorGreen", "100");
                }

                //Check - Led Color Blue
                if (ConfigurationManager.AppSettings["LedColorBlue"] == null)
                {
                    AVSettings.Save(vConfiguration, "LedColorBlue", "100");
                }

                //Check - Led Capture Range
                if (ConfigurationManager.AppSettings["LedCaptureRange"] == null)
                {
                    AVSettings.Save(vConfiguration, "LedCaptureRange", "20");
                }

                //Check - Led Side Types
                if (ConfigurationManager.AppSettings["LedSideFirst"] == null)
                {
                    AVSettings.Save(vConfiguration, "LedSideFirst", "0");
                }
                if (ConfigurationManager.AppSettings["LedSideSecond"] == null)
                {
                    AVSettings.Save(vConfiguration, "LedSideSecond", "0");
                }
                if (ConfigurationManager.AppSettings["LedSideThird"] == null)
                {
                    AVSettings.Save(vConfiguration, "LedSideThird", "0");
                }
                if (ConfigurationManager.AppSettings["LedSideFourth"] == null)
                {
                    AVSettings.Save(vConfiguration, "LedSideFourth", "0");
                }

                //Check - Led Count
                if (ConfigurationManager.AppSettings["LedCountFirst"] == null)
                {
                    AVSettings.Save(vConfiguration, "LedCountFirst", "0");
                }
                if (ConfigurationManager.AppSettings["LedCountSecond"] == null)
                {
                    AVSettings.Save(vConfiguration, "LedCountSecond", "0");
                }
                if (ConfigurationManager.AppSettings["LedCountThird"] == null)
                {
                    AVSettings.Save(vConfiguration, "LedCountThird", "0");
                }
                if (ConfigurationManager.AppSettings["LedCountFourth"] == null)
                {
                    AVSettings.Save(vConfiguration, "LedCountFourth", "0");
                }

                //Check - Led Output
                if (ConfigurationManager.AppSettings["LedOutput"] == null)
                {
                    AVSettings.Save(vConfiguration, "LedOutput", "0");
                }

                //Check - Update Rate
                if (ConfigurationManager.AppSettings["UpdateRate"] == null)
                {
                    AVSettings.Save(vConfiguration, "UpdateRate", "20");
                }

                //Check - Led Smoothing
                if (ConfigurationManager.AppSettings["LedSmoothing"] == null)
                {
                    AVSettings.Save(vConfiguration, "LedSmoothing", "5");
                }

                //Check - Debug Mode
                if (ConfigurationManager.AppSettings["DebugBlackBar"] == null)
                {
                    AVSettings.Save(vConfiguration, "DebugBlackBar", "False");
                }
                if (ConfigurationManager.AppSettings["DebugLedPreview"] == null)
                {
                    AVSettings.Save(vConfiguration, "DebugLedPreview", "True");
                }
                if (ConfigurationManager.AppSettings["DebugColor"] == null)
                {
                    AVSettings.Save(vConfiguration, "DebugColor", "True");
                }
            }
            catch
            {
                Debug.WriteLine("Failed to check the settings.");
            }
        }
    }
}