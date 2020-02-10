using AmbiPro.Calibrate;
using AmbiPro.Help;
using AmbiPro.Settings;
using System.Reflection;
using System.Windows;
using static ArnoldVinkCode.AVFirewall;

namespace AmbiPro
{
    public partial class App : Application
    {
        //Application Windows
        public static FormSettings vFormSettings = new FormSettings();
        public static FormCalibrate vFormCalibrate = new FormCalibrate();
        public static FormHelp vFormHelp = new FormHelp();

        protected override async void OnStartup(StartupEventArgs e)
        {
            try
            {
                //Allow application in firewall
                string appFilePath = Assembly.GetEntryAssembly().Location;
                Firewall_ExecutableAllow("AmbiPro", appFilePath, true);

                AppStartup AppStartup = new AppStartup();
                await AppStartup.Application_Startup();
            }
            catch { }
        }
    }
}