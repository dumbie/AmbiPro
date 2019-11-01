using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;

namespace AmbiPro
{
    class AppCheck
    {
        public static void AppLaunchCheck()
        {
            try
            {
                Debug.WriteLine("Checking application status...");

                //Set the working directory to the executable directory
                Directory.SetCurrentDirectory(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));

                //Check - If application is already running
                if (Process.GetProcessesByName("AmbiPro").Length > 1) { Environment.Exit(0); return; }

                //Check - Missing application config
                if (!File.Exists("AmbiPro.exe.Config"))
                {
                    MessageBox.Show("File: AmbiPro.exe.Config could not be found, please check your installation.", "AmbiPro");
                    Environment.Exit(0);
                    return;
                }

                //Check - Missing application Updater
                if (!File.Exists("Updater.exe"))
                {
                    MessageBox.Show("File: Updater.exe could not be found, please check your installation.", "AmbiPro");
                    Environment.Exit(0);
                    return;
                }

                //Check - Missing application dlls
                if (!File.Exists("Resources\\ScreenCapture.dll"))
                {
                    MessageBox.Show("File: Resources\\ScreenCapture.dll could not be found, please check your installation.", "AmbiPro");
                    Environment.Exit(0);
                    return;
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
                if (File.Exists("AmbiPro-Update.zip")) { try { File.Delete("AmbiPro-Update.zip"); } catch { } }

                //Set processor affinity to the last core
                try
                {
                    Int32 LastProcessorCore = Convert.ToInt32(Environment.GetEnvironmentVariable("NUMBER_OF_PROCESSORS"));
                    Process.GetCurrentProcess().ProcessorAffinity = new IntPtr((1 << LastProcessorCore - 1));
                }
                catch { }
            }
            catch { }
        }
    }
}