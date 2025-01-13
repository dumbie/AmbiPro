using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static AmbiPro.AppEnums;
using static AmbiPro.AppVariables;
using static ArnoldVinkCode.AVSettings;

namespace AmbiPro.Settings
{
    partial class FormSettings
    {
        //Application variables
        public static bool vSingleTappedEvent = true;

        //Handle main menu mouse/touch tapped
        async void lb_Menu_MousePressUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (e.ClickCount == 1)
                {
                    vSingleTappedEvent = true;
                    await Task.Delay(250);
                    if (vSingleTappedEvent) { await lb_Menu_SingleTap(); }
                }
            }
            catch { }
        }

        //Handle main menu single tap
        async Task lb_Menu_SingleTap()
        {
            try
            {
                if (lb_Menu.SelectedIndex >= 0)
                {
                    StackPanel selectedStackPanel = (StackPanel)lb_Menu.SelectedItem;
                    if (selectedStackPanel.Name != "menuButtonUpdate")
                    {
                        //Update current visible menu
                        vCurrentVisibleMenu = selectedStackPanel.Name;

                        //Disable debug capture
                        vDebugCaptureAllowed = false;
                        image_DebugPreview.Source = null;

                        //Hide debug led preview
                        grid_LedPreview.Visibility = Visibility.Collapsed;

                        //Hide all setting tabs
                        sp_Basics.Visibility = Visibility.Collapsed;
                        sp_Modes.Visibility = Visibility.Collapsed;
                        sp_Screen.Visibility = Visibility.Collapsed;
                        sp_Blackbars.Visibility = Visibility.Collapsed;
                        sp_LedRotate.Visibility = Visibility.Collapsed;
                        sp_Leds.Visibility = Visibility.Collapsed;
                        sp_Adjust.Visibility = Visibility.Collapsed;
                        sp_Shortcuts.Visibility = Visibility.Collapsed;
                        sp_Remote.Visibility = Visibility.Collapsed;
                        sp_Debug.Visibility = Visibility.Collapsed;
                        sp_Help.Visibility = Visibility.Collapsed;
                    }

                    if (selectedStackPanel.Name == "menuButtonBasics")
                    {
                        ShowBackground(BackgroundMode.Transparent);
                        sp_Basics.Visibility = Visibility.Visible;
                    }
                    else if (selectedStackPanel.Name == "menuButtonModes")
                    {
                        ShowBackground(BackgroundMode.Transparent);
                        sp_Modes.Visibility = Visibility.Visible;
                    }
                    else if (selectedStackPanel.Name == "menuButtonScreen")
                    {
                        ShowBackground(BackgroundMode.Transparent);
                        sp_Screen.Visibility = Visibility.Visible;
                    }
                    else if (selectedStackPanel.Name == "menuButtonLeds")
                    {
                        ShowBackground(BackgroundMode.Blocks);
                        sp_Leds.Visibility = Visibility.Visible;
                    }
                    else if (selectedStackPanel.Name == "menuButtonAdjust")
                    {
                        ShowBackground(BackgroundMode.Solid);
                        sp_Adjust.Visibility = Visibility.Visible;
                    }
                    else if (selectedStackPanel.Name == "menuButtonShortcuts")
                    {
                        ShowBackground(BackgroundMode.Transparent);
                        sp_Shortcuts.Visibility = Visibility.Visible;
                    }
                    else if (selectedStackPanel.Name == "menuButtonRemote")
                    {
                        ShowBackground(BackgroundMode.Transparent);
                        sp_Remote.Visibility = Visibility.Visible;
                    }
                    else if (selectedStackPanel.Name == "menuButtonUpdate")
                    {
                        await AppUpdate.CheckAppUpdate(false);
                    }
                    else if (selectedStackPanel.Name == "menuButtonHelp")
                    {
                        ShowBackground(BackgroundMode.Transparent);
                        sp_Help.Visibility = Visibility.Visible;
                    }
                    else if (selectedStackPanel.Name == "menuButtonDebug")
                    {
                        ShowBackground(BackgroundMode.Transparent);
                        sp_Debug.Visibility = Visibility.Visible;

                        //Enable debug capture
                        vDebugCaptureAllowed = true;

                        //Show debug led preview
                        if (SettingLoad(vConfiguration, "DebugLedPreview", typeof(bool)))
                        {
                            grid_LedPreview.Visibility = Visibility.Visible;
                        }
                    }
                    else if (selectedStackPanel.Name == "menuButtonBlackbars")
                    {
                        ShowBackground(BackgroundMode.Blackbars);
                        sp_Blackbars.Visibility = Visibility.Visible;
                    }
                    else if (selectedStackPanel.Name == "menuButtonRotate")
                    {
                        ShowBackground(BackgroundMode.Blocks);
                        sp_LedRotate.Visibility = Visibility.Visible;
                    }
                }
            }
            catch { }
        }

        //Handle main menu double tap
        void lb_Menu_MouseDoublePress(object sender, MouseButtonEventArgs e)
        {
            try
            {
                vSingleTappedEvent = false;
                if (lb_Menu.SelectedIndex >= 0)
                {
                    StackPanel selectedStackPanel = (StackPanel)lb_Menu.SelectedItem;
                    //if (selectedStackPanel.Name == "menuButtonShutdown") { await Application_Exit(false); }
                }
            }
            catch { }
        }
    }
}