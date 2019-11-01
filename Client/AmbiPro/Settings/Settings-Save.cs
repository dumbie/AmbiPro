using ArnoldVinkCode;
using System;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Media;
using System.Windows.Threading;

namespace AmbiPro.Settings
{
    public partial class FormSettings
    {
        //Application variables
        private static DispatcherTimer vTextBoxTimer_LedCount = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(500) };
        private static DispatcherTimer vTextBoxTimer_RemotePort = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(500) };

        //Save - Application Settings
        public void SettingsSave()
        {
            try
            {
                Debug.WriteLine("Saving application settings...");

                //Save - Com Port
                cb_ComPort.SelectionChanged += async (sender, e) =>
                {
                    SettingsFunction.Save("ComPort", (cb_ComPort.SelectedIndex + 1).ToString());
                    if (!Convert.ToBoolean(ConfigurationManager.AppSettings["FirstLaunch"])) { await SerialMonitor.LedSwitch(false, true); }
                };

                //Save - Baud Rate
                cb_BaudRate.SelectionChanged += async (sender, e) =>
                {
                    if (cb_BaudRate.SelectedIndex == 0) { SettingsFunction.Save("BaudRate", "9600"); }
                    else if (cb_BaudRate.SelectedIndex == 1) { SettingsFunction.Save("BaudRate", "14400"); }
                    else if (cb_BaudRate.SelectedIndex == 2) { SettingsFunction.Save("BaudRate", "19200"); }
                    else if (cb_BaudRate.SelectedIndex == 3) { SettingsFunction.Save("BaudRate", "28800"); }
                    else if (cb_BaudRate.SelectedIndex == 4) { SettingsFunction.Save("BaudRate", "38400"); }
                    else if (cb_BaudRate.SelectedIndex == 5) { SettingsFunction.Save("BaudRate", "57600"); }
                    else if (cb_BaudRate.SelectedIndex == 6) { SettingsFunction.Save("BaudRate", "115200"); }

                    if (!Convert.ToBoolean(ConfigurationManager.AppSettings["FirstLaunch"])) { await SerialMonitor.LedSwitch(false, true); }
                };

                //Save - Enable or Disable Led Automatic
                cb_LedAutoOnOff.Click += (sender, e) =>
                {
                    if ((bool)cb_LedAutoOnOff.IsChecked)
                    {
                        SettingsFunction.Save("LedAutoOnOff", "True");
                        timepicker_LedAutoTime.IsEnabled = true;
                    }
                    else
                    {
                        SettingsFunction.Save("LedAutoOnOff", "False");
                        timepicker_LedAutoTime.IsEnabled = false;
                    }
                };

                //Save - Led Automatic Time
                timepicker_LedAutoTime.DateTimeChanged += dateTime =>
                {
                    try
                    {
                        CultureInfo AppCultureInfo = CultureInfo.InvariantCulture;
                        SettingsFunction.Save("LedAutoTime", dateTime.Value.ToString(AppCultureInfo));
                    }
                    catch { }
                };

                //Save - Remote Port
                vTextBoxTimer_RemotePort.Tick += vTextBoxTimer_RemotePort_Tick;
                tb_RemotePort.TextChanged += (sender, e) => { AVFunctions.ResetTimer(vTextBoxTimer_RemotePort); };

                //Save - Adjust Black Bars
                cb_AdjustBlackBars.Click += async (sender, e) =>
                {
                    if ((bool)cb_AdjustBlackBars.IsChecked) { SettingsFunction.Save("AdjustBlackBars", "True"); }
                    else { SettingsFunction.Save("AdjustBlackBars", "False"); }
                    if (!Convert.ToBoolean(ConfigurationManager.AppSettings["FirstLaunch"])) { await SerialMonitor.LedSwitch(false, true); }
                };

                //Save - Monitor Capture
                cb_MonitorCapture.SelectionChanged += async (sender, e) =>
                {
                    SettingsFunction.Save("MonitorCapture", cb_MonitorCapture.SelectedIndex.ToString());
                    if (!Convert.ToBoolean(ConfigurationManager.AppSettings["FirstLaunch"])) { await SerialMonitor.LedSwitch(false, true); }
                };

                //Save - Led Mode
                cb_LedMode.SelectionChanged += async (sender, e) =>
                {
                    SettingsFunction.Save("LedMode", cb_LedMode.SelectedIndex.ToString());
                    await SerialMonitor.LedSwitch(false, true);
                };

                //Save - Led Brightness
                sldr_LedBrightness.ValueChanged += (sender, e) =>
                {
                    SettingsFunction.Save("LedBrightness", sldr_LedBrightness.Value.ToString("0"));
                    tb_LedBrightness.Text = "Led maximum brightness: " + sldr_LedBrightness.Value.ToString("0");
                };

                //Save - Led Minimum Brightness
                sldr_LedMinBrightness.ValueChanged += (sender, e) =>
                {
                    SettingsFunction.Save("LedMinBrightness", sldr_LedMinBrightness.Value.ToString("0"));
                    tb_LedMinBrightness.Text = "Led minimum brightness: " + sldr_LedMinBrightness.Value.ToString("0");
                };

                //Save - Led Gamma
                sldr_LedGamma.ValueChanged += (sender, e) =>
                {
                    SettingsFunction.Save("LedGamma", sldr_LedGamma.Value.ToString("0"));
                    tb_LedGamma.Text = "Led display gamma: " + sldr_LedGamma.Value.ToString("0");
                };

                //Save - Led Vibrance
                sldr_LedVibrance.ValueChanged += (sender, e) =>
                {
                    SettingsFunction.Save("LedVibrance", sldr_LedVibrance.Value.ToString("0"));
                    tb_LedVibrance.Text = "Led color vibrance: " + sldr_LedVibrance.Value.ToString("0");
                };

                //Save - Color Loop Speed
                sldr_ColorLoopSpeed.ValueChanged += (sender, e) =>
                {
                    SettingsFunction.Save("ColorLoopSpeed", sldr_ColorLoopSpeed.Value.ToString("0"));
                    tb_ColorLoopSpeed.Text = "Color loop speed: " + sldr_ColorLoopSpeed.Value.ToString("0") + " ms";
                };

                //Save - Spectrum Rotation Speed
                sldr_SpectrumRotationSpeed.ValueChanged += (sender, e) =>
                {
                    SettingsFunction.Save("SpectrumRotationSpeed", sldr_SpectrumRotationSpeed.Value.ToString("0"));
                    tb_SpectrumRotationSpeed.Text = "Spectrum rotation speed: " + sldr_SpectrumRotationSpeed.Value.ToString("0") + " sec";
                };

                //Save - Solid Led Color
                colorpicker_SolidLedColor.SelectedColorChanged += (Color color) =>
                {
                    SettingsFunction.Save("SolidLedColor", color.ToString());
                };

                //Save - Led Hue
                sldr_LedHue.ValueChanged += (sender, e) =>
                {
                    SettingsFunction.Save("LedHue", sldr_LedHue.Value.ToString("0"));
                    tb_LedHue.Text = "Led color hue: " + sldr_LedHue.Value.ToString("0");
                };

                //Save - Led Color Cut
                sldr_LedColorCut.ValueChanged += (sender, e) =>
                {
                    SettingsFunction.Save("LedColorCut", sldr_LedColorCut.Value.ToString("0"));
                    tb_LedColorCut.Text = "Minimum color brightness: " + sldr_LedColorCut.Value.ToString("0");
                };

                //Save - Led Color Red
                sldr_LedColorRed.ValueChanged += (sender, e) =>
                {
                    SettingsFunction.Save("LedColorRed", sldr_LedColorRed.Value.ToString("0"));
                    tb_LedColorRed.Text = "Red: " + sldr_LedColorRed.Value.ToString("0");
                };

                //Save - Led Color Green
                sldr_LedColorGreen.ValueChanged += (sender, e) =>
                {
                    SettingsFunction.Save("LedColorGreen", sldr_LedColorGreen.Value.ToString("0"));
                    tb_LedColorGreen.Text = "Green: " + sldr_LedColorGreen.Value.ToString("0");
                };

                //Save - Led Color Blue
                sldr_LedColorBlue.ValueChanged += (sender, e) =>
                {
                    SettingsFunction.Save("LedColorBlue", sldr_LedColorBlue.Value.ToString("0"));
                    tb_LedColorBlue.Text = "Blue: " + sldr_LedColorBlue.Value.ToString("0");
                };

                //Save - Led Capture Range
                sldr_LedCaptureRange.ValueChanged += (sender, e) =>
                {
                    SettingsFunction.Save("LedCaptureRange", sldr_LedCaptureRange.Value.ToString("0"));
                    tb_LedCaptureRange.Text = "Led capture range: " + sldr_LedCaptureRange.Value.ToString("0") + "%";
                };

                //Save - Adjust Black Bar Level
                sldr_AdjustBlackBarLevel.ValueChanged += (sender, e) =>
                {
                    SettingsFunction.Save("AdjustBlackBarLevel", sldr_AdjustBlackBarLevel.Value.ToString("0"));
                    tb_AdjustBlackBarLevel.Text = "Minimum black bar level: " + sldr_AdjustBlackBarLevel.Value.ToString("0");
                };

                //Save - Led Smoothing
                cb_LedSmoothing.SelectionChanged += (sender, e) =>
                {
                    if (cb_LedSmoothing.SelectedIndex == 0) { SettingsFunction.Save("LedSmoothing", "3"); }
                    else if (cb_LedSmoothing.SelectedIndex == 1) { SettingsFunction.Save("LedSmoothing", "2"); }
                    else if (cb_LedSmoothing.SelectedIndex == 2) { SettingsFunction.Save("LedSmoothing", "1"); }
                };

                //Save - Led Count
                vTextBoxTimer_LedCount.Tick += vTextBoxTimer_LedCount_Tick;
                tb_LedCount.TextChanged += (sender, e) => { AVFunctions.ResetTimer(vTextBoxTimer_LedCount); };

                //Save - Led Output
                cb_LedOutput.SelectionChanged += (sender, e) =>
                {
                    SettingsFunction.Save("LedOutput", cb_LedOutput.SelectedIndex.ToString());
                };

                //Save - Update Rate
                sldr_UpdateRate.ValueChanged += (sender, e) =>
                {
                    SettingsFunction.Save("UpdateRate", sldr_UpdateRate.Value.ToString("0"));
                    tb_UpdateRate.Text = "Led update rate: " + sldr_UpdateRate.Value.ToString("0") + " ms";
                };

                //Save - Led Sides
                cb_LedSides.SelectionChanged += (sender, e) =>
                {
                    SettingsFunction.Save("LedSides", cb_LedSides.SelectedIndex.ToString());
                };

                //Save - Led Direction
                cb_LedDirection.SelectionChanged += (sender, e) =>
                {
                    SettingsFunction.Save("LedDirection", cb_LedDirection.SelectedIndex.ToString());
                };

                //Save - Windows Startup
                cb_WindowsStartup.Click += (sender, e) => { ManageShortcutStartup(); };
            }
            catch { Debug.WriteLine("Failed to save the settings."); }
        }

        //Update led count after delay
        private async void vTextBoxTimer_LedCount_Tick(object sender, EventArgs e)
        {
            try
            {
                //Stop the timer
                vTextBoxTimer_LedCount.Stop();

                //Color brushes
                BrushConverter BrushConvert = new BrushConverter();
                Brush BrushInvalid = BrushConvert.ConvertFromString("#cd1a2b") as Brush;
                Brush BrushValid = BrushConvert.ConvertFromString("#1db954") as Brush;

                if (String.IsNullOrWhiteSpace(tb_LedCount.Text)) { tb_LedCount.BorderBrush = BrushInvalid; return; }
                if (Regex.IsMatch(tb_LedCount.Text, "(\\D+)")) { tb_LedCount.BorderBrush = BrushInvalid; return; }
                if (Convert.ToInt32(tb_LedCount.Text) < 10) { tb_LedCount.BorderBrush = BrushInvalid; return; }

                SettingsFunction.Save("LedCount", tb_LedCount.Text);
                tb_LedCount.BorderBrush = BrushValid;
                if (!Convert.ToBoolean(ConfigurationManager.AppSettings["FirstLaunch"])) { await SerialMonitor.LedSwitch(false, true); }
            }
            catch { }
        }

        //Update remote port after delay
        private async void vTextBoxTimer_RemotePort_Tick(object sender, EventArgs e)
        {
            try
            {
                //Stop the timer
                vTextBoxTimer_RemotePort.Stop();

                //Color brushes
                BrushConverter BrushConvert = new BrushConverter();
                Brush BrushInvalid = BrushConvert.ConvertFromString("#cd1a2b") as Brush;
                Brush BrushValid = BrushConvert.ConvertFromString("#1db954") as Brush;

                //Check for text input and length
                if (tb_RemotePort.Text.Length > 5) { tb_RemotePort.BorderBrush = BrushInvalid; return; }
                if (String.IsNullOrWhiteSpace(tb_RemotePort.Text)) { tb_RemotePort.BorderBrush = BrushInvalid; return; }

                //Check if text input has invalid characters
                if (tb_RemotePort.Text.Contains("-")) { tb_RemotePort.Text = tb_RemotePort.Text.Replace("-", ""); tb_RemotePort.SelectionStart = tb_RemotePort.Text.Length; }
                if (tb_RemotePort.Text.Contains(",")) { tb_RemotePort.Text = tb_RemotePort.Text.Replace(",", ""); tb_RemotePort.SelectionStart = tb_RemotePort.Text.Length; }
                if (tb_RemotePort.Text.Contains(".")) { tb_RemotePort.Text = tb_RemotePort.Text.Replace(".", ""); tb_RemotePort.SelectionStart = tb_RemotePort.Text.Length; }
                if (Regex.IsMatch(tb_RemotePort.Text, "(\\D+)")) { tb_RemotePort.BorderBrush = BrushInvalid; return; }

                //Convert text input to number
                Int32 ServerPort = Convert.ToInt32(tb_RemotePort.Text);
                if (ServerPort < 1 || ServerPort > 65535) { tb_RemotePort.BorderBrush = BrushInvalid; return; }

                SettingsFunction.Save("RemotePort", tb_RemotePort.Text);
                tb_RemotePort.BorderBrush = BrushValid;

                //Restart the remote server
                await Socket.SocketServerSwitch(false, true);
            }
            catch { }
        }

        //Create startup shortcut
        void ManageShortcutStartup()
        {
            try
            {
                //Set application shortcut paths
                string TargetFilePath_Normal = Assembly.GetExecutingAssembly().CodeBase;
                string TargetName_Normal = Assembly.GetExecutingAssembly().GetName().Name;
                string TargetFileShortcut_Normal = Environment.GetFolderPath(Environment.SpecialFolder.Startup) + "\\" + TargetName_Normal + ".url";

                //Check if the shortcut already exists
                if (!File.Exists(TargetFileShortcut_Normal))
                {
                    using (StreamWriter StreamWriter = new StreamWriter(TargetFileShortcut_Normal))
                    {
                        StreamWriter.WriteLine("[InternetShortcut]");
                        StreamWriter.WriteLine("URL=" + TargetFilePath_Normal);
                        StreamWriter.WriteLine("IconFile=" + TargetFilePath_Normal.Replace("file:///", ""));
                        StreamWriter.WriteLine("IconIndex=0");
                        StreamWriter.Flush();
                    }
                }
                else
                {
                    Debug.WriteLine("Removing application from Windows startup");
                    if (File.Exists(TargetFileShortcut_Normal)) { File.Delete(TargetFileShortcut_Normal); }
                }
            }
            catch { }
        }
    }
}