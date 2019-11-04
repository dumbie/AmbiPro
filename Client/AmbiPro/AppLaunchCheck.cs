using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;

namespace AmbiPro
{
    public partial class AppLaunchCheck
    {
        public static void Application_LaunchCheck(string ApplicationName, string ProcessName, bool PriorityRealTime, bool skipFileCheck)
        {
            try
            {
                Debug.WriteLine("Checking application status.");

                //Check - If application is already running
                if (Process.GetProcessesByName(ProcessName).Length > 1)
                {
                    Debug.WriteLine("Application is already running.");
                    Environment.Exit(0);
                    return;
                }

                //Set the working directory to executable directory
                Directory.SetCurrentDirectory(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location));

                //Set the application priority to realtime
                if (PriorityRealTime)
                {
                    Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.RealTime;
                }

                //Check for missing application files
                if (!skipFileCheck)
                {
                    string[] ApplicationFiles = { "AmbiPro.exe", "AmbiPro.exe.Config", "Updater.exe", "Updater.exe.Config" };
                    foreach (string CheckFile in ApplicationFiles)
                    {
                        if (!File.Exists(CheckFile))
                        {
                            MessageBox.Show("File: " + CheckFile + " could not be found, please check your installation.", ApplicationName);
                            Environment.Exit(0);
                            return;
                        }
                    }
                }

                //Check - If the updater has been updated
                if (File.Exists("UpdaterNew.exe"))
                {
                    try
                    {
                        Debug.WriteLine("Renaming: UpdaterNew.exe to Updater.exe");
                        if (File.Exists("Updater.exe")) { File.Delete("Updater.exe"); }
                        File.Move("UpdaterNew.exe", "Updater.exe");
                    }
                    catch { }
                }

                //Check - If the updater failed to cleanup
                if (File.Exists("AppUpdate.zip"))
                {
                    try
                    {
                        File.Delete("AppUpdate.zip");
                    }
                    catch { }
                }
            }
            catch { }
        }
    }
}