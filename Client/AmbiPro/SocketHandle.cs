using AmbiPro.Settings;
using System.Threading.Tasks;

namespace AmbiPro
{
    public partial class Socket
    {
        //Handle received socket data
        public static async Task<string> SocketHandle(string[] SocketData)
        {
            try
            {
                if (SocketData[0].Contains("LedSwitch")) { await SerialMonitor.LedSwitch(false, false); }
                else if (SocketData[0].Contains("LedBrightness")) { SettingsFunction.Save("LedBrightness", SocketData[1]); }
                else if (SocketData[0].Contains("LedMode"))
                {
                    SettingsFunction.Save("LedMode", SocketData[1]);
                    await SerialMonitor.LedSwitch(false, true);
                }
            }
            catch { }
            return "Ok";
        }
    }
}