using ArnoldVinkCode;
using System;
using System.Diagnostics;
using static AmbiPro.AppVariables;
using static ArnoldVinkCode.AVClasses;
using static ArnoldVinkCode.AVJsonFunctions;

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

                keyboard_SwitchLedsOnOff.TriggerChanged += Shortcut_Keyboard_TriggerChanged;
                keyboard_ModeScreenCapture.TriggerChanged += Shortcut_Keyboard_TriggerChanged;
                keyboard_ModeSolidColor.TriggerChanged += Shortcut_Keyboard_TriggerChanged;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to save application shortcuts: " + ex.Message);
            }
        }

        void Shortcut_Keyboard_TriggerChanged(ShortcutTriggerKeyboard triggers)
        {
            try
            {
                if (vShortcutTriggers.ListReplaceFirstItem(x => x.Name == triggers.Name, triggers))
                {
                    JsonSaveObject(vShortcutTriggers, @"Profiles\ShortcutKeyboard.json");
                }
            }
            catch { }
        }
    }
}