using System.Drawing;
using System.Reflection;
using System.Threading.Tasks;
using static AmbiPro.AppTasks;
using static AmbiPro.AppVariables;
using static ArnoldVinkCode.AVActions;

namespace AmbiPro
{
    partial class SerialMonitor
    {
        //Loop through the set colors
        private static async Task ModeColorLoop(int InitByteSize, byte[] SerialBytes)
        {
            try
            {
                //Loop mode variables
                bool ConnectionFailed = false;
                int ColorLoopState = 0;

                //Update the tray icon
                AppTray.NotifyIcon.Icon = new Icon(Assembly.GetEntryAssembly().GetManifestResourceStream("AmbiPro.Assets.ApplicationIcon.ico"));

                //Current byte information
                while (!vTask_LedUpdate.TaskStopRequest)
                {
                    //Set the used colors
                    if (ColorLoopState == 0) //Red
                    {
                        vCurrentLoopColor = Color.FromArgb(vCurrentLoopColor.R + 1, vCurrentLoopColor.G, vCurrentLoopColor.B);
                        if (vCurrentLoopColor.G > 0) { vCurrentLoopColor = Color.FromArgb(vCurrentLoopColor.R, vCurrentLoopColor.G - 1, vCurrentLoopColor.B); }
                        if (vCurrentLoopColor.B > 0) { vCurrentLoopColor = Color.FromArgb(vCurrentLoopColor.R, vCurrentLoopColor.G, vCurrentLoopColor.B - 1); }
                        if (vCurrentLoopColor.R == 220 && vCurrentLoopColor.G == 0 && vCurrentLoopColor.B == 0) { ColorLoopState++; }
                    }
                    else if (ColorLoopState == 1)  //Green
                    {
                        vCurrentLoopColor = Color.FromArgb(vCurrentLoopColor.R, vCurrentLoopColor.G + 1, vCurrentLoopColor.B);
                        if (vCurrentLoopColor.R > 0) { vCurrentLoopColor = Color.FromArgb(vCurrentLoopColor.R - 1, vCurrentLoopColor.G, vCurrentLoopColor.B); }
                        if (vCurrentLoopColor.B > 0) { vCurrentLoopColor = Color.FromArgb(vCurrentLoopColor.R, vCurrentLoopColor.G, vCurrentLoopColor.B - 1); }
                        if (vCurrentLoopColor.R == 0 && vCurrentLoopColor.G == 220 && vCurrentLoopColor.B == 0) { ColorLoopState++; }
                    }
                    else if (ColorLoopState == 2) //Blue
                    {
                        vCurrentLoopColor = Color.FromArgb(vCurrentLoopColor.R, vCurrentLoopColor.G, vCurrentLoopColor.B + 1);
                        if (vCurrentLoopColor.G > 0) { vCurrentLoopColor = Color.FromArgb(vCurrentLoopColor.R, vCurrentLoopColor.G - 1, vCurrentLoopColor.B); }
                        if (vCurrentLoopColor.R > 0) { vCurrentLoopColor = Color.FromArgb(vCurrentLoopColor.R - 1, vCurrentLoopColor.G, vCurrentLoopColor.B); }
                        if (vCurrentLoopColor.R == 0 && vCurrentLoopColor.G == 0 && vCurrentLoopColor.B == 220) { ColorLoopState++; }
                    }
                    if (ColorLoopState == 3) { ColorLoopState = 0; }
                    Color AdjustedColor = AdjustLedColors(vCurrentLoopColor);

                    //Set the current color to the bytes
                    int CurrentSerialByte = InitByteSize;
                    while (CurrentSerialByte < SerialBytes.Length)
                    {
                        SerialBytes[CurrentSerialByte] = AdjustedColor.R;
                        CurrentSerialByte++;

                        SerialBytes[CurrentSerialByte] = AdjustedColor.G;
                        CurrentSerialByte++;

                        SerialBytes[CurrentSerialByte] = AdjustedColor.B;
                        CurrentSerialByte++;
                    }

                    //Send the serial bytes to device
                    if (!SerialComPortWrite(SerialBytes))
                    {
                        ConnectionFailed = true;
                        break;
                    }

                    //Delay the loop task
                    await TaskDelayLoop(setColorLoopSpeed, vTask_LedUpdate);
                }

                //Show failed connection message
                if (ConnectionFailed)
                {
                    ShowFailedConnectionMessage();
                }
            }
            catch { }
        }
    }
}