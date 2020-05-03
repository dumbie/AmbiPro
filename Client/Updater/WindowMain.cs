using ArnoldVinkCode;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using static ArnoldVinkCode.ProcessWin32Functions;

namespace Updater
{
    public partial class WindowMain : Window
    {
        //Window Initialize
        public WindowMain() { InitializeComponent(); }

        //Update the textblock
        public void TextBlockUpdate(string Text)
        {
            try
            {
                AVActions.ActionDispatcherInvoke(delegate
                {
                    textblock_Status.Text = Text;
                });
            }
            catch { }
        }

        //Update the progressbar
        public void ProgressBarUpdate(double Progress, bool Indeterminate)
        {
            try
            {
                AVActions.ActionDispatcherInvoke(delegate
                {
                    progressbar_Status.IsIndeterminate = Indeterminate;
                    progressbar_Status.Value = Progress;
                });
            }
            catch { }
        }

        //Window Startup
        public async Task Startup()
        {
            try
            {
                //Check if previous update files are in the way
                if (File.Exists("UpdaterNew.exe")) { try { File.Delete("UpdaterNew.exe"); } catch { } }
                if (File.Exists("AppUpdate.zip")) { try { File.Delete("AppUpdate.zip"); } catch { } }

                //Check if application is running and close it
                bool AppRunning = false;
                foreach (Process CloseProcess in Process.GetProcessesByName("AmbiPro"))
                {
                    AppRunning = true;
                    CloseProcess.Kill();
                }

                //Wait for applications to have closed
                await Task.Delay(1000);

                //Download application update from the website
                try
                {
                    WebClient WebClient = new WebClient();
                    WebClient.Headers[HttpRequestHeader.UserAgent] = "Application Updater";
                    WebClient.DownloadProgressChanged += (object Object, DownloadProgressChangedEventArgs Args) =>
                    {
                        ProgressBarUpdate(Args.ProgressPercentage, false);
                        TextBlockUpdate("Downloading update file: " + Args.ProgressPercentage + "%");
                    };
                    await WebClient.DownloadFileTaskAsync(new Uri("https://download.arnoldvink.com/?dl=AmbiPro.zip"), "AppUpdate.zip");
                    Debug.WriteLine("Update file has been downloaded");
                }
                catch
                {
                    await Application_Exit("Failed to download update, closing in a bit.");
                    return;
                }

                try
                {
                    //Extract the downloaded update archive
                    TextBlockUpdate("Updating the application to the latest version.");
                    using (ZipArchive ZipArchive = ZipFile.OpenRead("AppUpdate.zip"))
                    {
                        foreach (ZipArchiveEntry ZipFile in ZipArchive.Entries)
                        {
                            string ExtractPath = AVFunctions.StringReplaceFirst(ZipFile.FullName, "AmbiPro/", "", false);
                            if (!string.IsNullOrWhiteSpace(ExtractPath))
                            {
                                if (string.IsNullOrWhiteSpace(ZipFile.Name)) { Directory.CreateDirectory(ExtractPath); }
                                else
                                {
                                    if (File.Exists(ExtractPath) && ExtractPath.ToLower().EndsWith("AmbiPro.exe.Config".ToLower())) { Debug.WriteLine("Skipping: AmbiPro.exe.Config"); continue; }

                                    if (File.Exists(ExtractPath) && ExtractPath.ToLower().EndsWith("Updater.exe".ToLower()))
                                    {
                                        Debug.WriteLine("Renaming: Updater.exe");
                                        ExtractPath = ExtractPath.Replace("Updater.exe", "UpdaterNew.exe");
                                    }

                                    ZipFile.ExtractToFile(ExtractPath, true);
                                }
                            }
                        }
                    }
                }
                catch
                {
                    await Application_Exit("Failed to extract update, closing in a bit.");
                    return;
                }

                //Delete the update installation zip file
                TextBlockUpdate("Cleaning up the update installation files.");
                if (File.Exists("AppUpdate.zip"))
                {
                    Debug.WriteLine("Removing: AppUpdate.zip");
                    File.Delete("AppUpdate.zip");
                }

                //Start application after the update has completed.
                if (AppRunning)
                {
                    TextBlockUpdate("Running the updated version of the application.");
                    await ProcessLauncherWin32Async("AmbiPro.exe", "", "", false, false);
                }

                //Close the application
                await Application_Exit("Application has been updated, closing in a bit.");
            }
            catch { }
        }

        //Application Close Handler
        protected override void OnClosing(CancelEventArgs e)
        {
            try
            {
                e.Cancel = true;
            }
            catch { }
        }

        //Close the application
        async Task Application_Exit(string ExitMessage)
        {
            try
            {
                Debug.WriteLine("Exiting Updater.");

                //Delete the update installation zip file
                if (File.Exists("AppUpdate.zip"))
                {
                    Debug.WriteLine("Removing: AppUpdate.zip");
                    File.Delete("AppUpdate.zip");
                }

                //Set the exit reason text message
                TextBlockUpdate(ExitMessage);
                ProgressBarUpdate(100, false);

                //Close the application after x seconds
                await Task.Delay(2000);
                Environment.Exit(0);
            }
            catch { }
        }
    }
}