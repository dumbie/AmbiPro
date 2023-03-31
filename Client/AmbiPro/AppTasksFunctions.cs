using System.Threading.Tasks;
using static AmbiPro.PreloadSettings;
using static ArnoldVinkCode.AVActions;

namespace AmbiPro
{
    public partial class AppTasks
    {
        public static async Task vTaskLoop_UpdateSettings()
        {
            try
            {
                while (TaskCheckLoop(vTask_UpdateSettings))
                {
                    UpdateSettingsPreload();

                    //Delay the loop task
                    await TaskDelay(2000, vTask_UpdateSettings);
                }
            }
            catch { }
        }
    }
}