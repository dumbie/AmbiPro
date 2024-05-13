using System;
using System.Diagnostics;
using System.Threading.Tasks;
using static AmbiPro.AppEnums;
using static AmbiPro.AppVariables;
using static AmbiPro.SerialMonitor;

namespace AmbiPro
{
    public class AppExit
    {
        public static async Task Exit()
        {
            try
            {
                Debug.WriteLine("Exiting application.");

                //Stop updating the leds
                await LedSwitch(LedSwitches.Disable);

                //Stop all the background tasks
                await AppTasks.TasksBackgroundStop();

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