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
        //Loop through the set colors
        private static async Task ModeColorLoop(int initByteSize, int totalByteSize, byte[] serialBytes)
        {
            try
            {
                //Loop mode variables
                bool ConnectionFailed = false;
                int ColorLoopState = 0;
                int LoopDelayMs = 0;

                //Current byte information
                while (!vTask_UpdateLed.TaskStopRequested)
                {
                    try
                    {
                        //Check monitor sleeping and send black leds update
                        if (setLedOffMonitorSleep && vMonitorSleeping)
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
                            int CurrentSerialByte = initByteSize;
                            while (CurrentSerialByte < totalByteSize)
                            {
                                serialBytes[CurrentSerialByte] = AdjustedColor.R;
                                CurrentSerialByte++;

                                serialBytes[CurrentSerialByte] = AdjustedColor.G;
                                CurrentSerialByte++;

                                serialBytes[CurrentSerialByte] = AdjustedColor.B;
                                CurrentSerialByte++;
                            }

                            //Set loop delay time
                            LoopDelayMs = setColorLoopSpeed;
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
                        Debug.WriteLine("Mode color loop failed: " + ex.Message);
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