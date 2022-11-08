using System.Threading.Tasks;
using static AmbiPro.AppClasses;
using static AmbiPro.AppTasks;
using static AmbiPro.PreloadSettings;
using static ArnoldVinkCode.AVActions;

namespace AmbiPro
{
    partial class SerialMonitor
    {
        //Set the solid color to the leds
        private static async Task ModeSolidColor(int initByteSize, int totalByteSize, byte[] serialBytes)
        {
            try
            {
                //Loop mode variables
                bool ConnectionFailed = false;

                //Current byte information
                while (!vTask_UpdateLed.TaskStopRequest)
                {
                    //Set the used colors and adjust it
                    ColorRGBA CurrentColor = ColorRGBA.HexToRGBA(setSolidLedColor);
                    AdjustLedColors(ref CurrentColor);

                    //Set the current color to the bytes
                    int CurrentSerialByte = initByteSize;
                    while (CurrentSerialByte < totalByteSize)
                    {
                        serialBytes[CurrentSerialByte] = CurrentColor.R;
                        CurrentSerialByte++;

                        serialBytes[CurrentSerialByte] = CurrentColor.G;
                        CurrentSerialByte++;

                        serialBytes[CurrentSerialByte] = CurrentColor.B;
                        CurrentSerialByte++;
                    }

                    //Send the serial bytes to device
                    if (!SerialComPortWrite(totalByteSize, serialBytes))
                    {
                        ConnectionFailed = true;
                        break;
                    }

                    //Delay the loop task
                    await TaskDelayLoop(1000, vTask_UpdateLed);
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