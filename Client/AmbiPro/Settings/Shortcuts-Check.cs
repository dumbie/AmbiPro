using ArnoldVinkCode;
using System;
using System.Diagnostics;
using System.Linq;
using static AmbiPro.AppVariables;
using static ArnoldVinkCode.AVClasses;
using static ArnoldVinkCode.AVInputOutputClass;

namespace AmbiPro.Settings
{
    public partial class FormSettings
    {
        public void ShortcutsCheck()
        {
            try
            {
                Debug.WriteLine("Checking application shortcuts...");

                if (!vShortcutTriggers.Any(x => x.Name == "SwitchLedsOnOff"))
                {
                    ShortcutTriggerKeyboard shortcutTrigger = new ShortcutTriggerKeyboard();
                    shortcutTrigger.Name = "SwitchLedsOnOff";
                    shortcutTrigger.Trigger = [KeysVirtual.WindowsLeft, KeysVirtual.None, KeysVirtual.F12];
                    vShortcutTriggers.Add(shortcutTrigger);
                    AVJsonFunctions.JsonSaveObject(vShortcutTriggers, @"Profiles\ShortcutKeyboard.json");
                }
                if (!vShortcutTriggers.Any(x => x.Name == "ModeScreenCapture"))
                {
                    ShortcutTriggerKeyboard shortcutTrigger = new ShortcutTriggerKeyboard();
                    shortcutTrigger.Name = "ModeScreenCapture";
                    shortcutTrigger.Trigger = [KeysVirtual.WindowsLeft, KeysVirtual.None, KeysVirtual.F11];
                    vShortcutTriggers.Add(shortcutTrigger);
                    AVJsonFunctions.JsonSaveObject(vShortcutTriggers, @"Profiles\ShortcutKeyboard.json");
                }
                if (!vShortcutTriggers.Any(x => x.Name == "ModeSolidColor"))
                {
                    ShortcutTriggerKeyboard shortcutTrigger = new ShortcutTriggerKeyboard();
                    shortcutTrigger.Name = "ModeSolidColor";
                    shortcutTrigger.Trigger = [KeysVirtual.WindowsLeft, KeysVirtual.None, KeysVirtual.F10];
                    vShortcutTriggers.Add(shortcutTrigger);
                    AVJsonFunctions.JsonSaveObject(vShortcutTriggers, @"Profiles\ShortcutKeyboard.json");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to check application shortcuts: " + ex.Message);
            }
        }
    }
}