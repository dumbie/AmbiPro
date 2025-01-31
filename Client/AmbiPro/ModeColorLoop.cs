using System;
using System.Diagnostics;
using System.Threading.Tasks;
using static AmbiPro.AppClasses;
using static AmbiPro.AppTasks;
using static AmbiPro.AppVariables;
using static AmbiPro.PreloadSettings;
using static ArnoldVinkCode.AVActions;
using static ArnoldVinkCode.AVArrayFunctions;

namespace AmbiPro
{
    partial class SerialMonitor
    {
        //Loop through the set colors
        private static async Task ModeColorLoop()
        {
            try
            {
                //Loop mode variables
                bool ConnectionFailed = false;
                int ColorLoopState = 0;
                int LoopDelayMs = 0;

                //Create led ColorRGBA array
                ColorRGBA[] colorArray = CreateArray(setLedCountTotal, ColorRGBA.Black);

                //Start updating leds
                while (await TaskCheckLoop(vTask_UpdateLed, LoopDelayMs))
                {
                    try
                    {
                        //Check monitor sleeping and send black leds update
                        if (setLedOffMonitorSleep && vMonitorSleeping)
                        {
                            //Reset color array
                            ResetArray(colorArray, ColorRGBA.Black);

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

                            //Set color to array
                            for (int i = 0; i < colorArray.Length; i++)
                            {
                                colorArray[i] = ColorRGBA.Clone(vCurrentLoopColor);
                            }

                            //Adjust leds color to settings
                            AdjustLedColors(colorArray);

                            //Adjust leds to energy mode
                            AdjustLedEnergyMode(colorArray);

                            //Set loop delay time
                            LoopDelayMs = setColorLoopSpeed;
                        }

                        //Send serial bytes to device
                        if (!SerialComPortWrite(colorArray))
                        {
                            ConnectionFailed = true;
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("Mode color loop failed: " + ex.Message);
                    }
                }

                //Show failed connection message
                if (ConnectionFailed)
                {
                    await ShowFailedConnectionMessage();
                }
            }
            catch { }
        }
    }
}