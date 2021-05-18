using System.Threading.Tasks;
using static AmbiPro.AppClasses;
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

                //Update led status icons
                UpdateLedStatusIcons(true);

                //Current byte information
                while (!vTask_LedUpdate.TaskStopRequest)
                {
                    //Set the used colors
                    if (ColorLoopState == 0) //Red
                    {
                        vCurrentLoopColor.R++;
                        if (vCurrentLoopColor.G > 0) { vCurrentLoopColor.G--; }
                        if (vCurrentLoopColor.B > 0) { vCurrentLoopColor.B--; }
                        if (vCurrentLoopColor.R == 220 && vCurrentLoopColor.G == 0 && vCurrentLoopColor.B == 0) { ColorLoopState++; }
                    }
                    else if (ColorLoopState == 1) //Green
                    {
                        if (vCurrentLoopColor.R > 0) { vCurrentLoopColor.R--; }
                        vCurrentLoopColor.G++;
                        if (vCurrentLoopColor.B > 0) { vCurrentLoopColor.B--; }
                        if (vCurrentLoopColor.R == 0 && vCurrentLoopColor.G == 220 && vCurrentLoopColor.B == 0) { ColorLoopState++; }
                    }
                    else if (ColorLoopState == 2) //Blue
                    {
                        if (vCurrentLoopColor.R > 0) { vCurrentLoopColor.R--; }
                        if (vCurrentLoopColor.G > 0) { vCurrentLoopColor.G--; }
                        vCurrentLoopColor.B++;
                        if (vCurrentLoopColor.R == 0 && vCurrentLoopColor.G == 0 && vCurrentLoopColor.B == 220) { ColorLoopState++; }
                    }

                    //Reset color loop to red
                    if (ColorLoopState == 3)
                    {
                        ColorLoopState = 0;
                    }

                    //Adjust the color
                    ColorRGBA AdjustedColor = ColorRGBA.Clone(vCurrentLoopColor);
                    AdjustLedColors(ref AdjustedColor);

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