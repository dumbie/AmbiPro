using System;
using System.Diagnostics;
using static AmbiPro.AppVariables;
using static ArnoldVinkCode.AVSettings;

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
                if (!vSettings.Check("FirstLaunch2"))
                {
                    vSettings.Set("FirstLaunch2", "True");
                }

                //Check - Com Port
                if (!vSettings.Check("ComPort"))
                {
                    vSettings.Set("ComPort", "1");
                }

                //Check - Baud Rate
                if (!vSettings.Check("BaudRate"))
                {
                    vSettings.Set("BaudRate", "115200");
                }

                //Check - Enable or Disable Led Automatic
                if (!vSettings.Check("LedAutoOnOffBefore"))
                {
                    vSettings.Set("LedAutoOnOffBefore", "False");
                }
                if (!vSettings.Check("LedAutoTimeBefore"))
                {
                    vSettings.Set("LedAutoTimeBefore", "01/01/1970 9:00:00");
                }
                if (!vSettings.Check("LedAutoOnOffAfter"))
                {
                    vSettings.Set("LedAutoOnOffAfter", "False");
                }
                if (!vSettings.Check("LedAutoTimeAfter"))
                {
                    vSettings.Set("LedAutoTimeAfter", "01/01/1970 17:00:00");
                }

                //Check - Server Port
                if (!vSettings.Check("ServerPort"))
                {
                    vSettings.Set("ServerPort", "1020");
                }

                //Check - Monitor Capture
                if (!vSettings.Check("MonitorCapture"))
                {
                    vSettings.Set("MonitorCapture", "0");
                }

                //Check - Led Mode
                if (!vSettings.Check("LedMode"))
                {
                    vSettings.Set("LedMode", "0");
                }

                //Check - Adjust to Blackbars
                if (!vSettings.Check("AdjustBlackBars"))
                {
                    vSettings.Set("AdjustBlackBars", "True");
                }

                //Check - Blackbar Update Rate
                if (!vSettings.Check("AdjustBlackbarRate2"))
                {
                    vSettings.Set("AdjustBlackbarRate2", "60");
                }

                //Check - Blackbar Update Range
                if (!vSettings.Check("AdjustBlackbarRange"))
                {
                    vSettings.Set("AdjustBlackbarRange", "20");
                }

                //Check - Blackbar Brightness
                if (!vSettings.Check("AdjustBlackBarBrightness"))
                {
                    vSettings.Set("AdjustBlackBarBrightness", "1");
                }

                //Check - Led Bottom Gap
                if (!vSettings.Check("LedBottomGap"))
                {
                    vSettings.Set("LedBottomGap", "0");
                }

                //Check - Led Contrast Level
                if (!vSettings.Check("LedContrast"))
                {
                    vSettings.Set("LedContrast", "0");
                }

                //Check - Led Brightness Level
                if (!vSettings.Check("LedBrightness"))
                {
                    vSettings.Set("LedBrightness", "80");
                }

                //Check - Led Gamma
                if (!vSettings.Check("LedGamma4"))
                {
                    vSettings.Set("LedGamma4", "0.65");
                }

                //Check - Led Saturation
                if (!vSettings.Check("LedSaturation"))
                {
                    vSettings.Set("LedSaturation", "100");
                }

                //Check - Color Loop Speed
                if (!vSettings.Check("ColorLoopSpeed"))
                {
                    vSettings.Set("ColorLoopSpeed", "75");
                }

                //Check - Spectrum Rotation Speed
                if (!vSettings.Check("SpectrumRotationSpeed"))
                {
                    vSettings.Set("SpectrumRotationSpeed", "20");
                }

                //Check - Solid Led Color
                if (!vSettings.Check("SolidLedColor"))
                {
                    vSettings.Set("SolidLedColor", "#FFA500");
                }

                //Check - Led Energy Saving Mode
                if (!vSettings.Check("LedEnergyMode"))
                {
                    vSettings.Set("LedEnergyMode", "False");
                }

                //Check - Led Off Monitor Sleep
                if (!vSettings.Check("LedOffMonitorSleep"))
                {
                    vSettings.Set("LedOffMonitorSleep", "True");
                }

                //Check - Led Minimum Brightness
                if (!vSettings.Check("LedMinBrightness"))
                {
                    vSettings.Set("LedMinBrightness", "0");
                }

                //Check - Led Minimum Color
                if (!vSettings.Check("LedMinColor2"))
                {
                    vSettings.Set("LedMinColor2", "0");
                }

                //Check - Led Color Red
                if (!vSettings.Check("LedColorRed"))
                {
                    vSettings.Set("LedColorRed", "100");
                }

                //Check - Led Color Green
                if (!vSettings.Check("LedColorGreen"))
                {
                    vSettings.Set("LedColorGreen", "100");
                }

                //Check - Led Color Blue
                if (!vSettings.Check("LedColorBlue"))
                {
                    vSettings.Set("LedColorBlue", "100");
                }

                //Check - Led Capture Range
                if (!vSettings.Check("LedCaptureRange"))
                {
                    vSettings.Set("LedCaptureRange", "20");
                }

                //Check - Led Side Types
                if (!vSettings.Check("LedSideFirst"))
                {
                    vSettings.Set("LedSideFirst", "0");
                }
                if (!vSettings.Check("LedSideSecond"))
                {
                    vSettings.Set("LedSideSecond", "0");
                }
                if (!vSettings.Check("LedSideThird"))
                {
                    vSettings.Set("LedSideThird", "0");
                }
                if (!vSettings.Check("LedSideFourth"))
                {
                    vSettings.Set("LedSideFourth", "0");
                }

                //Check - Led Count
                if (!vSettings.Check("LedCountFirst"))
                {
                    vSettings.Set("LedCountFirst", "0");
                }
                if (!vSettings.Check("LedCountSecond"))
                {
                    vSettings.Set("LedCountSecond", "0");
                }
                if (!vSettings.Check("LedCountThird"))
                {
                    vSettings.Set("LedCountThird", "0");
                }
                if (!vSettings.Check("LedCountFourth"))
                {
                    vSettings.Set("LedCountFourth", "0");
                }

                //Check - Led Output
                if (!vSettings.Check("LedOutput"))
                {
                    vSettings.Set("LedOutput", "0");
                }

                //Check - Update Rate
                if (!vSettings.Check("UpdateRate"))
                {
                    vSettings.Set("UpdateRate", "20");
                }

                //Check - Led Smooth Frame
                if (!vSettings.Check("LedSmoothFrame"))
                {
                    vSettings.Set("LedSmoothFrame", "4");
                }

                //Check - Led Smooth Object
                if (!vSettings.Check("LedSmoothObject"))
                {
                    vSettings.Set("LedSmoothObject", "2");
                }

                //Check - Capture HDR Paper White
                if (!vSettings.Check("CaptureHDRPaperWhite"))
                {
                    vSettings.Set("CaptureHDRPaperWhite", "80");
                }

                //Check - Capture HDR Maximum Nits
                if (!vSettings.Check("CaptureHDRMaximumNits"))
                {
                    vSettings.Set("CaptureHDRMaximumNits", "1000");
                }

                //Check - Debug Mode
                if (!vSettings.Check("DebugLedPreview"))
                {
                    vSettings.Set("DebugLedPreview", "False");
                }
                if (!vSettings.Check("DebugColorLeftRight"))
                {
                    vSettings.Set("DebugColorLeftRight", "False");
                }
                if (!vSettings.Check("DebugColorTopBottom"))
                {
                    vSettings.Set("DebugColorTopBottom", "False");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to check application settings: " + ex.Message);
            }
        }
    }
}