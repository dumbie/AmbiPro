using AmbiPro.Settings;
using ArnoldVinkCode;
using System;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using static AmbiPro.AppVariables;

namespace AmbiPro.Calibrate
{
    public partial class FormCalibrate : Window
    {
        //Window Initialize
        public FormCalibrate() { InitializeComponent(); }

        //Window Variables
        private int vPreviousLedCaptureRange = Convert.ToInt32(ConfigurationManager.AppSettings["LedCaptureRange"]);
        private int vPreviousLedColorCut = Convert.ToInt32(ConfigurationManager.AppSettings["LedColorCut"]);
        private string vPreviousAdjustBlackBars = Convert.ToString(ConfigurationManager.AppSettings["AdjustBlackBars"]);
        private string vCurrentRatio = string.Empty;
        private int vCurrentRotation = 0;
        private int vCurrentColor = 0;
        private int vCurrentBlackbar = 0;

        //Handle window activated event
        protected override void OnActivated(EventArgs e)
        {
            try
            {
                Debug.WriteLine("Time to calibrate.");

                //Fullscreen the calibration window
                WindowStyle = WindowStyle.None;
                WindowState = WindowState.Maximized;

                //Temporarily set led capture range to 2
                SettingsFunction.Save("LedCaptureRange", "2");

                //Temporarily set led color cut off to 0
                SettingsFunction.Save("LedColorCut", "0");

                //Temporarily enable blackbar detection
                SettingsFunction.Save("AdjustBlackBars", "True");

                //Set the current screen resolution and ratio
                vCurrentRatio = AVFunctions.ScreenAspectRatio(vScreenWidth, vScreenHeight, false);
                if (string.IsNullOrWhiteSpace(vCurrentRatio))
                {
                    btn_RotateCounterwise.IsEnabled = false;
                    btn_RotateClockwise.IsEnabled = false;
                    btn_RotateReset.IsEnabled = false;
                    tb_RotateResolution.Text = "Switch to screen capture mode to start calibration.";
                    return;
                }
                else
                {
                    tb_RotateResolution.Text = "Capture resolution: " + vScreenWidth + "x" + vScreenHeight + " (" + vCurrentRatio + ")";
                }

                //Update the rotation based on ratio
                if (SettingsFunction.Check("LedRotate" + vCurrentRatio))
                {
                    vCurrentRotation = Convert.ToInt32(ConfigurationManager.AppSettings["LedRotate" + vCurrentRatio]);
                }
                tb_RotateValue.Text = "Led rotation: " + vCurrentRotation;

                //Check the maximum rotation count
                CheckMaximumRotationCount();
            }
            catch { }
        }

        //Handle window closing event
        protected override void OnClosing(CancelEventArgs e)
        {
            try
            {
                e.Cancel = true;
                Debug.WriteLine("Closing the calibrate window.");

                //Restore the temporarily changed settings
                SettingsFunction.Save("LedCaptureRange", vPreviousLedCaptureRange.ToString());
                SettingsFunction.Save("LedColorCut", vPreviousLedColorCut.ToString());
                SettingsFunction.Save("AdjustBlackBars", vPreviousAdjustBlackBars);

                //Hide the window
                this.Hide();
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