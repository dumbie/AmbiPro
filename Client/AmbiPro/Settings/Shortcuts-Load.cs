using System;
using System.Diagnostics;
using System.Linq;
using static AmbiPro.AppVariables;

namespace AmbiPro.Settings
{
    public partial class FormSettings
    {
        public void ShortcutsLoad()
        {
            try
            {
                Debug.WriteLine("Loading application shortcuts...");

                keyboard_SwitchLedsOnOff.Set(vShortcutTriggers.FirstOrDefault(x => x.Name == keyboard_SwitchLedsOnOff.TriggerName));
                keyboard_ModeScreenCapture.Set(vShortcutTriggers.FirstOrDefault(x => x.Name == keyboard_ModeScreenCapture.TriggerName));
                keyboard_ModeSolidColor.Set(vShortcutTriggers.FirstOrDefault(x => x.Name == keyboard_ModeSolidColor.TriggerName));
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to load application shortcuts: " + ex.Message);
            }
        }
    }
}