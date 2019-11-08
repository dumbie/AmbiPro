using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Media;
using static AmbiPro.AppVariables;

namespace AmbiPro.Settings
{
    public partial class FormSettings
    {
        //Load - Application Settings
        public void SettingsLoad()
        {
            try
            {
                Debug.WriteLine("Loading application settings...");

                //Load - Com Port
                cb_ComPort.SelectedIndex = (Convert.ToInt32(ConfigurationManager.AppSettings["ComPort"]) - 1);

                //Load - Baud Rate
                if (ConfigurationManager.AppSettings["BaudRate"].ToString() == "9600") { cb_BaudRate.SelectedIndex = 0; }
                else if (ConfigurationManager.AppSettings["BaudRate"].ToString() == "14400") { cb_BaudRate.SelectedIndex = 1; }
                else if (ConfigurationManager.AppSettings["BaudRate"].ToString() == "19200") { cb_BaudRate.SelectedIndex = 2; }
                else if (ConfigurationManager.AppSettings["BaudRate"].ToString() == "28800") { cb_BaudRate.SelectedIndex = 3; }
                else if (ConfigurationManager.AppSettings["BaudRate"].ToString() == "38400") { cb_BaudRate.SelectedIndex = 4; }
                else if (ConfigurationManager.AppSettings["BaudRate"].ToString() == "57600") { cb_BaudRate.SelectedIndex = 5; }
                else if (ConfigurationManager.AppSettings["BaudRate"].ToString() == "115200") { cb_BaudRate.SelectedIndex = 6; }

                //Load - Led Automatic Enable or Disable
                bool LedAutoOnOff = Convert.ToBoolean(ConfigurationManager.AppSettings["LedAutoOnOff"]);
                cb_LedAutoOnOff.IsChecked = LedAutoOnOff;
                if (LedAutoOnOff)
                {
                    timepicker_LedAutoTime.IsEnabled = true;
                }
                else
                {
                    timepicker_LedAutoTime.IsEnabled = false;
                }

                //Load - Led Automatic Time
                timepicker_LedAutoTime.DateTimeValue = DateTime.Parse(ConfigurationManager.AppSettings["LedAutoTime"], vAppCultureInfo);

                //Load - Remote Port
                tb_ServerPort.Text = ConfigurationManager.AppSettings["ServerPort"].ToString();

                //Load - Monitor Capture
                cb_MonitorCapture.SelectedIndex = Convert.ToInt32(ConfigurationManager.AppSettings["MonitorCapture"]);

                //Load - Led Mode
                cb_LedMode.SelectedIndex = Convert.ToInt32(ConfigurationManager.AppSettings["LedMode"]);

                //Load - Adjust Black Bars
                cb_AdjustBlackBars.IsChecked = Convert.ToBoolean(ConfigurationManager.AppSettings["AdjustBlackBars"]);

                //Load - Led Brightness
                tb_LedBrightness.Text = "Led maximum brightness: " + Convert.ToInt32(ConfigurationManager.AppSettings["LedBrightness"]);
                sldr_LedBrightness.Value = Convert.ToInt32(ConfigurationManager.AppSettings["LedBrightness"]);

                //Load - Led Minimum Brightness
                tb_LedMinBrightness.Text = "Led minimum brightness: " + Convert.ToInt32(ConfigurationManager.AppSettings["LedMinBrightness"]);
                sldr_LedMinBrightness.Value = Convert.ToInt32(ConfigurationManager.AppSettings["LedMinBrightness"]);

                //Load - Led Gamma
                tb_LedGamma.Text = "Led display gamma: " + Convert.ToInt32(ConfigurationManager.AppSettings["LedGamma"]);
                sldr_LedGamma.Value = Convert.ToInt32(ConfigurationManager.AppSettings["LedGamma"]);

                //Load - Led Vibrance
                tb_LedVibrance.Text = "Led color vibrance: " + Convert.ToInt32(ConfigurationManager.AppSettings["LedVibrance"]);
                sldr_LedVibrance.Value = Convert.ToInt32(ConfigurationManager.AppSettings["LedVibrance"]);

                //Load - Color Loop Speed
                tb_ColorLoopSpeed.Text = "Color loop speed: " + Convert.ToInt32(ConfigurationManager.AppSettings["ColorLoopSpeed"]) + " ms";
                sldr_ColorLoopSpeed.Value = Convert.ToInt32(ConfigurationManager.AppSettings["ColorLoopSpeed"]);

                //Load - Spectrum Rotation Speed
                tb_SpectrumRotationSpeed.Text = "Spectrum rotation speed: " + Convert.ToInt32(ConfigurationManager.AppSettings["SpectrumRotationSpeed"]) + " sec";
                sldr_SpectrumRotationSpeed.Value = Convert.ToInt32(ConfigurationManager.AppSettings["SpectrumRotationSpeed"]);

                //Load - Solid Led Color
                string SolidLedColor = ConfigurationManager.AppSettings["SolidLedColor"].ToString();
                colorpicker_SolidLedColor.Background = new BrushConverter().ConvertFrom(SolidLedColor) as SolidColorBrush;

                //Load - Led Hue
                tb_LedHue.Text = "Led color hue: " + Convert.ToInt32(ConfigurationManager.AppSettings["LedHue"]);
                sldr_LedHue.Value = Convert.ToInt32(ConfigurationManager.AppSettings["LedHue"]);

                //Load - Led Color Cut
                tb_LedColorCut.Text = "Minimum color brightness: " + Convert.ToInt32(ConfigurationManager.AppSettings["LedColorCut"]);
                sldr_LedColorCut.Value = Convert.ToInt32(ConfigurationManager.AppSettings["LedColorCut"]);

                //Load - Led Color Red
                tb_LedColorRed.Text = "Red: " + Convert.ToInt32(ConfigurationManager.AppSettings["LedColorRed"]);
                sldr_LedColorRed.Value = Convert.ToInt32(ConfigurationManager.AppSettings["LedColorRed"]);

                //Load - Led Color Green
                tb_LedColorGreen.Text = "Green: " + Convert.ToInt32(ConfigurationManager.AppSettings["LedColorGreen"]);
                sldr_LedColorGreen.Value = Convert.ToInt32(ConfigurationManager.AppSettings["LedColorGreen"]);

                //Load - Led Color Blue
                tb_LedColorBlue.Text = "Blue: " + Convert.ToInt32(ConfigurationManager.AppSettings["LedColorBlue"]);
                sldr_LedColorBlue.Value = Convert.ToInt32(ConfigurationManager.AppSettings["LedColorBlue"]);

                //Load - Led Capture Range
                tb_LedCaptureRange.Text = "Led capture range: " + Convert.ToInt32(ConfigurationManager.AppSettings["LedCaptureRange"]) + "%";
                sldr_LedCaptureRange.Value = Convert.ToInt32(ConfigurationManager.AppSettings["LedCaptureRange"]);

                //Load - Adjust Black Bar Level
                tb_AdjustBlackBarLevel.Text = "Minimum black bar level: " + Convert.ToInt32(ConfigurationManager.AppSettings["AdjustBlackBarLevel"]);
                sldr_AdjustBlackBarLevel.Value = Convert.ToInt32(ConfigurationManager.AppSettings["AdjustBlackBarLevel"]);

                //Load - Led Smoothing
                if (ConfigurationManager.AppSettings["LedSmoothing"].ToString() == "1") { cb_LedSmoothing.SelectedIndex = 2; }
                else if (ConfigurationManager.AppSettings["LedSmoothing"].ToString() == "2") { cb_LedSmoothing.SelectedIndex = 1; }
                else if (ConfigurationManager.AppSettings["LedSmoothing"].ToString() == "3") { cb_LedSmoothing.SelectedIndex = 0; }

                //Load - Led Count
                tb_LedCount.Text = ConfigurationManager.AppSettings["LedCount"];

                //Load - Led Output
                cb_LedOutput.SelectedIndex = Convert.ToInt32(ConfigurationManager.AppSettings["LedOutput"]);

                //Load - Update Rate
                tb_UpdateRate.Text = "Led update rate: " + Convert.ToInt32(ConfigurationManager.AppSettings["UpdateRate"]) + " ms";
                sldr_UpdateRate.Value = Convert.ToInt32(ConfigurationManager.AppSettings["UpdateRate"]);

                //Load - Led Sides
                cb_LedSides.SelectedIndex = Convert.ToInt32(ConfigurationManager.AppSettings["LedSides"]);

                //Load - Led Direction
                cb_LedDirection.SelectedIndex = Convert.ToInt32(ConfigurationManager.AppSettings["LedDirection"]);

                //Check if application is set to launch on Windows startup
                string TargetName_Normal = Assembly.GetEntryAssembly().GetName().Name;
                string TargetFileStartup_Normal = Environment.GetFolderPath(Environment.SpecialFolder.Startup) + "\\" + TargetName_Normal + ".url";
                if (File.Exists(TargetFileStartup_Normal)) { cb_WindowsStartup.IsChecked = true; }
            }
            catch
            {
                Debug.WriteLine("Failed to load the settings.");
            }
        }
    }
}