using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
                ShortcutTriggerKeyboard shortcutTrigger = vShortcutTriggers.Where(x => x.Name == "SwitchLedsOnOff").FirstOrDefault();
                if (shortcutTrigger != null)
                {
                    if (CheckHotkeyPress(keysPressed, shortcutTrigger.Trigger))
                    {
                        Debug.WriteLine("Button Global - SwitchLedsOnOff");
                        await LedSwitch(LedSwitches.Automatic);
                        return;
                    }
                }

                shortcutTrigger = vShortcutTriggers.Where(x => x.Name == "ModeScreenCapture").FirstOrDefault();
                if (shortcutTrigger != null)
                {
                    if (CheckHotkeyPress(keysPressed, shortcutTrigger.Trigger))
                    {
                        Debug.WriteLine("Button Global - ModeScreenCapture");
                        SettingSave(vConfiguration, "LedMode", "0");
                        await LedSwitch(LedSwitches.Restart);
                        return;
                    }
                }

                shortcutTrigger = vShortcutTriggers.Where(x => x.Name == "ModeSolidColor").FirstOrDefault();
                if (shortcutTrigger != null)
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
            catch { }
        }
    }
}