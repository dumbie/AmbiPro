using AmbiPro.Settings;
using System;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.Threading.Tasks;

namespace AmbiPro
{
    class AppStartup
    {
        //Application Startup
        public async Task ApplicationStartup()
        {
            try
            {
                Debug.WriteLine("Welcome to AmbiPro.");

                //Check application files
                AppCheck.AppLaunchCheck();

                //Check application settings
                FormSettings.SettingsCheck();

                //Create application tray menu
                AppTray.CreateTrayMenu();

                //Register application timers
                AppTimers.ApplicationTimersRegister();

                //Settings screen if first run
                if (Convert.ToBoolean(ConfigurationManager.AppSettings["FirstLaunch"]))
                {
                    Debug.WriteLine("First launch, showing the settings screen.");
                    FormSettings FormSettings = new FormSettings();
                    FormSettings.Show();
                    return;
                }

                //Start updating the leds
                CultureInfo AppCultureInfo = CultureInfo.InvariantCulture;
                if (Convert.ToBoolean(ConfigurationManager.AppSettings["LedAutoOnOff"]))
                {
                    DateTime LedTime = DateTime.Parse(ConfigurationManager.AppSettings["LedAutoTime"], AppCultureInfo);
                    if (DateTime.Now.TimeOfDay >= LedTime.TimeOfDay)
                    {
                        await SerialMonitor.LedSwitch(false, false);
                    }
                    else
                    {
                        await SerialMonitor.LedSwitch(true, false);
                    }
                }
                else
                {
                    await SerialMonitor.LedSwitch(false, false);
                }

                //Start remote server
                await Socket.SocketServerSwitch(false, false);

                //Check for available application update
                if (DateTime.Now.Subtract(DateTime.Parse(ConfigurationManager.AppSettings["AppUpdateCheck2"], AppCultureInfo)).Days >= 5)
                {
                    await AppUpdate.CheckForAppUpdate(true);
                }
            }
            catch { }
        }

        //Application Startup
        public static async Task ApplicationExit()
        {
            try
            {
                Debug.WriteLine("Exiting AmbiPro...");

                //Stop updating the leds
                await SerialMonitor.LedSwitch(true, false); //Improve never completes when timed out?

                //Disable application timers
                AppTimers.ApplicationTimersDisable();

                //Reset the screen capturer
                AppImport.CaptureReset();

                //Hide the tray icon
                AppTray.NotifyIcon.Visible = false;

                //Exit the application
                Environment.Exit(0);
            }
            catch
            {
                Debug.WriteLine("Force exiting AmbiPro...");

                //Hide the tray icon
                AppTray.NotifyIcon.Visible = false;

                //Exit the application
                Environment.Exit(0);
            }
        }
    }
}