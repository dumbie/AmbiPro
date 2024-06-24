using System.Collections.Generic;
using System.Diagnostics;
using static AmbiPro.AppEnums;
using static AmbiPro.AppVariables;
using static AmbiPro.SerialMonitor;
using static ArnoldVinkCode.AVClasses;
using static ArnoldVinkCode.AVInputOutputClass;
using static ArnoldVinkCode.AVInputOutputHotkey;
using static ArnoldVinkCode.AVSettings;

namespace AmbiPro
{
    public partial class AppHotkeys
    {
        public static async void EventHotkeyPressed(List<KeysVirtual> keysPressed)
        {
            try
            {
                foreach (ShortcutTriggerKeyboard shortcutTrigger in vShortcutTriggers)
                {
                    if (shortcutTrigger.Name == "SwitchLedsOnOff")
                    {
                        if (CheckHotkeyPress(keysPressed, shortcutTrigger.Trigger))
                        {
                            Debug.WriteLine("Button Global - SwitchLedsOnOff");
                            await LedSwitch(LedSwitches.Automatic);
                            return;
                        }
                    }
                    else if (shortcutTrigger.Name == "ModeScreenCapture")
                    {
                        if (CheckHotkeyPress(keysPressed, shortcutTrigger.Trigger))
                        {
                            Debug.WriteLine("Button Global - ModeScreenCapture");
                            SettingSave(vConfiguration, "LedMode", "0");
                            await LedSwitch(LedSwitches.Restart);
                            return;
                        }
                    }
                    else if (shortcutTrigger.Name == "ModeSolidColor")
                    {
                        if (CheckHotkeyPress(keysPressed, shortcutTrigger.Trigger))
                        {
                            Debug.WriteLine("Button Global - ModeSolidColor");
                            SettingSave(vConfiguration, "LedMode", "1");
                            await LedSwitch(LedSwitches.Restart);
                            return;
                        }
                    }
                }
            }
            catch { }
        }
    }
}