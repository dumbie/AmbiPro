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
        public const int vBlackbarAdjustStep = 8;
        public const int vBlackbarDetectStep = 4;
        public static int vBlackbarRangeHorizontal = 0;
        public static int vBlackbarRangeVertical = 0;
        public static int vBlackbarLastUpdate = 0;

        //Screen capture Variables
        public static int vCaptureRange = 0;
        public static int vCaptureMarginTop = 0;
        public static int vCaptureMarginBottom = 0;
        public static int vCaptureMarginLeft = 0;
        public static int vCaptureMarginRight = 0;
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
                vBlackbarRangeHorizontal = 0;
                vBlackbarRangeVertical = 0;
                vBlackbarLastUpdate = 0;

                //Screen capture Variables
                vCaptureRange = 0;
                vCaptureMarginTop = 0;
                vCaptureMarginBottom = 0;
                vCaptureMarginLeft = 0;
                vCaptureMarginRight = 0;
                vCaptureDetails = new CaptureDetails();
                vCaptureSettings = new CaptureSettings();
                vCaptureByteHistoryArray = new byte[20][];
            }
            catch { }
        }
    }
}