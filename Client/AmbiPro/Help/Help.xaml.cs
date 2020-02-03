using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace AmbiPro.Help
{
    public partial class FormHelp : Window
    {
        public FormHelp()
        {
            try
            {
                InitializeComponent();
                Loaded += (sender, args) =>
                {
                    if (sp_Help.Children.Count == 0)
                    {
                        sp_Help.Children.Add(new TextBlock() { Text = "My leds do not always seem to update?", Style = (Style)App.Current.Resources["TextBlockBlack"] });
                        sp_Help.Children.Add(new TextBlock() { Text = "If a game does not update the leds try running the game in fullscreen windowed mode, administrator prompts and remote desktop connections may also keep the lights from updating while they are active.", Style = (Style)App.Current.Resources["TextBlockGrayLight"], TextWrapping = TextWrapping.Wrap });

                        sp_Help.Children.Add(new TextBlock() { Text = "\r\nSome video streams cause black leds?", Style = (Style)App.Current.Resources["TextBlockBlack"] });
                        sp_Help.Children.Add(new TextBlock() { Text = "DRM streams may not work and can turn your leds off because the video gets captured as a black screen due to the copy protection.", Style = (Style)App.Current.Resources["TextBlockGrayLight"], TextWrapping = TextWrapping.Wrap });

                        sp_Help.Children.Add(new TextBlock() { Text = "\r\nAmbiPro seems to impact my performance?", Style = (Style)App.Current.Resources["TextBlockBlack"] });
                        sp_Help.Children.Add(new TextBlock() { Text = "AmbiPro captures your screen and based on that it creates calculations to see which leds need to be displayed and due to this it is a pretty resourceful application, but still optimized for the best performance possible.", Style = (Style)App.Current.Resources["TextBlockGrayLight"], TextWrapping = TextWrapping.Wrap });

                        sp_Help.Children.Add(new TextBlock() { Text = "\r\nScreen capture is sometimes lagging behind", Style = (Style)App.Current.Resources["TextBlockBlack"] });
                        sp_Help.Children.Add(new TextBlock() { Text = "When your CPU or GPU usage if very high AmbiPro might not be able to keep up with the content displayed on screen, if this happens in a certain game you can try lowering the graphics settings a bit to improve the capture performance.", Style = (Style)App.Current.Resources["TextBlockGrayLight"], TextWrapping = TextWrapping.Wrap });

                        sp_Help.Children.Add(new TextBlock() { Text = "\r\nDo you recommend an Arduino script?", Style = (Style)App.Current.Resources["TextBlockBlack"] });
                        sp_Help.Children.Add(new TextBlock() { Text = "AmbiPro contains an Arduino compatible script that is optimized for usage with AmbiPro, it can be found in the applications installation directory in the 'Script' directory, the FastLED library is required to install this script on your Arduino board.", Style = (Style)App.Current.Resources["TextBlockGrayLight"], TextWrapping = TextWrapping.Wrap });

                        sp_Help.Children.Add(new TextBlock() { Text = "\r\nMy led strip only shows x amount of leds?", Style = (Style)App.Current.Resources["TextBlockBlack"] });
                        sp_Help.Children.Add(new TextBlock() { Text = "Depending on your Arduino board's memory (RAM) only a certain maximum amount of leds can be displayed due to the low memory limitations.", Style = (Style)App.Current.Resources["TextBlockGrayLight"], TextWrapping = TextWrapping.Wrap });

                        sp_Help.Children.Add(new TextBlock() { Text = "\r\nHow can I quickly turn the leds on or off?", Style = (Style)App.Current.Resources["TextBlockBlack"] });
                        sp_Help.Children.Add(new TextBlock() { Text = "You can click on the tray icon with your middle mouse button to quickly switch the leds on or off.", Style = (Style)App.Current.Resources["TextBlockGrayLight"], TextWrapping = TextWrapping.Wrap });

                        sp_Help.Children.Add(new TextBlock() { Text = "\r\nWhere can I find the required drivers?", Style = (Style)App.Current.Resources["TextBlockBlack"] });
                        sp_Help.Children.Add(new TextBlock() { Text = "If you are an Arduino user you can install the drivers by installing the Arduino IDE software found on the official website: http://arduino.cc", Style = (Style)App.Current.Resources["TextBlockGrayLight"], TextWrapping = TextWrapping.Wrap });

                        sp_Help.Children.Add(new TextBlock() { Text = "\r\nSupport and bug reporting", Style = (Style)App.Current.Resources["TextBlockBlack"] });
                        sp_Help.Children.Add(new TextBlock() { Text = "When you are walking into any problems or a bug you can go to my help page at https://help.arnoldvink.com so I can try to help you out and get everything working.", Style = (Style)App.Current.Resources["TextBlockGrayLight"], TextWrapping = TextWrapping.Wrap });

                        sp_Help.Children.Add(new TextBlock() { Text = "\r\nDeveloper donation", Style = (Style)App.Current.Resources["TextBlockBlack"] });
                        sp_Help.Children.Add(new TextBlock() { Text = "If you appreciate my project and want to support me with my projects you can make a donation through https://donation.arnoldvink.com", Style = (Style)App.Current.Resources["TextBlockGrayLight"], TextWrapping = TextWrapping.Wrap });

                        //Set the version text
                        sp_Help.Children.Add(new TextBlock() { Text = "\r\nApplication made by Arnold Vink", Style = (Style)App.Current.Resources["TextBlockBlack"] });
                        sp_Help.Children.Add(new TextBlock() { Text = "Version: v" + Assembly.GetEntryAssembly().FullName.Split('=')[1].Split(',')[0], Style = (Style)App.Current.Resources["TextBlockGrayLight"], TextWrapping = TextWrapping.Wrap });
                    }
                };
            }
            catch { }
        }

        //Open the web browser
        void btn_Help_ProjectWebsite_Click(object sender, RoutedEventArgs e) { Process.Start("http://projects.arnoldvink.com"); }
        void btn_Help_OpenDonation_Click(object sender, RoutedEventArgs e) { Process.Start("http://donation.arnoldvink.com"); }

        //Handle window closing event
        protected override void OnClosing(CancelEventArgs e)
        {
            try
            {
                e.Cancel = true;
                Debug.WriteLine("Closing the help window.");

                this.Hide();
            }
            catch { }
        }
    }
}