using System.Diagnostics;
using static AmbiPro.AppVariables;
using static ArnoldVinkCode.AVSettings;

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
                if (!SettingCheck(vConfiguration, "FirstLaunch2"))
                {
                    SettingSave(vConfiguration, "FirstLaunch2", "True");
                }

                //Check - Com Port
                if (!SettingCheck(vConfiguration, "ComPort"))
                {
                    SettingSave(vConfiguration, "ComPort", "1");
                }

                //Check - Baud Rate
                if (!SettingCheck(vConfiguration, "BaudRate"))
                {
                    SettingSave(vConfiguration, "BaudRate", "115200");
                }

                //Check - Enable or Disable Led Automatic
                if (!SettingCheck(vConfiguration, "LedAutoOnOffBefore"))
                {
                    SettingSave(vConfiguration, "LedAutoOnOffBefore", "False");
                }
                if (!SettingCheck(vConfiguration, "LedAutoTimeBefore"))
                {
                    SettingSave(vConfiguration, "LedAutoTimeBefore", "01/01/1970 9:00:00");
                }
                if (!SettingCheck(vConfiguration, "LedAutoOnOffAfter"))
                {
                    SettingSave(vConfiguration, "LedAutoOnOffAfter", "False");
                }
                if (!SettingCheck(vConfiguration, "LedAutoTimeAfter"))
                {
                    SettingSave(vConfiguration, "LedAutoTimeAfter", "01/01/1970 17:00:00");
                }

                //Check - Server Port
                if (!SettingCheck(vConfiguration, "ServerPort"))
                {
                    SettingSave(vConfiguration, "ServerPort", "1020");
                }

                //Check - Monitor Capture
                if (!SettingCheck(vConfiguration, "MonitorCapture"))
                {
                    SettingSave(vConfiguration, "MonitorCapture", "0");
                }

                //Check - Led Mode
                if (!SettingCheck(vConfiguration, "LedMode"))
                {
                    SettingSave(vConfiguration, "LedMode", "0");
                }

                //Check - Adjust to Blackbars
                if (!SettingCheck(vConfiguration, "AdjustBlackBars"))
                {
                    SettingSave(vConfiguration, "AdjustBlackBars", "True");
                }

                //Check - Blackbar Update Range
                if (!SettingCheck(vConfiguration, "AdjustBlackbarRange"))
                {
                    SettingSave(vConfiguration, "AdjustBlackbarRange", "20");
                }

                //Check - Blackbar Brightness
                if (!SettingCheck(vConfiguration, "AdjustBlackBarBrightness"))
                {
                    SettingSave(vConfiguration, "AdjustBlackBarBrightness", "2");
                }

                //Check - Led Bottom Gap
                if (!SettingCheck(vConfiguration, "LedBottomGap"))
                {
                    SettingSave(vConfiguration, "LedBottomGap", "0");
                }

                //Check - Led Contrast Level
                if (!SettingCheck(vConfiguration, "LedContrast"))
                {
                    SettingSave(vConfiguration, "LedContrast", "0");
                }

                //Check - Led Brightness Level
                if (!SettingCheck(vConfiguration, "LedBrightness"))
                {
                    SettingSave(vConfiguration, "LedBrightness", "90");
                }

                //Check - Led Gamma
                if (!SettingCheck(vConfiguration, "LedGamma3"))
                {
                    SettingSave(vConfiguration, "LedGamma3", "2.10");
                }

                //Check - Led Saturation
                if (!SettingCheck(vConfiguration, "LedSaturation"))
                {
                    SettingSave(vConfiguration, "LedSaturation", "110");
                }

                //Check - Color Loop Speed
                if (!SettingCheck(vConfiguration, "ColorLoopSpeed"))
                {
                    SettingSave(vConfiguration, "ColorLoopSpeed", "75");
                }

                //Check - Spectrum Rotation Speed
                if (!SettingCheck(vConfiguration, "SpectrumRotationSpeed"))
                {
                    SettingSave(vConfiguration, "SpectrumRotationSpeed", "20");
                }

                //Check - Solid Led Color
                if (!SettingCheck(vConfiguration, "SolidLedColor"))
                {
                    SettingSave(vConfiguration, "SolidLedColor", "#FFA500");
                }

                //Check - Led Energy Saving Mode
                if (!SettingCheck(vConfiguration, "LedEnergyMode"))
                {
                    SettingSave(vConfiguration, "LedEnergyMode", "False");
                }

                //Check - Led Minimum Brightness
                if (!SettingCheck(vConfiguration, "LedMinBrightness"))
                {
                    SettingSave(vConfiguration, "LedMinBrightness", "0");
                }

                //Check - Led Minimum Color
                if (!SettingCheck(vConfiguration, "LedMinColor2"))
                {
                    SettingSave(vConfiguration, "LedMinColor2", "0");
                }

                //Check - Led Color Red
                if (!SettingCheck(vConfiguration, "LedColorRed"))
                {
                    SettingSave(vConfiguration, "LedColorRed", "100");
                }

                //Check - Led Color Green
                if (!SettingCheck(vConfiguration, "LedColorGreen"))
                {
                    SettingSave(vConfiguration, "LedColorGreen", "100");
                }

                //Check - Led Color Blue
                if (!SettingCheck(vConfiguration, "LedColorBlue"))
                {
                    SettingSave(vConfiguration, "LedColorBlue", "100");
                }

                //Check - Led Capture Range
                if (!SettingCheck(vConfiguration, "LedCaptureRange"))
                {
                    SettingSave(vConfiguration, "LedCaptureRange", "20");
                }

                //Check - Led Side Types
                if (!SettingCheck(vConfiguration, "LedSideFirst"))
                {
                    SettingSave(vConfiguration, "LedSideFirst", "0");
                }
                if (!SettingCheck(vConfiguration, "LedSideSecond"))
                {
                    SettingSave(vConfiguration, "LedSideSecond", "0");
                }
                if (!SettingCheck(vConfiguration, "LedSideThird"))
                {
                    SettingSave(vConfiguration, "LedSideThird", "0");
                }
                if (!SettingCheck(vConfiguration, "LedSideFourth"))
                {
                    SettingSave(vConfiguration, "LedSideFourth", "0");
                }

                //Check - Led Count
                if (!SettingCheck(vConfiguration, "LedCountFirst"))
                {
                    SettingSave(vConfiguration, "LedCountFirst", "0");
                }
                if (!SettingCheck(vConfiguration, "LedCountSecond"))
                {
                    SettingSave(vConfiguration, "LedCountSecond", "0");
                }
                if (!SettingCheck(vConfiguration, "LedCountThird"))
                {
                    SettingSave(vConfiguration, "LedCountThird", "0");
                }
                if (!SettingCheck(vConfiguration, "LedCountFourth"))
                {
                    SettingSave(vConfiguration, "LedCountFourth", "0");
                }

                //Check - Led Output
                if (!SettingCheck(vConfiguration, "LedOutput"))
                {
                    SettingSave(vConfiguration, "LedOutput", "0");
                }

                //Check - Update Rate
                if (!SettingCheck(vConfiguration, "UpdateRate"))
                {
                    SettingSave(vConfiguration, "UpdateRate", "22");
                }

                //Check - Led Smoothing
                if (!SettingCheck(vConfiguration, "LedSmoothing"))
                {
                    SettingSave(vConfiguration, "LedSmoothing", "6");
                }

                //Check - Capture Blur
                if (!SettingCheck(vConfiguration, "CaptureBlur"))
                {
                    SettingSave(vConfiguration, "CaptureBlur", "0");
                }

                //Check - Capture HDR Paper White
                if (!SettingCheck(vConfiguration, "CaptureHDRPaperWhite"))
                {
                    SettingSave(vConfiguration, "CaptureHDRPaperWhite", "80");
                }

                //Check - Capture HDR Maximum Nits
                if (!SettingCheck(vConfiguration, "CaptureHDRMaximumNits"))
                {
                    SettingSave(vConfiguration, "CaptureHDRMaximumNits", "1000");
                }

                //Check - Debug Mode
                if (!SettingCheck(vConfiguration, "DebugLedPreview"))
                {
                    SettingSave(vConfiguration, "DebugLedPreview", "False");
                }
                if (!SettingCheck(vConfiguration, "DebugColorLeftRight"))
                {
                    SettingSave(vConfiguration, "DebugColorLeftRight", "False");
                }
                if (!SettingCheck(vConfiguration, "DebugColorTopBottom"))
                {
                    SettingSave(vConfiguration, "DebugColorTopBottom", "False");
                }
            }
            catch
            {
                Debug.WriteLine("Failed to check the settings.");
            }
        }
    }
}