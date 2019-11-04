using AmbiPro.Settings;
using ArnoldVinkMessageBox;
using ArnoldVinkCode;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Threading.Tasks;

namespace AmbiPro
{
    class AppUpdate
    {
        //Update check variables
        public static bool vCheckingForUpdate = false;
        public static CultureInfo vAppCultureInfo = CultureInfo.InvariantCulture;

        //Check for available application update
        public static async Task CheckForAppUpdate(bool Silent)
        {
            try
            {
                if (!vCheckingForUpdate)
                {
                    vCheckingForUpdate = true;

                    string ResCurrentVersion = await AVDownloader.DownloadStringAsync(5000, "AmbiPro", null, new Uri("http://download.arnoldvink.com/AmbiPro.zip-version.txt" + "?nc=" + Environment.TickCount));
                    if (ResCurrentVersion != String.Empty && ResCurrentVersion != Assembly.GetExecutingAssembly().FullName.Split('=')[1].Split(',')[0])
                    {
                        Int32 MsgBoxResult = await AVMessageBox.Popup("A newer version has been found: v" + ResCurrentVersion, "Would you like to update the application to the newest version available?", "Update", "Cancel", "", "");
                        if (MsgBoxResult == 1)
                        {
                            //Launch the updater
                            Process LaunchProcess = new Process();
                            LaunchProcess.StartInfo.FileName = "Updater.exe";
                            LaunchProcess.StartInfo.UseShellExecute = false;
                            LaunchProcess.Start();

                            //Close AmbiPro process
                            await AppStartup.ApplicationExit();
                        }
                    }
                    else { if (!Silent) { await AVMessageBox.Popup("No new application update has been found.", "", "Ok", "", "", ""); } }

                    //Set the last application update check date
                    CultureInfo AppCultureInfo = CultureInfo.InvariantCulture;
                    SettingsFunction.Save("AppUpdateCheck2", DateTime.Now.ToString(AppCultureInfo));
                    vCheckingForUpdate = false;
                }
            }
            catch
            {
                vCheckingForUpdate = false;
                if (!Silent) { await AVMessageBox.Popup("Failed to check for the latest application version", "Please check your internet connection and try again.", "Ok", "", "", ""); }
            }
        }
    }
}