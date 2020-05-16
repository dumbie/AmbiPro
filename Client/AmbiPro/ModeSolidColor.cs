using System.Drawing;
using System.Reflection;
using static AmbiPro.AppTasks;
using static ArnoldVinkCode.AVActions;

namespace AmbiPro
{
    partial class SerialMonitor
    {
        //Set the solid color to the leds
        private static void ModeSolidColor(int InitByteSize, byte[] SerialBytes)
        {
            try
            {
                //Update the tray icon
                AppTray.NotifyIcon.Icon = new Icon(Assembly.GetEntryAssembly().GetManifestResourceStream("AmbiPro.Assets.ApplicationIcon.ico"));

                //Current byte information
                while (!vTask_LedUpdate.TaskStopRequest)
                {
                    //Set the used colors and adjust it
                    Color CurrentColor = ColorTranslator.FromHtml(setSolidLedColor);
                    CurrentColor = AdjustLedColors(CurrentColor);

                    //Set the current color to the bytes
                    int CurrentSerialByte = InitByteSize;
                    while (CurrentSerialByte < SerialBytes.Length)
                    {
                        SerialBytes[CurrentSerialByte] = CurrentColor.R;
                        CurrentSerialByte++;

                        SerialBytes[CurrentSerialByte] = CurrentColor.G;
                        CurrentSerialByte++;

                        SerialBytes[CurrentSerialByte] = CurrentColor.B;
                        CurrentSerialByte++;
                    }

                    //Send the serial bytes to device
                    //Debug.WriteLine("Serial bytes sended: " + SerialBytes.Length);
                    vSerialComPort.Write(SerialBytes, 0, SerialBytes.Length);

                    //Delay the loop task
                    TaskDelayLoop(1000, vTask_LedUpdate);
                }
            }
            catch { }
        }
    }
}