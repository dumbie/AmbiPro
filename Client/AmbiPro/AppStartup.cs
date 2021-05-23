using AmbiPro.Settings;
using ArnoldVinkCode;
using System;
using System.Configuration;
using System.Diagnostics;
using System.Threading.Tasks;
using static AmbiPro.AppEnums;
using static AmbiPro.AppLaunchCheck;
using static AmbiPro.AppVariables;
using static AmbiPro.SerialMonitor;

namespace AmbiPro
{
    class AppStartup
    {
        //Application Startup
        public async Task Application_Startup()
        {
            try
            {
                Debug.WriteLine("Welcome to AmbiPro.");

                //Check the application status
                await Application_LaunchCheck("AmbiPro", "AmbiPro", ProcessPriorityClass.High, false);

                //Check application settings
                SettingsFunction.SettingsCheck();

                //Create application tray menu
                AppTray.CreateTrayMenu();

                //Register application timers
                AppTimers.ApplicationTimersRegister();

                //Settings screen if first run
                if (Convert.ToBoolean(ConfigurationManager.AppSettings["FirstLaunch2"]))
                {
                    Debug.WriteLine("First launch, showing the settings screen.");
                    App.vFormSettings.Show();
                    return;
                }

                //Start updating the leds
                bool turnLedsOn = false;
                if (Convert.ToBoolean(ConfigurationManager.AppSettings["LedAutoOnOffBefore"]) || Convert.ToBoolean(ConfigurationManager.AppSettings["LedAutoOnOffAfter"]))
                {
                    DateTime LedTimeBefore = DateTime.Parse(ConfigurationManager.AppSettings["LedAutoTimeBefore"], vAppCultureInfo);
                    if (DateTime.Now.TimeOfDay < LedTimeBefore.TimeOfDay)
                    {
                        turnLedsOn = true;
                    }
                    DateTime LedTimeAfter = DateTime.Parse(ConfigurationManager.AppSettings["LedAutoTimeAfter"], vAppCultureInfo);
                    if (DateTime.Now.TimeOfDay >= LedTimeAfter.TimeOfDay)
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

                //Enable the socket server
                await EnableSocketServer();

                //Check for available application update
                await AppUpdate.CheckForAppUpdate(true);
            }
            catch { }
        }

        //Enable the socket server
        private async Task EnableSocketServer()
        {
            try
            {
                int SocketServerPort = Convert.ToInt32(ConfigurationManager.AppSettings["ServerPort"]);

                vArnoldVinkSockets = new ArnoldVinkSockets("127.0.0.1", SocketServerPort, true, false);
                vArnoldVinkSockets.vSocketTimeout = 2000;
                vArnoldVinkSockets.EventBytesReceived += SocketHandlers.ReceivedSocketHandler;
                await vArnoldVinkSockets.SocketServerEnable();
            }
            catch { }
        }

        //Application Exit
        public static async Task Application_Exit()
        {
            try
            {
                Debug.WriteLine("Exiting application.");

                //Stop updating the leds
                await LedSwitch(LedSwitches.Disable);

                //Disable the socket server
                if (vArnoldVinkSockets != null)
                {
                    await vArnoldVinkSockets.SocketServerDisable();
                }

                //Disable application timers
                AppTimers.ApplicationTimersDisable();

                //Hide the tray icon
                AppTray.NotifyIcon.Visible = false;

                //Exit the application
                Environment.Exit(0);
            }
            catch { }
        }
    }
}