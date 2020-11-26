using System.Diagnostics;
using System.Windows;
using static AmbiPro.AppLaunchCheck;

namespace Updater
{
    public partial class App : Application
    {
        //Application Windows
        public static WindowMain vWindowMain = new WindowMain();

        //Application Startup
        protected override void OnStartup(StartupEventArgs e)
        {
            try
            {
                //Check the application status
                Application_LaunchCheck("Application Updater", "Updater", ProcessPriorityClass.Normal, true);

                //Open the application window
                vWindowMain.Show();
            }
            catch { }
        }
    }
}