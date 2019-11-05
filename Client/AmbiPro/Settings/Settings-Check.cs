using System;
using System.Configuration;
using System.Diagnostics;
using static AmbiPro.AppVariables;

namespace AmbiPro.Settings
{
    public partial class FormSettings
    {
        //Check - Application Settings
        public void SettingsCheck()
        {
            try
            {
                Debug.WriteLine("Checking application settings...");

                //Check - First launch
                if (ConfigurationManager.AppSettings["FirstLaunch"] == null)
                {
                    SettingsFunction.Save("FirstLaunch", "True");
                }

                //Check - Last update check
                if (ConfigurationManager.AppSettings["AppUpdateCheck2"] == null)
                {
                    SettingsFunction.Save("AppUpdateCheck2", DateTime.Now.ToString(vAppCultureInfo));
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

                //Check - Adjust Black Bars
                if (ConfigurationManager.AppSettings["AdjustBlackBars"] == null)
                {
                    SettingsFunction.Save("AdjustBlackBars", "True");
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

                //Check - Adjust Black Bar Level
                if (ConfigurationManager.AppSettings["AdjustBlackBarLevel"] == null)
                {
                    SettingsFunction.Save("AdjustBlackBarLevel", "3");
                }

                //Check - Led Smoothing
                if (ConfigurationManager.AppSettings["LedSmoothing"] == null)
                {
                    SettingsFunction.Save("LedSmoothing", "2");
                }

                //Check - Led Count
                if (ConfigurationManager.AppSettings["LedCount"] == null)
                {
                    SettingsFunction.Save("LedCount", "60");
                }

                //Check - Led Output
                if (ConfigurationManager.AppSettings["LedOutput"] == null)
                {
                    SettingsFunction.Save("LedOutput", "0");
                }

                //Check - Led Rotate 16:9
                if (ConfigurationManager.AppSettings["LedRotate16:9"] == null)
                {
                    SettingsFunction.Save("LedRotate16:9", "0");
                }

                //Check - Led Rotate 4:3
                if (ConfigurationManager.AppSettings["LedRotate4:3"] == null)
                {
                    SettingsFunction.Save("LedRotate4:3", "0");
                }

                //Check - Update Rate
                if (ConfigurationManager.AppSettings["UpdateRate"] == null)
                {
                    SettingsFunction.Save("UpdateRate", "25");
                }

                //Check - Led Sides
                if (ConfigurationManager.AppSettings["LedSides"] == null)
                {
                    SettingsFunction.Save("LedSides", "2");
                }

                //Check - Led Direction
                if (ConfigurationManager.AppSettings["LedDirection"] == null)
                {
                    SettingsFunction.Save("LedDirection", "1");
                }
            }
            catch { Debug.WriteLine("Failed to check the settings."); }
        }
    }
}