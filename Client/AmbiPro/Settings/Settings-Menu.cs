using ArnoldVinkCode;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static AmbiPro.AppVariables;

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

                    if (selectedStackPanel.Name != "menuButtonUpdate" && selectedStackPanel.Name != "menuButtonClose" && selectedStackPanel.Name != "menuButtonExit")
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
                        sp_Remote.Visibility = Visibility.Collapsed;
                        sp_Debug.Visibility = Visibility.Collapsed;
                        sp_Help.Visibility = Visibility.Collapsed;
                    }

                    if (selectedStackPanel.Name == "menuButtonBasics")
                    {
                        SwitchBackground(false, false, false, true);
                        sp_Basics.Visibility = Visibility.Visible;
                    }
                    else if (selectedStackPanel.Name == "menuButtonModes")
                    {
                        SwitchBackground(false, false, false, true);
                        sp_Modes.Visibility = Visibility.Visible;
                    }
                    else if (selectedStackPanel.Name == "menuButtonScreen")
                    {
                        SwitchBackground(false, false, false, true);
                        sp_Screen.Visibility = Visibility.Visible;
                    }
                    else if (selectedStackPanel.Name == "menuButtonLeds")
                    {
                        SwitchBackground(true, false, false, false);
                        sp_Leds.Visibility = Visibility.Visible;
                    }
                    else if (selectedStackPanel.Name == "menuButtonAdjust")
                    {
                        SwitchBackground(false, false, true, false);
                        sp_Adjust.Visibility = Visibility.Visible;
                    }
                    else if (selectedStackPanel.Name == "menuButtonRemote")
                    {
                        SwitchBackground(false, false, false, true);
                        sp_Remote.Visibility = Visibility.Visible;
                    }
                    else if (selectedStackPanel.Name == "menuButtonUpdate")
                    {
                        await AppUpdate.CheckAppUpdate(false);
                    }
                    else if (selectedStackPanel.Name == "menuButtonHelp")
                    {
                        SwitchBackground(false, false, false, true);
                        sp_Help.Visibility = Visibility.Visible;
                    }
                    else if (selectedStackPanel.Name == "menuButtonDebug")
                    {
                        SwitchBackground(false, false, false, true);
                        sp_Debug.Visibility = Visibility.Visible;

                        //Enable debug capture
                        vDebugCaptureAllowed = true;

                        //Show debug led preview
                        if (AVSettings.Load(vConfiguration, "DebugLedPreview", typeof(bool)))
                        {
                            grid_LedPreview.Visibility = Visibility.Visible;
                        }
                    }
                    else if (selectedStackPanel.Name == "menuButtonBlackbars")
                    {
                        SwitchBackground(false, true, false, false);
                        sp_Blackbars.Visibility = Visibility.Visible;
                    }
                    else if (selectedStackPanel.Name == "menuButtonRotate")
                    {
                        SwitchBackground(true, false, false, false);
                        sp_LedRotate.Visibility = Visibility.Visible;
                    }
                    else if (selectedStackPanel.Name == "menuButtonClose")
                    {
                        this.Close();
                    }
                    else if (selectedStackPanel.Name == "menuButtonExit")
                    {
                        await AppStartup.Exit();
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