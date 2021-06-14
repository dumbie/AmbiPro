using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
                    StackPanel SelStackPanel = (StackPanel)lb_Menu.SelectedItem;

                    if (SelStackPanel.Name == "menuButtonBasics")
                    {
                        SwitchBackground(false, false, true);
                        sp_Basics.Visibility = Visibility.Visible;
                        sp_Modes.Visibility = Visibility.Collapsed;
                        sp_Screen.Visibility = Visibility.Collapsed;
                        sp_Blackbars.Visibility = Visibility.Collapsed;
                        sp_LedRotate.Visibility = Visibility.Collapsed;
                        sp_Leds.Visibility = Visibility.Collapsed;
                        sp_Adjust.Visibility = Visibility.Collapsed;
                        sp_Remote.Visibility = Visibility.Collapsed;
                        sp_Debug.Visibility = Visibility.Collapsed;
                        sp_Help.Visibility = Visibility.Collapsed;
                    }
                    if (SelStackPanel.Name == "menuButtonModes")
                    {
                        SwitchBackground(false, false, true);
                        sp_Basics.Visibility = Visibility.Collapsed;
                        sp_Modes.Visibility = Visibility.Visible;
                        sp_Screen.Visibility = Visibility.Collapsed;
                        sp_Blackbars.Visibility = Visibility.Collapsed;
                        sp_LedRotate.Visibility = Visibility.Collapsed;
                        sp_Leds.Visibility = Visibility.Collapsed;
                        sp_Adjust.Visibility = Visibility.Collapsed;
                        sp_Remote.Visibility = Visibility.Collapsed;
                        sp_Debug.Visibility = Visibility.Collapsed;
                        sp_Help.Visibility = Visibility.Collapsed;
                    }
                    else if (SelStackPanel.Name == "menuButtonScreen")
                    {
                        SwitchBackground(false, false, true);
                        sp_Basics.Visibility = Visibility.Collapsed;
                        sp_Modes.Visibility = Visibility.Collapsed;
                        sp_Screen.Visibility = Visibility.Visible;
                        sp_Blackbars.Visibility = Visibility.Collapsed;
                        sp_LedRotate.Visibility = Visibility.Collapsed;
                        sp_Leds.Visibility = Visibility.Collapsed;
                        sp_Adjust.Visibility = Visibility.Collapsed;
                        sp_Remote.Visibility = Visibility.Collapsed;
                        sp_Debug.Visibility = Visibility.Collapsed;
                        sp_Help.Visibility = Visibility.Collapsed;
                    }
                    else if (SelStackPanel.Name == "menuButtonLeds")
                    {
                        SwitchBackground(true, false, false);
                        sp_Basics.Visibility = Visibility.Collapsed;
                        sp_Modes.Visibility = Visibility.Collapsed;
                        sp_Screen.Visibility = Visibility.Collapsed;
                        sp_Blackbars.Visibility = Visibility.Collapsed;
                        sp_LedRotate.Visibility = Visibility.Collapsed;
                        sp_Leds.Visibility = Visibility.Visible;
                        sp_Adjust.Visibility = Visibility.Collapsed;
                        sp_Remote.Visibility = Visibility.Collapsed;
                        sp_Debug.Visibility = Visibility.Collapsed;
                        sp_Help.Visibility = Visibility.Collapsed;
                    }
                    else if (SelStackPanel.Name == "menuButtonAdjust")
                    {
                        SwitchBackground(false, false, true);
                        sp_Basics.Visibility = Visibility.Collapsed;
                        sp_Modes.Visibility = Visibility.Collapsed;
                        sp_Screen.Visibility = Visibility.Collapsed;
                        sp_Blackbars.Visibility = Visibility.Collapsed;
                        sp_LedRotate.Visibility = Visibility.Collapsed;
                        sp_Leds.Visibility = Visibility.Collapsed;
                        sp_Adjust.Visibility = Visibility.Visible;
                        sp_Remote.Visibility = Visibility.Collapsed;
                        sp_Debug.Visibility = Visibility.Collapsed;
                        sp_Help.Visibility = Visibility.Collapsed;
                    }
                    else if (SelStackPanel.Name == "menuButtonRemote")
                    {
                        SwitchBackground(false, false, true);
                        sp_Basics.Visibility = Visibility.Collapsed;
                        sp_Modes.Visibility = Visibility.Collapsed;
                        sp_Screen.Visibility = Visibility.Collapsed;
                        sp_Blackbars.Visibility = Visibility.Collapsed;
                        sp_LedRotate.Visibility = Visibility.Collapsed;
                        sp_Leds.Visibility = Visibility.Collapsed;
                        sp_Adjust.Visibility = Visibility.Collapsed;
                        sp_Remote.Visibility = Visibility.Visible;
                        sp_Debug.Visibility = Visibility.Collapsed;
                        sp_Help.Visibility = Visibility.Collapsed;
                    }
                    else if (SelStackPanel.Name == "menuButtonUpdate")
                    {
                        await AppUpdate.CheckForAppUpdate(false);
                    }
                    else if (SelStackPanel.Name == "menuButtonHelp")
                    {
                        SwitchBackground(false, false, true);
                        sp_Basics.Visibility = Visibility.Collapsed;
                        sp_Modes.Visibility = Visibility.Collapsed;
                        sp_Screen.Visibility = Visibility.Collapsed;
                        sp_Blackbars.Visibility = Visibility.Collapsed;
                        sp_LedRotate.Visibility = Visibility.Collapsed;
                        sp_Leds.Visibility = Visibility.Collapsed;
                        sp_Adjust.Visibility = Visibility.Collapsed;
                        sp_Remote.Visibility = Visibility.Collapsed;
                        sp_Debug.Visibility = Visibility.Collapsed;
                        sp_Help.Visibility = Visibility.Visible;
                    }
                    else if (SelStackPanel.Name == "menuButtonDebug")
                    {
                        SwitchBackground(false, false, true);
                        sp_Basics.Visibility = Visibility.Collapsed;
                        sp_Modes.Visibility = Visibility.Collapsed;
                        sp_Screen.Visibility = Visibility.Collapsed;
                        sp_Blackbars.Visibility = Visibility.Collapsed;
                        sp_LedRotate.Visibility = Visibility.Collapsed;
                        sp_Leds.Visibility = Visibility.Collapsed;
                        sp_Adjust.Visibility = Visibility.Collapsed;
                        sp_Remote.Visibility = Visibility.Collapsed;
                        sp_Debug.Visibility = Visibility.Visible;
                        sp_Help.Visibility = Visibility.Collapsed;
                    }
                    else if (SelStackPanel.Name == "menuButtonBlackbars")
                    {
                        SwitchBackground(false, true, false);
                        sp_Basics.Visibility = Visibility.Collapsed;
                        sp_Modes.Visibility = Visibility.Collapsed;
                        sp_Screen.Visibility = Visibility.Collapsed;
                        sp_Blackbars.Visibility = Visibility.Visible;
                        sp_LedRotate.Visibility = Visibility.Collapsed;
                        sp_Leds.Visibility = Visibility.Collapsed;
                        sp_Adjust.Visibility = Visibility.Collapsed;
                        sp_Remote.Visibility = Visibility.Collapsed;
                        sp_Debug.Visibility = Visibility.Collapsed;
                        sp_Help.Visibility = Visibility.Collapsed;
                    }
                    else if (SelStackPanel.Name == "menuButtonRotate")
                    {
                        SwitchBackground(true, false, false);
                        sp_Basics.Visibility = Visibility.Collapsed;
                        sp_Modes.Visibility = Visibility.Collapsed;
                        sp_Screen.Visibility = Visibility.Collapsed;
                        sp_Blackbars.Visibility = Visibility.Collapsed;
                        sp_LedRotate.Visibility = Visibility.Visible;
                        sp_Leds.Visibility = Visibility.Collapsed;
                        sp_Adjust.Visibility = Visibility.Collapsed;
                        sp_Remote.Visibility = Visibility.Collapsed;
                        sp_Debug.Visibility = Visibility.Collapsed;
                        sp_Help.Visibility = Visibility.Collapsed;
                    }
                    else if (SelStackPanel.Name == "menuButtonClose")
                    {
                        this.Close();
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
                    StackPanel SelStackPanel = (StackPanel)lb_Menu.SelectedItem;
                    //if (SelStackPanel.Name == "menuButtonShutdown") { await Application_Exit(false); }
                }
            }
            catch { }
        }
    }
}