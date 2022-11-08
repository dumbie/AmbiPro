using System;
using System.Diagnostics;
using static AmbiPro.PreloadSettings;

namespace AmbiPro
{
    public partial class SerialMonitor
    {
        //Adjust to led energy mode
        private static void AdjustLedEnergyMode(int totalByteSize, byte[] serialBytes)
        {
            try
            {
                if (setLedEnergyMode)
                {
                    int CurrentSerialByte = 3;
                    int CurrentLedEnergySkip = 6;
                    while (CurrentSerialByte < totalByteSize)
                    {
                        if (CurrentSerialByte == CurrentLedEnergySkip)
                        {
                            serialBytes[CurrentSerialByte] = 0;
                            CurrentSerialByte++;
                            serialBytes[CurrentSerialByte] = 0;
                            CurrentSerialByte++;
                            serialBytes[CurrentSerialByte] = 0;
                            CurrentSerialByte++;
                            CurrentLedEnergySkip += 6;
                        }
                        else
                        {
                            CurrentSerialByte += 3;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to adjust led energy mode: " + ex.Message);
            }
        }
    }
}