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

                vSettings.Set("LedRotate" + vCurrentRatio, vCurrentRotation.ToString());
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

                vSettings.Set("LedRotate" + vCurrentRatio, vCurrentRotation.ToString());
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

                vSettings.Set("LedRotate" + vCurrentRatio, "0");
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
                bool captureEnabled = vTask_UpdateLed.TaskRunning && vSettings.Load("LedMode", typeof(int)) == 0;
                if (!captureEnabled)
                {
                    btn_RotateCounterwise.IsEnabled = false;
                    btn_RotateClockwise.IsEnabled = false;
                    btn_RotateReset.IsEnabled = false;
                    textblock_DebugInfo.Text = "Select screen capture mode and turn leds on to debug.";
                    textblock_DebugFramerate.Text = string.Empty;
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

                //Update debug information
                textblock_DebugInfo.Text = "Capture information: " + captureInfo;

                //Update rotation screen
                if (vSettings.Check("LedRotate" + vCurrentRatio))
                {
                    vCurrentRotation = vSettings.Load("LedRotate" + vCurrentRatio, typeof(int));
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

                int maximumLedCount = vSettings.Load("LedCountFirst", typeof(int)) + vSettings.Load("LedCountSecond", typeof(int)) + vSettings.Load("LedCountThird", typeof(int)) + vSettings.Load("LedCountFourth", typeof(int));
                if (vCurrentRotation > -maximumLedCount) { btn_RotateClockwise.IsEnabled = true; } else { btn_RotateClockwise.IsEnabled = false; }
                if (vCurrentRotation < maximumLedCount) { btn_RotateCounterwise.IsEnabled = true; } else { btn_RotateCounterwise.IsEnabled = false; }
                btn_RotateReset.IsEnabled = true;
            }
            catch { }
        }
    }
}