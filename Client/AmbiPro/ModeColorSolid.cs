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
        //Set the solid color to the leds
        private static async Task ModeSolidColor()
        {
            try
            {
                //Loop mode variables
                bool ConnectionFailed = false;
                int LoopDelayMs = 0;

                //Create byte array
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
                            //Get current color
                            ColorRGBA CurrentColor = ColorRGBA.HexToRGBA(setSolidLedColor);

                            //Set color to array
                            for (int i = 0; i < colorArray.Length; i++)
                            {
                                colorArray[i] = ColorRGBA.Clone(CurrentColor);
                            }

                            //Adjust leds color to settings
                            AdjustLedColors(colorArray);

                            //Adjust leds to energy mode
                            AdjustLedEnergyMode(colorArray);

                            //Set loop delay time
                            LoopDelayMs = 1000;
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
                        Debug.WriteLine("Mode color solid loop failed: " + ex.Message);
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