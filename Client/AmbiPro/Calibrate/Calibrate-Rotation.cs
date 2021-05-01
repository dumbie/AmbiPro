using AmbiPro.Settings;
using System;
using System.Configuration;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;

namespace AmbiPro.Calibrate
{
    partial class FormCalibrate
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

        //Check the maximum rotation count
        void CheckMaximumRotationCount()
        {
            try
            {
                int maximumLedCount = Convert.ToInt32(ConfigurationManager.AppSettings["LedCountFirst"]) + Convert.ToInt32(ConfigurationManager.AppSettings["LedCountSecond"]) + Convert.ToInt32(ConfigurationManager.AppSettings["LedCountThird"]) + Convert.ToInt32(ConfigurationManager.AppSettings["LedCountFourth"]);
                if (vCurrentRotation > -maximumLedCount) { btn_RotateClockwise.IsEnabled = true; } else { btn_RotateClockwise.IsEnabled = false; }
                if (vCurrentRotation < maximumLedCount) { btn_RotateCounterwise.IsEnabled = true; } else { btn_RotateCounterwise.IsEnabled = false; }
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
    }
}