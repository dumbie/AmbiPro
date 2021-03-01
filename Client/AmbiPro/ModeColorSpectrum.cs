using ArnoldVinkCode;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Threading.Tasks;
using static AmbiPro.AppTasks;
using static ArnoldVinkCode.AVActions;

namespace AmbiPro
{
    partial class SerialMonitor
    {
        //Rotating color spectrum
        private static async Task ModeColorSpectrum(int InitByteSize, byte[] SerialBytes)
        {
            try
            {
                int? PreviousLedOutput = null;
                double? PreviousBrightness = null;
                DateTime PreviousRotation = DateTime.Now;

                //Update the tray icon
                AppTray.NotifyIcon.Icon = new Icon(Assembly.GetEntryAssembly().GetManifestResourceStream("AmbiPro.Assets.ApplicationIcon.ico"));

                //Current byte information
                while (!vTask_LedUpdate.TaskStopRequest)
                {
                    //Reset the colors when brightness has changed.
                    if (PreviousBrightness != setLedBrightness || PreviousLedOutput != setLedOutput)
                    {
                        //Update previous variables
                        PreviousBrightness = setLedBrightness;
                        PreviousLedOutput = setLedOutput;

                        //Rotate color variables
                        int UsedColors = 0;
                        int CurrentLedRange = 0;
                        int TotalLedRange = setLedCount / 13;

                        //Set the used colors and adjust them
                        Color CurrentColor = ColorTranslator.FromHtml("#f8d000"); CurrentColor = AdjustLedColors(CurrentColor);
                        Color Color1 = ColorTranslator.FromHtml("#ffb000"); Color1 = AdjustLedColors(Color1);
                        Color Color2 = ColorTranslator.FromHtml("#ff7000"); Color2 = AdjustLedColors(Color2);
                        Color Color3 = ColorTranslator.FromHtml("#fa0000"); Color3 = AdjustLedColors(Color3);
                        Color Color4 = ColorTranslator.FromHtml("#e80096"); Color4 = AdjustLedColors(Color4);
                        Color Color5 = ColorTranslator.FromHtml("#70009c"); Color5 = AdjustLedColors(Color5);
                        Color Color6 = ColorTranslator.FromHtml("#0000be"); Color6 = AdjustLedColors(Color6);
                        Color Color7 = ColorTranslator.FromHtml("#003ace"); Color7 = AdjustLedColors(Color7);
                        Color Color8 = ColorTranslator.FromHtml("#00bae7"); Color8 = AdjustLedColors(Color8);
                        Color Color9 = ColorTranslator.FromHtml("#004500"); Color9 = AdjustLedColors(Color9);
                        Color Color10 = ColorTranslator.FromHtml("#276800"); Color10 = AdjustLedColors(Color10);
                        Color Color11 = ColorTranslator.FromHtml("#6c9f00"); Color11 = AdjustLedColors(Color11);
                        Color Color12 = ColorTranslator.FromHtml("#93b300"); Color12 = AdjustLedColors(Color12);

                        //Set the current color to the bytes
                        int CurrentSerialByte = InitByteSize;
                        while (CurrentSerialByte < SerialBytes.Length)
                        {
                            //Check if the next color has been reached
                            if (CurrentLedRange == TotalLedRange)
                            {
                                if (UsedColors == 0) { CurrentColor = Color1; }
                                else if (UsedColors == 1) { CurrentColor = Color2; }
                                else if (UsedColors == 2) { CurrentColor = Color3; }
                                else if (UsedColors == 3) { CurrentColor = Color4; }
                                else if (UsedColors == 4) { CurrentColor = Color5; }
                                else if (UsedColors == 5) { CurrentColor = Color6; }
                                else if (UsedColors == 6) { CurrentColor = Color7; }
                                else if (UsedColors == 7) { CurrentColor = Color8; }
                                else if (UsedColors == 8) { CurrentColor = Color9; }
                                else if (UsedColors == 9) { CurrentColor = Color10; }
                                else if (UsedColors == 10) { CurrentColor = Color11; }
                                else if (UsedColors == 11) { CurrentColor = Color12; }
                                CurrentLedRange = 0;
                                UsedColors++;
                            }

                            SerialBytes[CurrentSerialByte] = CurrentColor.R;
                            CurrentSerialByte++;

                            SerialBytes[CurrentSerialByte] = CurrentColor.G;
                            CurrentSerialByte++;

                            SerialBytes[CurrentSerialByte] = CurrentColor.B;
                            CurrentSerialByte++;

                            CurrentLedRange += 1;
                        }

                        Debug.WriteLine("Resetting the spectrum color bytes.");
                    }

                    //Rotate the previous color bytes
                    if (DateTime.Now.Subtract(PreviousRotation).TotalSeconds >= setSpectrumRotationSpeed)
                    {
                        //Debug.WriteLine("Rotating the spectrum color bytes.");

                        AVFunctions.MoveByteInArrayLeft(SerialBytes, 3, SerialBytes.Length - 1);
                        AVFunctions.MoveByteInArrayLeft(SerialBytes, 3, SerialBytes.Length - 1);
                        AVFunctions.MoveByteInArrayLeft(SerialBytes, 3, SerialBytes.Length - 1);

                        PreviousRotation = DateTime.Now;
                    }

                    //Send the serial bytes to device
                    //Debug.WriteLine("Serial bytes sended: " + SerialBytes.Length);
                    vSerialComPort.Write(SerialBytes, 0, SerialBytes.Length);

                    //Delay the loop task
                    await TaskDelayLoop(1000, vTask_LedUpdate);
                }
            }
            catch { }
        }
    }
}