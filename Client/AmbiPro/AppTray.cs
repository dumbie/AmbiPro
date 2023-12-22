using System;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using static AmbiPro.AppEnums;
using static AmbiPro.AppVariables;
using static AmbiPro.SerialMonitor;
using static ArnoldVinkCode.AVSettings;

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
                MenuModes.MenuItems.Add(new MenuItem("Color solid", OnChangeMode));
                MenuModes.MenuItems.Add(new MenuItem("Color loop", OnChangeMode));
                MenuModes.MenuItems.Add(new MenuItem("Color spectrum", OnChangeMode));

                //Create a context menu for systray.
                ContextMenu.MenuItems.Add("On/Off", OnSwitchOnOff);
                ContextMenu.MenuItems.Add(MenuModes);
                ContextMenu.MenuItems.Add("Settings", OnSettings);
                ContextMenu.MenuItems.Add("Website", OnWebsite);
                ContextMenu.MenuItems.Add("Exit", OnExit);

                //Initialize the tray notify icon. 
                NotifyIcon.Text = "AmbiPro";
                NotifyIcon.Icon = new Icon(Assembly.GetEntryAssembly().GetManifestResourceStream("AmbiPro.Assets.ApplicationIcon-Disabled.ico"));

                // Handle Double Click event
                NotifyIcon.DoubleClick += NotifyIcon_DoubleClick;

                // Handle Middle Click event
                NotifyIcon.MouseClick += NotifyIcon_MiddleClick;

                // Add menu to tray icon and show it.  
                NotifyIcon.ContextMenu = ContextMenu;
                NotifyIcon.Visible = true;
            }
            catch { }
        }

        private static void NotifyIcon_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                vFormSettings.Show();
            }
            catch { }
        }

        private static async void NotifyIcon_MiddleClick(object sender, MouseEventArgs e)
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
                vFormSettings.Show();
            }
            catch { }
        }

        private static async void OnChangeMode(object sender, EventArgs e)
        {
            try
            {
                MenuItem ClickMenuItem = (sender as MenuItem);
                if (ClickMenuItem.Text == "Screen capture") { SettingSave(vConfiguration, "LedMode", "0"); }
                else if (ClickMenuItem.Text == "Color solid") { SettingSave(vConfiguration, "LedMode", "1"); }
                else if (ClickMenuItem.Text == "Color loop") { SettingSave(vConfiguration, "LedMode", "2"); }
                else if (ClickMenuItem.Text == "Color spectrum") { SettingSave(vConfiguration, "LedMode", "3"); }
                await LedSwitch(LedSwitches.Restart);
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

        private static async void OnExit(object sender, EventArgs e)
        {
            try
            {
                await AppStartup.Exit();
            }
            catch { }
        }
    }
}