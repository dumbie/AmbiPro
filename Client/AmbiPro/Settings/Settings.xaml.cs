using ArnoldVinkCode;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Windows;
using static AmbiPro.AppEnums;
using static AmbiPro.AppVariables;
using static AmbiPro.SerialMonitor;

namespace AmbiPro.Settings
{
    public partial class FormSettings : Window
    {
        //Form initialization
        public FormSettings() { InitializeComponent(); }

        //Open projects site
        private void Btn_Projects_Open_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start("https://projects.arnoldvink.com");
            }
            catch { }
        }

        //Open remote site
        private void Btn_Remote_Open_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start("https://ambipro.arnoldvink.com?ip=" + tb_RemoteIp.Text + "&port=" + tb_ServerPort.Text);
            }
            catch { }
        }

        //Browse debug image files
        private async void btn_BrowseDebugImages_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start("Debug");
            }
            catch
            {
                await new AVMessageBox().Popup(null, "Debug image folder not found.", "", "Ok", "", "", "");
            }
        }

        //Delete debug image files
        private async void btn_DeleteDebugImages_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Directory.Exists("Debug"))
                {
                    Directory.Delete("Debug", true);
                }

                await new AVMessageBox().Popup(null, "Debug image files have been deleted.", "", "Ok", "", "", "");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to delete debug bitmap images: " + ex.Message);
            }
        }

        //Handle switch button
        private async void Btn_SwitchLedsOnOrOff_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await LedSwitch(LedSwitches.Automatic);
            }
            catch { }
        }

        //Handle window initialized event
        protected override void OnSourceInitialized(EventArgs e)
        {
            try
            {
                //Load and save the settings
                SettingsLoad();
                SettingsSave();

                //Load and set the help text
                Load_Help_Text();

                //Set first launch setting to false
                SettingsFunction.Save("FirstLaunch2", "False");

                Debug.WriteLine("Settings window initialized.");
            }
            catch { }
        }

        //Handle window activated event
        protected override void OnActivated(EventArgs e)
        {
            try
            {
                //Fix trigger when resolution changed and led mode changed

                //Update the rotation ratio
                UpdateRotationRatio();

                //Check the maximum rotation count
                CheckMaximumRotationCount();

                //Load current device's ipv4 adres
                txt_Remote_IpAdres.Text = string.Empty;
                IPAddress[] ListIpAdresses = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
                if (ListIpAdresses.Any())
                {
                    foreach (IPAddress ip in ListIpAdresses)
                    {
                        if (ip.AddressFamily == AddressFamily.InterNetwork)
                        {
                            tb_RemoteIp.Text = ip.ToString();
                            if (ListIpAdresses.Count() == 1) { txt_Remote_IpAdres.Text = ip.ToString(); }
                            else if (ListIpAdresses.Count() > 1) { txt_Remote_IpAdres.Text = txt_Remote_IpAdres.Text + ", " + ip.ToString(); }
                        }
                    }
                    txt_Remote_IpAdres.Text = AVFunctions.StringReplaceFirst(txt_Remote_IpAdres.Text, ", ", "", false);
                }
                else
                {
                    txt_Remote_IpAdres.Text = "Not connected";
                }

                //Check current socket status
                if (!vArnoldVinkSockets.vTask_TcpReceiveLoop.TaskRunning)
                {
                    txt_RemoteErrorServerPort.Visibility = Visibility.Visible;
                }

                Debug.WriteLine("Settings window activated.");
            }
            catch { }
        }

        //Handle window closing event
        protected override void OnClosing(CancelEventArgs e)
        {
            try
            {
                e.Cancel = true;
                Debug.WriteLine("Settings window closing.");

                //Disable debug to save performance
                SettingsFunction.Save("DebugMode", "False");
                checkbox_DebugMode.IsChecked = false;

                //Hide the settings window
                this.Hide();
            }
            catch { }
        }
    }
}