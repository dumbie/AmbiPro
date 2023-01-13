using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace AmbiPro.Settings
{
    partial class FormSettings
    {
        //Open the web browser
        void btn_Help_ProjectWebsite_Click(object sender, RoutedEventArgs e) { Process.Start("https://projects.arnoldvink.com"); }
        void btn_Help_OpenDonation_Click(object sender, RoutedEventArgs e) { Process.Start("https://donation.arnoldvink.com"); }

        //Load and set the help text
        void Load_Help_Text()
        {
            try
            {
                Debug.WriteLine("Loading application help text: " + sp_Help_Text.Children.Count);
                if (sp_Help_Text.Children.Count == 0)
                {
                    sp_Help_Text.Children.Add(new TextBlock() { Text = "My leds do not always seem to update?", Style = (Style)App.Current.Resources["TextBlockBlack"] });
                    sp_Help_Text.Children.Add(new TextBlock() { Text = "If a game does not update the leds try running the game in fullscreen windowed or borderless mode, administrator prompts and remote desktop connections may also keep the lights from updating.", Style = (Style)App.Current.Resources["TextBlockGrayLight"], TextWrapping = TextWrapping.Wrap });

                    sp_Help_Text.Children.Add(new TextBlock() { Text = "\r\nSome video streams cause leds to turn off?", Style = (Style)App.Current.Resources["TextBlockBlack"] });
                    sp_Help_Text.Children.Add(new TextBlock() { Text = "Streams with copy protection (DRM) can turn your leds off because the video gets captured as a black screen, as a workaround you can use an alternative video player like Kodi, or in Chromium based browsers you can change the angle graphics backend to DirectX 9, you can do this by typing 'chrome://flags/#use-angle' in your address bar and set the value to D3D9.", Style = (Style)App.Current.Resources["TextBlockGrayLight"], TextWrapping = TextWrapping.Wrap });

                    sp_Help_Text.Children.Add(new TextBlock() { Text = "\r\nHDR content colors seems to be washed out?", Style = (Style)App.Current.Resources["TextBlockBlack"] });
                    sp_Help_Text.Children.Add(new TextBlock() { Text = "When you are viewing HDR content make sure that you enable HDR in the Windows display settings, when this setting is disabled AmbiPro is not able to capture all the colors of HDR content making the leds look dull, for example orange might be shown more as yellow.", Style = (Style)App.Current.Resources["TextBlockGrayLight"], TextWrapping = TextWrapping.Wrap });

                    sp_Help_Text.Children.Add(new TextBlock() { Text = "\r\nAmbiPro seems to impact my game performance?", Style = (Style)App.Current.Resources["TextBlockBlack"] });
                    sp_Help_Text.Children.Add(new TextBlock() { Text = "AmbiPro captures your screen and based on the screenshot it calculates which colors need to be displayed on the leds, due to this it is a pretty resourceful application.", Style = (Style)App.Current.Resources["TextBlockGrayLight"], TextWrapping = TextWrapping.Wrap });

                    sp_Help_Text.Children.Add(new TextBlock() { Text = "\r\nScreen capture is sometimes lagging behind", Style = (Style)App.Current.Resources["TextBlockBlack"] });
                    sp_Help_Text.Children.Add(new TextBlock() { Text = "When your CPU or GPU usage if very high AmbiPro might not be able to keep up with the content displayed on screen, if this happens in a certain game you can try lowering the graphics settings a bit to improve the capture performance, some games may cause capture lag when VSync is turned off.", Style = (Style)App.Current.Resources["TextBlockGrayLight"], TextWrapping = TextWrapping.Wrap });

                    sp_Help_Text.Children.Add(new TextBlock() { Text = "\r\nDo you recommend an Arduino script?", Style = (Style)App.Current.Resources["TextBlockBlack"] });
                    sp_Help_Text.Children.Add(new TextBlock() { Text = "AmbiPro contains an Arduino compatible script that is optimized for usage with AmbiPro, it can be found in the applications installation directory in the 'Script' directory, the FastLED library is required to install this script on your Arduino board.", Style = (Style)App.Current.Resources["TextBlockGrayLight"], TextWrapping = TextWrapping.Wrap });

                    sp_Help_Text.Children.Add(new TextBlock() { Text = "\r\nMy led strip only shows x amount of leds?", Style = (Style)App.Current.Resources["TextBlockBlack"] });
                    sp_Help_Text.Children.Add(new TextBlock() { Text = "Depending on your Arduino board's memory (RAM) only a certain maximum amount of leds can be displayed due to the low memory limitations.", Style = (Style)App.Current.Resources["TextBlockGrayLight"], TextWrapping = TextWrapping.Wrap });

                    sp_Help_Text.Children.Add(new TextBlock() { Text = "\r\nHow can I quickly turn the leds on or off?", Style = (Style)App.Current.Resources["TextBlockBlack"] });
                    sp_Help_Text.Children.Add(new TextBlock() { Text = "You can click on the tray icon with your middle mouse button to quickly switch the leds on or off.", Style = (Style)App.Current.Resources["TextBlockGrayLight"], TextWrapping = TextWrapping.Wrap });

                    sp_Help_Text.Children.Add(new TextBlock() { Text = "\r\nWhere can I find the required drivers?", Style = (Style)App.Current.Resources["TextBlockBlack"] });
                    sp_Help_Text.Children.Add(new TextBlock() { Text = "If you are an Arduino user you can install the drivers by installing the Arduino IDE software found on the official website: https://arduino.cc", Style = (Style)App.Current.Resources["TextBlockGrayLight"], TextWrapping = TextWrapping.Wrap });

                    sp_Help_Text.Children.Add(new TextBlock() { Text = "\r\nSupport and bug reporting", Style = (Style)App.Current.Resources["TextBlockBlack"] });
                    sp_Help_Text.Children.Add(new TextBlock() { Text = "When you are walking into any problems or a bug you can go to my help page at https://help.arnoldvink.com so I can try to help you out and get everything working.", Style = (Style)App.Current.Resources["TextBlockGrayLight"], TextWrapping = TextWrapping.Wrap });

                    sp_Help_Text.Children.Add(new TextBlock() { Text = "\r\nDeveloper donation", Style = (Style)App.Current.Resources["TextBlockBlack"] });
                    sp_Help_Text.Children.Add(new TextBlock() { Text = "If you appreciate my project and want to support me with my projects you can make a donation through https://donation.arnoldvink.com", Style = (Style)App.Current.Resources["TextBlockGrayLight"], TextWrapping = TextWrapping.Wrap });

                    //Set the version text
                    sp_Help_Text.Children.Add(new TextBlock() { Text = "\r\nApplication made by Arnold Vink", Style = (Style)App.Current.Resources["TextBlockBlack"] });
                    sp_Help_Text.Children.Add(new TextBlock() { Text = "Version: v" + Assembly.GetEntryAssembly().FullName.Split('=')[1].Split(',')[0], Style = (Style)App.Current.Resources["TextBlockGrayLight"], TextWrapping = TextWrapping.Wrap });
                }
            }
            catch { }
        }
    }
}