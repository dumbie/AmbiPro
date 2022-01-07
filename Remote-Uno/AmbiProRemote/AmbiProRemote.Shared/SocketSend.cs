using System;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
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

                    //Create udp client
                    using (UdpClient udpClient = new UdpClient(serverIp, serverPort))
                    {
                        await udpClient.SendAsync(targetBytes, targetBytes.Length);
                    }

                    System.Diagnostics.Debug.WriteLine("Socket sended: " + sendData);

                    //app_StatusBar.Visibility = Visibility.Collapsed;
                    //txt_ErrorConnect.Visibility = Visibility.Collapsed;
                    //txt_ErrorConnect2.Visibility = Visibility.Collapsed;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Socket send failed: " + ex.Message);

                //app_StatusBar.Visibility = Visibility.Collapsed;
                //if (!AVFunctions.DevMobile())
                //{
                //    txt_ErrorConnect.Visibility = Visibility.Visible;
                //    txt_ErrorConnect2.Visibility = Visibility.Visible;
                //}

                //await new MessageDialog("Failed to connect to the AmbiPro application on your PC please check this app settings, your network connection and make sure that AmbiPro is running on the target PC.", App.vApplicationName).ShowAsync();
            }
            finally
            {
                vSendingSocketMsg = false;
            }
        }
    }
}
