using System;
using System.Windows;
using static ArnoldVinkCode.AVAssembly;

namespace AmbiPro
{
    public partial class App : Application
    {
        protected override async void OnStartup(StartupEventArgs e)
        {
            try
            {
                //Resolve missing assembly dll files
                AppDomain.CurrentDomain.AssemblyResolve += AppAssemblyResolveFile;

                //Run application startup code
                await AppStartup.Startup();
            }
            catch { }
        }
    }
}