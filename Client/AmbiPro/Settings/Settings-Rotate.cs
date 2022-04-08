using ArnoldVinkCode;
using System;
using System.Configuration;
using System.Diagnostics;
using System.Windows;
using static AmbiPro.AppTasks;
using static AmbiPro.AppVariables;

namespace AmbiPro.Settings
{
    partial class FormSettings
    {
        private void btn_RotateClockwise_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Debug.WriteLine("Rotating the leds clockwise.");
                vCurrentRotation -= 1;

                SettingsFunction.Save("LedRotate" + vCurrentRatio, vCurrentRotation.ToString());
                tb_RotateValue.Text = "Led rotation: " + vCurrentRotation;

                //Check the maximum rotation count
                CheckMaximumRotationCount();
            }
            catch { }
        }

        private void btn_RotateCounterwise_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Debug.WriteLine("Rotating the leds counterwise.");
                vCurrentRotation += 1;

                SettingsFunction.Save("LedRotate" + vCurrentRatio, vCurrentRotation.ToString());
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

                SettingsFunction.Save("LedRotate" + vCurrentRatio, "0");
                tb_RotateValue.Text = "Led rotation: 0";

                //Check the maximum rotation count
                CheckMaximumRotationCount();
            }
            catch { }
        }

        //Update screen information
        public void UpdateScreenInformation()
        {
            try
            {
                //Check if screen capture is enabled
                bool captureEnabled = vTask_UpdateLed.TaskRunning && SettingsFunction.Load("LedMode", typeof(int)) == 0;
                if (!captureEnabled)
                {
                    btn_RotateCounterwise.IsEnabled = false;
                    btn_RotateClockwise.IsEnabled = false;
                    btn_RotateReset.IsEnabled = false;
                    textblock_DebugPreview.Text = "Select screen capture mode and turn leds on to debug.";
                    image_DebugPreview.Source = null;
                    tb_RotateResolution.Text = "Select screen capture mode and turn leds on to start calibration.";
                    return;
                }

                //Get the current screen resolution and ratio
                vCurrentRatio = AVFunctions.ScreenAspectRatio(vCaptureWidth, vCaptureHeight, false);
                string captureInfo = vCaptureWidth + "x" + vCaptureHeight + " (" + vCurrentRatio + ")";
                if (vCaptureHDREnabled)
                {
                    captureInfo += " (HDR)";
                }
                else
                {
                    captureInfo += " (SDR)";
                }

                //Update debug screen
                textblock_DebugPreview.Text = "Screen capture preview " + captureInfo + ":";

                //Update rotation screen
                if (SettingsFunction.Check("LedRotate" + vCurrentRatio))
                {
                    vCurrentRotation = Convert.ToInt32(ConfigurationManager.AppSettings["LedRotate" + vCurrentRatio]);
                }
                tb_RotateValue.Text = "Led rotation: " + vCurrentRotation;
                tb_RotateResolution.Text = "Capture resolution: " + captureInfo;
                CheckMaximumRotationCount();
            }
            catch { }
        }

        //Check the maximum rotation count
        void CheckMaximumRotationCount()
        {
            try
            {
                if (vCaptureWidth == 0 || vCaptureHeight == 0)
                {
                    btn_RotateClockwise.IsEnabled = false;
                    btn_RotateCounterwise.IsEnabled = false;
                    return;
                }

                int maximumLedCount = Convert.ToInt32(ConfigurationManager.AppSettings["LedCountFirst"]) + Convert.ToInt32(ConfigurationManager.AppSettings["LedCountSecond"]) + Convert.ToInt32(ConfigurationManager.AppSettings["LedCountThird"]) + Convert.ToInt32(ConfigurationManager.AppSettings["LedCountFourth"]);
                if (vCurrentRotation > -maximumLedCount) { btn_RotateClockwise.IsEnabled = true; } else { btn_RotateClockwise.IsEnabled = false; }
                if (vCurrentRotation < maximumLedCount) { btn_RotateCounterwise.IsEnabled = true; } else { btn_RotateCounterwise.IsEnabled = false; }
                btn_RotateReset.IsEnabled = true;
            }
            catch { }
        }
    }
}