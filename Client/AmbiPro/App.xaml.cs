using AmbiPro.Calibrate;
using AmbiPro.Help;
using AmbiPro.Settings;
using System.Windows;

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
            AppStartup AppStartup = new AppStartup();
            await AppStartup.Application_Startup();
        }
    }
}