using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Reflection;
using System.Windows.Media;
using static AmbiPro.AppVariables;

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
                string ledCountString = scriptLines.Where(x => x.StartsWith("#define LedAmount")).FirstOrDefault();
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

        //Load - Application Settings
        public void SettingsLoad()
        {
            try
            {
                Debug.WriteLine("Loading application settings...");

                //Check connected com ports
                foreach (string PortName in SerialPort.GetPortNames())
                {
                    int PortNumberRaw = Convert.ToInt32(PortName.Replace("COM", string.Empty));
                    cb_ComPort.Items[PortNumberRaw - 1] = PortName + " (Connected)";

                    if (Convert.ToBoolean(ConfigurationManager.AppSettings["FirstLaunch2"]))
                    {
                        SettingsFunction.Save("ComPort", PortNumberRaw.ToString());
                    }
                }

                //Load - Com Port
                cb_ComPort.SelectedIndex = (Convert.ToInt32(ConfigurationManager.AppSettings["ComPort"]) - 1);

                //Load - Baud Rate
                textbox_BaudRate.Text = ConfigurationManager.AppSettings["BaudRate"].ToString();

                //Load - Led Automatic Before
                bool LedAutoOnOffBefore = Convert.ToBoolean(ConfigurationManager.AppSettings["LedAutoOnOffBefore"]);
                cb_LedAutoOnOffBefore.IsChecked = LedAutoOnOffBefore;
                timepicker_LedAutoTimeBefore.IsEnabled = LedAutoOnOffBefore;
                timepicker_LedAutoTimeBefore.DateTimeValue = DateTime.Parse(ConfigurationManager.AppSettings["LedAutoTimeBefore"], vAppCultureInfo);

                //Load - Led Automatic After
                bool LedAutoOnOffAfter = Convert.ToBoolean(ConfigurationManager.AppSettings["LedAutoOnOffAfter"]);
                cb_LedAutoOnOffAfter.IsChecked = LedAutoOnOffAfter;
                timepicker_LedAutoTimeAfter.IsEnabled = LedAutoOnOffAfter;
                timepicker_LedAutoTimeAfter.DateTimeValue = DateTime.Parse(ConfigurationManager.AppSettings["LedAutoTimeAfter"], vAppCultureInfo);

                //Load - Server Port
                tb_ServerPort.Text = ConfigurationManager.AppSettings["ServerPort"].ToString();

                //Load - Monitor Capture
                cb_MonitorCapture.SelectedIndex = Convert.ToInt32(ConfigurationManager.AppSettings["MonitorCapture"]);

                //Load - Led Mode
                cb_LedMode.SelectedIndex = Convert.ToInt32(ConfigurationManager.AppSettings["LedMode"]);

                //Load - Adjust Black Bars
                cb_AdjustBlackBars.IsChecked = Convert.ToBoolean(ConfigurationManager.AppSettings["AdjustBlackBars"]);

                //Load - Led Bottom Gap
                tb_LedBottomGap.Text = "Led gap bottom stand: " + Convert.ToInt32(ConfigurationManager.AppSettings["LedBottomGap"]) + " leds";
                sldr_LedBottomGap.Value = Convert.ToInt32(ConfigurationManager.AppSettings["LedBottomGap"]);

                //Load - Led contrast level
                tb_LedContrast.Text = "Contrast level: " + Convert.ToInt32(ConfigurationManager.AppSettings["LedContrast"]);
                sldr_LedContrast.Value = Convert.ToInt32(ConfigurationManager.AppSettings["LedContrast"]);

                //Load - Led brightness level
                tb_LedBrightness.Text = "Brightness level: " + Convert.ToInt32(ConfigurationManager.AppSettings["LedBrightness"]) + "%";
                sldr_LedBrightness.Value = Convert.ToInt32(ConfigurationManager.AppSettings["LedBrightness"]);

                //Load - Led Minimum Brightness
                tb_LedMinBrightness.Text = "Minimum brightness level: " + Convert.ToInt32(ConfigurationManager.AppSettings["LedMinBrightness"]) + "%";
                sldr_LedMinBrightness.Value = Convert.ToInt32(ConfigurationManager.AppSettings["LedMinBrightness"]);

                //Load - Led Gamma
                tb_LedGamma.Text = "Gamma level: " + Convert.ToInt32(ConfigurationManager.AppSettings["LedGamma"]) + "%";
                sldr_LedGamma.Value = Convert.ToInt32(ConfigurationManager.AppSettings["LedGamma"]);

                //Load - Led Saturation
                tb_LedSaturation.Text = "Color saturation: " + Convert.ToInt32(ConfigurationManager.AppSettings["LedSaturation"]) + "%";
                sldr_LedSaturation.Value = Convert.ToInt32(ConfigurationManager.AppSettings["LedSaturation"]);

                //Load - Led Temperature
                tb_LedTemperature.Text = "Color temperature: " + Convert.ToInt32(ConfigurationManager.AppSettings["LedTemperature"]) + "K";
                sldr_LedTemperature.Value = Convert.ToInt32(ConfigurationManager.AppSettings["LedTemperature"]);

                //Load - Color Loop Speed
                tb_ColorLoopSpeed.Text = "Color loop speed: " + Convert.ToInt32(ConfigurationManager.AppSettings["ColorLoopSpeed"]) + " ms";
                sldr_ColorLoopSpeed.Value = Convert.ToInt32(ConfigurationManager.AppSettings["ColorLoopSpeed"]);

                //Load - Spectrum Rotation Speed
                tb_SpectrumRotationSpeed.Text = "Spectrum rotation speed: " + Convert.ToInt32(ConfigurationManager.AppSettings["SpectrumRotationSpeed"]) + " sec";
                sldr_SpectrumRotationSpeed.Value = Convert.ToInt32(ConfigurationManager.AppSettings["SpectrumRotationSpeed"]);

                //Load - Solid Led Color
                string SolidLedColor = ConfigurationManager.AppSettings["SolidLedColor"].ToString();
                button_ColorPickerSolid.Background = new BrushConverter().ConvertFrom(SolidLedColor) as SolidColorBrush;

                //Load - Led Hue
                tb_LedHue.Text = "Color hue: " + Convert.ToInt32(ConfigurationManager.AppSettings["LedHue2"]) + "°";
                sldr_LedHue.Value = Convert.ToInt32(ConfigurationManager.AppSettings["LedHue2"]);

                //Load - Led Minimum Color
                tb_LedMinColor.Text = "Minimum color brightness: " + Convert.ToInt32(ConfigurationManager.AppSettings["LedMinColor"]) + "%";
                sldr_LedMinColor.Value = Convert.ToInt32(ConfigurationManager.AppSettings["LedMinColor"]);

                //Load - Led Color Red
                tb_LedColorRed.Text = "Red: " + Convert.ToInt32(ConfigurationManager.AppSettings["LedColorRed"]) + "%";
                sldr_LedColorRed.Value = Convert.ToInt32(ConfigurationManager.AppSettings["LedColorRed"]);

                //Load - Led Color Green
                tb_LedColorGreen.Text = "Green: " + Convert.ToInt32(ConfigurationManager.AppSettings["LedColorGreen"]) + "%";
                sldr_LedColorGreen.Value = Convert.ToInt32(ConfigurationManager.AppSettings["LedColorGreen"]);

                //Load - Led Color Blue
                tb_LedColorBlue.Text = "Blue: " + Convert.ToInt32(ConfigurationManager.AppSettings["LedColorBlue"]) + "%";
                sldr_LedColorBlue.Value = Convert.ToInt32(ConfigurationManager.AppSettings["LedColorBlue"]);

                //Load - Led Capture Range
                tb_LedCaptureRange.Text = "Led capture range: " + Convert.ToInt32(ConfigurationManager.AppSettings["LedCaptureRange"]) + "%";
                sldr_LedCaptureRange.Value = Convert.ToInt32(ConfigurationManager.AppSettings["LedCaptureRange"]);

                //Load - Blackbar detect rate
                tb_AdjustBlackbarRate.Text = "Blackbar detection rate: " + Convert.ToInt32(ConfigurationManager.AppSettings["AdjustBlackbarRate"]) + " ms";
                sldr_AdjustBlackbarRate.Value = Convert.ToInt32(ConfigurationManager.AppSettings["AdjustBlackbarRate"]);

                //Load - Blackbar detect range
                tb_AdjustBlackbarRange.Text = "Blackbar detection range: " + Convert.ToInt32(ConfigurationManager.AppSettings["AdjustBlackbarRange"]) + "%";
                sldr_AdjustBlackbarRange.Value = Convert.ToInt32(ConfigurationManager.AppSettings["AdjustBlackbarRange"]);

                //Load - Adjust Blackbar Brightness
                tb_AdjustBlackBarBrightness.Text = "Blackbar minimum brightness: " + Convert.ToInt32(ConfigurationManager.AppSettings["AdjustBlackBarBrightness"]) + "%";
                sldr_AdjustBlackBarBrightness.Value = Convert.ToInt32(ConfigurationManager.AppSettings["AdjustBlackBarBrightness"]);

                //Load - Led Side Types
                combobox_LedSideFirst.SelectedIndex = Convert.ToInt32(ConfigurationManager.AppSettings["LedSideFirst"]);
                combobox_LedSideSecond.SelectedIndex = Convert.ToInt32(ConfigurationManager.AppSettings["LedSideSecond"]);
                combobox_LedSideThird.SelectedIndex = Convert.ToInt32(ConfigurationManager.AppSettings["LedSideThird"]);
                combobox_LedSideFourth.SelectedIndex = Convert.ToInt32(ConfigurationManager.AppSettings["LedSideFourth"]);

                //Load - Led Count
                textbox_LedCountFirst.Text = Convert.ToString(ConfigurationManager.AppSettings["LedCountFirst"]);
                textbox_LedCountSecond.Text = Convert.ToString(ConfigurationManager.AppSettings["LedCountSecond"]);
                textbox_LedCountThird.Text = Convert.ToString(ConfigurationManager.AppSettings["LedCountThird"]);
                textbox_LedCountFourth.Text = Convert.ToString(ConfigurationManager.AppSettings["LedCountFourth"]);
                int totalCount = Convert.ToInt32(textbox_LedCountFirst.Text) + Convert.ToInt32(textbox_LedCountSecond.Text) + Convert.ToInt32(textbox_LedCountThird.Text) + Convert.ToInt32(textbox_LedCountFourth.Text);
                textblock_LedCount.Text = "Total led count: " + totalCount + " (must be equal with arduino script)";

                //Load - Led Output
                cb_LedOutput.SelectedIndex = Convert.ToInt32(ConfigurationManager.AppSettings["LedOutput"]);

                //Load - Update Rate
                int updateRateMs = Convert.ToInt32(ConfigurationManager.AppSettings["UpdateRate"]);
                string updateRateFps = Convert.ToInt32(1000 / updateRateMs).ToString();
                tb_UpdateRate.Text = "Led update rate: " + updateRateMs + " ms (" + updateRateFps + " fps)";
                sldr_UpdateRate.Value = updateRateMs;

                //Load - Debug Mode
                checkbox_DebugMode.IsChecked = Convert.ToBoolean(ConfigurationManager.AppSettings["DebugMode"]);
                checkbox_DebugBlackBar.IsChecked = Convert.ToBoolean(ConfigurationManager.AppSettings["DebugBlackBar"]);
                checkbox_DebugColor.IsChecked = Convert.ToBoolean(ConfigurationManager.AppSettings["DebugColor"]);
                checkbox_DebugSave.IsChecked = Convert.ToBoolean(ConfigurationManager.AppSettings["DebugSave"]);

                //Check if application is set to launch on Windows startup
                string targetName = Assembly.GetEntryAssembly().GetName().Name;
                string targetFileShortcut = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Startup), targetName + ".url");
                if (File.Exists(targetFileShortcut)) { cb_WindowsStartup.IsChecked = true; }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to load the settings: " + ex.Message);
            }
        }
    }
}