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
        //Simulate a slow burning candle
        private static async Task ModeColorCandle()
        {
            try
            {
                //Loop mode variables
                bool ConnectionFailed = false;
                int ColorLoopState = 0;
                int LoopDelayMs = 0;

                //Create led ColorRGBA array
                ColorRGBA vCurrentCandleColor = new ColorRGBA() { R = 255, G = 200 };
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
                            //Check current color
                            if (vCurrentCandleColor.G == 160)
                            {
                                ColorLoopState = 0;
                            }
                            else if (vCurrentCandleColor.G == 200)
                            {
                                ColorLoopState = 1;
                            }

                            //Set the used colors
                            if (ColorLoopState == 0)
                            {
                                //Yellow
                                vCurrentCandleColor.G++;
                            }
                            else
                            {
                                //Orange
                                vCurrentCandleColor.G--;
                            }

                            //Set color to array
                            for (int i = 0; i < colorArray.Length; i++)
                            {
                                colorArray[i] = ColorRGBA.Clone(vCurrentCandleColor);
                            }

                            //Adjust leds color to settings
                            AdjustLedColors(colorArray);

                            //Adjust leds to energy mode
                            AdjustLedEnergyMode(colorArray);

                            //Set loop delay time
                            LoopDelayMs = 100;
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
                        Debug.WriteLine("Mode color candle failed: " + ex.Message);
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