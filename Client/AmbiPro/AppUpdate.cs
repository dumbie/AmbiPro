using ArnoldVinkMessageBox;
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
                        int MsgBoxResult = await AVMessageBox.Popup("A newer version has been found: " + onlineVersion, "Would you like to update the application to the newest version available?", "Update", "Cancel", "", "");
                        if (MsgBoxResult == 1)
                        {
                            await ProcessLauncherWin32Async("Updater.exe", "", "", false, false);
                            await AppStartup.Application_Exit();
                        }
                    }
                    else
                    {
                        if (!Silent)
                        {
                            await AVMessageBox.Popup("No new application update has been found.", "", "Ok", "", "", "");
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
                    await AVMessageBox.Popup("Failed to check for the latest application version", "Please check your internet connection and try again.", "Ok", "", "", "");
                }
            }
        }
    }
}