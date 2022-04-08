using ArnoldVinkCode;
using System.Configuration;
using System.Globalization;
using static AmbiPro.AppClasses;

namespace AmbiPro
{
    partial class AppVariables
    {
        //Application Variables
        public static Configuration vConfiguration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        public static CultureInfo vAppCultureInfo = CultureInfo.InvariantCulture;

        //Setting Variables
        public static string vCurrentVisibleMenu = string.Empty;
        public static string vCurrentRatio = string.Empty;
        public static int vCurrentRotation = 0;
        public static int vCurrentBlackbar = 0;
        public static int vCurrentBackground = 0;
        public static int vCurrentSolidColor = 0;

        //Color loop Variables
        public static ColorRGBA vCurrentLoopColor = new ColorRGBA() { R = 20 };

        //Blackbar Variables
        public const int vMarginBlackAccuracy = 4;
        public const int vMarginMinimumOffset = 2;
        public static int vMarginBlackLastUpdate = 0;
        public static int vMarginTop = vMarginMinimumOffset;
        public static int vMarginBottom = vMarginMinimumOffset;
        public static int vMarginLeft = vMarginMinimumOffset;
        public static int vMarginRight = vMarginMinimumOffset;
        public static int vBlackBarStepHorizontal = 0;
        public static int vBlackBarStepVertical = 0;
        public static int vBlackBarRangeHorizontal = 0;
        public static int vBlackBarRangeVertical = 0;

        //Screen capture Variables
        public static int vCaptureRange = 0;
        public static byte[] vCaptureByteHistory1 = null;
        public static byte[] vCaptureByteHistory2 = null;
        public static byte[] vCaptureByteHistory3 = null;
        public static int vCaptureWidth = 0;
        public static int vCaptureHeight = 0;
        public static int vCaptureTotalByteSize = 0;
        public static bool vCaptureHDREnabled = false;

        //Update Variables
        public static bool vCheckingForUpdate = false;
        public static bool vDebugCaptureAllowed = false;

        //Sockets Variables
        public static ArnoldVinkSockets vArnoldVinkSockets = null;

        //Reset default variables
        public static void ResetVariables()
        {
            try
            {
                //Blackbar Variables
                vMarginBlackLastUpdate = 0;
                vMarginTop = vMarginMinimumOffset;
                vMarginBottom = vMarginMinimumOffset;
                vMarginLeft = vMarginMinimumOffset;
                vMarginRight = vMarginMinimumOffset;
                vBlackBarStepHorizontal = 0;
                vBlackBarStepVertical = 0;
                vBlackBarRangeHorizontal = 0;
                vBlackBarRangeVertical = 0;

                //Screen capture Variables
                vCaptureRange = 0;
                vCaptureByteHistory1 = null;
                vCaptureByteHistory2 = null;
                vCaptureByteHistory3 = null;
                vCaptureWidth = 0;
                vCaptureHeight = 0;
                vCaptureTotalByteSize = 0;
                vCaptureHDREnabled = false;
            }
            catch { }
        }
    }
}