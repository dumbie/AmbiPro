﻿using System.Threading.Tasks;
using static AmbiPro.AppClasses;
using static AmbiPro.AppTasks;
using static AmbiPro.PreloadSettings;
using static ArnoldVinkCode.AVActions;

namespace AmbiPro
{
    partial class SerialMonitor
    {
        //Set the solid color to the leds
        private static async Task ModeSolidColor(int InitByteSize, byte[] SerialBytes)
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
                    if (!SerialComPortWrite(SerialBytes))
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