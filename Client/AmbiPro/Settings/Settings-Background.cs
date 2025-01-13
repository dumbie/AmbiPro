using ArnoldVinkCode.Styles;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;
using static AmbiPro.AppEnums;
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
                SwitchBackground();
            }
            catch { }
        }

        //Show hide test background
        private void button_BackgroundShowHide_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ShowHideBackground();
            }
            catch { }
        }

        private void ShowHideBackground()
        {
            try
            {
                if (grid_BackgroundSolid.Visibility == Visibility.Visible || grid_BackgroundBlocks.Visibility == Visibility.Visible || grid_BackgroundBlackbars.Visibility == Visibility.Visible)
                {
                    //Hide backgrounds
                    HideBackgrounds();
                }
                else
                {
                    //Show background
                    ShowBackground(vCurrentBackgroundMode);
                }
            }
            catch { }
        }

        private void HideBackgrounds()
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
            }
            catch { }
        }

        private void ShowBackground(BackgroundMode backgroundMode)
        {
            try
            {
                //Hide backgrounds
                HideBackgrounds();

                //Change background
                if (backgroundMode == BackgroundMode.Blocks)
                {
                    border_ColorMenu.Visibility = Visibility.Visible;
                    grid_ColorMenu_Blocks.Visibility = Visibility.Visible;
                    grid_BackgroundBlocks.Visibility = Visibility.Visible;
                    vCurrentBackgroundMode = BackgroundMode.Blocks;
                }
                else if (backgroundMode == BackgroundMode.Solid)
                {
                    border_ColorMenu.Visibility = Visibility.Visible;
                    grid_ColorMenu_SolidColor.Visibility = Visibility.Visible;
                    grid_BackgroundSolid.Visibility = Visibility.Visible;
                    vCurrentBackgroundMode = BackgroundMode.Solid;
                }
                else if (backgroundMode == BackgroundMode.Blackbars)
                {
                    grid_MainWindow.Margin = new Thickness(0, 0, 0, 0);
                    border_ColorMenu.Visibility = Visibility.Visible;
                    grid_ColorMenu_Blackbars.Visibility = Visibility.Visible;
                    grid_BackgroundBlackbars.Visibility = Visibility.Visible;
                    vCurrentBackgroundMode = BackgroundMode.Blackbars;
                }
                else if (backgroundMode == BackgroundMode.Transparent)
                {
                    vCurrentBackgroundMode = BackgroundMode.Transparent;
                }
            }
            catch { }
        }

        private void SwitchBackground()
        {
            try
            {
                //Hide backgrounds
                HideBackgrounds();

                //Change background
                if (vCurrentBackgroundMode == BackgroundMode.Transparent)
                {
                    border_ColorMenu.Visibility = Visibility.Visible;
                    grid_ColorMenu_Blocks.Visibility = Visibility.Visible;
                    grid_BackgroundBlocks.Visibility = Visibility.Visible;
                    button_BackgroundShowHide.IsEnabled = true;
                    vCurrentBackgroundMode = BackgroundMode.Blocks;
                }
                else if (vCurrentBackgroundMode == BackgroundMode.Blocks)
                {
                    border_ColorMenu.Visibility = Visibility.Visible;
                    grid_ColorMenu_SolidColor.Visibility = Visibility.Visible;
                    grid_BackgroundSolid.Visibility = Visibility.Visible;
                    button_BackgroundShowHide.IsEnabled = true;
                    vCurrentBackgroundMode = BackgroundMode.Solid;
                }
                else if (vCurrentBackgroundMode == BackgroundMode.Solid)
                {
                    grid_MainWindow.Margin = new Thickness(0, 0, 0, 0);
                    border_ColorMenu.Visibility = Visibility.Visible;
                    grid_ColorMenu_Blackbars.Visibility = Visibility.Visible;
                    grid_BackgroundBlackbars.Visibility = Visibility.Visible;
                    button_BackgroundShowHide.IsEnabled = true;
                    vCurrentBackgroundMode = BackgroundMode.Blackbars;
                }
                else if (vCurrentBackgroundMode == BackgroundMode.Blackbars)
                {
                    button_BackgroundShowHide.IsEnabled = false;
                    vCurrentBackgroundMode = BackgroundMode.Transparent;
                }
            }
            catch { }
        }

        private async void button_BackgroundSolidChange_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Color? newColor = await new ColorPickerPreset().Popup(null);
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
                if (senderSlider.Value > 1.0)
                {
                    grid_BackgroundSolid.Background = new SolidColorBrush(Colors.White);
                    grid_BackgroundSolidColor.Opacity = 2.0 - senderSlider.Value;
                }
                else if (senderSlider.Value < 1.0)
                {
                    grid_BackgroundSolid.Background = new SolidColorBrush(Colors.Black);
                    grid_BackgroundSolidColor.Opacity = senderSlider.Value;
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