using System;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using static AmbiPro.AppEnums;
using static AmbiPro.AppVariables;
using static AmbiPro.SerialMonitor;
using static ArnoldVinkCode.AVFunctions;
using static ArnoldVinkCode.AVSettings;

namespace AmbiPro
{
    class AppTray
    {
        //Application Variables
        public static NotifyIcon NotifyIcon = new NotifyIcon();
        private static ContextMenuStrip ContextMenu = new ContextMenuStrip();

        //Create the tray menu
        public static void CreateTrayMenu()
        {
            try
            {
                Debug.WriteLine("Creating application tray menu...");

                //Create sub menus
                ToolStripMenuItem MenuModes = new ToolStripMenuItem("Led mode");
                MenuModes.DropDownItems.Add("Screen capture", null, OnChangeMode);
                MenuModes.DropDownItems.Add("Color solid", null, OnChangeMode);
                MenuModes.DropDownItems.Add("Color loop", null, OnChangeMode);
                MenuModes.DropDownItems.Add("Color spectrum", null, OnChangeMode);

                //Create a context menu for systray
                ContextMenu.Items.Add("On/Off", null, OnSwitchOnOff);
                ContextMenu.Items.Add(MenuModes);
                ContextMenu.Items.Add("Settings", null, OnSettings);
                ContextMenu.Items.Add("Website", null, OnWebsite);
                ContextMenu.Items.Add("Exit", null, OnExit);

                //Initialize the tray notify icon. 
                NotifyIcon.Text = "AmbiPro";
                NotifyIcon.Icon = new Icon(Assembly.GetEntryAssembly().GetManifestResourceStream("AmbiPro.Assets.ApplicationIcon-Disabled.ico"));

                //Handle Double Click event
                NotifyIcon.DoubleClick += NotifyIcon_DoubleClick;

                //Handle Middle Click event
                NotifyIcon.MouseClick += NotifyIcon_MiddleClick;

                //Add menu to tray icon and show it
                NotifyIcon.ContextMenuStrip = ContextMenu;
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
                ToolStripMenuItem clickedMenuItem = (sender as ToolStripMenuItem);
                if (clickedMenuItem.Text == "Screen capture") { SettingSave(vConfiguration, "LedMode", "0"); }
                else if (clickedMenuItem.Text == "Color solid") { SettingSave(vConfiguration, "LedMode", "1"); }
                else if (clickedMenuItem.Text == "Color loop") { SettingSave(vConfiguration, "LedMode", "2"); }
                else if (clickedMenuItem.Text == "Color spectrum") { SettingSave(vConfiguration, "LedMode", "3"); }
                await LedSwitch(LedSwitches.Restart);
            }
            catch { }
        }

        private static void OnWebsite(object sender, EventArgs e)
        {
            try
            {
                OpenWebsiteBrowser("https://projects.arnoldvink.com");
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