using AmbiPro.Settings;
using ArnoldVinkCode;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using static AmbiPro.SerialMonitor;

namespace AmbiPro
{
    partial class SocketHandlers
    {
        //Handle received socket data
        public static async Task ReceivedSocketHandler(TcpClient tcpClient, byte[] receivedBytes)
        {
            try
            {
                async void TaskAction()
                {
                    try
                    {
                        await ReceivedSocketHandlerThread(tcpClient, receivedBytes);
                    }
                    catch { }
                }
                await AVActions.TaskStart(TaskAction);
            }
            catch { }
        }

        public static async Task ReceivedSocketHandlerThread(TcpClient tcpClient, byte[] receivedBytes)
        {
            try
            {
                //Receive message
                string StringReceived = Encoding.UTF8.GetString(receivedBytes, 0, receivedBytes.Length);
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
                    SettingsFunction.Save("LedBrightness", messageValue);
                }
                else if (messageType.Contains("LedMode"))
                {
                    SettingsFunction.Save("LedMode", messageValue);
                    await LedSwitch(LedSwitches.Restart);
                }
            }
            catch { }
            return "Ok";
        }
    }
}