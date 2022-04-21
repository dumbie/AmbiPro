using ArnoldVinkCode;
using ScreenCapture;
using System.Configuration;
using System.Globalization;
using System.IO.Ports;
using static AmbiPro.AppClasses;

namespace AmbiPro
{
    partial class AppVariables
    {
        //Application Variables
        public static Configuration vConfiguration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        public static CultureInfo vAppCultureInfo = CultureInfo.InvariantCulture;

        //Led variables
        public static bool vLedSwitching = false;

        //Serial port
        public static SerialPort vSerialComPort = null;

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
        public static CaptureDetails vCaptureDetails;
        public static CaptureSettings vCaptureSettings;
        public static byte[][] vCaptureByteHistoryArray = new byte[20][];

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
                vCaptureDetails = new CaptureDetails();
                vCaptureSettings = new CaptureSettings();
                vCaptureByteHistoryArray = new byte[20][];
            }
            catch { }
        }
    }
}