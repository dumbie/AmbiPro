using ArnoldVinkCode;
using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Media;
using static AmbiPro.AppEnums;
using static AmbiPro.AppTimers;
using static AmbiPro.AppVariables;
using static AmbiPro.SerialMonitor;

namespace AmbiPro.Settings
{
    public partial class FormSettings
    {
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
                    if (!Convert.ToBoolean(ConfigurationManager.AppSettings["FirstLaunch2"]))
                    {
                        await LedSwitch(LedSwitches.Restart);
                    }
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
                    if (!Convert.ToBoolean(ConfigurationManager.AppSettings["FirstLaunch2"]))
                    {
                        await LedSwitch(LedSwitches.Restart);
                    }
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
                        SettingsFunction.Save("LedAutoTime", dateTime.Value.ToString(vAppCultureInfo));
                    }
                    catch { }
                };

                //Save - Remote Port
                vDispatcherTimer_ServerPort.Tick += vDispatcherTimer_ServerPort_Tick;
                tb_ServerPort.TextChanged += (sender, e) =>
                {
                    AVFunctions.TimerReset(vDispatcherTimer_ServerPort);
                };

                //Save - Adjust Black Bars
                cb_AdjustBlackBars.Click += async (sender, e) =>
                {
                    if ((bool)cb_AdjustBlackBars.IsChecked) { SettingsFunction.Save("AdjustBlackBars", "True"); }
                    else { SettingsFunction.Save("AdjustBlackBars", "False"); }
                    if (!Convert.ToBoolean(ConfigurationManager.AppSettings["FirstLaunch2"]))
                    {
                        await LedSwitch(LedSwitches.Restart);
                    }
                };

                //Save - Monitor Capture
                cb_MonitorCapture.SelectionChanged += async (sender, e) =>
                {
                    SettingsFunction.Save("MonitorCapture", cb_MonitorCapture.SelectedIndex.ToString());
                    if (!Convert.ToBoolean(ConfigurationManager.AppSettings["FirstLaunch2"]))
                    {
                        await LedSwitch(LedSwitches.Restart);
                    }
                };

                //Save - Led Mode
                cb_LedMode.SelectionChanged += async (sender, e) =>
                {
                    SettingsFunction.Save("LedMode", cb_LedMode.SelectedIndex.ToString());
                    if (!Convert.ToBoolean(ConfigurationManager.AppSettings["FirstLaunch2"]))
                    {
                        await LedSwitch(LedSwitches.Restart);
                    }
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

                //Save - Led Side Types
                combobox_LedSideFirst.SelectionChanged += async (sender, e) =>
                {
                    await SettingSaveLedSide("LedSideFirst", combobox_LedSideFirst.SelectedIndex.ToString());
                };
                combobox_LedSideSecond.SelectionChanged += async (sender, e) =>
                {
                    await SettingSaveLedSide("LedSideSecond", combobox_LedSideSecond.SelectedIndex.ToString());
                };
                combobox_LedSideThird.SelectionChanged += async (sender, e) =>
                {
                    await SettingSaveLedSide("LedSideThird", combobox_LedSideThird.SelectedIndex.ToString());
                };
                combobox_LedSideFourth.SelectionChanged += async (sender, e) =>
                {
                    await SettingSaveLedSide("LedSideFourth", combobox_LedSideFourth.SelectedIndex.ToString());
                };

                //Save - Led Count
                vDispatcherTimer_LedCount.Tick += SettingSaveLedCount;
                textbox_LedCountFirst.TextChanged += (sender, e) =>
                {
                    AVFunctions.TimerReset(vDispatcherTimer_LedCount);
                };
                textbox_LedCountSecond.TextChanged += (sender, e) =>
                {
                    AVFunctions.TimerReset(vDispatcherTimer_LedCount);
                };
                textbox_LedCountThird.TextChanged += (sender, e) =>
                {
                    AVFunctions.TimerReset(vDispatcherTimer_LedCount);
                };
                textbox_LedCountFourth.TextChanged += (sender, e) =>
                {
                    AVFunctions.TimerReset(vDispatcherTimer_LedCount);
                };

                //Save - Led Output
                cb_LedOutput.SelectionChanged += (sender, e) =>
                {
                    SettingsFunction.Save("LedOutput", cb_LedOutput.SelectedIndex.ToString());
                };

                //Save - Update Rate
                sldr_UpdateRate.ValueChanged += (sender, e) =>
                {
                    SettingsFunction.Save("UpdateRate", sldr_UpdateRate.Value.ToString("0"));
                    int updateRateMs = Convert.ToInt32(sldr_UpdateRate.Value);
                    string updateRateFps = Convert.ToInt32(1000 / updateRateMs).ToString();
                    tb_UpdateRate.Text = "Led update rate: " + updateRateMs + " ms (" + updateRateFps + " fps)";
                };

                //Save - Windows Startup
                cb_WindowsStartup.Click += (sender, e) => { ManageShortcutStartup(); };

                //Save - Debug mode
                checkbox_DebugMode.Click += (sender, e) =>
                {
                    if ((bool)checkbox_DebugMode.IsChecked)
                    {
                        SettingsFunction.Save("DebugMode", "True");
                    }
                    else
                    {
                        SettingsFunction.Save("DebugMode", "False");
                    }
                };

                //Save - Debug BlackBar
                checkbox_DebugBlackBar.Click += (sender, e) =>
                {
                    if ((bool)checkbox_DebugBlackBar.IsChecked)
                    {
                        SettingsFunction.Save("DebugBlackBar", "True");
                    }
                    else
                    {
                        SettingsFunction.Save("DebugBlackBar", "False");
                    }
                };

                //Save - Debug color
                checkbox_DebugColor.Click += (sender, e) =>
                {
                    if ((bool)checkbox_DebugColor.IsChecked)
                    {
                        SettingsFunction.Save("DebugColor", "True");
                    }
                    else
                    {
                        SettingsFunction.Save("DebugColor", "False");
                    }
                };

                //Save - Debug Save
                checkbox_DebugSave.Click += (sender, e) =>
                {
                    if ((bool)checkbox_DebugSave.IsChecked)
                    {
                        SettingsFunction.Save("DebugSave", "True");
                    }
                    else
                    {
                        SettingsFunction.Save("DebugSave", "False");
                    }
                };
            }
            catch
            {
                Debug.WriteLine("Failed to save the settings.");
            }
        }

        //Reset all led rotate settings
        void SettingResetLedRotate()
        {
            try
            {
                Debug.WriteLine("Resetting all led rotate settings.");
                foreach (string settingName in ConfigurationManager.AppSettings)
                {
                    if (settingName.StartsWith("LedRotate") && settingName.Contains(":"))
                    {
                        SettingsFunction.Save(settingName, "0");
                    }
                }
            }
            catch { }
        }

        //Save led sides
        public async Task SettingSaveLedSide(string sideName, string sideIndex)
        {
            try
            {
                //Save the new led side setting
                SettingsFunction.Save(sideName, sideIndex);

                //Reset led rotate setting
                SettingResetLedRotate();

                //Restart the leds
                if (!Convert.ToBoolean(ConfigurationManager.AppSettings["FirstLaunch2"]))
                {
                    await LedSwitch(LedSwitches.Restart);
                }
            }
            catch { }
        }

        //Save led count after delay
        public async void SettingSaveLedCount(object sender, EventArgs e)
        {
            try
            {
                //Stop the timer
                vDispatcherTimer_LedCount.Stop();

                //Color brushes
                BrushConverter BrushConvert = new BrushConverter();
                Brush BrushInvalid = BrushConvert.ConvertFromString("#cd1a2b") as Brush;
                Brush BrushValid = BrushConvert.ConvertFromString("#1db954") as Brush;

                //Check the led count value
                bool invalidCount = false;
                if (string.IsNullOrWhiteSpace(textbox_LedCountFirst.Text)) { textbox_LedCountFirst.BorderBrush = BrushInvalid; invalidCount = true; }
                if (Regex.IsMatch(textbox_LedCountFirst.Text, "(\\D+)")) { textbox_LedCountFirst.BorderBrush = BrushInvalid; invalidCount = true; }

                if (string.IsNullOrWhiteSpace(textbox_LedCountSecond.Text)) { textbox_LedCountSecond.BorderBrush = BrushInvalid; invalidCount = true; }
                if (Regex.IsMatch(textbox_LedCountSecond.Text, "(\\D+)")) { textbox_LedCountSecond.BorderBrush = BrushInvalid; invalidCount = true; }

                if (string.IsNullOrWhiteSpace(textbox_LedCountThird.Text)) { textbox_LedCountThird.BorderBrush = BrushInvalid; invalidCount = true; }
                if (Regex.IsMatch(textbox_LedCountThird.Text, "(\\D+)")) { textbox_LedCountThird.BorderBrush = BrushInvalid; invalidCount = true; }

                if (string.IsNullOrWhiteSpace(textbox_LedCountFourth.Text)) { textbox_LedCountFourth.BorderBrush = BrushInvalid; invalidCount = true; }
                if (Regex.IsMatch(textbox_LedCountFourth.Text, "(\\D+)")) { textbox_LedCountFourth.BorderBrush = BrushInvalid; invalidCount = true; }
                if (invalidCount) { return; }

                //Save the new led count setting
                SettingsFunction.Save("LedCountFirst", textbox_LedCountFirst.Text);
                SettingsFunction.Save("LedCountSecond", textbox_LedCountSecond.Text);
                SettingsFunction.Save("LedCountThird", textbox_LedCountThird.Text);
                SettingsFunction.Save("LedCountFourth", textbox_LedCountFourth.Text);
                textbox_LedCountFirst.BorderBrush = BrushValid;
                textbox_LedCountSecond.BorderBrush = BrushValid;
                textbox_LedCountThird.BorderBrush = BrushValid;
                textbox_LedCountFourth.BorderBrush = BrushValid;

                //Update total led count
                int totalCount = Convert.ToInt32(textbox_LedCountFirst.Text) + Convert.ToInt32(textbox_LedCountSecond.Text) + Convert.ToInt32(textbox_LedCountThird.Text) + Convert.ToInt32(textbox_LedCountFourth.Text);
                textblock_LedCount.Text = "Total led count: " + totalCount + " (must be equal with arduino script)";

                //Reset led rotate setting
                SettingResetLedRotate();

                //Restart the leds
                if (!Convert.ToBoolean(ConfigurationManager.AppSettings["FirstLaunch2"]))
                {
                    await LedSwitch(LedSwitches.Restart);
                }
            }
            catch { }
        }

        //Update remote port after delay
        private async void vDispatcherTimer_ServerPort_Tick(object sender, EventArgs e)
        {
            try
            {
                //Stop the timer
                vDispatcherTimer_ServerPort.Stop();

                //Color brushes
                BrushConverter BrushConvert = new BrushConverter();
                Brush BrushInvalid = BrushConvert.ConvertFromString("#cd1a2b") as Brush;
                Brush BrushValid = BrushConvert.ConvertFromString("#1db954") as Brush;

                //Check for text input and length
                if (tb_ServerPort.Text.Length > 5) { tb_ServerPort.BorderBrush = BrushInvalid; return; }
                if (String.IsNullOrWhiteSpace(tb_ServerPort.Text)) { tb_ServerPort.BorderBrush = BrushInvalid; return; }

                //Check if text input has invalid characters
                if (tb_ServerPort.Text.Contains("-")) { tb_ServerPort.Text = tb_ServerPort.Text.Replace("-", ""); tb_ServerPort.SelectionStart = tb_ServerPort.Text.Length; }
                if (tb_ServerPort.Text.Contains(",")) { tb_ServerPort.Text = tb_ServerPort.Text.Replace(",", ""); tb_ServerPort.SelectionStart = tb_ServerPort.Text.Length; }
                if (tb_ServerPort.Text.Contains(".")) { tb_ServerPort.Text = tb_ServerPort.Text.Replace(".", ""); tb_ServerPort.SelectionStart = tb_ServerPort.Text.Length; }
                if (Regex.IsMatch(tb_ServerPort.Text, "(\\D+)")) { tb_ServerPort.BorderBrush = BrushInvalid; return; }

                //Convert text input to number
                Int32 ServerPort = Convert.ToInt32(tb_ServerPort.Text);
                if (ServerPort < 1 || ServerPort > 65535) { tb_ServerPort.BorderBrush = BrushInvalid; return; }

                SettingsFunction.Save("ServerPort", tb_ServerPort.Text);
                tb_ServerPort.BorderBrush = BrushValid;

                //Restart the socket server
                vArnoldVinkSockets.vSocketServerPort = Convert.ToInt32(tb_ServerPort.Text);
                await vArnoldVinkSockets.SocketServerRestart();
            }
            catch { }
        }

        //Create startup shortcut
        void ManageShortcutStartup()
        {
            try
            {
                //Set application shortcut paths
                string TargetIconPath = Assembly.GetEntryAssembly().CodeBase.Replace("file:///", string.Empty);
                string TargetFilePath = Assembly.GetEntryAssembly().CodeBase.Replace(".exe", "-Admin.exe").Replace("file:///", string.Empty);
                string TargetName = Assembly.GetEntryAssembly().GetName().Name;
                string TargetFileShortcut = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Startup), TargetName + ".url");

                //Check if the shortcut already exists
                if (!File.Exists(TargetFileShortcut))
                {
                    Debug.WriteLine("Adding application to Windows startup.");
                    using (StreamWriter StreamWriter = new StreamWriter(TargetFileShortcut))
                    {
                        StreamWriter.WriteLine("[InternetShortcut]");
                        StreamWriter.WriteLine("URL=" + TargetFilePath);
                        StreamWriter.WriteLine("IconFile=" + TargetIconPath);
                        StreamWriter.WriteLine("IconIndex=0");
                        StreamWriter.Flush();
                    }
                }
                else
                {
                    Debug.WriteLine("Removing application from Windows startup.");
                    if (File.Exists(TargetFileShortcut))
                    {
                        File.Delete(TargetFileShortcut);
                    }
                }
            }
            catch
            {
                Debug.WriteLine("Failed creating startup shortcut.");
            }
        }
    }
}