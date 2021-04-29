using ArnoldVinkCode;
using System.Configuration;
using System.Drawing;
using System.Globalization;

namespace AmbiPro
{
    partial class AppVariables
    {
        //Application Variables
        public static Configuration vConfiguration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        public static CultureInfo vAppCultureInfo = CultureInfo.InvariantCulture;

        //Color loop Variables
        public static Color vCurrentLoopColor = Color.FromArgb(20, 0, 0);

        //Capture Variables
        public static int vCaptureZoneRange = 0;
        public const int vMarginMinimumOffset = 2;
        public static int vMarginTop = 2;
        public static int vMarginBottom = 2;
        public static int vMarginLeft = 2;
        public static int vMarginRight = 2;

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