using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using static AmbiPro.AppEnums;
using static AmbiPro.AppVariables;
using static AmbiPro.SerialMonitor;
using static ArnoldVinkCode.AVInputOutputClass;
using static ArnoldVinkCode.AVSettings;

namespace AmbiPro
{
    public partial class AppHotKey
    {
        public static async void EventHotKeyPressed(List<KeysVirtual> keysPressed)
        {
            try
            {
                List<KeysVirtual> usedKeysSwitchOnOff = new List<KeysVirtual>
                {
                    KeysVirtual.WindowsLeft,
                    KeysVirtual.F12
                };
                List<KeysVirtual> usedKeysModeScreenCapture = new List<KeysVirtual>
                {
                    KeysVirtual.WindowsLeft,
                    KeysVirtual.F11
                };
                List<KeysVirtual> usedKeysModeSolid = new List<KeysVirtual>
                {
                    KeysVirtual.WindowsLeft,
                    KeysVirtual.F10
                };

                if (usedKeysSwitchOnOff.Count > 0 && !usedKeysSwitchOnOff.Where(x => x != KeysVirtual.None).Except(keysPressed).Any())
                {
                    if (SettingLoad(vConfiguration, "ShortcutSwitchOnOff", typeof(bool)))
                    {
                        Debug.WriteLine("Button Global - Windows + F12");
                        await LedSwitch(LedSwitches.Automatic);
                    }
                }
                else if (usedKeysModeScreenCapture.Count > 0 && !usedKeysModeScreenCapture.Where(x => x != KeysVirtual.None).Except(keysPressed).Any())
                {
                    if (SettingLoad(vConfiguration, "ShortcutModeScreenCapture", typeof(bool)))
                    {
                        Debug.WriteLine("Button Global - Windows + F11");
                        SettingSave(vConfiguration, "LedMode", "0");
                        await LedSwitch(LedSwitches.Restart);
                    }
                }
                else if (usedKeysModeSolid.Count > 0 && !usedKeysModeSolid.Where(x => x != KeysVirtual.None).Except(keysPressed).Any())
                {
                    if (SettingLoad(vConfiguration, "ShortcutModeSolidColor", typeof(bool)))
                    {
                        Debug.WriteLine("Button Global - Windows + F10");
                        SettingSave(vConfiguration, "LedMode", "1");
                        await LedSwitch(LedSwitches.Restart);
                    }
                }
            }
            catch { }
        }
    }
}