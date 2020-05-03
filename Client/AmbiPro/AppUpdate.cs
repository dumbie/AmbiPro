using AmbiPro.Settings;
using ArnoldVinkCode;
using ArnoldVinkMessageBox;
using System;
using System.Reflection;
using System.Threading.Tasks;
using static AmbiPro.AppVariables;
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

                    string ResCurrentVersion = await AVDownloader.DownloadStringAsync(5000, "AmbiPro", null, new Uri("https://download.arnoldvink.com/AmbiPro.zip-version.txt" + "?nc=" + Environment.TickCount));
                    if (!string.IsNullOrWhiteSpace(ResCurrentVersion) && ResCurrentVersion != Assembly.GetEntryAssembly().FullName.Split('=')[1].Split(',')[0])
                    {
                        int MsgBoxResult = await AVMessageBox.Popup("A newer version has been found: v" + ResCurrentVersion, "Would you like to update the application to the newest version available?", "Update", "Cancel", "", "");
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

                    //Set the last application update check date
                    SettingsFunction.Save("AppUpdateCheck2", DateTime.Now.ToString(vAppCultureInfo));
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