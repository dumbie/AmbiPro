using System.Windows;

namespace AmbiPro
{
    public partial class App : Application
    {
        protected override async void OnStartup(StartupEventArgs e)
        {
            AppStartup AppStartup = new AppStartup();
            await AppStartup.ApplicationStartup();
        }
    }
}