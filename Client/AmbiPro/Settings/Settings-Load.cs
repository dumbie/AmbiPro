using ArnoldVinkCode;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;
using static AmbiPro.AppVariables;
using static ArnoldVinkCode.AVSettings;

namespace AmbiPro.Settings
{
    public partial class FormSettings
    {
        //Load - Led count from script
        public int LoadLedCountScript()
        {
            try
            {
                string[] scriptLines = File.ReadAllLines(@"Script\Script.ino");
                string ledCountString = scriptLines.FirstOrDefault(x => x.StartsWith("#define LedAmount"));
                if (!string.IsNullOrWhiteSpace(ledCountString))
                {
                    int ledCountInt = Convert.ToInt32(ledCountString.Replace("#define LedAmount", string.Empty));
                    Debug.WriteLine("Script led count: " + ledCountInt);
                    return ledCountInt;
                }
                else
                {
                    Debug.WriteLine("Script led count not found.");
                    return -1;
                }
            }
            catch
            {
                Debug.WriteLine("Failed to load script led count.");
                return -1;
            }
        }

        //Load - Update connected com ports
        public void SettingsUpdateComPorts()
        {
            try
            {
                foreach (string PortName in SerialPort.GetPortNames())
                {
                    int PortNumberRaw = Convert.ToInt32(PortName.Replace("COM", string.Empty));
                    cb_ComPort.Items[PortNumberRaw - 1] = PortName + " (Connected)";

                    if (SettingLoad(vConfiguration, "FirstLaunch2", typeof(bool)))
                    {
                        SettingSave(vConfiguration, "ComPort", PortNumberRaw.ToString());
                    }
                }
            }
            catch { }
        }

        //Load - Application Settings
        public async Task SettingsLoad()
        {
            try
            {
                Debug.WriteLine("Loading application settings...");

                //Update connected com ports
                SettingsUpdateComPorts();

                //Load - Com Port
                cb_ComPort.SelectedIndex = SettingLoad(vConfiguration, "ComPort", typeof(int)) - 1;

                //Load - Baud Rate
                textbox_BaudRate.Text = SettingLoad(vConfiguration, "BaudRate", typeof(string));

                //Load - Led Automatic Before
                bool LedAutoOnOffBefore = SettingLoad(vConfiguration, "LedAutoOnOffBefore", typeof(bool));
                cb_LedAutoOnOffBefore.IsChecked = LedAutoOnOffBefore;
                timepicker_LedAutoTimeBefore.IsEnabled = LedAutoOnOffBefore;
                timepicker_LedAutoTimeBefore.DateTimeValue = SettingLoad(vConfiguration, "LedAutoTimeBefore", typeof(DateTime));

                //Load - Led Automatic After
                bool LedAutoOnOffAfter = SettingLoad(vConfiguration, "LedAutoOnOffAfter", typeof(bool));
                cb_LedAutoOnOffAfter.IsChecked = LedAutoOnOffAfter;
                timepicker_LedAutoTimeAfter.IsEnabled = LedAutoOnOffAfter;
                timepicker_LedAutoTimeAfter.DateTimeValue = SettingLoad(vConfiguration, "LedAutoTimeAfter", typeof(DateTime));

                //Load - Server Port
                tb_ServerPort.Text = SettingLoad(vConfiguration, "ServerPort", typeof(string));

                //Load - Monitor Capture
                cb_MonitorCapture.SelectedIndex = SettingLoad(vConfiguration, "MonitorCapture", typeof(int));

                //Load - Led Mode
                cb_LedMode.SelectedIndex = SettingLoad(vConfiguration, "LedMode", typeof(int));

                //Load - Adjust Black Bars
                cb_AdjustBlackBars.IsChecked = SettingLoad(vConfiguration, "AdjustBlackBars", typeof(bool));

                //Load - Led Bottom Gap
                tb_LedBottomGap.Text = "Led gap bottom stand: " + SettingLoad(vConfiguration, "LedBottomGap", typeof(int)) + " leds";
                sldr_LedBottomGap.Value = SettingLoad(vConfiguration, "LedBottomGap", typeof(int));

                //Load - Led contrast level
                tb_LedContrast.Text = "Contrast level: " + SettingLoad(vConfiguration, "LedContrast", typeof(int));
                sldr_LedContrast.Value = SettingLoad(vConfiguration, "LedContrast", typeof(int));

                //Load - Led brightness level
                tb_LedBrightness.Text = "Brightness level: " + SettingLoad(vConfiguration, "LedBrightness", typeof(int)) + "%";
                sldr_LedBrightness.Value = SettingLoad(vConfiguration, "LedBrightness", typeof(int));

                //Load - Led Energy Saving Mode
                cb_LedEnergyMode.IsChecked = SettingLoad(vConfiguration, "LedEnergyMode", typeof(bool));

                //Load - Led Off Monitor Sleep
                cb_LedOffMonitorSleep.IsChecked = SettingLoad(vConfiguration, "LedOffMonitorSleep", typeof(bool));

                //Load - Led Minimum Brightness
                tb_LedMinBrightness.Text = "Minimum brightness level: " + SettingLoad(vConfiguration, "LedMinBrightness", typeof(int)) + "%";
                sldr_LedMinBrightness.Value = SettingLoad(vConfiguration, "LedMinBrightness", typeof(int));

                //Load - Led Gamma
                tb_LedGamma.Text = "Gamma curve: " + SettingLoad(vConfiguration, "LedGamma4", typeof(float)).ToString("0.00");
                sldr_LedGamma.Value = SettingLoad(vConfiguration, "LedGamma4", typeof(float));

                //Load - Led Saturation
                tb_LedSaturation.Text = "Color saturation: " + SettingLoad(vConfiguration, "LedSaturation", typeof(int)) + "%";
                sldr_LedSaturation.Value = SettingLoad(vConfiguration, "LedSaturation", typeof(int));

                //Load - Color Loop Speed
                tb_ColorLoopSpeed.Text = "Color loop speed: " + SettingLoad(vConfiguration, "ColorLoopSpeed", typeof(int)) + " ms";
                sldr_ColorLoopSpeed.Value = SettingLoad(vConfiguration, "ColorLoopSpeed", typeof(int));

                //Load - Spectrum Rotation Speed
                tb_SpectrumRotationSpeed.Text = "Spectrum rotation speed: " + SettingLoad(vConfiguration, "SpectrumRotationSpeed", typeof(int)) + " sec";
                sldr_SpectrumRotationSpeed.Value = SettingLoad(vConfiguration, "SpectrumRotationSpeed", typeof(int));

                //Load - Solid Led Color
                string solidLedColor = SettingLoad(vConfiguration, "SolidLedColor", typeof(string));
                button_ColorPickerSolid.Background = new BrushConverter().ConvertFrom(solidLedColor) as SolidColorBrush;

                //Load - Led Minimum Color
                tb_LedMinColor.Text = "Minimum color brightness: " + SettingLoad(vConfiguration, "LedMinColor2", typeof(int)) + "%";
                sldr_LedMinColor.Value = SettingLoad(vConfiguration, "LedMinColor2", typeof(int));

                //Load - Led Color Red
                tb_LedColorRed.Text = "Red: " + SettingLoad(vConfiguration, "LedColorRed", typeof(int)) + "%";
                sldr_LedColorRed.Value = SettingLoad(vConfiguration, "LedColorRed", typeof(int));

                //Load - Led Color Green
                tb_LedColorGreen.Text = "Green: " + SettingLoad(vConfiguration, "LedColorGreen", typeof(int)) + "%";
                sldr_LedColorGreen.Value = SettingLoad(vConfiguration, "LedColorGreen", typeof(int));

                //Load - Led Color Blue
                tb_LedColorBlue.Text = "Blue: " + SettingLoad(vConfiguration, "LedColorBlue", typeof(int)) + "%";
                sldr_LedColorBlue.Value = SettingLoad(vConfiguration, "LedColorBlue", typeof(int));

                //Load - Led Capture Range
                tb_LedCaptureRange.Text = "Led capture range: " + SettingLoad(vConfiguration, "LedCaptureRange", typeof(int)) + "%";
                sldr_LedCaptureRange.Value = SettingLoad(vConfiguration, "LedCaptureRange", typeof(int));

                //Load - Blackbar detect rate
                int updateRateBarMs = SettingLoad(vConfiguration, "AdjustBlackbarRate2", typeof(int));
                string updateRateBarFps = Convert.ToInt32(1000 / updateRateBarMs).ToString();
                tb_AdjustBlackbarRate.Text = "Blackbar detection rate: " + updateRateBarMs + " ms (" + updateRateBarFps + " fps)";
                sldr_AdjustBlackbarRate.Value = updateRateBarMs;

                //Load - Blackbar detect range
                tb_AdjustBlackbarRange.Text = "Blackbar detection range: " + SettingLoad(vConfiguration, "AdjustBlackbarRange", typeof(int)) + "%";
                sldr_AdjustBlackbarRange.Value = SettingLoad(vConfiguration, "AdjustBlackbarRange", typeof(int));

                //Load - Blackbar Brightness
                tb_AdjustBlackBarBrightness.Text = "Blackbar minimum brightness: " + SettingLoad(vConfiguration, "AdjustBlackBarBrightness", typeof(int)) + "%";
                sldr_AdjustBlackBarBrightness.Value = SettingLoad(vConfiguration, "AdjustBlackBarBrightness", typeof(int));

                //Load - Led Side Types
                combobox_LedSideFirst.SelectedIndex = SettingLoad(vConfiguration, "LedSideFirst", typeof(int));
                combobox_LedSideSecond.SelectedIndex = SettingLoad(vConfiguration, "LedSideSecond", typeof(int));
                combobox_LedSideThird.SelectedIndex = SettingLoad(vConfiguration, "LedSideThird", typeof(int));
                combobox_LedSideFourth.SelectedIndex = SettingLoad(vConfiguration, "LedSideFourth", typeof(int));

                //Load - Led Count
                textbox_LedCountFirst.Text = SettingLoad(vConfiguration, "LedCountFirst", typeof(string));
                textbox_LedCountSecond.Text = SettingLoad(vConfiguration, "LedCountSecond", typeof(string));
                textbox_LedCountThird.Text = SettingLoad(vConfiguration, "LedCountThird", typeof(string));
                textbox_LedCountFourth.Text = SettingLoad(vConfiguration, "LedCountFourth", typeof(string));
                int totalCount = Convert.ToInt32(textbox_LedCountFirst.Text) + Convert.ToInt32(textbox_LedCountSecond.Text) + Convert.ToInt32(textbox_LedCountThird.Text) + Convert.ToInt32(textbox_LedCountFourth.Text);
                textblock_LedCount.Text = "Total led count: " + totalCount + " (must be equal with arduino script)";

                //Load - Led Output
                cb_LedOutput.SelectedIndex = SettingLoad(vConfiguration, "LedOutput", typeof(int));

                //Load - Update Rate
                int updateRateScreenMs = SettingLoad(vConfiguration, "UpdateRate", typeof(int));
                string updateRateScreenFps = Convert.ToInt32(1000 / updateRateScreenMs).ToString();
                tb_UpdateRate.Text = "Led update rate: " + updateRateScreenMs + " ms (" + updateRateScreenFps + " fps)";
                sldr_UpdateRate.Value = updateRateScreenMs;

                //Load - Led Smooth Frame
                int smoothingFrame = SettingLoad(vConfiguration, "LedSmoothFrame", typeof(int));
                tb_LedSmoothFrame.Text = "Frame smoothing: " + smoothingFrame + " frames";
                sldr_LedSmoothFrame.Value = smoothingFrame;

                //Load - Led Smooth Object
                int smoothingObject = SettingLoad(vConfiguration, "LedSmoothObject", typeof(int));
                tb_LedSmoothObject.Text = "Object smoothing: " + smoothingObject + " leds";
                sldr_LedSmoothObject.Value = smoothingObject;

                //Load - Capture HDR Paper White
                int captureHDRPaperWhite = SettingLoad(vConfiguration, "CaptureHDRPaperWhite", typeof(int));
                tb_CaptureHDRPaperWhite.Text = "HDR paper white: " + captureHDRPaperWhite;
                sldr_CaptureHDRPaperWhite.Value = captureHDRPaperWhite;

                //Load - Capture HDR Maximum Nits
                int captureHDRMaximumNits = SettingLoad(vConfiguration, "CaptureHDRMaximumNits", typeof(int));
                tb_CaptureHDRMaximumNits.Text = "HDR maximum nits: " + captureHDRMaximumNits;
                sldr_CaptureHDRMaximumNits.Value = captureHDRMaximumNits;

                //Load - Debug Mode
                checkbox_DebugLedPreview.IsChecked = SettingLoad(vConfiguration, "DebugLedPreview", typeof(bool));
                checkbox_DebugColorLeftRight.IsChecked = SettingLoad(vConfiguration, "DebugColorLeftRight", typeof(bool));
                checkbox_DebugColorTopBottom.IsChecked = SettingLoad(vConfiguration, "DebugColorTopBottom", typeof(bool));

                //Launch settings
                cb_WindowsStartup.IsChecked = AVSettings.StartupShortcutCheck();

                //Wait for settings to have loaded
                await Task.Delay(1500);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to load application settings: " + ex.Message);
            }
        }
    }
}