using ArnoldVinkCode;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using static AmbiPro.AppClasses;
using static AmbiPro.AppEnums;
using static AmbiPro.AppTasks;
using static AmbiPro.AppVariables;
using static AmbiPro.PreloadSettings;
using static ArnoldVinkCode.AVActions;
using static ArnoldVinkCode.AVArrayFunctions;
using static ArnoldVinkCode.AVSettings;

namespace AmbiPro
{
    partial class SerialMonitor
    {
        //Turn the leds on or off
        public static async Task LedSwitch(LedSwitches ledSwitch)
        {
            try
            {
                if (!vLedSwitching)
                {
                    vLedSwitching = true;

                    //Update settings preload
                    UpdateSettingsPreload();

                    //Restart the leds
                    if (ledSwitch == LedSwitches.Restart)
                    {
                        Debug.WriteLine("Restarting the led updates.");
                        await LedsDisable(true);
                        await LedsEnable();
                        return;
                    }

                    //Check if leds are enabled
                    if (ledSwitch == LedSwitches.Disable || vTask_UpdateLed.TaskRunning)
                    {
                        //Disable the leds
                        await LedsDisable(false);

                        //Update screen information
                        DispatcherInvoke(delegate
                        {
                            vFormSettings.UpdateScreenInformation();
                        });
                    }
                    else
                    {
                        //Enable the leds
                        await LedsEnable();
                    }
                }
            }
            catch
            {
                Debug.WriteLine("Failed switching the leds on or off.");
            }
            finally
            {
                vLedSwitching = false;
            }
        }

        //Update led preview
        public static void UpdateLedPreview(bool ledsOn)
        {
            try
            {
                if (ledsOn && setLedMode == 0)
                {
                    DispatcherInvoke(delegate
                    {
                        vFormSettings.listbox_LedPreviewLeft.Items.Clear();
                        vFormSettings.listbox_LedPreviewTop.Items.Clear();
                        vFormSettings.listbox_LedPreviewRight.Items.Clear();
                        vFormSettings.listbox_LedPreviewBottom.Items.Clear();

                        LedPreviewAddDefaultLeds(setLedSideFirst, setLedCountFirst);
                        LedPreviewAddDefaultLeds(setLedSideSecond, setLedCountSecond);
                        LedPreviewAddDefaultLeds(setLedSideThird, setLedCountThird);
                        LedPreviewAddDefaultLeds(setLedSideFourth, setLedCountFourth);

                        vFormSettings.border_LedPreviewLeft.Visibility = System.Windows.Visibility.Visible;
                        vFormSettings.border_LedPreviewTop.Visibility = System.Windows.Visibility.Visible;
                        vFormSettings.border_LedPreviewRight.Visibility = System.Windows.Visibility.Visible;
                        vFormSettings.border_LedPreviewBottom.Visibility = System.Windows.Visibility.Visible;
                    });
                }
                else
                {
                    DispatcherInvoke(delegate
                    {
                        vFormSettings.border_LedPreviewLeft.Visibility = System.Windows.Visibility.Collapsed;
                        vFormSettings.border_LedPreviewTop.Visibility = System.Windows.Visibility.Collapsed;
                        vFormSettings.border_LedPreviewRight.Visibility = System.Windows.Visibility.Collapsed;
                        vFormSettings.border_LedPreviewBottom.Visibility = System.Windows.Visibility.Collapsed;

                        vFormSettings.listbox_LedPreviewLeft.Items.Clear();
                        vFormSettings.listbox_LedPreviewTop.Items.Clear();
                        vFormSettings.listbox_LedPreviewRight.Items.Clear();
                        vFormSettings.listbox_LedPreviewBottom.Items.Clear();
                    });
                }
            }
            catch { }
        }

        private static void LedPreviewAddDefaultLeds(LedSideTypes ledSideTypes, int ledSideCount)
        {
            try
            {
                if (ledSideTypes == LedSideTypes.LeftTopToBottom || ledSideTypes == LedSideTypes.LeftBottomToTop)
                {
                    for (int ledsAdded = 0; ledsAdded < ledSideCount; ledsAdded++)
                    {
                        vFormSettings.listbox_LedPreviewLeft.Items.Add(new SolidColorBrush(Colors.Black));
                    }
                }
                else if (ledSideTypes == LedSideTypes.TopLeftToRight || ledSideTypes == LedSideTypes.TopRightToLeft)
                {
                    for (int ledsAdded = 0; ledsAdded < ledSideCount; ledsAdded++)
                    {
                        vFormSettings.listbox_LedPreviewTop.Items.Add(new SolidColorBrush(Colors.Black));
                    }
                }
                else if (ledSideTypes == LedSideTypes.RightTopToBottom || ledSideTypes == LedSideTypes.RightBottomToTop)
                {
                    for (int ledsAdded = 0; ledsAdded < ledSideCount; ledsAdded++)
                    {
                        vFormSettings.listbox_LedPreviewRight.Items.Add(new SolidColorBrush(Colors.Black));
                    }
                }
                else if (ledSideTypes == LedSideTypes.BottomLeftToRight || ledSideTypes == LedSideTypes.BottomRightToLeft)
                {
                    for (int ledsAdded = 0; ledsAdded < ledSideCount; ledsAdded++)
                    {
                        vFormSettings.listbox_LedPreviewBottom.Items.Add(new SolidColorBrush(Colors.Black));
                    }
                }
            }
            catch { }
        }

        //Update led status icons
        public static void UpdateLedStatusIcons(bool ledsOn)
        {
            try
            {
                if (ledsOn)
                {
                    DispatcherInvoke(delegate
                    {
                        vFormSettings.image_SwitchLedsOnOrOff.Source = new BitmapImage(new Uri("/Assets/Icons/Leds.png", UriKind.RelativeOrAbsolute));
                    });
                    AppTray.NotifyIcon.Icon = new Icon(AVEmbedded.EmbeddedResourceToStream(null, "AmbiPro.Assets.ApplicationIcon.ico"));
                }
                else
                {
                    DispatcherInvoke(delegate
                    {
                        vFormSettings.image_SwitchLedsOnOrOff.Source = new BitmapImage(new Uri("/Assets/Icons/LedsOff.png", UriKind.RelativeOrAbsolute));
                    });
                    AppTray.NotifyIcon.Icon = new Icon(AVEmbedded.EmbeddedResourceToStream(null, "AmbiPro.Assets.ApplicationIcon-Disabled.ico"));
                }
            }
            catch { }
        }

        //Enable the led updates
        private static async Task LedsEnable()
        {
            try
            {
                Debug.WriteLine("Enabling the led updates.");

                //Check led count
                if (setLedCountTotal <= 0)
                {
                    await ShowNoLedsSideCountSetup();
                    return;
                }

                //Update led status icons
                UpdateLedStatusIcons(true);

                //Update led preview
                UpdateLedPreview(true);

                //Start led update loop
                AVActions.TaskStartLoop(LoopUpdateLeds, vTask_UpdateLed);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to enable the led updates: " + ex.Message);
            }
        }

        //Disable the led updates
        private static async Task LedsDisable(bool restartLeds)
        {
            try
            {
                Debug.WriteLine("Disabling the led updates.");

                //Update led status icons
                UpdateLedStatusIcons(false);

                //Update led preview
                UpdateLedPreview(false);

                //Cancel the led task
                await AVActions.TaskStopLoop(vTask_UpdateLed, 3000);

                //Disable the serial port
                if (vSerialComPort != null && vSerialComPort.IsOpen)
                {
                    //Send black leds update
                    if (!restartLeds)
                    {
                        //Create led ColorRGBA array
                        ColorRGBA[] colorArray = CreateArray(setLedCountTotal, ColorRGBA.Black);

                        //Send serial bytes to device
                        SerialComPortWrite(colorArray);
                    }

                    vSerialComPort.Close();
                    vSerialComPort.Dispose();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to disable the led updates: " + ex.Message);
            }
        }

        //Write to serial com port
        public static bool SerialComPortWrite(ColorRGBA[] colorArray)
        {
            try
            {
                //Convert led ColorRGBA to byte array
                byte[] serialBytes = ConvertColorRGBAtoLedByteArray(colorArray);

                //Write bytes to serial com port
                vSerialComPort.Write(serialBytes, 0, serialBytes.Length);
                //Debug.WriteLine("Bytes written to com port: " + totalByteSize);
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to write to com port: " + ex.Message);
                return false;
            }
        }

        //Show the settings window
        private static void ShowSettings()
        {
            try
            {
                AVActions.DispatcherInvoke(delegate
                {
                    vFormSettings.Show();
                });
            }
            catch { }
        }

        //Show led setup message
        private static async Task ShowNoLedsSideCountSetup()
        {
            try
            {
                Debug.WriteLine("There are currently no leds configured.");
                await AVActions.DispatcherInvoke(async delegate
                {
                    List<string> MsgBoxAnswers = new List<string>();
                    MsgBoxAnswers.Add("Ok");

                    await new AVMessageBox().Popup(null, "Failed to turn leds on or off", "Please make sure that you have setup your led sides and count.", MsgBoxAnswers);
                });
            }
            catch { }
        }

        //Show device connection message
        private static async Task ShowFailedConnectionMessage()
        {
            try
            {
                await AVActions.DispatcherInvoke(async delegate
                {
                    List<string> MsgBoxAnswers = new List<string>();
                    MsgBoxAnswers.Add("Change com port");
                    MsgBoxAnswers.Add("Retry to connect");
                    MsgBoxAnswers.Add("Exit application");

                    string MsgBoxResult = await new AVMessageBox().Popup(null, "Failed to connect to your com port device", "Please make sure the device is not in use by another application, the correct com port is selected and that the required drivers are installed on your system.", MsgBoxAnswers);
                    if (MsgBoxResult == "Change com port")
                    {
                        await LedSwitch(LedSwitches.Disable);
                        ShowSettings();
                    }
                    else if (MsgBoxResult == "Retry to connect")
                    {
                        await LedSwitch(LedSwitches.Restart);
                    }
                    else if (MsgBoxResult == "Exit application")
                    {
                        await AppExit.Exit();
                    }
                });
            }
            catch { }
        }

        //Show failed capture message
        private static async Task ShowFailedCaptureMessage(string captureMessage)
        {
            try
            {
                await AVActions.DispatcherInvoke(async delegate
                {
                    List<string> MsgBoxAnswers = new List<string>();
                    MsgBoxAnswers.Add("Change monitor setting");
                    MsgBoxAnswers.Add("Change current led mode");
                    MsgBoxAnswers.Add("Retry to capture screen");
                    MsgBoxAnswers.Add("Exit application");

                    string MsgBoxResult = await new AVMessageBox().Popup(null, "Failed to start capturing your screen", "Please make sure the correct screen is selected, all the requirements are installed on your PC, that you have a Windows 11 64bit installation and that you have a DirectX 12 or higher capable graphics adapter.\n\nError: " + captureMessage, MsgBoxAnswers);
                    if (MsgBoxResult == "Change monitor setting")
                    {
                        await LedSwitch(LedSwitches.Disable);
                        ShowSettings();
                    }
                    else if (MsgBoxResult == "Change current led mode")
                    {
                        await LedSwitch(LedSwitches.Disable);
                        ShowSettings();
                    }
                    else if (MsgBoxResult == "Retry to capture screen")
                    {
                        await LedSwitch(LedSwitches.Restart);
                    }
                    else if (MsgBoxResult == "Exit application")
                    {
                        await AppExit.Exit();
                    }
                });
            }
            catch { }
        }

        //Convert ColorRGBA array to led byte array
        public static byte[] ConvertColorRGBAtoLedByteArray(ColorRGBA[] colorArray)
        {
            try
            {
                //Calculate bytes size
                int initByteSize = 3;
                int ledByteSize = setLedCountTotal * 3;
                int totalByteSize = initByteSize + ledByteSize;

                //Create led byte array
                byte[] serialBytes = new byte[totalByteSize];
                serialBytes[0] = Encoding.Unicode.GetBytes("A").First();
                serialBytes[1] = Encoding.Unicode.GetBytes("d").First();
                serialBytes[2] = Encoding.Unicode.GetBytes("a").First();

                //Convert color array
                int currentSerialByte = initByteSize;
                foreach (ColorRGBA colorRGBA in colorArray)
                {
                    serialBytes[currentSerialByte] = colorRGBA.R;
                    currentSerialByte++;

                    serialBytes[currentSerialByte] = colorRGBA.G;
                    currentSerialByte++;

                    serialBytes[currentSerialByte] = colorRGBA.B;
                    currentSerialByte++;
                }

                return serialBytes;
            }
            catch
            {
                return Array.Empty<byte>();
            }
        }

        //Update leds in loop
        private static async Task LoopUpdateLeds()
        {
            try
            {
                //Connect to the device
                vSerialComPort = new SerialPort(setSerialPortName, setSerialBaudRate);
                vSerialComPort.Open();
                Debug.WriteLine("Connected to the com port device: " + setSerialPortName + "/" + setSerialBaudRate);

                //Reset default variables
                ResetVariables();

                //Set first launch setting to false
                SettingSave(vConfiguration, "FirstLaunch2", "False");

                //Check led display mode
                if (setLedMode == 0) { await ModeScreenCapture(); }
                else if (setLedMode == 1) { await ModeSolidColor(); }
                else if (setLedMode == 2) { await ModeColorLoop(); }
                else if (setLedMode == 3) { await ModeColorSpectrum(); }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to update the leds: " + ex.Message);
                await ShowFailedConnectionMessage();
            }
        }
    }
}