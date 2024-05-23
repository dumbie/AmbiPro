using System;
using System.Diagnostics;
using System.Linq;
using static AmbiPro.AppVariables;
using static ArnoldVinkCode.AVClasses;

namespace AmbiPro.Settings
{
    public partial class FormSettings
    {
        public void ShortcutsLoad()
        {
            try
            {
                Debug.WriteLine("Loading application shortcuts...");

                ShortcutTriggerKeyboard shortcutTrigger = vShortcutTriggers.Where(x => x.Name == "SwitchLedsOnOff").FirstOrDefault();
                if (shortcutTrigger != null)
                {
                    hotkey_SwitchLedsOnOff.Set(shortcutTrigger.Trigger);
                }
                shortcutTrigger = vShortcutTriggers.Where(x => x.Name == "ModeScreenCapture").FirstOrDefault();
                if (shortcutTrigger != null)
                {
                    hotkey_ModeScreenCapture.Set(shortcutTrigger.Trigger);
                }
                shortcutTrigger = vShortcutTriggers.Where(x => x.Name == "ModeSolidColor").FirstOrDefault();
                if (shortcutTrigger != null)
                {
                    hotkey_ModeSolidColor.Set(shortcutTrigger.Trigger);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to load application shortcuts: " + ex.Message);
            }
        }
    }
}