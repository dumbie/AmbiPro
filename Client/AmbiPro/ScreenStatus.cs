using System;
using System.Configuration;
using System.Threading.Tasks;
using static AmbiPro.AppVariables;
using static ArnoldVinkCode.AVActions;
using static ArnoldVinkCode.AVDisplayMonitor;

namespace AmbiPro
{
    public partial class SerialMonitor
    {
        public static async Task vTaskLoop_UpdateScreenStatus()
        {
            try
            {
                while (!vTask_UpdateScreenStatus.TaskStopRequest)
                {
                    try
                    {
                        //Check if HDR is enabled
                        int monitorId = Convert.ToInt32(ConfigurationManager.AppSettings["MonitorCapture"]);
                        DisplayMonitor displayConfig = MonitorDisplayConfig(monitorId);
                        if (displayConfig != null)
                        {
                            vScreenOutputHDR = displayConfig.HdrEnabled;
                            vScreenOutputHDRWhiteLevel = displayConfig.HdrWhiteLevel;
                        }

                        //Delay the loop task
                        await TaskDelayLoop(1000, vTask_UpdateScreenStatus);
                    }
                    catch { }
                }
            }
            catch { }
        }
    }
}