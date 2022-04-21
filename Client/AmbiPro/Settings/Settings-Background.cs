using ArnoldVinkCode;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;
using static AmbiPro.AppVariables;

namespace AmbiPro.Settings
{
    partial class FormSettings
    {
        //Switch test background
        private void button_BackgroundSwitch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SwitchBackground(false, false, false, false);
            }
            catch { }
        }

        private void SwitchBackground(bool forceBlocks, bool forceBlackbars, bool forceSolid, bool forceTransparent)
        {
            try
            {
                //Hide backgrounds
                grid_BackgroundBlocks.Visibility = Visibility.Collapsed;
                grid_BackgroundBlackbars.Visibility = Visibility.Collapsed;
                grid_BackgroundSolid.Visibility = Visibility.Collapsed;

                //Hide top menus
                border_ColorMenu.Visibility = Visibility.Collapsed;
                grid_ColorMenu_Blocks.Visibility = Visibility.Collapsed;
                grid_ColorMenu_Blackbars.Visibility = Visibility.Collapsed;
                grid_ColorMenu_SolidColor.Visibility = Visibility.Collapsed;

                //Change background forced
                if (forceBlocks)
                {
                    border_ColorMenu.Visibility = Visibility.Visible;
                    grid_ColorMenu_Blocks.Visibility = Visibility.Visible;
                    grid_BackgroundBlocks.Visibility = Visibility.Visible;
                    vCurrentBackground = 1;
                    return;
                }
                else if (forceBlackbars)
                {
                    grid_MainWindow.Margin = new Thickness(0, 0, 0, 0);
                    border_ColorMenu.Visibility = Visibility.Visible;
                    grid_ColorMenu_Blackbars.Visibility = Visibility.Visible;
                    grid_BackgroundBlackbars.Visibility = Visibility.Visible;
                    vCurrentBackground = 2;
                    return;
                }
                else if (forceSolid)
                {
                    border_ColorMenu.Visibility = Visibility.Visible;
                    grid_ColorMenu_SolidColor.Visibility = Visibility.Visible;
                    grid_BackgroundSolid.Visibility = Visibility.Visible;
                    vCurrentBackground = 3;
                    return;
                }
                else if (forceTransparent)
                {
                    vCurrentBackground = 0;
                    return;
                }

                //Change background automatic
                if (vCurrentBackground == 0)
                {
                    border_ColorMenu.Visibility = Visibility.Visible;
                    grid_ColorMenu_Blocks.Visibility = Visibility.Visible;
                    grid_BackgroundBlocks.Visibility = Visibility.Visible;
                    vCurrentBackground = 1;
                }
                else if (vCurrentBackground == 1)
                {
                    grid_MainWindow.Margin = new Thickness(0, 0, 0, 0);
                    border_ColorMenu.Visibility = Visibility.Visible;
                    grid_ColorMenu_Blackbars.Visibility = Visibility.Visible;
                    grid_BackgroundBlackbars.Visibility = Visibility.Visible;
                    vCurrentBackground = 2;
                }
                else if (vCurrentBackground == 2)
                {
                    border_ColorMenu.Visibility = Visibility.Visible;
                    grid_ColorMenu_SolidColor.Visibility = Visibility.Visible;
                    grid_BackgroundSolid.Visibility = Visibility.Visible;
                    vCurrentBackground = 3;
                }
                else if (vCurrentBackground == 3)
                {
                    vCurrentBackground = 0;
                }
            }
            catch { }
        }

        private void button_BackgroundSolidShowHide_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (grid_BackgroundSolid.Visibility == Visibility.Visible)
                {
                    grid_BackgroundSolid.Visibility = Visibility.Collapsed;
                }
                else
                {
                    grid_BackgroundSolid.Visibility = Visibility.Visible;
                }
            }
            catch { }
        }

        private async void button_BackgroundSolidChange_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Color? newColor = await new AVColorPicker().Popup(null);
                if (newColor != null)
                {
                    grid_BackgroundSolidColor.Background = new SolidColorBrush((Color)newColor);
                }
            }
            catch { }
        }

        private void slider_BackgroundSolidBrightness_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
                Slider senderSlider = (Slider)sender;
                grid_BackgroundSolidColor.Opacity = senderSlider.Value;
            }
            catch { }
        }

        private void button_BackgroundBlocksShowHide_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (grid_BackgroundBlocks.Visibility == Visibility.Visible)
                {
                    grid_BackgroundBlocks.Visibility = Visibility.Collapsed;
                }
                else
                {
                    grid_BackgroundBlocks.Visibility = Visibility.Visible;
                }
            }
            catch { }
        }

        private void sp_DecreaseBlockSize_PreviewMouseUp(object sender, RoutedEventArgs e)
        {
            try
            {
                Debug.WriteLine("Decreasing the block sizes..." + sp_Block1.Width);
                if (sp_Block1.Width >= 50)
                {
                    sp_Block1.Width -= 10;
                    sp_Block1.Height -= 10;
                    sp_Block2.Width -= 10;
                    sp_Block2.Height -= 10;
                    sp_Block3.Width -= 10;
                    sp_Block3.Height -= 10;
                    sp_Block4.Width -= 10;
                    sp_Block4.Height -= 10;
                    sp_Block5.Width -= 10;
                    sp_Block5.Height -= 10;
                    sp_Block6.Width -= 10;
                    sp_Block6.Height -= 10;
                    sp_Block7.Width -= 10;
                    sp_Block7.Height -= 10;
                    sp_Block8.Width -= 10;
                    sp_Block8.Height -= 10;
                }
            }
            catch { }
        }

        private void sp_IncreaseBlockSize_PreviewMouseUp(object sender, RoutedEventArgs e)
        {
            try
            {
                Debug.WriteLine("Increasing the block sizes..." + sp_Block1.Width);
                if (sp_Block1.Width < (Screen.PrimaryScreen.Bounds.Height / 4))
                {
                    sp_Block1.Width += 10;
                    sp_Block1.Height += 10;
                    sp_Block2.Width += 10;
                    sp_Block2.Height += 10;
                    sp_Block3.Width += 10;
                    sp_Block3.Height += 10;
                    sp_Block4.Width += 10;
                    sp_Block4.Height += 10;
                    sp_Block5.Width += 10;
                    sp_Block5.Height += 10;
                    sp_Block6.Width += 10;
                    sp_Block6.Height += 10;
                    sp_Block7.Width += 10;
                    sp_Block7.Height += 10;
                    sp_Block8.Width += 10;
                    sp_Block8.Height += 10;
                }
            }
            catch { }
        }
    }
}