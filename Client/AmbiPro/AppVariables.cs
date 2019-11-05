﻿using ArnoldVinkCode;
using System.Configuration;
using System.Globalization;

namespace AmbiPro
{
    partial class AppVariables
    {
        //Application Variables
        public static Configuration vConfiguration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        public static CultureInfo vAppCultureInfo = CultureInfo.InvariantCulture;

        //Update Variables
        public static bool vCheckingForUpdate = false;

        //Socket Variables
        public static ArnoldVinkSocketServer vSocketServer = new ArnoldVinkSocketServer();
    }
}