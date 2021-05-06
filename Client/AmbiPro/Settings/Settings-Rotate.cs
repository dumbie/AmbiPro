using ArnoldVinkCode;
using System;
using System.Configuration;
using System.Diagnostics;
using System.Windows;
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

        //Update the rotation ratio
        void UpdateRotationRatio()
        {
            try
            {
                //Set the current screen resolution and ratio
                vCurrentRatio = AVFunctions.ScreenAspectRatio(vScreenWidth, vScreenHeight, false);
                if (string.IsNullOrWhiteSpace(vCurrentRatio))
                {
                    btn_RotateCounterwise.IsEnabled = false;
                    btn_RotateClockwise.IsEnabled = false;
                    btn_RotateReset.IsEnabled = false;
                    textblock_DebugPreview.Text = "Select screen capture mode and turn leds on to debug.";
                    tb_RotateResolution.Text = "Select screen capture mode and turn leds on to start calibration.";
                    return;
                }
                else
                {
                    textblock_DebugPreview.Text = "Screen capture preview " + vScreenWidth + "x" + vScreenHeight + " (" + vCurrentRatio + "):";
                    tb_RotateResolution.Text = "Capture resolution: " + vScreenWidth + "x" + vScreenHeight + " (" + vCurrentRatio + ")";
                }

                //Update the rotation based on ratio
                if (SettingsFunction.Check("LedRotate" + vCurrentRatio))
                {
                    vCurrentRotation = Convert.ToInt32(ConfigurationManager.AppSettings["LedRotate" + vCurrentRatio]);
                }
                tb_RotateValue.Text = "Led rotation: " + vCurrentRotation;
            }
            catch { }
        }

        //Check the maximum rotation count
        void CheckMaximumRotationCount()
        {
            try
            {
                if (vScreenWidth == 0 || vScreenHeight == 0)
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