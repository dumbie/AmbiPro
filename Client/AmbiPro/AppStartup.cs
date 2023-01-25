using AmbiPro.Settings;
using ArnoldVinkCode;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using static AmbiPro.AppEnums;
using static AmbiPro.AppTasks;
using static AmbiPro.AppVariables;
using static AmbiPro.SerialMonitor;
using static ArnoldVinkCode.AVFirewall;
using static ArnoldVinkCode.AVSettings;

namespace AmbiPro
{
    class AppStartup
    {
        public async static Task Startup()
        {
            try
            {
                Debug.WriteLine("Welcome to AmbiPro.");

                //Application startup checks
                AppCheck.StartupCheck("AmbiPro", ProcessPriorityClass.High);

                //Application update checks
                await AppUpdate.UpdateCheck(true);

                //Allow application in firewall
                string appFilePath = Assembly.GetEntryAssembly().Location;
                Firewall_ExecutableAllow("AmbiPro", appFilePath, true);

                //Check application settings
                SettingsFunction.SettingsCheck();

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

                //Enable the socket server
                await EnableSocketServer();
            }
            catch { }
        }

        //Enable the socket server
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

        //Application Exit
        public static async Task Exit()
        {
            try
            {
                Debug.WriteLine("Exiting application.");

                //Stop updating the leds
                await LedSwitch(LedSwitches.Disable);

                //Stop all the background tasks
                await TasksBackgroundStop();

                //Disable the socket server
                if (vArnoldVinkSockets != null)
                {
                    await vArnoldVinkSockets.SocketServerDisable();
                }

                //Hide the tray icon
                AppTray.NotifyIcon.Visible = false;

                //Exit the application
                Environment.Exit(0);
            }
            catch { }
        }
    }
}