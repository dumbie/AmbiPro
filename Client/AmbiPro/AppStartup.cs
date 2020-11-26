using ArnoldVinkCode;
using System;
using System.Configuration;
using System.Diagnostics;
using System.Threading.Tasks;
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
                Application_LaunchCheck("AmbiPro", "AmbiPro", ProcessPriorityClass.High, false);

                //Check application settings
                App.vFormSettings.SettingsCheck();

                //Create application tray menu
                AppTray.CreateTrayMenu();

                //Register application timers
                AppTimers.ApplicationTimersRegister();

                //Enable the socket server
                EnableSocketServer();

                //Settings screen if first run
                if (Convert.ToBoolean(ConfigurationManager.AppSettings["FirstLaunch"]))
                {
                    Debug.WriteLine("First launch, showing the settings screen.");

                    App.vFormSettings.Show();
                    return;
                }

                //Start updating the leds
                if (Convert.ToBoolean(ConfigurationManager.AppSettings["LedAutoOnOff"]))
                {
                    DateTime LedTime = DateTime.Parse(ConfigurationManager.AppSettings["LedAutoTime"], vAppCultureInfo);
                    if (DateTime.Now.TimeOfDay >= LedTime.TimeOfDay)
                    {
                        await LedSwitch(LedSwitches.Automatic);
                    }
                    else
                    {
                        await LedSwitch(LedSwitches.Disable);
                    }
                }
                else
                {
                    await LedSwitch(LedSwitches.Automatic);
                }

                //Check for available application update
                await AppUpdate.CheckForAppUpdate(true);
            }
            catch { }
        }

        //Enable the socket server
        private void EnableSocketServer()
        {
            try
            {
                int SocketServerPort = Convert.ToInt32(ConfigurationManager.AppSettings["ServerPort"]);

                vArnoldVinkSockets = new ArnoldVinkSockets("127.0.0.1", SocketServerPort);
                vArnoldVinkSockets.EventBytesReceived += SocketHandlers.ReceivedSocketHandler;
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

                //Reset the screen capturer
                AppImport.CaptureReset();

                //Hide the tray icon
                AppTray.NotifyIcon.Visible = false;

                //Exit the application
                Environment.Exit(0);
            }
            catch { }
        }
    }
}