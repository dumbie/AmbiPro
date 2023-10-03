using ArnoldVinkCode;
using System.Diagnostics;
using System.Windows;
using static AmbiPro.AppTasks;
using static AmbiPro.AppVariables;
using static ArnoldVinkCode.AVSettings;

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

                SettingSave(vConfiguration, "LedRotate" + vCurrentRatio, vCurrentRotation.ToString());
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

                SettingSave(vConfiguration, "LedRotate" + vCurrentRatio, vCurrentRotation.ToString());
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

                SettingSave(vConfiguration, "LedRotate" + vCurrentRatio, "0");
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
                bool captureEnabled = vTask_UpdateLed.TaskRunning && SettingLoad(vConfiguration, "LedMode", typeof(int)) == 0;
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
                vCurrentRatio = AVFunctions.ScreenAspectRatio(vCaptureDetails.OutputWidth, vCaptureDetails.OutputHeight, false);
                string captureInfo = vCaptureDetails.OutputWidth + "x" + vCaptureDetails.OutputHeight + " (" + vCurrentRatio + ")";
                if (vCaptureDetails.HDREnabled)
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
                if (SettingCheck(vConfiguration, "LedRotate" + vCurrentRatio))
                {
                    vCurrentRotation = SettingLoad(vConfiguration, "LedRotate" + vCurrentRatio, typeof(int));
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
                if (vCaptureDetails.OutputWidth == 0 || vCaptureDetails.OutputHeight == 0)
                {
                    btn_RotateClockwise.IsEnabled = false;
                    btn_RotateCounterwise.IsEnabled = false;
                    return;
                }

                int maximumLedCount = SettingLoad(vConfiguration, "LedCountFirst", typeof(int)) + SettingLoad(vConfiguration, "LedCountSecond", typeof(int)) + SettingLoad(vConfiguration, "LedCountThird", typeof(int)) + SettingLoad(vConfiguration, "LedCountFourth", typeof(int));
                if (vCurrentRotation > -maximumLedCount) { btn_RotateClockwise.IsEnabled = true; } else { btn_RotateClockwise.IsEnabled = false; }
                if (vCurrentRotation < maximumLedCount) { btn_RotateCounterwise.IsEnabled = true; } else { btn_RotateCounterwise.IsEnabled = false; }
                btn_RotateReset.IsEnabled = true;
            }
            catch { }
        }
    }
}