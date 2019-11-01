using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Updater
{
    partial class Processes
    {
        //Launch a win32 app manually
        public static bool LaunchProcessManuallyWin32(string PathExe, string PathLaunch, string Arguments, bool RunAsAdmin)
        {
            try
            {
                //Check if the application exe file exists
                if (!File.Exists(PathExe)) { return false; }

                //Check if process is running
                if (CheckRunningProcessByName(Path.GetFileNameWithoutExtension(PathExe), false)) { return false; }

                //Show launching message
                Debug.WriteLine("Launching: " + Path.GetFileNameWithoutExtension(PathExe));

                //Launch Win32 application
                Process LaunchProcess = new Process();
                LaunchProcess.StartInfo.FileName = PathExe;
                if (RunAsAdmin)
                {
                    LaunchProcess.StartInfo.UseShellExecute = true;
                    LaunchProcess.StartInfo.Verb = "runas";
                }
                else
                {
                    LaunchProcess.StartInfo.UseShellExecute = false;
                    LaunchProcess.StartInfo.CreateNoWindow = true;
                }
                if (!string.IsNullOrWhiteSpace(PathLaunch)) { LaunchProcess.StartInfo.WorkingDirectory = PathLaunch; } else { LaunchProcess.StartInfo.WorkingDirectory = Path.GetDirectoryName(PathExe); }
                if (!string.IsNullOrWhiteSpace(Arguments)) { LaunchProcess.StartInfo.Arguments = Arguments; }
                LaunchProcess.Start();

                return true;
            }
            catch { return false; }
        }

        //Check if a specific process is running by name
        public static bool CheckRunningProcessByName(string ProcessName, bool WindowTitle)
        {
            try
            {
                if (WindowTitle) { return Process.GetProcesses().Any(x => x.MainWindowTitle.Contains(ProcessName)); }
                else { return Process.GetProcessesByName(ProcessName).Any(); }
            }
            catch { return false; }
        }
    }
}