using ArnoldVinkCode;
using System;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Windows;
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

        //Handle start button
        private async void Btn_WelcomeStart_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Set first launch setting to false
                SettingsFunction.Save("FirstLaunch", "False");

                //Start updating the leds
                await LedSwitch(LedSwitches.Automatic);

                //Close the settings window
                this.Close();
            }
            catch { }
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
                //Check connected com ports
                foreach (string PortName in SerialPort.GetPortNames())
                {
                    int PortNumberRaw = Convert.ToInt32(PortName.Replace("COM", "")) - 1;
                    cb_ComPort.Items[PortNumberRaw] = PortName + " (Connected)";
                }

                //Load and save the settings
                SettingsLoad();
                SettingsSave();

                Debug.WriteLine("Settings window initialized.");
            }
            catch { }
        }

        //Handle window activated event
        protected override void OnActivated(EventArgs e)
        {
            try
            {
                //Check for first launch
                if (Convert.ToBoolean(ConfigurationManager.AppSettings["FirstLaunch"]))
                {
                    txt_Welcome.Visibility = Visibility.Visible;
                    btn_Welcome_Start1.Visibility = Visibility.Visible;
                    btn_Welcome_Start2.Visibility = Visibility.Visible;
                    btn_Welcome_Start3.Visibility = Visibility.Visible;
                    btn_Welcome_Start4.Visibility = Visibility.Visible;
                    menuButtonModes.Visibility = Visibility.Collapsed;
                    menuButtonCalibrate.Visibility = Visibility.Collapsed;
                    menuButtonRemote.Visibility = Visibility.Collapsed;
                    menuButtonUpdate.Visibility = Visibility.Collapsed;
                    menuButtonHelp.Visibility = Visibility.Collapsed;
                    btn_SwitchLedsOnOrOff.Visibility = Visibility.Collapsed;

                    //Set the first connected device
                    foreach (string PortName in SerialPort.GetPortNames())
                    {
                        int PortNumberRaw = Convert.ToInt32(PortName.Replace("COM", "")) - 1;
                        cb_ComPort.SelectedIndex = PortNumberRaw;
                    }
                }
                else
                {
                    txt_Welcome.Visibility = Visibility.Collapsed;
                    btn_Welcome_Start1.Visibility = Visibility.Collapsed;
                    btn_Welcome_Start2.Visibility = Visibility.Collapsed;
                    btn_Welcome_Start3.Visibility = Visibility.Collapsed;
                    btn_Welcome_Start4.Visibility = Visibility.Collapsed;
                    menuButtonModes.Visibility = Visibility.Visible;
                    menuButtonCalibrate.Visibility = Visibility.Visible;
                    menuButtonRemote.Visibility = Visibility.Visible;
                    menuButtonUpdate.Visibility = Visibility.Visible;
                    menuButtonHelp.Visibility = Visibility.Visible;
                    btn_SwitchLedsOnOrOff.Visibility = Visibility.Visible;
                }

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
                if (!vArnoldVinkSockets.vTask_SocketServer.TaskRunning)
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
                this.Hide();
            }
            catch { }
        }
    }
}