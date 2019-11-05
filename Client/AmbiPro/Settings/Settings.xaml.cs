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

namespace AmbiPro.Settings
{
    public partial class FormSettings : Window
    {
        //Application Variables
        private static Configuration vConfiguration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

        //Form initialization
        public FormSettings()
        {
            try
            {
                InitializeComponent();
                Loaded += (sender, args) =>
                {
                    //Check connected com ports
                    foreach (string PortName in SerialPort.GetPortNames())
                    {
                        Int32 PortNumberRaw = Convert.ToInt32(PortName.Replace("COM", "")) - 1;
                        cb_ComPort.Items[PortNumberRaw] = PortName + " (Connected)";
                    }

                    //Load and save the settings
                    SettingsLoad();
                    SettingsSave();

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
                        btn_SwitchLedsOnorOff.Visibility = Visibility.Collapsed;

                        //Set the first connected device
                        foreach (string PortName in SerialPort.GetPortNames())
                        {
                            int PortNumberRaw = Convert.ToInt32(PortName.Replace("COM", "")) - 1;
                            cb_ComPort.SelectedIndex = PortNumberRaw;
                        }
                    }

                    //Load current device's ipv4 adres
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
                    if (!vSocketServer.vIsServerRunning()) { txt_RemoteErrorServerPort.Visibility = Visibility.Visible; }
                };
            }
            catch { }
        }

        //Open remote site
        private void btn_Remote_Open_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start("https://ambipro.arnoldvink.com?ip=" + tb_RemoteIp.Text + "&port=" + tb_ServerPort.Text);
            }
            catch { }
        }

        //Handle start button
        private void btn_WelcomeStart_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Close();
            }
            catch { }
        }

        //Handle switch button
        private async void btn_SwitchLedsOnorOff_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await SerialMonitor.LedSwitch();
            }
            catch { }
        }

        //Check the first launch setting
        private static void WelcomeCheck()
        {
            try
            {
                if (Convert.ToBoolean(ConfigurationManager.AppSettings["FirstLaunch"]))
                {
                    //Set first launch setting to false
                    SettingsFunction.Save("FirstLaunch", "False");

                    //Start updating the leds
                    SerialMonitor.LedsEnable();
                }
            }
            catch { }
        }

        //Handle window closing event
        protected override void OnClosing(CancelEventArgs e)
        {
            try
            {
                e.Cancel = true;
                Debug.WriteLine("Closing the settings window.");

                WelcomeCheck();

                this.Hide();
            }
            catch { }
        }
    }
}