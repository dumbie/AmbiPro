using AmbiPro.Settings;
using ArnoldVinkCode;
using System;
using System.Configuration;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using static AmbiPro.AppEnums;
using static AmbiPro.AppVariables;
using static AmbiPro.SerialMonitor;
using static ArnoldVinkCode.AVFirewall;

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

                //Register application timers
                AppTimers.ApplicationTimersRegister();

                //Settings check if first launch
                if (Convert.ToBoolean(ConfigurationManager.AppSettings["FirstLaunch2"]))
                {
                    Debug.WriteLine("First launch, showing the settings screen.");
                    vFormSettings.Show();
                }
                else
                {
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
                int SocketServerPort = Convert.ToInt32(ConfigurationManager.AppSettings["ServerPort"]);

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