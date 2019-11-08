using ArnoldVinkCode;
using System;
using System.Drawing;
using System.Reflection;
using System.Threading.Tasks;

namespace AmbiPro
{
    partial class SerialMonitor
    {
        //Set the solid color to the leds
        private static async Task ModeSolidColor(Int32 InitByteSize, byte[] SerialBytes)
        {
            try
            {
                //Update the tray icon
                AppTray.NotifyIcon.Icon = new Icon(Assembly.GetEntryAssembly().GetManifestResourceStream("AmbiPro.Assets.ApplicationIcon.ico"));

                //Current byte information
                while (AVActions.TaskRunningCheck(AppTasks.LedToken))
                {
                    //Set the used colors and adjust it
                    Color CurrentColor = ColorTranslator.FromHtml(setSolidLedColor);
                    CurrentColor = AdjustLedColors(CurrentColor);

                    //Set the current color to the bytes
                    Int32 CurrentSerialByte = InitByteSize;
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

                    await Task.Delay(1000);
                }
            }
            catch { }
        }
    }
}