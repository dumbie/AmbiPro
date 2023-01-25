using ArnoldVinkCode;
using System;
using System.Configuration;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using static AmbiPro.AppEnums;
using static AmbiPro.AppVariables;
using static AmbiPro.SerialMonitor;
using static ArnoldVinkCode.AVSettings;

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
                    SettingSave(vConfiguration, "ComPort", (cb_ComPort.SelectedIndex + 1).ToString());
                    if (!SettingLoad(vConfiguration, "FirstLaunch2", typeof(bool)))
                    {
                        await LedSwitch(LedSwitches.Restart);
                    }
                };

                //Save - Baud Rate
                textbox_BaudRate.TextChanged += (sender, e) =>
                {
                    SettingSaveBaudRate();
                };

                //Save - Led Automatic Enable or Disable 
                cb_LedAutoOnOffBefore.Click += (sender, e) =>
                {
                    bool enabledDisabled = (bool)cb_LedAutoOnOffBefore.IsChecked;
                    SettingSave(vConfiguration, "LedAutoOnOffBefore", enabledDisabled.ToString());
                    timepicker_LedAutoTimeBefore.IsEnabled = enabledDisabled;
                };
                cb_LedAutoOnOffAfter.Click += (sender, e) =>
                {
                    bool enabledDisabled = (bool)cb_LedAutoOnOffAfter.IsChecked;
                    SettingSave(vConfiguration, "LedAutoOnOffAfter", enabledDisabled.ToString());
                    timepicker_LedAutoTimeAfter.IsEnabled = enabledDisabled;
                };

                //Save - Led Automatic Time
                timepicker_LedAutoTimeBefore.DateTimeChanged += dateTime =>
                {
                    try
                    {
                        SettingSave(vConfiguration, "LedAutoTimeBefore", dateTime.Value.ToString(vAppCultureInfo));
                    }
                    catch { }
                };
                timepicker_LedAutoTimeAfter.DateTimeChanged += dateTime =>
                {
                    try
                    {
                        SettingSave(vConfiguration, "LedAutoTimeAfter", dateTime.Value.ToString(vAppCultureInfo));
                    }
                    catch { }
                };

                //Save - Server Port
                tb_ServerPort.TextChanged += (sender, e) =>
                {
                    SettingSaveServerPort();
                };

                //Save - Adjust Black Bars
                cb_AdjustBlackBars.Click += async (sender, e) =>
                {
                    if ((bool)cb_AdjustBlackBars.IsChecked) { SettingSave(vConfiguration, "AdjustBlackBars", "True"); }
                    else { SettingSave(vConfiguration, "AdjustBlackBars", "False"); }
                    if (!SettingLoad(vConfiguration, "FirstLaunch2", typeof(bool)))
                    {
                        await LedSwitch(LedSwitches.Restart);
                    }
                };

                //Save - Monitor Capture
                cb_MonitorCapture.SelectionChanged += async (sender, e) =>
                {
                    SettingSave(vConfiguration, "MonitorCapture", cb_MonitorCapture.SelectedIndex.ToString());
                    if (!SettingLoad(vConfiguration, "FirstLaunch2", typeof(bool)))
                    {
                        await LedSwitch(LedSwitches.Restart);
                        await UpdateSettingsInformation(true);
                    }
                };

                //Save - Led Mode
                cb_LedMode.SelectionChanged += async (sender, e) =>
                {
                    SettingSave(vConfiguration, "LedMode", cb_LedMode.SelectedIndex.ToString());
                    if (!SettingLoad(vConfiguration, "FirstLaunch2", typeof(bool)))
                    {
                        await LedSwitch(LedSwitches.Restart);
                        await UpdateSettingsInformation(true);
                    }
                };

                //Save - Led Bottom Gap
                sldr_LedBottomGap.ValueChanged += (sender, e) =>
                {
                    SettingSave(vConfiguration, "LedBottomGap", sldr_LedBottomGap.Value.ToString("0"));
                    tb_LedBottomGap.Text = "Led gap bottom stand: " + sldr_LedBottomGap.Value.ToString("0") + " leds";
                };

                //Save - Led contrast level
                sldr_LedContrast.ValueChanged += (sender, e) =>
                {
                    SettingSave(vConfiguration, "LedContrast", sldr_LedContrast.Value.ToString("0"));
                    tb_LedContrast.Text = "Contrast level: " + sldr_LedContrast.Value.ToString("0");
                };

                //Save - Led brightness level
                sldr_LedBrightness.ValueChanged += (sender, e) =>
                {
                    SettingSave(vConfiguration, "LedBrightness", sldr_LedBrightness.Value.ToString("0"));
                    tb_LedBrightness.Text = "Brightness level: " + sldr_LedBrightness.Value.ToString("0") + "%";
                };

                //Save - Led Energy Saving Mode
                cb_LedEnergyMode.Click += (sender, e) =>
                {
                    if ((bool)cb_LedEnergyMode.IsChecked) { SettingSave(vConfiguration, "LedEnergyMode", "True"); }
                    else { SettingSave(vConfiguration, "LedEnergyMode", "False"); }
                };

                //Save - Led Minimum Brightness
                sldr_LedMinBrightness.ValueChanged += (sender, e) =>
                {
                    SettingSave(vConfiguration, "LedMinBrightness", sldr_LedMinBrightness.Value.ToString("0"));
                    tb_LedMinBrightness.Text = "Minimum brightness level: " + sldr_LedMinBrightness.Value.ToString("0") + "%";
                };

                //Save - Led Gamma
                sldr_LedGamma.ValueChanged += (sender, e) =>
                {
                    SettingSave(vConfiguration, "LedGamma3", sldr_LedGamma.Value);
                    tb_LedGamma.Text = "Gamma curve: " + sldr_LedGamma.Value.ToString("0.00");
                };

                //Save - Led Saturation
                sldr_LedSaturation.ValueChanged += (sender, e) =>
                {
                    SettingSave(vConfiguration, "LedSaturation", sldr_LedSaturation.Value.ToString("0"));
                    tb_LedSaturation.Text = "Color saturation: " + sldr_LedSaturation.Value.ToString("0") + "%";
                };

                //Save - Color Loop Speed
                sldr_ColorLoopSpeed.ValueChanged += (sender, e) =>
                {
                    SettingSave(vConfiguration, "ColorLoopSpeed", sldr_ColorLoopSpeed.Value.ToString("0"));
                    tb_ColorLoopSpeed.Text = "Color loop speed: " + sldr_ColorLoopSpeed.Value.ToString("0") + " ms";
                };

                //Save - Spectrum Rotation Speed
                sldr_SpectrumRotationSpeed.ValueChanged += (sender, e) =>
                {
                    SettingSave(vConfiguration, "SpectrumRotationSpeed", sldr_SpectrumRotationSpeed.Value.ToString("0"));
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
                        SettingSave(vConfiguration, "SolidLedColor", newColor.ToString());
                    }
                };

                //Save - Led Minimum Color
                sldr_LedMinColor.ValueChanged += (sender, e) =>
                {
                    SettingSave(vConfiguration, "LedMinColor2", sldr_LedMinColor.Value.ToString("0"));
                    tb_LedMinColor.Text = "Minimum color brightness: " + sldr_LedMinColor.Value.ToString("0") + "%";
                };

                //Save - Led Color Red
                sldr_LedColorRed.ValueChanged += (sender, e) =>
                {
                    SettingSave(vConfiguration, "LedColorRed", sldr_LedColorRed.Value.ToString("0"));
                    tb_LedColorRed.Text = "Red: " + sldr_LedColorRed.Value.ToString("0") + "%";
                };

                //Save - Led Color Green
                sldr_LedColorGreen.ValueChanged += (sender, e) =>
                {
                    SettingSave(vConfiguration, "LedColorGreen", sldr_LedColorGreen.Value.ToString("0"));
                    tb_LedColorGreen.Text = "Green: " + sldr_LedColorGreen.Value.ToString("0") + "%";
                };

                //Save - Led Color Blue
                sldr_LedColorBlue.ValueChanged += (sender, e) =>
                {
                    SettingSave(vConfiguration, "LedColorBlue", sldr_LedColorBlue.Value.ToString("0"));
                    tb_LedColorBlue.Text = "Blue: " + sldr_LedColorBlue.Value.ToString("0") + "%";
                };

                //Save - Led Capture Range
                sldr_LedCaptureRange.ValueChanged += (sender, e) =>
                {
                    SettingSave(vConfiguration, "LedCaptureRange", sldr_LedCaptureRange.Value.ToString("0"));
                    tb_LedCaptureRange.Text = "Led capture range: " + sldr_LedCaptureRange.Value.ToString("0") + "%";
                };

                //Save - Blackbar detect range
                sldr_AdjustBlackbarRange.ValueChanged += (sender, e) =>
                {
                    SettingSave(vConfiguration, "AdjustBlackbarRange", sldr_AdjustBlackbarRange.Value.ToString("0"));
                    tb_AdjustBlackbarRange.Text = "Blackbar detection range: " + Convert.ToInt32(sldr_AdjustBlackbarRange.Value) + "%";
                };

                //Save - Blackbar Brightness
                sldr_AdjustBlackBarBrightness.ValueChanged += (sender, e) =>
                {
                    SettingSave(vConfiguration, "AdjustBlackBarBrightness", sldr_AdjustBlackBarBrightness.Value.ToString("0"));
                    tb_AdjustBlackBarBrightness.Text = "Blackbar minimum brightness: " + sldr_AdjustBlackBarBrightness.Value.ToString("0") + "%";
                };

                //Save - Blackbar Update Rate
                sldr_AdjustBlackbarUpdateRate.ValueChanged += (sender, e) =>
                {
                    SettingSave(vConfiguration, "AdjustBlackbarUpdateRate", sldr_AdjustBlackbarUpdateRate.Value.ToString("0"));
                    tb_AdjustBlackbarUpdateRate.Text = "Blackbar update rate: " + sldr_AdjustBlackbarUpdateRate.Value.ToString("0") + "ms";
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
                textbox_LedCountFirst.TextChanged += (sender, e) =>
                {
                    SettingSaveLedCount();
                };
                textbox_LedCountSecond.TextChanged += (sender, e) =>
                {
                    SettingSaveLedCount();
                };
                textbox_LedCountThird.TextChanged += (sender, e) =>
                {
                    SettingSaveLedCount();
                };
                textbox_LedCountFourth.TextChanged += (sender, e) =>
                {
                    SettingSaveLedCount();
                };

                //Save - Led Output
                cb_LedOutput.SelectionChanged += (sender, e) =>
                {
                    SettingSave(vConfiguration, "LedOutput", cb_LedOutput.SelectedIndex.ToString());
                };

                //Save - Update Rate
                sldr_UpdateRate.ValueChanged += (sender, e) =>
                {
                    SettingSave(vConfiguration, "UpdateRate", sldr_UpdateRate.Value.ToString("0"));
                    int updateRateMs = Convert.ToInt32(sldr_UpdateRate.Value);
                    string updateRateFps = Convert.ToInt32(1000 / updateRateMs).ToString();
                    tb_UpdateRate.Text = "Led update rate: " + updateRateMs + " ms (" + updateRateFps + " fps)";
                };

                //Save - Led Smoothing
                sldr_LedSmoothing.ValueChanged += (sender, e) =>
                {
                    SettingSave(vConfiguration, "LedSmoothing", sldr_LedSmoothing.Value.ToString("0"));
                    int smoothingFrames = Convert.ToInt32(sldr_LedSmoothing.Value);
                    tb_LedSmoothing.Text = "Led smoothing: " + smoothingFrames + " frames";
                };

                //Save - Capture HDR Brightness
                sldr_CaptureHdrBrightness.ValueChanged += (sender, e) =>
                {
                    SettingSave(vConfiguration, "CaptureHdrBrightness", sldr_CaptureHdrBrightness.Value.ToString("0"));
                    int captureHdrBrightness = Convert.ToInt32(sldr_CaptureHdrBrightness.Value);
                    tb_CaptureHdrBrightness.Text = "HDR capture brightness: " + captureHdrBrightness;
                    if (!SettingLoad(vConfiguration, "FirstLaunch2", typeof(bool)))
                    {
                        UpdateCaptureSettings();
                    }
                };

                //Save - Windows Startup
                cb_WindowsStartup.Click += (sender, e) =>
                {
                    AVSettings.ManageStartupShortcut("Launcher.exe");
                };

                //Save - Debug BlackBar
                checkbox_DebugBlackBar.Click += (sender, e) =>
                {
                    if ((bool)checkbox_DebugBlackBar.IsChecked)
                    {
                        SettingSave(vConfiguration, "DebugBlackBar", "True");
                    }
                    else
                    {
                        SettingSave(vConfiguration, "DebugBlackBar", "False");
                    }
                };

                //Save - Debug LedPreview
                checkbox_DebugLedPreview.Click += (sender, e) =>
                {
                    if ((bool)checkbox_DebugLedPreview.IsChecked)
                    {
                        SettingSave(vConfiguration, "DebugLedPreview", "True");
                        grid_LedPreview.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        SettingSave(vConfiguration, "DebugLedPreview", "False");
                        grid_LedPreview.Visibility = Visibility.Collapsed;
                    }
                };

                //Save - Debug color
                checkbox_DebugColor.Click += (sender, e) =>
                {
                    if ((bool)checkbox_DebugColor.IsChecked)
                    {
                        SettingSave(vConfiguration, "DebugColor", "True");
                    }
                    else
                    {
                        SettingSave(vConfiguration, "DebugColor", "False");
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
                        SettingSave(vConfiguration, settingName, "0");
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
                SettingSave(vConfiguration, sideName, sideIndex);

                //Reset led rotate setting
                SettingResetLedRotate();

                //Restart the leds
                if (!SettingLoad(vConfiguration, "FirstLaunch2", typeof(bool)))
                {
                    await LedSwitch(LedSwitches.Restart);
                }
            }
            catch { }
        }

        //Save baud rate after delay
        public async void SettingSaveBaudRate()
        {
            try
            {
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
                SettingSave(vConfiguration, "BaudRate", textbox_BaudRate.Text);
                textbox_BaudRate.BorderBrush = BrushValid;

                //Restart the leds
                if (!SettingLoad(vConfiguration, "FirstLaunch2", typeof(bool)))
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
        public async void SettingSaveLedCount()
        {
            try
            {
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
                SettingSave(vConfiguration, "LedCountFirst", textbox_LedCountFirst.Text);
                SettingSave(vConfiguration, "LedCountSecond", textbox_LedCountSecond.Text);
                SettingSave(vConfiguration, "LedCountThird", textbox_LedCountThird.Text);
                SettingSave(vConfiguration, "LedCountFourth", textbox_LedCountFourth.Text);
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
                if (!SettingLoad(vConfiguration, "FirstLaunch2", typeof(bool)))
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
        private async void SettingSaveServerPort()
        {
            try
            {
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

                SettingSave(vConfiguration, "ServerPort", tb_ServerPort.Text);
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
    }
}