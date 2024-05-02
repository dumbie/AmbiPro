using System.Collections.Generic;
using System.Diagnostics;
using static AmbiPro.AppEnums;
using static AmbiPro.AppVariables;
using static AmbiPro.SerialMonitor;
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
                //Check hotkeys
                List<KeysVirtual> usedKeysSwitchLedsOnOff = new List<KeysVirtual>
                {
                    (KeysVirtual)SettingLoad(vConfiguration, "Hotkey0SwitchLedsOnOff", typeof(byte)),
                    (KeysVirtual)SettingLoad(vConfiguration, "Hotkey1SwitchLedsOnOff", typeof(byte)),
                    (KeysVirtual)SettingLoad(vConfiguration, "Hotkey2SwitchLedsOnOff", typeof(byte))
                };
                List<KeysVirtual> usedKeysModeScreenCapture = new List<KeysVirtual>
                {
                    (KeysVirtual)SettingLoad(vConfiguration, "Hotkey0ModeScreenCapture", typeof(byte)),
                    (KeysVirtual)SettingLoad(vConfiguration, "Hotkey1ModeScreenCapture", typeof(byte)),
                    (KeysVirtual)SettingLoad(vConfiguration, "Hotkey2ModeScreenCapture", typeof(byte))
                };
                List<KeysVirtual> usedKeysModeSolidColor = new List<KeysVirtual>
                {
                    (KeysVirtual)SettingLoad(vConfiguration, "Hotkey0ModeSolidColor", typeof(byte)),
                    (KeysVirtual)SettingLoad(vConfiguration, "Hotkey1ModeSolidColor", typeof(byte)),
                    (KeysVirtual)SettingLoad(vConfiguration, "Hotkey2ModeSolidColor", typeof(byte))
                };

                //Check presses
                if (CheckHotkeyPress(keysPressed, usedKeysSwitchLedsOnOff))
                {
                    Debug.WriteLine("Button Global - SwitchLedsOnOff");
                    await LedSwitch(LedSwitches.Automatic);
                }
                else if (CheckHotkeyPress(keysPressed, usedKeysModeScreenCapture))
                {
                    Debug.WriteLine("Button Global - ModeScreenCapture");
                    SettingSave(vConfiguration, "LedMode", "0");
                    await LedSwitch(LedSwitches.Restart);
                }
                else if (CheckHotkeyPress(keysPressed, usedKeysModeSolidColor))
                {
                    Debug.WriteLine("Button Global - ModeSolidColor");
                    SettingSave(vConfiguration, "LedMode", "1");
                    await LedSwitch(LedSwitches.Restart);
                }
            }
            catch { }
        }
    }
}