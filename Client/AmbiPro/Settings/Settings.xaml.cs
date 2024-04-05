using ArnoldVinkCode;
using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using static AmbiPro.AppEnums;
using static AmbiPro.AppVariables;
using static AmbiPro.SerialMonitor;
using static ArnoldVinkCode.AVFunctions;
using static ArnoldVinkCode.AVSettings;

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
                OpenWebsiteBrowser("http://ambipro.arnoldvink.com?ip=" + tb_RemoteIp.Text + "&port=" + tb_ServerPort.Text);
            }
            catch { }
        }

        //Handle move window
        private Point MoveWindowPointBegin;
        private void btn_MoveWindow_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    MoveWindowPointBegin = Mouse.GetPosition(grid_MainWindow);
                }
            }
            catch { }
        }
        private void btn_MoveWindow_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    Point mouseOffset = Mouse.GetPosition(grid_MainWindow);
                    double marginLeft = mouseOffset.X + grid_MainWindow.Margin.Left - MoveWindowPointBegin.X;
                    double marginTop = mouseOffset.Y + grid_MainWindow.Margin.Top - MoveWindowPointBegin.Y;
                    grid_MainWindow.Margin = new Thickness(marginLeft, marginTop, 0, 0);
                }
            }
            catch { }
        }

        //Handle exit button
        private async void button_ExitApplication_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await AppStartup.Exit();
            }
            catch { }
        }

        //Handle close button
        private void btn_CloseWindow_Click(object sender, RoutedEventArgs e)
        {
            try
            {
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
        protected override async void OnSourceInitialized(EventArgs e)
        {
            try
            {
                //Load and save the settings
                await SettingsLoad();
                SettingsSave();

                //Load and set the help text
                Load_Help_Text();

                //Check if resolution has changed
                SystemEvents.DisplaySettingsChanged += SystemEvents_DisplaySettingsChanged;

                //Check if solid led color has changed
                AppEvents.EventUpdateSettingsSolidLedColor += UpdateSettingsSolidLedColor;

                Debug.WriteLine("Settings window initialized.");
            }
            catch { }
        }

        //Update solid led color
        void UpdateSettingsSolidLedColor()
        {
            try
            {
                Debug.WriteLine("Updating solid led color in interface.");
                AVActions.DispatcherInvoke(delegate
                {
                    string solidLedColor = SettingLoad(vConfiguration, "SolidLedColor", typeof(string));
                    button_ColorPickerSolid.Background = new BrushConverter().ConvertFrom(solidLedColor) as SolidColorBrush;
                });
            }
            catch { }
        }

        //Update settings information
        public async void SystemEvents_DisplaySettingsChanged(object sender, EventArgs e)
        {
            try
            {
                Debug.WriteLine("Resolution has been changed.");

                //Reset main window margin
                grid_MainWindow.Margin = new Thickness(0, 0, 0, 0);

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
                    await Task.Delay(1000);
                }

                //Update screen information
                UpdateScreenInformation();

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

                //Allow debug capture
                if (vCurrentVisibleMenu == "menuButtonDebug")
                {
                    vDebugCaptureAllowed = true;
                }

                Debug.WriteLine("Settings window activated.");
            }
            catch { }
        }

        protected override void OnStateChanged(EventArgs e)
        {
            try
            {
                Debug.WriteLine("Settings window state changed.");

                if (WindowState != WindowState.Maximized)
                {
                    //Disable debug capture
                    vDebugCaptureAllowed = false;
                    image_DebugPreview.Source = null;
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
                Debug.WriteLine("Settings window closing.");

                //Disable debug capture
                vDebugCaptureAllowed = false;
                image_DebugPreview.Source = null;

                //Hide the settings window
                this.Hide();
            }
            catch { }
        }
    }
}