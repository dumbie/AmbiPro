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
        //Rotating color spectrum
        private static async Task ModeColorSpectrum()
        {
            try
            {
                //Loop mode variables
                bool ConnectionFailed = false;
                int CurrentLedRotate = 0;
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
                            //Set used color spectrum
                            ColorRGBA Color0 = ColorRGBA.HexToRGBA("#f8d000");
                            ColorRGBA Color1 = ColorRGBA.HexToRGBA("#ffb000");
                            ColorRGBA Color2 = ColorRGBA.HexToRGBA("#ff7000");
                            ColorRGBA Color3 = ColorRGBA.HexToRGBA("#fa0000");
                            ColorRGBA Color4 = ColorRGBA.HexToRGBA("#e80096");
                            ColorRGBA Color5 = ColorRGBA.HexToRGBA("#70009c");
                            ColorRGBA Color6 = ColorRGBA.HexToRGBA("#0000be");
                            ColorRGBA Color7 = ColorRGBA.HexToRGBA("#003ace");
                            ColorRGBA Color8 = ColorRGBA.HexToRGBA("#00bae7");
                            ColorRGBA Color9 = ColorRGBA.HexToRGBA("#004500");
                            ColorRGBA Color10 = ColorRGBA.HexToRGBA("#276800");
                            ColorRGBA Color11 = ColorRGBA.HexToRGBA("#6c9f00");
                            ColorRGBA Color12 = ColorRGBA.HexToRGBA("#93b300");

                            //Rotate color variables
                            int CurrentColor = 0;
                            int CurrentLedRange = 0;
                            int TotalLedRange = setLedCountTotal / 13;

                            //Set color to array
                            for (int i = 0; i < colorArray.Length; i++)
                            {
                                //Check if the next color has been reached
                                if (CurrentLedRange == TotalLedRange)
                                {
                                    if (CurrentColor == 0) { Color0 = Color1; }
                                    else if (CurrentColor == 1) { Color0 = Color2; }
                                    else if (CurrentColor == 2) { Color0 = Color3; }
                                    else if (CurrentColor == 3) { Color0 = Color4; }
                                    else if (CurrentColor == 4) { Color0 = Color5; }
                                    else if (CurrentColor == 5) { Color0 = Color6; }
                                    else if (CurrentColor == 6) { Color0 = Color7; }
                                    else if (CurrentColor == 7) { Color0 = Color8; }
                                    else if (CurrentColor == 8) { Color0 = Color9; }
                                    else if (CurrentColor == 9) { Color0 = Color10; }
                                    else if (CurrentColor == 10) { Color0 = Color11; }
                                    else if (CurrentColor == 11) { Color0 = Color12; }
                                    CurrentLedRange = 0;
                                    CurrentColor++;
                                }

                                colorArray[i] = ColorRGBA.Clone(Color0);
                                CurrentLedRange++;
                            }

                            //Rotate color bytes
                            //Debug.WriteLine("Rotating the spectrum color bytes: " + CurrentLedRotate);
                            for (int rotateCount = 0; rotateCount < CurrentLedRotate; rotateCount++)
                            {
                                //MoveObjectInArrayLeft(colorArray, 0, colorArray.Length - 1);
                                MoveObjectInArrayRight(colorArray, colorArray.Length - 1, 0);
                            }

                            //Update rotate count
                            if (CurrentLedRotate == setLedCountTotal) { CurrentLedRotate = 0; } else { CurrentLedRotate++; }

                            //Adjust leds color to settings
                            AdjustLedColors(colorArray);

                            //Adjust leds to energy mode
                            AdjustLedEnergyMode(colorArray);

                            //Set loop delay time
                            LoopDelayMs = setSpectrumRotationSpeed * 1000;
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
                        Debug.WriteLine("Mode color spectrum loop failed: " + ex.Message);
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