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

                    if (vSettings.Load("FirstLaunch2", typeof(bool)))
                    {
                        vSettings.Set("ComPort", PortNumberRaw.ToString());
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
                cb_ComPort.SelectedIndex = vSettings.Load("ComPort", typeof(int)) - 1;

                //Load - Baud Rate
                textbox_BaudRate.Text = vSettings.Load("BaudRate", typeof(string));

                //Load - Led Automatic Before
                bool LedAutoOnOffBefore = vSettings.Load("LedAutoOnOffBefore", typeof(bool));
                cb_LedAutoOnOffBefore.IsChecked = LedAutoOnOffBefore;
                timepicker_LedAutoTimeBefore.IsEnabled = LedAutoOnOffBefore;
                timepicker_LedAutoTimeBefore.DateTimeValue = vSettings.Load("LedAutoTimeBefore", typeof(DateTime));

                //Load - Led Automatic After
                bool LedAutoOnOffAfter = vSettings.Load("LedAutoOnOffAfter", typeof(bool));
                cb_LedAutoOnOffAfter.IsChecked = LedAutoOnOffAfter;
                timepicker_LedAutoTimeAfter.IsEnabled = LedAutoOnOffAfter;
                timepicker_LedAutoTimeAfter.DateTimeValue = vSettings.Load("LedAutoTimeAfter", typeof(DateTime));

                //Load - Server Port
                tb_ServerPort.Text = vSettings.Load("ServerPort", typeof(string));

                //Load - Monitor Capture
                cb_MonitorCapture.SelectedIndex = vSettings.Load("MonitorCapture", typeof(int));

                //Load - Led Mode
                cb_LedMode.SelectedIndex = vSettings.Load("LedMode", typeof(int));

                //Load - Adjust Black Bars
                cb_AdjustBlackBars.IsChecked = vSettings.Load("AdjustBlackBars", typeof(bool));

                //Load - Led Bottom Gap
                tb_LedBottomGap.Text = "Led gap bottom stand: " + vSettings.Load("LedBottomGap", typeof(int)) + " leds";
                sldr_LedBottomGap.Value = vSettings.Load("LedBottomGap", typeof(int));

                //Load - Led contrast level
                tb_LedContrast.Text = "Contrast level: " + vSettings.Load("LedContrast", typeof(int));
                sldr_LedContrast.Value = vSettings.Load("LedContrast", typeof(int));

                //Load - Led brightness level
                tb_LedBrightness.Text = "Brightness level: " + vSettings.Load("LedBrightness", typeof(int)) + "%";
                sldr_LedBrightness.Value = vSettings.Load("LedBrightness", typeof(int));

                //Load - Led Energy Saving Mode
                cb_LedEnergyMode.IsChecked = vSettings.Load("LedEnergyMode", typeof(bool));

                //Load - Led Off Monitor Sleep
                cb_LedOffMonitorSleep.IsChecked = vSettings.Load("LedOffMonitorSleep", typeof(bool));

                //Load - Led Minimum Brightness
                tb_LedMinBrightness.Text = "Minimum brightness level: " + vSettings.Load("LedMinBrightness", typeof(int)) + "%";
                sldr_LedMinBrightness.Value = vSettings.Load("LedMinBrightness", typeof(int));

                //Load - Led Gamma
                tb_LedGamma.Text = "Gamma curve: " + vSettings.Load("LedGamma4", typeof(float)).ToString("0.00");
                sldr_LedGamma.Value = vSettings.Load("LedGamma4", typeof(float));

                //Load - Led Saturation
                tb_LedSaturation.Text = "Color saturation: " + vSettings.Load("LedSaturation", typeof(int)) + "%";
                sldr_LedSaturation.Value = vSettings.Load("LedSaturation", typeof(int));

                //Load - Color Loop Speed
                tb_ColorLoopSpeed.Text = "Color loop speed: " + vSettings.Load("ColorLoopSpeed", typeof(int)) + " ms";
                sldr_ColorLoopSpeed.Value = vSettings.Load("ColorLoopSpeed", typeof(int));

                //Load - Spectrum Rotation Speed
                tb_SpectrumRotationSpeed.Text = "Spectrum rotation speed: " + vSettings.Load("SpectrumRotationSpeed", typeof(int)) + " sec";
                sldr_SpectrumRotationSpeed.Value = vSettings.Load("SpectrumRotationSpeed", typeof(int));

                //Load - Solid Led Color
                string solidLedColor = vSettings.Load("SolidLedColor", typeof(string));
                button_ColorPickerSolid.Background = new BrushConverter().ConvertFrom(solidLedColor) as SolidColorBrush;

                //Load - Led Minimum Color
                tb_LedMinColor.Text = "Minimum color brightness: " + vSettings.Load("LedMinColor2", typeof(int)) + "%";
                sldr_LedMinColor.Value = vSettings.Load("LedMinColor2", typeof(int));

                //Load - Led Color Red
                tb_LedColorRed.Text = "Red: " + vSettings.Load("LedColorRed", typeof(int)) + "%";
                sldr_LedColorRed.Value = vSettings.Load("LedColorRed", typeof(int));

                //Load - Led Color Green
                tb_LedColorGreen.Text = "Green: " + vSettings.Load("LedColorGreen", typeof(int)) + "%";
                sldr_LedColorGreen.Value = vSettings.Load("LedColorGreen", typeof(int));

                //Load - Led Color Blue
                tb_LedColorBlue.Text = "Blue: " + vSettings.Load("LedColorBlue", typeof(int)) + "%";
                sldr_LedColorBlue.Value = vSettings.Load("LedColorBlue", typeof(int));

                //Load - Led Capture Range
                tb_LedCaptureRange.Text = "Led capture range: " + vSettings.Load("LedCaptureRange", typeof(int)) + "%";
                sldr_LedCaptureRange.Value = vSettings.Load("LedCaptureRange", typeof(int));

                //Load - Blackbar detect rate
                int updateRateBarMs = vSettings.Load("AdjustBlackbarRate2", typeof(int));
                string updateRateBarFps = Convert.ToInt32(1000 / updateRateBarMs).ToString();
                tb_AdjustBlackbarRate.Text = "Blackbar detection rate: " + updateRateBarMs + " ms (" + updateRateBarFps + " fps)";
                sldr_AdjustBlackbarRate.Value = updateRateBarMs;

                //Load - Blackbar detect range
                tb_AdjustBlackbarRange.Text = "Blackbar detection range: " + vSettings.Load("AdjustBlackbarRange", typeof(int)) + "%";
                sldr_AdjustBlackbarRange.Value = vSettings.Load("AdjustBlackbarRange", typeof(int));

                //Load - Blackbar Brightness
                tb_AdjustBlackBarBrightness.Text = "Blackbar minimum brightness: " + vSettings.Load("AdjustBlackBarBrightness", typeof(int)) + "%";
                sldr_AdjustBlackBarBrightness.Value = vSettings.Load("AdjustBlackBarBrightness", typeof(int));

                //Load - Led Side Types
                combobox_LedSideFirst.SelectedIndex = vSettings.Load("LedSideFirst", typeof(int));
                combobox_LedSideSecond.SelectedIndex = vSettings.Load("LedSideSecond", typeof(int));
                combobox_LedSideThird.SelectedIndex = vSettings.Load("LedSideThird", typeof(int));
                combobox_LedSideFourth.SelectedIndex = vSettings.Load("LedSideFourth", typeof(int));

                //Load - Led Count
                textbox_LedCountFirst.Text = vSettings.Load("LedCountFirst", typeof(string));
                textbox_LedCountSecond.Text = vSettings.Load("LedCountSecond", typeof(string));
                textbox_LedCountThird.Text = vSettings.Load("LedCountThird", typeof(string));
                textbox_LedCountFourth.Text = vSettings.Load("LedCountFourth", typeof(string));
                int totalCount = Convert.ToInt32(textbox_LedCountFirst.Text) + Convert.ToInt32(textbox_LedCountSecond.Text) + Convert.ToInt32(textbox_LedCountThird.Text) + Convert.ToInt32(textbox_LedCountFourth.Text);
                textblock_LedCount.Text = "Total led count: " + totalCount + " (must be equal with arduino script)";

                //Load - Led Output
                cb_LedOutput.SelectedIndex = vSettings.Load("LedOutput", typeof(int));

                //Load - Update Rate
                int updateRateScreenMs = vSettings.Load("UpdateRate", typeof(int));
                string updateRateScreenFps = Convert.ToInt32(1000 / updateRateScreenMs).ToString();
                tb_UpdateRate.Text = "Led update rate: " + updateRateScreenMs + " ms (" + updateRateScreenFps + " fps)";
                sldr_UpdateRate.Value = updateRateScreenMs;

                //Load - Led Smooth Frame
                int smoothingFrame = vSettings.Load("LedSmoothFrame", typeof(int));
                tb_LedSmoothFrame.Text = "Frame smoothing: " + smoothingFrame + " frames";
                sldr_LedSmoothFrame.Value = smoothingFrame;

                //Load - Led Smooth Object
                int smoothingObject = vSettings.Load("LedSmoothObject", typeof(int));
                tb_LedSmoothObject.Text = "Object smoothing: " + smoothingObject + " leds";
                sldr_LedSmoothObject.Value = smoothingObject;

                //Load - Capture HDR Paper White
                int captureHDRPaperWhite = vSettings.Load("CaptureHDRPaperWhite", typeof(int));
                tb_CaptureHDRPaperWhite.Text = "HDR paper white: " + captureHDRPaperWhite;
                sldr_CaptureHDRPaperWhite.Value = captureHDRPaperWhite;

                //Load - Capture HDR Maximum Nits
                int captureHDRMaximumNits = vSettings.Load("CaptureHDRMaximumNits", typeof(int));
                tb_CaptureHDRMaximumNits.Text = "HDR maximum nits: " + captureHDRMaximumNits;
                sldr_CaptureHDRMaximumNits.Value = captureHDRMaximumNits;

                //Load - Debug Mode
                checkbox_DebugLedPreview.IsChecked = vSettings.Load("DebugLedPreview", typeof(bool));
                checkbox_DebugColorLeftRight.IsChecked = vSettings.Load("DebugColorLeftRight", typeof(bool));
                checkbox_DebugColorTopBottom.IsChecked = vSettings.Load("DebugColorTopBottom", typeof(bool));

                //Launch settings
                cb_WindowsStartup.IsChecked = AVSettings.StartupShortcutCheck(StartupShortcutType.Startup);

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