﻿using System;
using System.Diagnostics;
using System.Windows.Threading;

namespace AmbiPro
{
    class AppTimers
    {
        //Application Timers
        public static DispatcherTimer vDispatcherTimer_UpdateSettings = new DispatcherTimer();

        //Register Application Timers
        public static void ApplicationTimersRegister()
        {
            try
            {
                Debug.WriteLine("Registering application timers...");

                //Create timer to update the led settings
                vDispatcherTimer_UpdateSettings.Interval = TimeSpan.FromSeconds(1);
                vDispatcherTimer_UpdateSettings.Tick += delegate { SerialMonitor.UpdateSettings(); };
                vDispatcherTimer_UpdateSettings.Start();
            }
            catch { }
        }

        //Disable Application Timers
        public static void ApplicationTimersDisable()
        {
            try
            {
                Debug.WriteLine("Disabling application timers...");

                vDispatcherTimer_UpdateSettings.Stop();
            }
            catch { }
        }
    }
}