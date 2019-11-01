using System;
using System.Configuration;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AmbiPro
{
    public partial class Socket
    {
        //Socket variables
        private static bool vSocketServerBusy = false;
        private static TcpListener vTcpListener = null;

        //Switch the server on or off
        public static async Task SocketServerSwitch(bool ForceOff, bool Restart)
        {
            try
            {
                if (!vSocketServerBusy)
                {
                    vSocketServerBusy = true;
                    if (Restart)
                    {
                        Debug.WriteLine("Restarting the socket server...");
                        await SocketServerDisable();
                        SocketServerEnable();
                        vSocketServerBusy = false;
                        return;
                    }

                    if (AppTasks.SocketServerRunning() || ForceOff)
                    {
                        Debug.WriteLine("Disabling the socket server...");
                        await SocketServerDisable();
                        vSocketServerBusy = false;
                        return;
                    }
                    else
                    {
                        Debug.WriteLine("Enabling the socket server...");
                        SocketServerEnable();
                        vSocketServerBusy = false;
                        return;
                    }
                }
            }
            catch
            {
                Debug.WriteLine("Failed switching the socket server on or off.");
                vSocketServerBusy = false;
            }
        }

        //Enable the socket server
        private static void SocketServerEnable()
        {
            try
            {
                AppTasks.SocketServerToken = new CancellationTokenSource();
                AppTasks.SocketServerTask = Task.Run(async () => await SocketListener(), AppTasks.SocketServerToken.Token);
            }
            catch (Exception ex) { Debug.WriteLine("Failed to enable the socket server: " + ex.Message); }
        }

        //Disable the socket server
        private static async Task SocketServerDisable()
        {
            try
            {
                //Stop the tcp listener
                if (vTcpListener != null)
                {
                    vTcpListener.Stop();
                    vTcpListener = null;
                }

                //Cancel the remote task
                AppTasks.SocketServerToken.Cancel();

                //Wait for previous task
                while (!AppTasks.SocketServerTask.IsCompleted)
                {
                    Debug.WriteLine("Waiting for task to complete...");
                    await Task.Delay(100);
                }

                AppTasks.SocketServerTask.Dispose();
                AppTasks.SocketServerToken.Dispose();
            }
            catch (Exception ex) { Debug.WriteLine("Failed to disable the socket server: " + ex.Message); }
        }

        //Receive socket message
        private static async Task SocketListener()
        {
            try
            {
                //Start the tcp listener
                int ServerPort = Convert.ToInt32(ConfigurationManager.AppSettings["RemotePort"]);
                vTcpListener = new TcpListener(IPAddress.Any, ServerPort);
                vTcpListener.Start();

                Debug.WriteLine("The server is running on: " + vTcpListener.LocalEndpoint);

                //Socket Listening loop
                while (AppTasks.SocketServerRunning())
                {
                    //Receive sockets
                    using (TcpClient tcpClient = await vTcpListener.AcceptTcpClientAsync())
                    {
                        using (NetworkStream tcpStream = tcpClient.GetStream())
                        {
                            try
                            {
                                //Receive message
                                byte[] bytesReceived = new byte[tcpClient.ReceiveBufferSize];
                                int bytesReceivedLength = await tcpStream.ReadAsync(bytesReceived, 0, tcpClient.ReceiveBufferSize);
                                string StringReceived = Encoding.UTF8.GetString(bytesReceived, 0, bytesReceivedLength);
                                Debug.WriteLine("Received string: " + StringReceived);

                                //Prepare response message
                                string[] SocketData = StringReceived.Split('‡');
                                string StringResponse = await SocketHandle(SocketData);

                                //Send response message
                                byte[] bytesResponse = Encoding.UTF8.GetBytes(StringResponse);
                                await tcpStream.WriteAsync(bytesResponse, 0, bytesResponse.Length);
                                Debug.WriteLine("Sended response: " + StringResponse);
                            }
                            catch { }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("The socket server crashed: " + ex.Message);
                await SocketServerSwitch(false, true);
            }
        }
    }
}