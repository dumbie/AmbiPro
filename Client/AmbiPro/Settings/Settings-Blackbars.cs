using System.Windows;
using static AmbiPro.AppVariables;

namespace AmbiPro.Settings
{
    partial class FormSettings
    {
        private void btn_BlackbarScenario_Click(object sender, RoutedEventArgs e)
        {
            if (vCurrentBlackbar == 0)
            {
                vCurrentBlackbar = 1;
                grid_BlackbarBlock1.Visibility = Visibility.Collapsed;
                grid_BlackbarBlock2.Visibility = Visibility.Visible;
                grid_BlackbarBlock3.Visibility = Visibility.Visible;
                grid_BlackbarBlock4.Visibility = Visibility.Visible;
            }
            else if (vCurrentBlackbar == 1)
            {
                vCurrentBlackbar = 2;
                grid_BlackbarBlock1.Visibility = Visibility.Visible;
                grid_BlackbarBlock2.Visibility = Visibility.Collapsed;
                grid_BlackbarBlock3.Visibility = Visibility.Visible;
                grid_BlackbarBlock4.Visibility = Visibility.Visible;
            }
            else if (vCurrentBlackbar == 2)
            {
                vCurrentBlackbar = 3;
                grid_BlackbarBlock1.Visibility = Visibility.Visible;
                grid_BlackbarBlock2.Visibility = Visibility.Visible;
                grid_BlackbarBlock3.Visibility = Visibility.Collapsed;
                grid_BlackbarBlock4.Visibility = Visibility.Visible;
            }
            else if (vCurrentBlackbar == 3)
            {
                vCurrentBlackbar = 4;
                grid_BlackbarBlock1.Visibility = Visibility.Visible;
                grid_BlackbarBlock2.Visibility = Visibility.Visible;
                grid_BlackbarBlock3.Visibility = Visibility.Visible;
                grid_BlackbarBlock4.Visibility = Visibility.Collapsed;
            }
            else if (vCurrentBlackbar == 4)
            {
                vCurrentBlackbar = 5;
                grid_BlackbarBlock1.Visibility = Visibility.Visible;
                grid_BlackbarBlock2.Visibility = Visibility.Visible;
                grid_BlackbarBlock3.Visibility = Visibility.Collapsed;
                grid_BlackbarBlock4.Visibility = Visibility.Collapsed;
            }
            else if (vCurrentBlackbar == 5)
            {
                vCurrentBlackbar = 6;
                grid_BlackbarBlock1.Visibility = Visibility.Collapsed;
                grid_BlackbarBlock2.Visibility = Visibility.Collapsed;
                grid_BlackbarBlock3.Visibility = Visibility.Visible;
                grid_BlackbarBlock4.Visibility = Visibility.Visible;
            }
            else if (vCurrentBlackbar == 6)
            {
                vCurrentBlackbar = 7;
                grid_BlackbarBlock1.Visibility = Visibility.Collapsed;
                grid_BlackbarBlock2.Visibility = Visibility.Collapsed;
                grid_BlackbarBlock3.Visibility = Visibility.Collapsed;
                grid_BlackbarBlock4.Visibility = Visibility.Collapsed;
            }
            else if (vCurrentBlackbar == 7)
            {
                vCurrentBlackbar = 0;
                grid_BlackbarBlock1.Visibility = Visibility.Visible;
                grid_BlackbarBlock2.Visibility = Visibility.Visible;
                grid_BlackbarBlock3.Visibility = Visibility.Visible;
                grid_BlackbarBlock4.Visibility = Visibility.Visible;
            }
        }
    }
}