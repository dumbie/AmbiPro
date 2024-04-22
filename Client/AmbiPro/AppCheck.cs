using ArnoldVinkCode;
using System;
using System.Diagnostics;

namespace AmbiPro
{
    public partial class AppCheck
    {
        public static void StartupCheck(string appName, ProcessPriorityClass priorityLevel)
        {
            try
            {
                Debug.WriteLine("Checking application status.");

                //Get current process information
                Process currentProcess = Process.GetCurrentProcess();
                string processName = currentProcess.ProcessName;
                Process[] activeProcesses = Process.GetProcessesByName(processName);

                //Check if application is already running
                if (activeProcesses.Length > 1)
                {
                    Debug.WriteLine("Application " + appName + " is already running, closing the process");
                    Environment.Exit(0);
                    return;
                }

                //Set the working directory to executable directory
                AVFunctions.ApplicationUpdateWorkingPath();

                //Set the application priority level
                try
                {
                    currentProcess.PriorityClass = priorityLevel;
                }
                catch { }
            }
            catch { }
        }
    }
}