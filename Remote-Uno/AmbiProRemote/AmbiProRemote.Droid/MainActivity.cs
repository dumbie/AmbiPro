using Android.App;
using Android.Views;

namespace AmbiProRemote.Droid
{
    [Activity(
            MainLauncher = true,
            ConfigurationChanges = global::Uno.UI.ActivityHelper.AllConfigChanges,
            WindowSoftInputMode = SoftInput.AdjustPan | SoftInput.StateHidden
    )]
    public class MainActivity : Windows.UI.Xaml.ApplicationActivity
    {
    }
}