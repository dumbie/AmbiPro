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

        //Setting Variables
        public static string vCurrentRatio = string.Empty;
        public static int vCurrentRotation = 0;
        public static int vCurrentBlackbar = 0;

        //Color loop Variables
        public static Color vCurrentLoopColor = Color.FromArgb(20, 0, 0);

        //Capture Variables
        public static int vCaptureZoneRange = 0;

        //Blackbar Variables
        public const int vMarginBlackAccuracy = 4;
        public const int vMarginMinimumOffset = 2;
        public static int vMarginBlackLastUpdate = 0;
        public static int vMarginTop = vMarginMinimumOffset;
        public static int vMarginBottom = vMarginMinimumOffset;
        public static int vMarginLeft = vMarginMinimumOffset;
        public static int vMarginRight = vMarginMinimumOffset;

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