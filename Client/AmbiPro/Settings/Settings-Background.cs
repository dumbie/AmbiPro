using System.Diagnostics;
using System.Windows;
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

                //Hide buttons
                btn_BlockIncrease.Visibility = Visibility.Collapsed;
                btn_BlockDecrease.Visibility = Visibility.Collapsed;
                btn_ColorSolidSwitch.Visibility = Visibility.Collapsed;
                btn_BlackbarsScenario.Visibility = Visibility.Collapsed;

                //Change background forced
                if (forceBlocks)
                {
                    btn_BlockIncrease.Visibility = Visibility.Visible;
                    btn_BlockDecrease.Visibility = Visibility.Visible;
                    grid_BackgroundBlocks.Visibility = Visibility.Visible;
                    vCurrentBackground = 1;
                    return;
                }
                else if (forceBlackbars)
                {
                    grid_MainWindow.Margin = new Thickness(0, 0, 0, 0);
                    btn_BlackbarsScenario.Visibility = Visibility.Visible;
                    grid_BackgroundBlackbars.Visibility = Visibility.Visible;
                    vCurrentBackground = 2;
                    return;
                }
                else if (forceSolid)
                {
                    btn_ColorSolidSwitch.Visibility = Visibility.Visible;
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
                    btn_BlockIncrease.Visibility = Visibility.Visible;
                    btn_BlockDecrease.Visibility = Visibility.Visible;
                    grid_BackgroundBlocks.Visibility = Visibility.Visible;
                    vCurrentBackground = 1;
                }
                else if (vCurrentBackground == 1)
                {
                    grid_MainWindow.Margin = new Thickness(0, 0, 0, 0);
                    btn_BlackbarsScenario.Visibility = Visibility.Visible;
                    grid_BackgroundBlackbars.Visibility = Visibility.Visible;
                    vCurrentBackground = 2;
                }
                else if (vCurrentBackground == 2)
                {
                    btn_ColorSolidSwitch.Visibility = Visibility.Visible;
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

        private void btn_ColorSolidSwitch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (vCurrentSolidColor == 0)
                {
                    grid_BackgroundSolid.Background = new SolidColorBrush(Colors.Red);
                    vCurrentSolidColor = 1;
                }
                else if (vCurrentSolidColor == 1)
                {
                    grid_BackgroundSolid.Background = new SolidColorBrush(Colors.Green);
                    vCurrentSolidColor = 2;
                }
                else if (vCurrentSolidColor == 2)
                {
                    grid_BackgroundSolid.Background = new SolidColorBrush(Colors.Blue);
                    vCurrentSolidColor = 3;
                }
                else if (vCurrentSolidColor == 3)
                {
                    grid_BackgroundSolid.Background = new SolidColorBrush(Colors.Cyan);
                    vCurrentSolidColor = 4;
                }
                else if (vCurrentSolidColor == 4)
                {
                    grid_BackgroundSolid.Background = new SolidColorBrush(Colors.Magenta);
                    vCurrentSolidColor = 5;
                }
                else if (vCurrentSolidColor == 5)
                {
                    grid_BackgroundSolid.Background = new SolidColorBrush(Colors.Yellow);
                    vCurrentSolidColor = 6;
                }
                else if (vCurrentSolidColor == 6)
                {
                    grid_BackgroundSolid.Background = new SolidColorBrush(Colors.White);
                    vCurrentSolidColor = 0;
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