using System;
using System.Drawing;
using System.Reflection;
using System.Threading.Tasks;

namespace AmbiPro
{
    partial class SerialMonitor
    {
        //Color loop variables
        private static Color CurrentColor = Color.FromArgb(20, 0, 0);

        //Loop through the set colors
        private static async Task ModeColorLoop(Int32 InitByteSize, byte[] SerialBytes)
        {
            try
            {
                //Update the tray icon
                AppTray.NotifyIcon.Icon = new Icon(Assembly.GetExecutingAssembly().GetManifestResourceStream("AmbiPro.Assets.ApplicationIcon.ico"));

                //Loop color variables
                Int32 ColorLoopState = 0;

                //Current byte information
                while (AppTasks.LedRunning())
                {
                    //Set the used colors
                    if (ColorLoopState == 0) //Red
                    {
                        CurrentColor = Color.FromArgb(CurrentColor.R + 1, CurrentColor.G, CurrentColor.B);
                        if (CurrentColor.G > 0) { CurrentColor = Color.FromArgb(CurrentColor.R, CurrentColor.G - 1, CurrentColor.B); }
                        if (CurrentColor.B > 0) { CurrentColor = Color.FromArgb(CurrentColor.R, CurrentColor.G, CurrentColor.B - 1); }
                        if (CurrentColor.R == 220 && CurrentColor.G == 0 && CurrentColor.B == 0) { ColorLoopState++; }
                    }
                    else if (ColorLoopState == 1)  //Green
                    {
                        CurrentColor = Color.FromArgb(CurrentColor.R, CurrentColor.G + 1, CurrentColor.B);
                        if (CurrentColor.R > 0) { CurrentColor = Color.FromArgb(CurrentColor.R - 1, CurrentColor.G, CurrentColor.B); }
                        if (CurrentColor.B > 0) { CurrentColor = Color.FromArgb(CurrentColor.R, CurrentColor.G, CurrentColor.B - 1); }
                        if (CurrentColor.R == 0 && CurrentColor.G == 220 && CurrentColor.B == 0) { ColorLoopState++; }
                    }
                    else if (ColorLoopState == 2) //Blue
                    {
                        CurrentColor = Color.FromArgb(CurrentColor.R, CurrentColor.G, CurrentColor.B + 1);
                        if (CurrentColor.G > 0) { CurrentColor = Color.FromArgb(CurrentColor.R, CurrentColor.G - 1, CurrentColor.B); }
                        if (CurrentColor.R > 0) { CurrentColor = Color.FromArgb(CurrentColor.R - 1, CurrentColor.G, CurrentColor.B); }
                        if (CurrentColor.R == 0 && CurrentColor.G == 0 && CurrentColor.B == 220) { ColorLoopState++; }
                    }
                    if (ColorLoopState == 3) { ColorLoopState = 0; }
                    Color AdjustedColor = AdjustLedColors(CurrentColor);

                    //Set the current color to the bytes
                    Int32 CurrentSerialByte = InitByteSize;
                    while (CurrentSerialByte < SerialBytes.Length)
                    {
                        SerialBytes[CurrentSerialByte] = AdjustedColor.R;
                        CurrentSerialByte++;

                        SerialBytes[CurrentSerialByte] = AdjustedColor.G;
                        CurrentSerialByte++;

                        SerialBytes[CurrentSerialByte] = AdjustedColor.B;
                        CurrentSerialByte++;
                    }

                    //Send the serial bytes to device
                    //Debug.WriteLine("Serial bytes sended: " + SerialBytes.Length);
                    vSerialComPort.Write(SerialBytes, 0, SerialBytes.Length);

                    await Task.Delay(setColorLoopSpeed);
                }
            }
            catch { }
        }
    }
}