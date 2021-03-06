﻿using System;
using System.Configuration;

namespace AmbiPro.Settings
{
    class SettingsFunction
    {
        //Application Variables
        private static Configuration vConfiguration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

        //Check - Application Setting
        public static bool Check(string Name)
        {
            try
            {
                if (ConfigurationManager.AppSettings[Name] == null) { return false; } else { return true; }
            }
            catch { return false; }
        }

        //Load - Application Setting
        //SettingsFunction.Load("Name", typeof(Type));
        public static dynamic Load(string Name, Type Type)
        {
            try
            {
                return Convert.ChangeType(ConfigurationManager.AppSettings[Name], Type);
            }
            catch { return null; }
        }

        //Save - Application Setting
        public static void Save(string Name, string Value)
        {
            try
            {
                vConfiguration.AppSettings.Settings.Remove(Name);
                vConfiguration.AppSettings.Settings.Add(Name, Value);
                vConfiguration.Save();
                ConfigurationManager.RefreshSection("appSettings");
            }
            catch { }
        }
    }
}