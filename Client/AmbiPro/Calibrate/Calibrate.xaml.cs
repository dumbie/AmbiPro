using AmbiPro.Settings;
using ArnoldVinkCode;
using System;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;

namespace AmbiPro.Calibrate
{
    public partial class FormCalibrate : Window
    {
        //Application Variables
        private Int32 vPreviousLedCaptureRange = Convert.ToInt32(ConfigurationManager.AppSettings["LedCaptureRange"]);
        private Int32 vPreviousLedColorCut = Convert.ToInt32(ConfigurationManager.AppSettings["LedColorCut"]);
        private Int32 vCurrentRotation = 0;
        private Int32 vCurrentColor = 0;

        public FormCalibrate()
        {
            try
            {
                InitializeComponent();
                Loaded += (sender, args) =>
                {
                    Debug.WriteLine("Time to calibrate.");

                    //Fullscreen the calibration window
                    WindowStyle = WindowStyle.None;
                    WindowState = WindowState.Maximized;

                    //Temporarily set led capture range to 2
                    SettingsFunction.Save("LedCaptureRange", "2");

                    //Temporarily set led color cut off to 0
                    SettingsFunction.Save("LedColorCut", "0");

                    //Set the current screen resolution and ratio
                    int ScreenWidth = Screen.PrimaryScreen.Bounds.Width;
                    int ScreenHeight = Screen.PrimaryScreen.Bounds.Height;
                    string ScreenRatio = AVFunctions.ScreenAspectRatio(ScreenWidth, ScreenHeight, false);
                    tb_RotateResolution.Text = "Resolution: " + ScreenWidth + "x" + ScreenHeight + " (" + ScreenRatio + ")";

                    //Update the rotation based on ratio
                    if (SettingsFunction.Check("LedRotate" + ScreenRatio)) { vCurrentRotation = Convert.ToInt32(ConfigurationManager.AppSettings["LedRotate" + ScreenRatio]); }
                    tb_RotateValue.Text = "Led rotation: " + vCurrentRotation;

                    //Check the maximum rotation count
                    CheckMaximumRotationCount();
                };
            }
            catch { }
        }

        //Handle window close button
        private void btn_Close_Click(object sender, RoutedEventArgs e) { try { this.Close(); } catch { } }

        //Handle window closing event
        protected override void OnClosing(CancelEventArgs e)
        {
            try
            {
                e.Cancel = true;
                Debug.WriteLine("Closing the calibrate window.");

                //Restore the led capture range to previous
                SettingsFunction.Save("LedCaptureRange", vPreviousLedCaptureRange.ToString());

                //Restore the led color cut off to previous
                SettingsFunction.Save("LedColorCut", vPreviousLedColorCut.ToString());

                this.Hide();
            }
            catch { }
        }

        private void btn_RotateLeft_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Debug.WriteLine("Rotating the leds to the left.");
                vCurrentRotation -= 1;

                string ScreenRatio = AVFunctions.ScreenAspectRatio(0, 0, true);
                SettingsFunction.Save("LedRotate" + ScreenRatio, vCurrentRotation.ToString());
                tb_RotateValue.Text = "Led rotation: " + vCurrentRotation;

                //Check the maximum rotation count
                CheckMaximumRotationCount();
            }
            catch { }
        }

        private void btn_RotateRight_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Debug.WriteLine("Rotating the leds to the right.");
                vCurrentRotation += 1;

                string ScreenRatio = AVFunctions.ScreenAspectRatio(0, 0, true);
                SettingsFunction.Save("LedRotate" + ScreenRatio, vCurrentRotation.ToString());
                tb_RotateValue.Text = "Led rotation: " + vCurrentRotation;

                //Check the maximum rotation count
                CheckMaximumRotationCount();
            }
            catch { }
        }

        private void btn_RotateReset_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Debug.WriteLine("Resetting the led rotation.");
                vCurrentRotation = 0;

                string ScreenRatio = AVFunctions.ScreenAspectRatio(0, 0, true);
                SettingsFunction.Save("LedRotate" + ScreenRatio, "0");
                tb_RotateValue.Text = "Led rotation: 0";

                //Check the maximum rotation count
                CheckMaximumRotationCount();
            }
            catch { }
        }

        //Check the maximum rotation count
        void CheckMaximumRotationCount()
        {
            try
            {
                Int32 CurrentLedCount = Convert.ToInt32(ConfigurationManager.AppSettings["LedCount"]);
                if (vCurrentRotation > -CurrentLedCount) { btn_RotateLeft.IsEnabled = true; } else { btn_RotateLeft.IsEnabled = false; }
                if (vCurrentRotation < CurrentLedCount) { btn_RotateRight.IsEnabled = true; } else { btn_RotateRight.IsEnabled = false; }
            }
            catch { }
        }

        //Rotate the background colors
        private void btn_RotateColors_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (vCurrentColor == 0)
                {
                    vCurrentColor = 1;
                    grid_CaliBackground1.Background = new SolidColorBrush(Colors.Yellow);
                    grid_CaliBackground2.Background = new SolidColorBrush(Colors.Red);
                    grid_CaliBackground3.Background = new SolidColorBrush(Colors.Green);
                    grid_CaliBackground4.Background = new SolidColorBrush(Colors.Blue);
                }
                else if (vCurrentColor == 1)
                {
                    vCurrentColor = 2;
                    grid_CaliBackground1.Background = new SolidColorBrush(Colors.Blue);
                    grid_CaliBackground2.Background = new SolidColorBrush(Colors.Yellow);
                    grid_CaliBackground3.Background = new SolidColorBrush(Colors.Red);
                    grid_CaliBackground4.Background = new SolidColorBrush(Colors.Green);
                }
                else if (vCurrentColor == 2)
                {
                    vCurrentColor = 3;
                    grid_CaliBackground1.Background = new SolidColorBrush(Colors.Green);
                    grid_CaliBackground2.Background = new SolidColorBrush(Colors.Blue);
                    grid_CaliBackground3.Background = new SolidColorBrush(Colors.Yellow);
                    grid_CaliBackground4.Background = new SolidColorBrush(Colors.Red);
                }
                else if (vCurrentColor == 3)
                {
                    vCurrentColor = 0;
                    grid_CaliBackground1.Background = new SolidColorBrush(Colors.Red);
                    grid_CaliBackground2.Background = new SolidColorBrush(Colors.Green);
                    grid_CaliBackground3.Background = new SolidColorBrush(Colors.Blue);
                    grid_CaliBackground4.Background = new SolidColorBrush(Colors.Yellow);
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