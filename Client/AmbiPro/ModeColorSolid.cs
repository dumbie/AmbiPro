using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AmbiPro.AppClasses;
using static AmbiPro.AppTasks;
using static AmbiPro.AppVariables;
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
                int LoopDelayMs = 0;

                //Current byte information
                while (!vTask_UpdateLed.TaskStopRequested)
                {
                    try
                    {
                        //Check monitor sleeping and send black leds update
                        if (vMonitorSleeping)
                        {
                            //Set led byte array
                            serialBytes = new byte[totalByteSize];
                            serialBytes[0] = Encoding.Unicode.GetBytes("A").First();
                            serialBytes[1] = Encoding.Unicode.GetBytes("d").First();
                            serialBytes[2] = Encoding.Unicode.GetBytes("a").First();

                            //Set loop delay time
                            LoopDelayMs = 500;
                        }
                        else
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

                            //Set loop delay time
                            LoopDelayMs = 1000;
                        }

                        //Send serial bytes to device
                        if (!SerialComPortWrite(totalByteSize, serialBytes))
                        {
                            ConnectionFailed = true;
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("Mode color solid loop failed: " + ex.Message);
                    }
                    finally
                    {
                        //Delay the loop task
                        await TaskDelay(LoopDelayMs, vTask_UpdateLed);
                    }
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