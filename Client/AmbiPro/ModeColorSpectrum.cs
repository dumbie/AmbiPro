﻿using ArnoldVinkCode;
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
        //Rotating color spectrum
        private static async Task ModeColorSpectrum(int initByteSize, int totalByteSize, byte[] serialBytes)
        {
            try
            {
                //Loop mode variables
                bool ConnectionFailed = false;
                int CurrentLedRotate = 0;
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
                            //Set the used colors and adjust them
                            ColorRGBA Color0 = ColorRGBA.HexToRGBA("#f8d000");
                            AdjustLedColors(ref Color0);
                            ColorRGBA Color1 = ColorRGBA.HexToRGBA("#ffb000");
                            AdjustLedColors(ref Color1);
                            ColorRGBA Color2 = ColorRGBA.HexToRGBA("#ff7000");
                            AdjustLedColors(ref Color2);
                            ColorRGBA Color3 = ColorRGBA.HexToRGBA("#fa0000");
                            AdjustLedColors(ref Color3);
                            ColorRGBA Color4 = ColorRGBA.HexToRGBA("#e80096");
                            AdjustLedColors(ref Color4);
                            ColorRGBA Color5 = ColorRGBA.HexToRGBA("#70009c");
                            AdjustLedColors(ref Color5);
                            ColorRGBA Color6 = ColorRGBA.HexToRGBA("#0000be");
                            AdjustLedColors(ref Color6);
                            ColorRGBA Color7 = ColorRGBA.HexToRGBA("#003ace");
                            AdjustLedColors(ref Color7);
                            ColorRGBA Color8 = ColorRGBA.HexToRGBA("#00bae7");
                            AdjustLedColors(ref Color8);
                            ColorRGBA Color9 = ColorRGBA.HexToRGBA("#004500");
                            AdjustLedColors(ref Color9);
                            ColorRGBA Color10 = ColorRGBA.HexToRGBA("#276800");
                            AdjustLedColors(ref Color10);
                            ColorRGBA Color11 = ColorRGBA.HexToRGBA("#6c9f00");
                            AdjustLedColors(ref Color11);
                            ColorRGBA Color12 = ColorRGBA.HexToRGBA("#93b300");
                            AdjustLedColors(ref Color12);

                            //Rotate color variables
                            int CurrentColor = 0;
                            int CurrentLedRange = 0;
                            int TotalLedRange = setLedCountTotal / 13;

                            //Set the current color to the bytes
                            int CurrentSerialByte = initByteSize;
                            while (CurrentSerialByte < totalByteSize)
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

                                serialBytes[CurrentSerialByte] = Color0.R;
                                CurrentSerialByte++;

                                serialBytes[CurrentSerialByte] = Color0.G;
                                CurrentSerialByte++;

                                serialBytes[CurrentSerialByte] = Color0.B;
                                CurrentSerialByte++;

                                CurrentLedRange++;
                            }

                            //Rotate the color bytes
                            //Debug.WriteLine("Rotating the spectrum color bytes: " + CurrentLedRotate);
                            for (int rotateCount = 0; rotateCount < CurrentLedRotate; rotateCount++)
                            {
                                AVFunctions.MoveByteInArrayLeft(totalByteSize, serialBytes, 3, totalByteSize - 1);
                                AVFunctions.MoveByteInArrayLeft(totalByteSize, serialBytes, 3, totalByteSize - 1);
                                AVFunctions.MoveByteInArrayLeft(totalByteSize, serialBytes, 3, totalByteSize - 1);
                            }

                            //Update the rotate count
                            if (CurrentLedRotate == setLedCountTotal) { CurrentLedRotate = 0; } else { CurrentLedRotate++; }

                            //Set loop delay time
                            LoopDelayMs = setSpectrumRotationSpeed * 1000;
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
                        Debug.WriteLine("Mode color spectrum loop failed: " + ex.Message);
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
                    await ShowFailedConnectionMessage();
                }
            }
            catch { }
        }
    }
}