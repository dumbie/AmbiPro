using ArnoldVinkCode;
using System;
using System.Diagnostics;
using System.Linq;
using static AmbiPro.AppVariables;
using static ArnoldVinkCode.AVClasses;

namespace AmbiPro.Settings
{
    public partial class FormSettings
    {
        //Save - Application Shortcuts
        public void ShortcutsSave()
        {
            try
            {
                Debug.WriteLine("Saving application shortcuts...");

                hotkey_SwitchLedsOnOff.TriggerChanged += (triggers) =>
                {
                    ShortcutTriggerKeyboard shortcutTrigger = vShortcutTriggers.Where(x => x.Name == "SwitchLedsOnOff").FirstOrDefault();
                    if (shortcutTrigger != null)
                    {
                        shortcutTrigger.Trigger = triggers;
                        AVJsonFunctions.JsonSaveObject(vShortcutTriggers, @"Profiles\ShortcutKeyboard.json");
                    }
                };
                hotkey_ModeScreenCapture.TriggerChanged += (triggers) =>
                {
                    ShortcutTriggerKeyboard shortcutTrigger = vShortcutTriggers.Where(x => x.Name == "ModeScreenCapture").FirstOrDefault();
                    if (shortcutTrigger != null)
                    {
                        shortcutTrigger.Trigger = triggers;
                        AVJsonFunctions.JsonSaveObject(vShortcutTriggers, @"Profiles\ShortcutKeyboard.json");
                    }
                };
                hotkey_ModeSolidColor.TriggerChanged += (triggers) =>
                {
                    ShortcutTriggerKeyboard shortcutTrigger = vShortcutTriggers.Where(x => x.Name == "ModeSolidColor").FirstOrDefault();
                    if (shortcutTrigger != null)
                    {
                        shortcutTrigger.Trigger = triggers;
                        AVJsonFunctions.JsonSaveObject(vShortcutTriggers, @"Profiles\ShortcutKeyboard.json");
                    }
                };
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to save application shortcuts: " + ex.Message);
            }
        }
    }
}