using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace AmbiProRemote
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            this.Loaded += MainPage_Loaded;
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            try 
            { 
                ColorPickerLoad();
                //Fix: Check if server ip and port are set open settings otherwise
            }
            catch { }
        }

        void ClosePopups()
        {
            try
            {
                popup_ColorPicker.Visibility = Visibility.Collapsed;
                popup_Help.Visibility = Visibility.Collapsed;
                popup_Settings.Visibility = Visibility.Collapsed;
            }
            catch { }
        }

        private void Button_ClosePopups_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ClosePopups();
            } 
            catch { }
        }

        private async void Button_LedsOnOffSwitch(object sender, RoutedEventArgs e)
        {
            try
            {
                await SocketSend.SocketSendAmbiPro("LedSwitch");
            }
            catch { }
        }

        private void Button_ShowColorPicker_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                popup_ColorPicker.Visibility = Visibility.Visible;
            }
            catch { }
        }

        private void Button_ShowHelp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                popup_Help.Visibility = Visibility.Visible;
            }
            catch { }
        }

        private void Button_ShowSettings_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                popup_Settings.Visibility = Visibility.Visible;
            }
            catch { }
        }
    }
}