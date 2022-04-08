using AmbiPro.Settings;
using System.IO;
using System.Reflection;
using System.Windows;
using static ArnoldVinkCode.AVFirewall;

namespace AmbiPro
{
    public partial class App : Application
    {
        //Application Windows
        public static FormSettings vFormSettings = new FormSettings();

        protected override async void OnStartup(StartupEventArgs e)
        {
            try
            {
                //Set the working directory to executable directory
                Directory.SetCurrentDirectory(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location));

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