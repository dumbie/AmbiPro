using ArnoldVinkCode;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using static AmbiPro.AppEnums;
using static AmbiPro.AppVariables;
using static AmbiPro.SerialMonitor;
using static ArnoldVinkCode.ArnoldVinkSockets;
using static ArnoldVinkCode.AVSettings;

namespace AmbiPro
{
    partial class SocketHandlers
    {
        //Handle received socket data
        public static void ReceivedSocketHandler(TcpClient tcpClient, UdpEndPointDetails endPoint, byte[] bytesReceived)
        {
            try
            {
                async void TaskAction()
                {
                    try
                    {
                        await ReceivedSocketHandlerThread(tcpClient, bytesReceived);
                    }
                    catch { }
                }
                AVActions.TaskStartBackground(TaskAction);
            }
            catch { }
        }

        public static async Task ReceivedSocketHandlerThread(TcpClient tcpClient, byte[] bytesReceived)
        {
            try
            {
                //Receive message
                string StringReceived = Encoding.UTF8.GetString(bytesReceived, 0, bytesReceived.Length);
                StringReceived = WebUtility.UrlDecode(StringReceived);
                StringReceived = WebUtility.HtmlDecode(StringReceived);
                StringReceived = StringReceived.TrimEnd('\0');
                Debug.WriteLine("Received string: " + StringReceived);

                //Prepare response message
                string[] SocketData = StringReceived.Split('‡');
                string StringResponse = await SocketStringHandle(SocketData);
                //byte[] bytesResponse = Encoding.UTF8.GetBytes(StringResponse);

                //Return response message
                //await vArnoldVinkSockets.TcpClientSendBytes(tcpClient, bytesResponse, vArnoldVinkSockets.vTcpClientTimeout, false);
            }
            catch { }
        }

        //Handle received socket string
        public static async Task<string> SocketStringHandle(string[] socketStringArray)
        {
            try
            {
                string messageType = socketStringArray[0].Replace("GET /?", string.Empty);
                string messageValue = string.Empty;
                if (socketStringArray.Count() > 1)
                {
                    messageValue = AVFunctions.StringRemoveAfter(socketStringArray[1], " ", 0);
                }

                if (messageType.Contains("LedSwitch"))
                {
                    await LedSwitch(LedSwitches.Automatic);
                }
                else if (messageType.Contains("LedBrightness"))
                {
                    SettingSave(vConfiguration, "LedBrightness", messageValue);
                }
                else if (messageType.Contains("LedMode"))
                {
                    SettingSave(vConfiguration, "LedMode", messageValue);
                    await LedSwitch(LedSwitches.Restart);
                }
                else if (messageType.Contains("SolidLedColor"))
                {
                    SettingSave(vConfiguration, "SolidLedColor", messageValue);
                    AppEvents.EventUpdateSettingsSolidLedColor();
                }
                else
                {
                    Debug.WriteLine("Unknown socket message type, switching leds on or off.");
                    await LedSwitch(LedSwitches.Automatic);
                }
            }
            catch { }
            return "Ok";
        }
    }
}