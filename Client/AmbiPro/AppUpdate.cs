using ArnoldVinkCode;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using static ArnoldVinkCode.ApiGitHub;
using static ArnoldVinkCode.AVFiles;
using static ArnoldVinkCode.ProcessFunctions;
using static ArnoldVinkCode.ProcessWin32Functions;

namespace AmbiPro
{
    class AppUpdate
    {
        public static async Task UpdateCheck(bool silentUpdate)
        {
            try
            {
                Debug.WriteLine("Checking application update.");

                //Close running application updater
                if (CloseProcessesByNameOrTitle("Updater.exe", false, true))
                {
                    await Task.Delay(1000);
                }

                //Check if the updater has been updated
                File_Move("Resources/UpdaterReplace.exe", "Updater.exe", true);

                //Check for available application update
                await CheckAppUpdate(silentUpdate);
            }
            catch { }
        }

        //Check for available application update
        public static async Task CheckAppUpdate(bool silentUpdate)
        {
            try
            {
                string onlineVersion = (await ApiGitHub_GetLatestVersion("dumbie", "AmbiPro")).ToLower();
                string currentVersion = "v" + Assembly.GetEntryAssembly().FullName.Split('=')[1].Split(',')[0];
                if (!string.IsNullOrWhiteSpace(onlineVersion) && onlineVersion != currentVersion)
                {
                    List<string> MsgBoxAnswers = new List<string>();
                    MsgBoxAnswers.Add("Update");
                    MsgBoxAnswers.Add("Cancel");

                    string MsgBoxResult = await new AVMessageBox().Popup(null, "A newer version has been found: " + onlineVersion, "Would you like to update the application to the newest version available?", MsgBoxAnswers);
                    if (MsgBoxResult == "Update")
                    {
                        await ProcessLauncherWin32Async("Updater.exe", "", "", false, false);
                        await AppStartup.Exit();
                    }
                }
                else
                {
                    if (!silentUpdate)
                    {
                        List<string> MsgBoxAnswers = new List<string>();
                        MsgBoxAnswers.Add("Ok");

                        await new AVMessageBox().Popup(null, "No new application update has been found.", "", MsgBoxAnswers);
                    }
                }
            }
            catch
            {
                if (!silentUpdate)
                {
                    List<string> MsgBoxAnswers = new List<string>();
                    MsgBoxAnswers.Add("Ok");

                    await new AVMessageBox().Popup(null, "Failed to check for the latest application version", "Please check your internet connection and try again.", MsgBoxAnswers);
                }
            }
        }
    }
}