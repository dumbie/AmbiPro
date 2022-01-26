using System;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;
using static AmbiProRemote.AppVariables;

namespace AmbiProRemote
{
    class SocketSend
    {
        //Check if the server ip adres is valid
        public static bool ValidateRemoteIpAdres(string ipAdres)
        {
            try
            {
                if (ipAdres.EndsWith(".") || ipAdres.EndsWith(",") || ipAdres.Count(x => x == '.') < 3)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch { }
            return false;
        }

        //Check if the server port is valid
        public static bool ValidateRemotePort(int port)
        {
            try
            {
                if (port < 1 || port > 65535)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch { }
            return false;
        }

        //Send socket message to AmbiPro
        public static async Task SocketSendAmbiPro(string sendData)
        {
            try
            {
                //Check if not sending a socket already
                if (!vSendingSocketMsg)
                {
                    //Check if server ip and port is valid
                    string serverIp = "192.168.0.3"; //Convert.ToString(vApplicationSettings["ServerIp"]);
                    int serverPort = 1020; //Convert.ToString(vApplicationSettings["ServerPortAmbiPro"]);
                    if (!ValidateRemoteIpAdres(serverIp)) { return; }
                    if (!ValidateRemotePort(serverPort)) { return; }

                    //Set the remote socket variables
                    vSendingSocketMsg = true;

                    //Convert string to bytes
                    byte[] targetBytes = Encoding.UTF8.GetBytes(sendData);

                    //Create tcp client
                    using (TcpClient tcpClient = new TcpClient(serverIp, serverPort))
                    {
                        using (NetworkStream networkStream = tcpClient.GetStream())
                        {
                            await networkStream.WriteAsync(targetBytes, 0, targetBytes.Length);
                        }
                    }

                    System.Diagnostics.Debug.WriteLine("Socket sended: " + sendData);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Socket send failed: " + ex.Message);
                await new MessageDialog("Failed to connect to the AmbiPro application on your PC please check this app settings, your network connection and make sure that AmbiPro is running on the target PC.", "AmbiPro Remote").ShowAsync();
            }
            finally
            {
                vSendingSocketMsg = false;
            }
        }
    }
}