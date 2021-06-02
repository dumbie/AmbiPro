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
        public static string vCurrentRatio = string.Empty;
        public static int vCurrentRotation = 0;
        public static int vCurrentBlackbar = 0;
        public static int vCurrentBackground = 0;

        //Color loop Variables
        public static ColorRGBA vCurrentLoopColor = new ColorRGBA() { R = 20 };

        //Capture Variables
        public static int vCaptureRange = 0;

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

        //Screen Variables
        public static int vScreenOutputWidth = 0;
        public static int vScreenOutputHeight = 0;
        public static int vScreenOutputSize = 0;
        public static bool vScreenOutputHDR = false;

        //Update Variables
        public static bool vCheckingForUpdate = false;

        //Sockets Variables
        public static ArnoldVinkSockets vArnoldVinkSockets = null;

        //Reset default variables
        public static void ResetVariables()
        {
            try
            {
                //Capture Variables
                vCaptureRange = 0;

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

                //Screen Variables
                vScreenOutputWidth = 0;
                vScreenOutputHeight = 0;
                vScreenOutputSize = 0;
                vScreenOutputHDR = false;
            }
            catch { }
        }
    }
}