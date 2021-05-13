using ArnoldVinkCode;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
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

        //Open remote site
        private void Btn_Remote_Open_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start("http://ambipro.arnoldvink.com?ip=" + tb_RemoteIp.Text + "&port=" + tb_ServerPort.Text);
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
                List<string> MsgBoxAnswers = new List<string>();
                MsgBoxAnswers.Add("Ok");

                await new AVMessageBox().Popup(null, "Debug image folder not found.", "", MsgBoxAnswers);
            }
        }

        //Delete debug image files
        private async void btn_DeleteDebugImages_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DirectoryInfo dirInfo = new DirectoryInfo("Debug");
                foreach (FileInfo fileInfo in dirInfo.GetFiles())
                {
                    fileInfo.Delete();
                }

                List<string> MsgBoxAnswers = new List<string>();
                MsgBoxAnswers.Add("Ok");

                await new AVMessageBox().Popup(null, "Debug image files have been deleted.", "", MsgBoxAnswers);
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

                //Check if resolution has changed
                SystemEvents.DisplaySettingsChanged += SystemEvents_DisplaySettingsChanged;

                //Set first launch setting to false
                SettingsFunction.Save("FirstLaunch2", "False");

                Debug.WriteLine("Settings window initialized.");
            }
            catch { }
        }

        //Update settings information
        public async void SystemEvents_DisplaySettingsChanged(object sender, EventArgs e)
        {
            try
            {
                Debug.WriteLine("Resolution has been changed.");

                //Update settings information
                await UpdateSettingsInformation(true);
            }
            catch { }
        }

        //Update settings information
        public async Task UpdateSettingsInformation(bool delayed)
        {
            try
            {
                //Wait for resolution change
                if (delayed)
                {
                    await Task.Delay(2000);
                }

                //Update the rotation ratio
                UpdateRotationRatio();

                //Check the maximum rotation count
                CheckMaximumRotationCount();

                //Load current device's ipv4 adres
                tb_RemoteIp.Text = string.Empty;
                txt_Remote_IpAdres.Text = string.Empty;
                IPAddress[] ListIpAdresses = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
                if (ListIpAdresses.Any())
                {
                    foreach (IPAddress ip in ListIpAdresses)
                    {
                        if (ip.AddressFamily == AddressFamily.InterNetwork)
                        {
                            string ipAddress = ip.ToString();
                            if (string.IsNullOrWhiteSpace(tb_RemoteIp.Text)) { tb_RemoteIp.Text = ipAddress; }
                            if (ListIpAdresses.Count() == 1)
                            {
                                txt_Remote_IpAdres.Text = ipAddress;
                            }
                            else
                            {
                                txt_Remote_IpAdres.Text = txt_Remote_IpAdres.Text + ", " + ipAddress;
                            }
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
            }
            catch { }
        }

        //Handle window activated event
        protected override async void OnActivated(EventArgs e)
        {
            try
            {
                //Update settings information
                await UpdateSettingsInformation(false);

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
                image_DebugPreview.Source = null;

                //Hide the settings window
                this.Hide();
            }
            catch { }
        }
    }
}