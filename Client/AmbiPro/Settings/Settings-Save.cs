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
                vDispatcherTimer_SettingBaudRate.Tick += SettingSaveBaudRate;
                textbox_BaudRate.TextChanged += (sender, e) =>
                {
                    AVFunctions.TimerReset(vDispatcherTimer_SettingBaudRate);
                };

                //Save - Led Automatic Enable or Disable 
                cb_LedAutoOnOffBefore.Click += (sender, e) =>
                {
                    bool enabledDisabled = (bool)cb_LedAutoOnOffBefore.IsChecked;
                    SettingsFunction.Save("LedAutoOnOffBefore", enabledDisabled.ToString());
                    timepicker_LedAutoTimeBefore.IsEnabled = enabledDisabled;
                };
                cb_LedAutoOnOffAfter.Click += (sender, e) =>
                {
                    bool enabledDisabled = (bool)cb_LedAutoOnOffAfter.IsChecked;
                    SettingsFunction.Save("LedAutoOnOffAfter", enabledDisabled.ToString());
                    timepicker_LedAutoTimeAfter.IsEnabled = enabledDisabled;
                };

                //Save - Led Automatic Time
                timepicker_LedAutoTimeBefore.DateTimeChanged += dateTime =>
                {
                    try
                    {
                        SettingsFunction.Save("LedAutoTimeBefore", dateTime.Value.ToString(vAppCultureInfo));
                    }
                    catch { }
                };
                timepicker_LedAutoTimeAfter.DateTimeChanged += dateTime =>
                {
                    try
                    {
                        SettingsFunction.Save("LedAutoTimeAfter", dateTime.Value.ToString(vAppCultureInfo));
                    }
                    catch { }
                };

                //Save - Server Port
                vDispatcherTimer_SettingServerPort.Tick += SettingSaveServerPort;
                tb_ServerPort.TextChanged += (sender, e) =>
                {
                    AVFunctions.TimerReset(vDispatcherTimer_SettingServerPort);
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
                        await UpdateSettingsInformation(true);
                    }
                };

                //Save - Led Mode
                cb_LedMode.SelectionChanged += async (sender, e) =>
                {
                    SettingsFunction.Save("LedMode", cb_LedMode.SelectedIndex.ToString());
                    if (!Convert.ToBoolean(ConfigurationManager.AppSettings["FirstLaunch2"]))
                    {
                        await LedSwitch(LedSwitches.Restart);
                        await UpdateSettingsInformation(true);
                    }
                };

                //Save - Led Bottom Gap
                sldr_LedBottomGap.ValueChanged += (sender, e) =>
                {
                    SettingsFunction.Save("LedBottomGap", sldr_LedBottomGap.Value.ToString("0"));
                    tb_LedBottomGap.Text = "Led gap bottom stand: " + sldr_LedBottomGap.Value.ToString("0") + " leds";
                };

                //Save - Led contrast level
                sldr_LedContrast.ValueChanged += (sender, e) =>
                {
                    SettingsFunction.Save("LedContrast", sldr_LedContrast.Value.ToString("0"));
                    tb_LedContrast.Text = "Contrast level: " + sldr_LedContrast.Value.ToString("0");
                };

                //Save - Led brightness level
                sldr_LedBrightness.ValueChanged += (sender, e) =>
                {
                    SettingsFunction.Save("LedBrightness", sldr_LedBrightness.Value.ToString("0"));
                    tb_LedBrightness.Text = "Brightness level: " + sldr_LedBrightness.Value.ToString("0") + "%";
                };

                //Save - Led Minimum Brightness
                sldr_LedMinBrightness.ValueChanged += (sender, e) =>
                {
                    SettingsFunction.Save("LedMinBrightness", sldr_LedMinBrightness.Value.ToString("0"));
                    tb_LedMinBrightness.Text = "Minimum brightness level: " + sldr_LedMinBrightness.Value.ToString("0") + "%";
                };

                //Save - Led Gamma
                sldr_LedGamma.ValueChanged += (sender, e) =>
                {
                    SettingsFunction.Save("LedGamma", sldr_LedGamma.Value.ToString("0"));
                    tb_LedGamma.Text = "Gamma level: " + sldr_LedGamma.Value.ToString("0") + "%";
                };

                //Save - Led Saturation
                sldr_LedSaturation.ValueChanged += (sender, e) =>
                {
                    SettingsFunction.Save("LedSaturation", sldr_LedSaturation.Value.ToString("0"));
                    tb_LedSaturation.Text = "Color saturation: " + sldr_LedSaturation.Value.ToString("0") + "%";
                };

                //Save - Led Temperature
                sldr_LedTemperature.ValueChanged += (sender, e) =>
                {
                    SettingsFunction.Save("LedTemperature", sldr_LedTemperature.Value.ToString("0"));
                    tb_LedTemperature.Text = "Color temperature: " + sldr_LedTemperature.Value.ToString("0") + "K";
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

                //Select - Solid Led Color
                button_ColorPickerSolid.Click += async (sender, e) =>
                {
                    Color? newColor = await new AVColorPicker().Popup(null);
                    if (newColor != null)
                    {
                        SolidColorBrush newBrush = new SolidColorBrush((Color)newColor);
                        button_ColorPickerSolid.Background = newBrush;
                        SettingsFunction.Save("SolidLedColor", newColor.ToString());
                    }
                };

                //Save - Led Hue
                sldr_LedHue.ValueChanged += (sender, e) =>
                {
                    SettingsFunction.Save("LedHue2", sldr_LedHue.Value.ToString("0"));
                    tb_LedHue.Text = "Color hue: " + sldr_LedHue.Value.ToString("0") + "°";
                };

                //Save - Led Minimum Color
                sldr_LedMinColor.ValueChanged += (sender, e) =>
                {
                    SettingsFunction.Save("LedMinColor", sldr_LedMinColor.Value.ToString("0"));
                    tb_LedMinColor.Text = "Minimum color brightness: " + sldr_LedMinColor.Value.ToString("0") + "%";
                };

                //Save - Led Color Red
                sldr_LedColorRed.ValueChanged += (sender, e) =>
                {
                    SettingsFunction.Save("LedColorRed", sldr_LedColorRed.Value.ToString("0"));
                    tb_LedColorRed.Text = "Red: " + sldr_LedColorRed.Value.ToString("0") + "%";
                };

                //Save - Led Color Green
                sldr_LedColorGreen.ValueChanged += (sender, e) =>
                {
                    SettingsFunction.Save("LedColorGreen", sldr_LedColorGreen.Value.ToString("0"));
                    tb_LedColorGreen.Text = "Green: " + sldr_LedColorGreen.Value.ToString("0") + "%";
                };

                //Save - Led Color Blue
                sldr_LedColorBlue.ValueChanged += (sender, e) =>
                {
                    SettingsFunction.Save("LedColorBlue", sldr_LedColorBlue.Value.ToString("0"));
                    tb_LedColorBlue.Text = "Blue: " + sldr_LedColorBlue.Value.ToString("0") + "%";
                };

                //Save - Led Capture Range
                sldr_LedCaptureRange.ValueChanged += (sender, e) =>
                {
                    SettingsFunction.Save("LedCaptureRange", sldr_LedCaptureRange.Value.ToString("0"));
                    tb_LedCaptureRange.Text = "Led capture range: " + sldr_LedCaptureRange.Value.ToString("0") + "%";
                };

                //Save - Blackbar detect rate
                sldr_AdjustBlackbarRate.ValueChanged += (sender, e) =>
                {
                    SettingsFunction.Save("AdjustBlackbarRate", sldr_AdjustBlackbarRate.Value.ToString("0"));
                    tb_AdjustBlackbarRate.Text = "Blackbar detection rate: " + Convert.ToInt32(sldr_AdjustBlackbarRate.Value) + " ms";
                };

                //Save - Blackbar detect range
                sldr_AdjustBlackbarRange.ValueChanged += (sender, e) =>
                {
                    SettingsFunction.Save("AdjustBlackbarRange", sldr_AdjustBlackbarRange.Value.ToString("0"));
                    tb_AdjustBlackbarRange.Text = "Blackbar detection range: " + Convert.ToInt32(sldr_AdjustBlackbarRange.Value) + "%";
                };

                //Save - Adjust Black Bar Level
                sldr_AdjustBlackBarBrightness.ValueChanged += (sender, e) =>
                {
                    SettingsFunction.Save("AdjustBlackBarBrightness", sldr_AdjustBlackBarBrightness.Value.ToString("0"));
                    tb_AdjustBlackBarBrightness.Text = "Blackbar minimum brightness: " + sldr_AdjustBlackBarBrightness.Value.ToString("0") + "%";
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
                vDispatcherTimer_SettingLedCount.Tick += SettingSaveLedCount;
                textbox_LedCountFirst.TextChanged += (sender, e) =>
                {
                    AVFunctions.TimerReset(vDispatcherTimer_SettingLedCount);
                };
                textbox_LedCountSecond.TextChanged += (sender, e) =>
                {
                    AVFunctions.TimerReset(vDispatcherTimer_SettingLedCount);
                };
                textbox_LedCountThird.TextChanged += (sender, e) =>
                {
                    AVFunctions.TimerReset(vDispatcherTimer_SettingLedCount);
                };
                textbox_LedCountFourth.TextChanged += (sender, e) =>
                {
                    AVFunctions.TimerReset(vDispatcherTimer_SettingLedCount);
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
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to save the settings: " + ex.Message);
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

        //Save baud rate after delay
        public async void SettingSaveBaudRate(object sender, EventArgs e)
        {
            try
            {
                //Stop the timer
                vDispatcherTimer_SettingBaudRate.Stop();

                //Color brushes
                BrushConverter BrushConvert = new BrushConverter();
                Brush BrushInvalid = BrushConvert.ConvertFromString("#cd1a2b") as Brush;
                Brush BrushValid = BrushConvert.ConvertFromString("#1db954") as Brush;

                //Check text input and length
                if (string.IsNullOrWhiteSpace(textbox_BaudRate.Text)) { textbox_BaudRate.BorderBrush = BrushInvalid; return; }

                //Check text input has invalid characters
                if (Regex.IsMatch(textbox_BaudRate.Text, "(\\D+)")) { textbox_BaudRate.BorderBrush = BrushInvalid; return; }

                //Check text input number
                int intBaudRate = Convert.ToInt32(textbox_BaudRate.Text);
                if (intBaudRate < 1 || intBaudRate > 268435456) { textbox_BaudRate.BorderBrush = BrushInvalid; return; }

                //Save the new baud rate setting
                SettingsFunction.Save("BaudRate", textbox_BaudRate.Text);
                textbox_BaudRate.BorderBrush = BrushValid;

                //Restart the leds
                if (!Convert.ToBoolean(ConfigurationManager.AppSettings["FirstLaunch2"]))
                {
                    await LedSwitch(LedSwitches.Restart);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed saving baudrate: " + ex.Message);
            }
        }

        //Save led count after delay
        public async void SettingSaveLedCount(object sender, EventArgs e)
        {
            try
            {
                //Stop the timer
                vDispatcherTimer_SettingLedCount.Stop();

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
            catch (Exception ex)
            {
                Debug.WriteLine("Failed saving ledcount: " + ex.Message);
            }
        }

        //Update server port after delay
        private async void SettingSaveServerPort(object sender, EventArgs e)
        {
            try
            {
                //Stop the timer
                vDispatcherTimer_SettingServerPort.Stop();

                //Color brushes
                BrushConverter BrushConvert = new BrushConverter();
                Brush BrushInvalid = BrushConvert.ConvertFromString("#cd1a2b") as Brush;
                Brush BrushValid = BrushConvert.ConvertFromString("#1db954") as Brush;

                //Check text input and length
                if (string.IsNullOrWhiteSpace(tb_ServerPort.Text)) { tb_ServerPort.BorderBrush = BrushInvalid; return; }

                //Check text input has invalid characters
                if (Regex.IsMatch(tb_ServerPort.Text, "(\\D+)")) { tb_ServerPort.BorderBrush = BrushInvalid; return; }

                //Check text input number
                int ServerPort = Convert.ToInt32(tb_ServerPort.Text);
                if (ServerPort < 1 || ServerPort > 65535) { tb_ServerPort.BorderBrush = BrushInvalid; return; }

                SettingsFunction.Save("ServerPort", tb_ServerPort.Text);
                tb_ServerPort.BorderBrush = BrushValid;

                //Restart the socket server
                vArnoldVinkSockets.vSocketServerPort = Convert.ToInt32(tb_ServerPort.Text);
                await vArnoldVinkSockets.SocketServerRestart();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed saving serverport: " + ex.Message);
            }
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