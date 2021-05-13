using ArnoldVinkCode;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using static AmbiPro.AppVariables;
using static ArnoldVinkCode.ApiGitHub;
using static ArnoldVinkCode.ProcessWin32Functions;

namespace AmbiPro
{
    class AppUpdate
    {
        //Check for available application update
        public static async Task CheckForAppUpdate(bool Silent)
        {
            try
            {
                if (!vCheckingForUpdate)
                {
                    vCheckingForUpdate = true;

                    string onlineVersion = await ApiGitHub_GetLatestVersion("dumbie", "AmbiPro");
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
                            await AppStartup.Application_Exit();
                        }
                    }
                    else
                    {
                        if (!Silent)
                        {
                            List<string> MsgBoxAnswers = new List<string>();
                            MsgBoxAnswers.Add("Ok");

                            await new AVMessageBox().Popup(null, "No new application update has been found.", "", MsgBoxAnswers);
                        }
                    }

                    vCheckingForUpdate = false;
                }
            }
            catch
            {
                vCheckingForUpdate = false;
                if (!Silent)
                {
                    List<string> MsgBoxAnswers = new List<string>();
                    MsgBoxAnswers.Add("Ok");

                    await new AVMessageBox().Popup(null, "Failed to check for the latest application version", "Please check your internet connection and try again.", MsgBoxAnswers);
                }
            }
        }
    }
}