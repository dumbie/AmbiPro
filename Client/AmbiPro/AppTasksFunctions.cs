using System.Collections.Generic;
using System.Threading.Tasks;
using static AmbiPro.PreloadSettings;
using static ArnoldVinkCode.AVActions;
using static ArnoldVinkCode.AVDevices.Enumerate;
using static ArnoldVinkCode.AVDevices.Interop;

namespace AmbiPro
{
    public partial class AppTasks
    {
        public static async Task vTaskLoop_UpdateSettings()
        {
            try
            {
                while (await TaskCheckLoop(vTask_UpdateSettings, 2000))
                {
                    UpdateSettingsPreload();
                }
            }
            catch { }
        }

        public static async Task vTaskLoop_UpdateStatus()
        {
            try
            {
                while (await TaskCheckLoop(vTask_UpdateStatus, 2000))
                {
                    //Check if monitor is sleeping
                    if (setLedOffMonitorSleep)
                    {
                        bool monitorSleeping = false;
                        List<EnumerateInfo> monitorDevices = EnumerateDevicesSetupApi(GUID_DEVINTERFACE_MONITOR, true);
                        foreach (EnumerateInfo monitorInfo in monitorDevices)
                        {
                            if (monitorInfo.PowerData != null)
                            {
                                DEVICE_POWER_STATE powerState = ((CM_POWER_DATA)monitorInfo.PowerData).PD_MostRecentPowerState;
                                if (powerState != DEVICE_POWER_STATE.PowerDeviceD0)
                                {
                                    monitorSleeping = true;
                                }
                                //Debug.WriteLine("Monitor sleeping: " + monitorSleeping);
                            }
                        }

                        //Update status variables
                        AppVariables.vMonitorSleeping = monitorSleeping;
                    }
                }
            }
            catch { }
        }
    }
}