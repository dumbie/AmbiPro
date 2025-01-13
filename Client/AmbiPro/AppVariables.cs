using AmbiPro.Settings;
using ArnoldVinkCode;
using ScreenCaptureImport;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO.Ports;
using static AmbiPro.AppClasses;
using static AmbiPro.AppEnums;
using static ArnoldVinkCode.AVClasses;
using static ArnoldVinkCode.AVJsonFunctions;
using static ArnoldVinkCode.AVSettings;

namespace AmbiPro
{
    partial class AppVariables
    {
        //Application Variables
        public static Configuration vConfiguration = SettingLoadConfig("AmbiPro.exe.Config");
        public static CultureInfo vAppCultureInfo = CultureInfo.InvariantCulture;

        //Application Windows
        public static FormSettings vFormSettings = new FormSettings();

        //Led variables
        public static bool vLedSwitching = false;
        public static bool vMonitorSleeping = false;

        //Serial port
        public static SerialPort vSerialComPort = null;

        //Setting Variables
        public static BackgroundMode vCurrentBackgroundMode = BackgroundMode.Transparent;
        public static string vCurrentVisibleMenu = string.Empty;
        public static string vCurrentRatio = string.Empty;
        public static int vCurrentRotation = 0;
        public static int vCurrentBlackbar = 0;
        public static int vCurrentSolidColor = 0;

        //Color loop Variables
        public static ColorRGBA vCurrentLoopColor = new ColorRGBA() { R = 20 };

        //Blackbar Variables
        public static int vBlackbarAdjustStepHorizontal = 1;
        public static int vBlackbarAdjustStepVertical = 1;
        public static int vBlackbarRangeHorizontal = 0;
        public static int vBlackbarRangeVertical = 0;
        public static int[] vBlackbarRangesTop = new int[1000];
        public static int[] vBlackbarRangesBottom = new int[1000];
        public static int[] vBlackbarRangesLeft = new int[1000];
        public static int[] vBlackbarRangesRight = new int[1000];

        //Screen capture Variables
        public static int vCaptureRangeHorizontal = 0;
        public static int vCaptureRangeVertical = 0;
        public static CaptureDetails vCaptureDetails;
        public static CaptureSettings vCaptureSettings;
        public static byte[][] vCaptureByteHistoryArray = new byte[20][];

        //Debug Variables
        public static bool vDebugCaptureAllowed = false;

        //Sockets Variables
        public static ArnoldVinkSockets vArnoldVinkSockets = null;

        //Application Lists
        public static List<ShortcutTriggerKeyboard> vShortcutTriggers = JsonLoadFile<List<ShortcutTriggerKeyboard>>(@"Profiles\ShortcutKeyboard.json");

        //Reset default variables
        public static void ResetVariables()
        {
            try
            {
                //Blackbar Variables
                vBlackbarAdjustStepHorizontal = 1;
                vBlackbarAdjustStepVertical = 1;
                vBlackbarRangeHorizontal = 0;
                vBlackbarRangeVertical = 0;
                vBlackbarRangesTop = new int[1000];
                vBlackbarRangesBottom = new int[1000];
                vBlackbarRangesLeft = new int[1000];
                vBlackbarRangesRight = new int[1000];

                //Screen capture Variables
                vCaptureRangeHorizontal = 0;
                vCaptureRangeVertical = 0;
                vCaptureDetails = new CaptureDetails();
                vCaptureSettings = new CaptureSettings();
                vCaptureByteHistoryArray = new byte[20][];
            }
            catch { }
        }
    }
}