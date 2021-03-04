using ArnoldVinkCode;
using System.Configuration;
using System.Globalization;

namespace AmbiPro
{
    partial class AppVariables
    {
        //Application Variables
        public static Configuration vConfiguration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        public static CultureInfo vAppCultureInfo = CultureInfo.InvariantCulture;

        //Capture Variables
        public static int vCaptureZoneMargin = 2;
        public static int vCaptureZoneSize = 2;

        //Screen Variables
        public static int vScreenWidth = 0;
        public static int vScreenHeight = 0;
        public static int vScreenOutputSize = 0;

        //Update Variables
        public static bool vCheckingForUpdate = false;

        //Sockets Variables
        public static ArnoldVinkSockets vArnoldVinkSockets = null;
    }
}