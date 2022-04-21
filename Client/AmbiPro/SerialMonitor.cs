using ArnoldVinkCode;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using static AmbiPro.AppEnums;
using static AmbiPro.AppTasks;
using static AmbiPro.AppVariables;
using static AmbiPro.PreloadSettings;
using static ArnoldVinkCode.AVActions;

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
                        LedsEnable();
                        return;
                    }

                    //Check if leds are enabled
                    if (ledSwitch == LedSwitches.Disable || vTask_UpdateLed.TaskRunning)
                    {
                        //Disable the leds
                        await LedsDisable(false);

                        //Update screen information
                        ActionDispatcherInvoke(delegate
                        {
                            App.vFormSettings.UpdateScreenInformation();
                        });
                    }
                    else
                    {
                        //Enable the leds
                        LedsEnable();
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

        //Update led status icons
        public static void UpdateLedStatusIcons(bool ledsOn)
        {
            try
            {
                if (ledsOn)
                {
                    ActionDispatcherInvoke(delegate
                    {
                        App.vFormSettings.image_SwitchLedsOnOrOff.Source = new BitmapImage(new Uri("/Assets/Icons/Leds.png", UriKind.RelativeOrAbsolute));
                    });
                    AppTray.NotifyIcon.Icon = new Icon(Assembly.GetEntryAssembly().GetManifestResourceStream("AmbiPro.Assets.ApplicationIcon.ico"));
                }
                else
                {
                    ActionDispatcherInvoke(delegate
                    {
                        App.vFormSettings.image_SwitchLedsOnOrOff.Source = new BitmapImage(new Uri("/Assets/Icons/LedsOff.png", UriKind.RelativeOrAbsolute));
                    });
                    AppTray.NotifyIcon.Icon = new Icon(Assembly.GetEntryAssembly().GetManifestResourceStream("AmbiPro.Assets.ApplicationIcon-Disabled.ico"));
                }
            }
            catch { }
        }

        //Enable the led updates
        private static void LedsEnable()
        {
            try
            {
                Debug.WriteLine("Enabling the led updates.");

                //Check led count
                if (setLedCountTotal <= 0)
                {
                    ShowNoLedsSideCountSetup();
                    return;
                }

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
                if (!restartLeds)
                {
                    UpdateLedStatusIcons(false);
                }

                //Cancel the led task
                await AVActions.TaskStopLoop(vTask_UpdateLed);

                //Disable the serial port
                if (vSerialComPort.IsOpen)
                {
                    //Send black leds update
                    if (!restartLeds)
                    {
                        //Calculate bytes size
                        int InitialByteSize = 3;
                        int ByteLedSize = (setLedCountTotal * 3);
                        int TotalBytes = InitialByteSize + ByteLedSize;

                        //Create led byte array
                        byte[] SerialBytes = new byte[TotalBytes];
                        SerialBytes[0] = Encoding.Unicode.GetBytes("A").First();
                        SerialBytes[1] = Encoding.Unicode.GetBytes("d").First();
                        SerialBytes[2] = Encoding.Unicode.GetBytes("a").First();

                        //Send the serial bytes to device
                        SerialComPortWrite(SerialBytes);
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
        public static bool SerialComPortWrite(byte[] SerialBytes)
        {
            try
            {
                vSerialComPort.Write(SerialBytes, 0, SerialBytes.Length);
                //Debug.WriteLine("Bytes written to com port: " + SerialBytes.Length);
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
                AVActions.ActionDispatcherInvoke(delegate
                {
                    App.vFormSettings.Show();
                });
            }
            catch { }
        }

        //Show led setup message
        private static void ShowNoLedsSideCountSetup()
        {
            try
            {
                Debug.WriteLine("There are currently no leds configured.");
                AVActions.ActionDispatcherInvoke(async delegate
                {
                    List<string> MsgBoxAnswers = new List<string>();
                    MsgBoxAnswers.Add("Ok");

                    await new AVMessageBox().Popup(null, "Failed to turn leds on or off", "Please make sure that you have setup your led sides and count.", MsgBoxAnswers);
                });
            }
            catch { }
        }

        //Show device connection message
        private static void ShowFailedConnectionMessage()
        {
            try
            {
                AVActions.ActionDispatcherInvoke(async delegate
                {
                    List<string> MsgBoxAnswers = new List<string>();
                    MsgBoxAnswers.Add("Change com port");
                    MsgBoxAnswers.Add("Retry to connect");
                    MsgBoxAnswers.Add("Close application");

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
                    else if (MsgBoxResult == "Close application")
                    {
                        await AppStartup.Application_Exit();
                    }
                });
            }
            catch { }
        }

        //Show monitor screen message
        private static void ShowFailedCaptureMessage()
        {
            try
            {
                AVActions.ActionDispatcherInvoke(async delegate
                {
                    List<string> MsgBoxAnswers = new List<string>();
                    MsgBoxAnswers.Add("Change monitor setting");
                    MsgBoxAnswers.Add("Change the led mode");
                    MsgBoxAnswers.Add("Retry to capture screen");
                    MsgBoxAnswers.Add("Close application");

                    string MsgBoxResult = await new AVMessageBox().Popup(null, "Failed to start capturing your monitor screen", "Please make sure the correct monitor screen is selected, all the requirements are installed on your PC, that you have a 64bit Windows installation and that you have a DirectX 12 or higher capable graphics adapter installed.", MsgBoxAnswers);
                    if (MsgBoxResult == "Change monitor setting")
                    {
                        await LedSwitch(LedSwitches.Disable);
                        ShowSettings();
                    }
                    else if (MsgBoxResult == "Change the led mode")
                    {
                        await LedSwitch(LedSwitches.Disable);
                        ShowSettings();
                    }
                    else if (MsgBoxResult == "Retry to capture screen")
                    {
                        await LedSwitch(LedSwitches.Restart);
                    }
                    else if (MsgBoxResult == "Close application")
                    {
                        await AppStartup.Application_Exit();
                    }
                });
            }
            catch { }
        }

        //Update the leds in loop
        private static async Task LoopUpdateLeds()
        {
            try
            {
                //Calculate bytes size
                int InitByteSize = 3;
                int LedByteSize = setLedCountTotal * 3;
                int TotalByteSize = InitByteSize + LedByteSize;

                //Connect to the device
                vSerialComPort = new SerialPort(setSerialPortName, setSerialBaudRate);
                vSerialComPort.Open();
                Debug.WriteLine("Connected to the com port device: " + setSerialPortName + "/" + setSerialBaudRate);

                //Create led byte array
                byte[] SerialBytes = new byte[TotalByteSize];
                SerialBytes[0] = Encoding.Unicode.GetBytes("A").First();
                SerialBytes[1] = Encoding.Unicode.GetBytes("d").First();
                SerialBytes[2] = Encoding.Unicode.GetBytes("a").First();

                //Reset default variables
                ResetVariables();

                //Set first launch setting to false
                AVSettings.Save(vConfiguration, "FirstLaunch2", "False");

                //Check led display mode
                if (setLedMode == 0) { await ModeScreenCapture(InitByteSize, SerialBytes); }
                else if (setLedMode == 1) { await ModeSolidColor(InitByteSize, SerialBytes); }
                else if (setLedMode == 2) { await ModeColorLoop(InitByteSize, SerialBytes); }
                else if (setLedMode == 3) { await ModeColorSpectrum(InitByteSize, SerialBytes); }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to update the leds: " + ex.Message);
                ShowFailedConnectionMessage();
            }
        }
    }
}