using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace Updater
{
    partial class WindowMain
    {
        public void Application_LaunchCheck()
        {
            try
            {
                Debug.WriteLine("Checking application status...");

                //Set the working directory to the executable directory
                Directory.SetCurrentDirectory(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));

                //Check - If application is already running
                if (Process.GetProcessesByName("Updater").Length > 1) { Environment.Exit(0); return; }
            }
            catch { }
        }
    }
}