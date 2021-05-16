using System.Diagnostics;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
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
                SwitchBackground(false, false, false);
            }
            catch { }
        }

        private void SwitchBackground(bool forceBlocks, bool forceBlackbars, bool forceTransparent)
        {
            try
            {
                //Hide backgrounds
                grid_BackgroundBlocks.Visibility = Visibility.Collapsed;
                grid_BackgroundBlackbars.Visibility = Visibility.Collapsed;
                grid_BackgroundSolidWhite.Visibility = Visibility.Collapsed;
                grid_BackgroundSolidRed.Visibility = Visibility.Collapsed;
                grid_BackgroundSolidGreen.Visibility = Visibility.Collapsed;
                grid_BackgroundSolidBlue.Visibility = Visibility.Collapsed;
                grid_BackgroundSolidYellow.Visibility = Visibility.Collapsed;

                //Show background
                if (forceBlocks)
                {
                    grid_BackgroundBlocks.Visibility = Visibility.Visible;
                    vCurrentBackground = 1;
                    return;
                }
                else if (forceBlackbars)
                {
                    grid_BackgroundBlackbars.Visibility = Visibility.Visible;
                    vCurrentBackground = 2;
                    return;
                }
                else if (forceTransparent)
                {
                    vCurrentBackground = 0;
                    return;
                }

                //Show background
                if (vCurrentBackground == 0)
                {
                    grid_BackgroundBlocks.Visibility = Visibility.Visible;
                    vCurrentBackground = 1;
                }
                else if (vCurrentBackground == 1)
                {
                    grid_BackgroundBlackbars.Visibility = Visibility.Visible;
                    vCurrentBackground = 2;
                }
                else if (vCurrentBackground == 2)
                {
                    grid_BackgroundSolidWhite.Visibility = Visibility.Visible;
                    vCurrentBackground = 3;
                }
                else if (vCurrentBackground == 3)
                {
                    grid_BackgroundSolidRed.Visibility = Visibility.Visible;
                    vCurrentBackground = 4;
                }
                else if (vCurrentBackground == 4)
                {
                    grid_BackgroundSolidGreen.Visibility = Visibility.Visible;
                    vCurrentBackground = 5;
                }
                else if (vCurrentBackground == 5)
                {
                    grid_BackgroundSolidBlue.Visibility = Visibility.Visible;
                    vCurrentBackground = 6;
                }
                else if (vCurrentBackground == 6)
                {
                    grid_BackgroundSolidYellow.Visibility = Visibility.Visible;
                    vCurrentBackground = 7;
                }
                else if (vCurrentBackground == 7)
                {
                    vCurrentBackground = 0;
                }
            }
            catch { }
        }

        private void sp_DecreaseBlockSize_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Debug.WriteLine("Decreasing the block sizes..." + sp_Block1.Width);
                if (sp_Block1.Width >= 50)
                {
                    sp_Block1.Width = sp_Block1.Width - 10;
                    sp_Block1.Height = sp_Block1.Height - 10;
                    sp_Block2.Width = sp_Block2.Width - 10;
                    sp_Block2.Height = sp_Block2.Height - 10;
                    sp_Block3.Width = sp_Block3.Width - 10;
                    sp_Block3.Height = sp_Block3.Height - 10;
                    sp_Block4.Width = sp_Block4.Width - 10;
                    sp_Block4.Height = sp_Block4.Height - 10;
                    sp_Block5.Width = sp_Block5.Width - 10;
                    sp_Block5.Height = sp_Block5.Height - 10;
                    sp_Block6.Width = sp_Block6.Width - 10;
                    sp_Block6.Height = sp_Block6.Height - 10;
                    sp_Block7.Width = sp_Block7.Width - 10;
                    sp_Block7.Height = sp_Block7.Height - 10;
                    sp_Block8.Width = sp_Block8.Width - 10;
                    sp_Block8.Height = sp_Block8.Height - 10;
                }
            }
            catch { }
        }

        private void sp_IncreaseBlockSize_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Debug.WriteLine("Increasing the block sizes..." + sp_Block1.Width);
                if (sp_Block1.Width < (Screen.PrimaryScreen.Bounds.Height / 4))
                {
                    sp_Block1.Width = sp_Block1.Width + 10;
                    sp_Block1.Height = sp_Block1.Height + 10;
                    sp_Block2.Width = sp_Block2.Width + 10;
                    sp_Block2.Height = sp_Block2.Height + 10;
                    sp_Block3.Width = sp_Block3.Width + 10;
                    sp_Block3.Height = sp_Block3.Height + 10;
                    sp_Block4.Width = sp_Block4.Width + 10;
                    sp_Block4.Height = sp_Block4.Height + 10;
                    sp_Block5.Width = sp_Block5.Width + 10;
                    sp_Block5.Height = sp_Block5.Height + 10;
                    sp_Block6.Width = sp_Block6.Width + 10;
                    sp_Block6.Height = sp_Block6.Height + 10;
                    sp_Block7.Width = sp_Block7.Width + 10;
                    sp_Block7.Height = sp_Block7.Height + 10;
                    sp_Block8.Width = sp_Block8.Width + 10;
                    sp_Block8.Height = sp_Block8.Height + 10;
                }
            }
            catch { }
        }
    }
}