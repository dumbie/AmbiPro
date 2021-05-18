using AmbiPro.Settings;
using ArnoldVinkCode;
using System;
using System.Collections.Generic;
using System.Configuration;
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
using static ArnoldVinkCode.AVActions;

namespace AmbiPro
{
    partial class SerialMonitor
    {
        //Application variables
        private static bool vSwitching = false;

        //Serial port
        private static SerialPort vSerialComPort = null;

        //Device settings
        private static string setSerialPortName = string.Empty;
        private static int setSerialBaudRate = 0;

        //Led settings
        private static bool setAdjustBlackBars = true;
        private static int setAdjustBlackbarRate = 0;
        private static int setAdjustBlackbarRange = 0;
        private static int setAdjustBlackBarBrightness = 0;
        private static int setUpdateRate = 0;
        private static int setLedBottomGap = 0;
        private static double setLedBrightness = 0;
        private static byte setLedMinBrightness = 0;
        private static double setLedGamma = 0;
        private static double setLedSaturation = 0;
        private static int setColorLoopSpeed = 0;
        private static int setSpectrumRotationSpeed = 0;
        private static string setSolidLedColor = string.Empty;
        private static double setLedHue = 0;
        private static int setLedColorCut = 0;
        private static double setLedColorRed = 0;
        private static double setLedColorGreen = 0;
        private static double setLedColorBlue = 0;
        private static LedOutputTypes setLedOutput = 0;
        private static int setLedCaptureRange = 0;
        private static int setLedRotate = 0;
        private static int setLedMode = 0;
        private static LedSideTypes setLedSideFirst = 0;
        private static LedSideTypes setLedSideSecond = 0;
        private static LedSideTypes setLedSideThird = 0;
        private static LedSideTypes setLedSideFourth = 0;
        private static int setLedCountFirst = 0;
        private static int setLedCountSecond = 0;
        private static int setLedCountThird = 0;
        private static int setLedCountFourth = 0;
        private static int setLedCountTotal = 0;
        private static bool setDebugMode = false;
        private static bool setDebugBlackBar = false;
        private static bool setDebugColor = true;
        private static bool setDebugSave = false;

        //Update the led settings
        public static void UpdateSettings()
        {
            try
            {
                //Device settings
                setSerialPortName = "COM" + ConfigurationManager.AppSettings["ComPort"].ToString();
                setSerialBaudRate = Convert.ToInt32(ConfigurationManager.AppSettings["BaudRate"]);

                //Led settings
                setAdjustBlackBars = Convert.ToBoolean(ConfigurationManager.AppSettings["AdjustBlackBars"]);
                setAdjustBlackbarRate = Convert.ToInt32(ConfigurationManager.AppSettings["AdjustBlackbarRate"]);
                setAdjustBlackbarRange = Convert.ToInt32(ConfigurationManager.AppSettings["AdjustBlackbarRange"]);
                setAdjustBlackBarBrightness = Convert.ToInt32(ConfigurationManager.AppSettings["AdjustBlackBarBrightness"]);
                setUpdateRate = Convert.ToInt32(ConfigurationManager.AppSettings["UpdateRate"]);
                setLedBottomGap = Convert.ToInt32(ConfigurationManager.AppSettings["LedBottomGap"]);
                setLedBrightness = Convert.ToDouble(ConfigurationManager.AppSettings["LedBrightness"]) / 100;
                setLedMinBrightness = Convert.ToByte(ConfigurationManager.AppSettings["LedMinBrightness"]);
                setLedGamma = Convert.ToDouble(ConfigurationManager.AppSettings["LedGamma"]) / 100;
                setLedSaturation = Convert.ToDouble(ConfigurationManager.AppSettings["LedSaturation"]) / 100;
                setColorLoopSpeed = Convert.ToInt32(ConfigurationManager.AppSettings["ColorLoopSpeed"]);
                setSpectrumRotationSpeed = Convert.ToInt32(ConfigurationManager.AppSettings["SpectrumRotationSpeed"]);
                setSolidLedColor = ConfigurationManager.AppSettings["SolidLedColor"].ToString();
                setLedHue = Convert.ToDouble(ConfigurationManager.AppSettings["LedHue2"]);
                setLedColorCut = Convert.ToInt32(ConfigurationManager.AppSettings["LedColorCut"]);
                setLedColorRed = Convert.ToDouble(ConfigurationManager.AppSettings["LedColorRed"]) / 100;
                setLedColorGreen = Convert.ToDouble(ConfigurationManager.AppSettings["LedColorGreen"]) / 100;
                setLedColorBlue = Convert.ToDouble(ConfigurationManager.AppSettings["LedColorBlue"]) / 100;
                setLedCaptureRange = Convert.ToInt32(ConfigurationManager.AppSettings["LedCaptureRange"]);
                setLedOutput = (LedOutputTypes)Convert.ToInt32(ConfigurationManager.AppSettings["LedOutput"]);
                setLedMode = Convert.ToInt32(ConfigurationManager.AppSettings["LedMode"]);
                setLedSideFirst = (LedSideTypes)Convert.ToInt32(ConfigurationManager.AppSettings["LedSideFirst"]);
                setLedSideSecond = (LedSideTypes)Convert.ToInt32(ConfigurationManager.AppSettings["LedSideSecond"]);
                setLedSideThird = (LedSideTypes)Convert.ToInt32(ConfigurationManager.AppSettings["LedSideThird"]);
                setLedSideFourth = (LedSideTypes)Convert.ToInt32(ConfigurationManager.AppSettings["LedSideFourth"]);
                setLedCountFirst = Convert.ToInt32(ConfigurationManager.AppSettings["LedCountFirst"]);
                setLedCountSecond = Convert.ToInt32(ConfigurationManager.AppSettings["LedCountSecond"]);
                setLedCountThird = Convert.ToInt32(ConfigurationManager.AppSettings["LedCountThird"]);
                setLedCountFourth = Convert.ToInt32(ConfigurationManager.AppSettings["LedCountFourth"]);
                setLedCountTotal = setLedCountFirst + setLedCountSecond + setLedCountThird + setLedCountFourth;

                //Debug settings
                setDebugMode = Convert.ToBoolean(ConfigurationManager.AppSettings["DebugMode"]);
                setDebugBlackBar = Convert.ToBoolean(ConfigurationManager.AppSettings["DebugBlackBar"]);
                setDebugColor = Convert.ToBoolean(ConfigurationManager.AppSettings["DebugColor"]);
                setDebugSave = Convert.ToBoolean(ConfigurationManager.AppSettings["DebugSave"]);

                //Update the rotation based on ratio
                string ScreenRatio = AVFunctions.ScreenAspectRatio(vScreenWidth, vScreenHeight, false);
                if (SettingsFunction.Check("LedRotate" + ScreenRatio))
                {
                    setLedRotate = Convert.ToInt32(ConfigurationManager.AppSettings["LedRotate" + ScreenRatio]);
                }
                else
                {
                    setLedRotate = 0;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed updating the settings: " + ex.Message);
            }
        }

        //Turn the leds on or off
        public static async Task LedSwitch(LedSwitches ledSwitch)
        {
            try
            {
                if (!vSwitching)
                {
                    vSwitching = true;

                    //Update settings
                    UpdateSettings();

                    //Restart the leds
                    if (ledSwitch == LedSwitches.Restart)
                    {
                        await LedsRestart();
                        vSwitching = false;
                        return;
                    }

                    //Disable the leds
                    if (ledSwitch == LedSwitches.Disable || vTask_LedUpdate.TaskRunning)
                    {
                        await LedsDisable(false);
                        vSwitching = false;
                        return;
                    }

                    //Enable the leds
                    LedsEnable();
                    vSwitching = false;
                    return;
                }
            }
            catch
            {
                Debug.WriteLine("Failed switching the leds on or off.");
                vSwitching = false;
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
                AVActions.TaskStartLoop(LoopUpdateLeds, vTask_LedUpdate);
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
                await AVActions.TaskStopLoop(vTask_LedUpdate);

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

        //Restart the led updates
        private static async Task LedsRestart()
        {
            try
            {
                Debug.WriteLine("Restarting the led updates.");
                await LedsDisable(true);
                LedsEnable();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to restart the led updates: " + ex.Message);
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
                    MsgBoxAnswers.Add("Retry to capture");
                    MsgBoxAnswers.Add("Close application");

                    string MsgBoxResult = await new AVMessageBox().Popup(null, "Failed to start capturing your monitor screen", "Please make sure the correct monitor screen is selected, Microsoft Visual C++ 2019 Redistributable is installed on your PC, that you have a 64bit Windows installation and that you have a DirectX 12 or higher capable graphics adapter installed.", MsgBoxAnswers);
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
                    else if (MsgBoxResult == "Retry to capture")
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
                //Update the led settings
                UpdateSettings();

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
                SettingsFunction.Save("FirstLaunch2", "False");

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