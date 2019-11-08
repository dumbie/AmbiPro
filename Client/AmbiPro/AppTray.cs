using AmbiPro.Settings;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using static AmbiPro.SerialMonitor;

namespace AmbiPro
{
    class AppTray
    {
        //Application Variables
        public static NotifyIcon NotifyIcon = new NotifyIcon();
        private static ContextMenu ContextMenu = new ContextMenu();

        //Create the tray menu
        public static void CreateTrayMenu()
        {
            try
            {
                Debug.WriteLine("Creating application tray menu...");

                //Create sub menus
                MenuItem MenuModes = new MenuItem("Led mode");
                MenuModes.MenuItems.Add(new MenuItem("Screen capture", OnChangeMode));
                MenuModes.MenuItems.Add(new MenuItem("Solid color", OnChangeMode));
                MenuModes.MenuItems.Add(new MenuItem("Colors loop", OnChangeMode));
                MenuModes.MenuItems.Add(new MenuItem("Color spectrum", OnChangeMode));

                //Create a context menu for systray.
                ContextMenu.MenuItems.Add("On/Off", OnSwitchOnOff);
                ContextMenu.MenuItems.Add("Settings", OnSettings);
                ContextMenu.MenuItems.Add(MenuModes);
                ContextMenu.MenuItems.Add("Calibrate", OnCalibrate);
                ContextMenu.MenuItems.Add("Website", OnWebsite);
                ContextMenu.MenuItems.Add("Help", OnHelp);
                ContextMenu.MenuItems.Add("Exit", OnExit);

                //Initialize the tray notify icon. 
                NotifyIcon.Text = "AmbiPro";
                NotifyIcon.Icon = new Icon(Assembly.GetEntryAssembly().GetManifestResourceStream("AmbiPro.Assets.ApplicationIcon.ico"));

                // Handle Double Click event
                NotifyIcon.DoubleClick += new EventHandler(NotifyIcon_DoubleClick);

                // Handle Middle Click event
                NotifyIcon.MouseClick += new MouseEventHandler(NotifyIcon_MiddleClick);

                // Add menu to tray icon and show it.  
                NotifyIcon.ContextMenu = ContextMenu;
                NotifyIcon.Visible = true;
            }
            catch { }
        }

        private static void NotifyIcon_DoubleClick(object Sender, EventArgs e)
        {
            try
            {
                App.vFormSettings.Show();
            }
            catch { }
        }

        private static async void NotifyIcon_MiddleClick(object Sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Middle)
                {
                    await LedSwitch(LedSwitches.Automatic);
                }
            }
            catch { }
        }

        private static async void OnSwitchOnOff(object sender, EventArgs e)
        {
            try
            {
                await LedSwitch(LedSwitches.Automatic);
            }
            catch { }
        }

        private static void OnSettings(object sender, EventArgs e)
        {
            try
            {
                App.vFormSettings.Show();
            }
            catch { }
        }

        private static async void OnChangeMode(object sender, EventArgs e)
        {
            try
            {
                MenuItem ClickMenuItem = (sender as MenuItem);
                if (ClickMenuItem.Text == "Screen capture") { SettingsFunction.Save("LedMode", "0"); }
                else if (ClickMenuItem.Text == "Solid color") { SettingsFunction.Save("LedMode", "1"); }
                else if (ClickMenuItem.Text == "Colors loop") { SettingsFunction.Save("LedMode", "2"); }
                else if (ClickMenuItem.Text == "Color spectrum") { SettingsFunction.Save("LedMode", "3"); }
                await LedSwitch(LedSwitches.Restart);
            }
            catch { }
        }

        private static void OnCalibrate(object sender, EventArgs e)
        {
            try
            {
                App.vFormCalibrate.Show();
            }
            catch { }
        }

        private static void OnWebsite(object sender, EventArgs e)
        {
            try
            {
                Process.Start("https://projects.arnoldvink.com");
            }
            catch { }
        }
        private static void OnHelp(object Sender, EventArgs e)
        {
            try
            {
                App.vFormHelp.Show();
            }
            catch { }
        }

        private static async void OnExit(object sender, EventArgs e)
        {
            try
            {
                await AppStartup.Application_Exit();
            }
            catch { }
        }
    }
}