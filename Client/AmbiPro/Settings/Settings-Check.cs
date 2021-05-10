using System.Configuration;
using System.Diagnostics;

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
                    SettingsFunction.Save("FirstLaunch2", "True");
                }

                //Check - Com Port
                if (ConfigurationManager.AppSettings["ComPort"] == null)
                {
                    SettingsFunction.Save("ComPort", "9");
                }

                //Check - Baud Rate
                if (ConfigurationManager.AppSettings["BaudRate"] == null)
                {
                    SettingsFunction.Save("BaudRate", "115200");
                }

                //Check - Enable or Disable Led Automatic
                if (ConfigurationManager.AppSettings["LedAutoOnOff"] == null)
                {
                    SettingsFunction.Save("LedAutoOnOff", "False");
                }

                //Check - Time Led Automatic
                if (ConfigurationManager.AppSettings["LedAutoTime"] == null)
                {
                    SettingsFunction.Save("LedAutoTime", "01/01/1970 17:00:00");
                }

                //Check - Remote Port
                if (ConfigurationManager.AppSettings["ServerPort"] == null)
                {
                    SettingsFunction.Save("ServerPort", "1020");
                }

                //Check - Monitor Capture
                if (ConfigurationManager.AppSettings["MonitorCapture"] == null)
                {
                    SettingsFunction.Save("MonitorCapture", "0");
                }

                //Check - Led Mode
                if (ConfigurationManager.AppSettings["LedMode"] == null)
                {
                    SettingsFunction.Save("LedMode", "0");
                }

                //Check - Adjust to Blackbars
                if (ConfigurationManager.AppSettings["AdjustBlackBars"] == null)
                {
                    SettingsFunction.Save("AdjustBlackBars", "True");
                }

                //Check - Blackbar Update Rate
                if (ConfigurationManager.AppSettings["AdjustBlackbarRate"] == null)
                {
                    SettingsFunction.Save("AdjustBlackbarRate", "1000");
                }

                //Check - Adjust Blackbar Brightness
                if (ConfigurationManager.AppSettings["AdjustBlackBarBrightness"] == null)
                {
                    SettingsFunction.Save("AdjustBlackBarBrightness", "3");
                }

                //Check - Led Bottom Gap
                if (ConfigurationManager.AppSettings["LedBottomGap"] == null)
                {
                    SettingsFunction.Save("LedBottomGap", "0");
                }

                //Check - Led Brightness
                if (ConfigurationManager.AppSettings["LedBrightness"] == null)
                {
                    SettingsFunction.Save("LedBrightness", "75");
                }

                //Check - Led Minimum Brightness
                if (ConfigurationManager.AppSettings["LedMinBrightness"] == null)
                {
                    SettingsFunction.Save("LedMinBrightness", "5");
                }

                //Check - Led Gamma
                if (ConfigurationManager.AppSettings["LedGamma"] == null)
                {
                    SettingsFunction.Save("LedGamma", "75");
                }

                //Check - Led Vibrance
                if (ConfigurationManager.AppSettings["LedVibrance"] == null)
                {
                    SettingsFunction.Save("LedVibrance", "30");
                }

                //Check - Color Loop Speed
                if (ConfigurationManager.AppSettings["ColorLoopSpeed"] == null)
                {
                    SettingsFunction.Save("ColorLoopSpeed", "75");
                }

                //Check - Spectrum Rotation Speed
                if (ConfigurationManager.AppSettings["SpectrumRotationSpeed"] == null)
                {
                    SettingsFunction.Save("SpectrumRotationSpeed", "20");
                }

                //Check - Solid Led Color
                if (ConfigurationManager.AppSettings["SolidLedColor"] == null)
                {
                    SettingsFunction.Save("SolidLedColor", "#ff9900");
                }

                //Check - Led Hue
                if (ConfigurationManager.AppSettings["LedHue"] == null)
                {
                    SettingsFunction.Save("LedHue", "100");
                }

                //Check - Led Color Cut
                if (ConfigurationManager.AppSettings["LedColorCut"] == null)
                {
                    SettingsFunction.Save("LedColorCut", "3");
                }

                //Check - Led Color Red
                if (ConfigurationManager.AppSettings["LedColorRed"] == null)
                {
                    SettingsFunction.Save("LedColorRed", "100");
                }

                //Check - Led Color Green
                if (ConfigurationManager.AppSettings["LedColorGreen"] == null)
                {
                    SettingsFunction.Save("LedColorGreen", "100");
                }

                //Check - Led Color Blue
                if (ConfigurationManager.AppSettings["LedColorBlue"] == null)
                {
                    SettingsFunction.Save("LedColorBlue", "100");
                }

                //Check - Led Capture Range
                if (ConfigurationManager.AppSettings["LedCaptureRange"] == null)
                {
                    SettingsFunction.Save("LedCaptureRange", "20");
                }

                //Check - Led Side Types
                if (ConfigurationManager.AppSettings["LedSideFirst"] == null)
                {
                    SettingsFunction.Save("LedSideFirst", "0");
                }
                if (ConfigurationManager.AppSettings["LedSideSecond"] == null)
                {
                    SettingsFunction.Save("LedSideSecond", "0");
                }
                if (ConfigurationManager.AppSettings["LedSideThird"] == null)
                {
                    SettingsFunction.Save("LedSideThird", "0");
                }
                if (ConfigurationManager.AppSettings["LedSideFourth"] == null)
                {
                    SettingsFunction.Save("LedSideFourth", "0");
                }

                //Check - Led Count
                if (ConfigurationManager.AppSettings["LedCountFirst"] == null)
                {
                    SettingsFunction.Save("LedCountFirst", "0");
                }
                if (ConfigurationManager.AppSettings["LedCountSecond"] == null)
                {
                    SettingsFunction.Save("LedCountSecond", "0");
                }
                if (ConfigurationManager.AppSettings["LedCountThird"] == null)
                {
                    SettingsFunction.Save("LedCountThird", "0");
                }
                if (ConfigurationManager.AppSettings["LedCountFourth"] == null)
                {
                    SettingsFunction.Save("LedCountFourth", "0");
                }

                //Check - Led Output
                if (ConfigurationManager.AppSettings["LedOutput"] == null)
                {
                    SettingsFunction.Save("LedOutput", "0");
                }

                //Check - Update Rate
                if (ConfigurationManager.AppSettings["UpdateRate"] == null)
                {
                    SettingsFunction.Save("UpdateRate", "30");
                }

                //Check - Debug Mode
                if (ConfigurationManager.AppSettings["DebugMode"] == null)
                {
                    SettingsFunction.Save("DebugMode", "False");
                }
                if (ConfigurationManager.AppSettings["DebugBlackBar"] == null)
                {
                    SettingsFunction.Save("DebugBlackBar", "False");
                }
                if (ConfigurationManager.AppSettings["DebugColor"] == null)
                {
                    SettingsFunction.Save("DebugColor", "True");
                }
                if (ConfigurationManager.AppSettings["DebugSave"] == null)
                {
                    SettingsFunction.Save("DebugSave", "False");
                }
            }
            catch
            {
                Debug.WriteLine("Failed to check the settings.");
            }
        }
    }
}