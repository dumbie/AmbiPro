using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AmbiPro.Calibrate
{
    partial class FormCalibrate
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
                    if (vSingleTappedEvent) { lb_Menu_SingleTap(); }
                }
            }
            catch { }
        }

        //Handle main menu single tap
        void lb_Menu_SingleTap()
        {
            try
            {
                if (lb_Menu.SelectedIndex >= 0)
                {
                    StackPanel SelStackPanel = (StackPanel)lb_Menu.SelectedItem;
                    if (SelStackPanel.Name == "menuButtonRotate")
                    {
                        grid_CalibrateRotate.Visibility = Visibility.Visible;
                        grid_CalibrateBlackbars.Visibility = Visibility.Collapsed;

                        stackpanel_LedRotation.Visibility = Visibility.Visible;
                        stackpanel_LedCount.Visibility = Visibility.Collapsed;
                        stackpanel_BlackbarsTest.Visibility = Visibility.Collapsed;

                        sp_Block1.Visibility = Visibility.Visible;
                        sp_Block2.Visibility = Visibility.Visible;
                        sp_Block3.Visibility = Visibility.Visible;
                        sp_Block4.Visibility = Visibility.Visible;
                        sp_Block5.Visibility = Visibility.Visible;
                        sp_Block6.Visibility = Visibility.Visible;
                        sp_Block7.Visibility = Visibility.Visible;
                        sp_Block8.Visibility = Visibility.Visible;
                    }
                    else if (SelStackPanel.Name == "menuButtonLeds")
                    {
                        grid_CalibrateRotate.Visibility = Visibility.Visible;
                        grid_CalibrateBlackbars.Visibility = Visibility.Collapsed;

                        stackpanel_LedRotation.Visibility = Visibility.Collapsed;
                        stackpanel_LedCount.Visibility = Visibility.Visible;
                        stackpanel_BlackbarsTest.Visibility = Visibility.Collapsed;

                        sp_Block1.Visibility = Visibility.Collapsed;
                        sp_Block2.Visibility = Visibility.Collapsed;
                        sp_Block3.Visibility = Visibility.Collapsed;
                        sp_Block4.Visibility = Visibility.Collapsed;
                        sp_Block5.Visibility = Visibility.Collapsed;
                        sp_Block6.Visibility = Visibility.Collapsed;
                        sp_Block7.Visibility = Visibility.Collapsed;
                        sp_Block8.Visibility = Visibility.Collapsed;
                    }
                    else if (SelStackPanel.Name == "menuButtonBlackbar")
                    {
                        grid_CalibrateRotate.Visibility = Visibility.Collapsed;
                        grid_CalibrateBlackbars.Visibility = Visibility.Visible;

                        stackpanel_LedRotation.Visibility = Visibility.Collapsed;
                        stackpanel_LedCount.Visibility = Visibility.Collapsed;
                        stackpanel_BlackbarsTest.Visibility = Visibility.Visible;
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