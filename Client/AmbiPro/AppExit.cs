using ArnoldVinkCode;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using static AmbiPro.AppEnums;
using static AmbiPro.AppVariables;
using static AmbiPro.SerialMonitor;

namespace AmbiPro
{
    public class AppExit
    {
        public static async Task Exit_Prompt()
        {
            try
            {
                List<string> messageAnswers = new List<string>();
                messageAnswers.Add("Exit application");
                messageAnswers.Add("Cancel");

                string messageResult = await new AVMessageBox().Popup(vFormSettings, "Do you really want to exit AmbiPro?", "This will stop updating your led strip and turn it off.", messageAnswers);
                if (messageResult == "Exit application")
                {
                    await Exit();
                }
            }
            catch { }
        }

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