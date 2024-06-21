using ArnoldVinkCode;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using static AmbiPro.AppEnums;
using static AmbiPro.AppVariables;
using static AmbiPro.SerialMonitor;
using static ArnoldVinkCode.AVInteropDll;
using static ArnoldVinkCode.AVSettings;

namespace AmbiPro
{
    public class AppStartup
    {
        public async static Task Startup()
        {
            try
            {
                Debug.WriteLine("Welcome to application.");

                //Application update checks
                await AppUpdate.UpdateCheck(true);

                //Application initialize settings
                vFormSettings.SettingsCheck();

                //Application initialize shortcuts
                vFormSettings.ShortcutsCheck();

                //Create application tray menu
                AppTray.CreateTrayMenu();

                //Start all the background tasks
                AppTasks.TasksBackgroundStart();

                //Settings check if first launch
                if (SettingLoad(vConfiguration, "FirstLaunch2", typeof(bool)))
                {
                    Debug.WriteLine("First launch, showing the settings screen.");
                    vFormSettings.Show();
                }
                else
                {
                    //Start updating the leds
                    bool turnLedsOn = false;
                    if (SettingLoad(vConfiguration, "LedAutoOnOffBefore", typeof(bool)) || SettingLoad(vConfiguration, "LedAutoOnOffAfter", typeof(bool)))
                    {
                        DateTime ledTimeBefore = SettingLoad(vConfiguration, "LedAutoTimeBefore", typeof(DateTime));
                        if (DateTime.Now.TimeOfDay < ledTimeBefore.TimeOfDay)
                        {
                            turnLedsOn = true;
                        }
                        DateTime ledTimeAfter = SettingLoad(vConfiguration, "LedAutoTimeAfter", typeof(DateTime));
                        if (DateTime.Now.TimeOfDay >= ledTimeAfter.TimeOfDay)
                        {
                            turnLedsOn = true;
                        }
                    }
                    else
                    {
                        turnLedsOn = true;
                    }

                    if (turnLedsOn)
                    {
                        await LedSwitch(LedSwitches.Automatic);
                    }
                }

                //Start keyboard hotkeys
                AVInputOutputHotkey.Start();
                AVInputOutputHotkey.EventHotkeyPressed += AppHotkeys.EventHotkeyPressed;

                //Enable socket server
                await EnableSocketServer();
            }
            catch { }
        }

        private static async Task EnableSocketServer()
        {
            try
            {
                int SocketServerPort = SettingLoad(vConfiguration, "ServerPort", typeof(int));

                vArnoldVinkSockets = new ArnoldVinkSockets("127.0.0.1", SocketServerPort, true, false);
                vArnoldVinkSockets.vSocketTimeout = 2000;
                vArnoldVinkSockets.EventBytesReceived += SocketHandlers.ReceivedSocketHandler;
                await vArnoldVinkSockets.SocketServerEnable();
            }
            catch { }
        }
    }
}