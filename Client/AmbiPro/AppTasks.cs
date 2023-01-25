using System.Threading.Tasks;
using static ArnoldVinkCode.AVActions;

namespace AmbiPro
{
    public partial class AppTasks
    {
        //Application Tasks
        public static AVTaskDetails vTask_UpdateLed = new AVTaskDetails("vTask_UpdateLed");
        public static AVTaskDetails vTask_UpdateSettings = new AVTaskDetails("vTask_UpdateSettings");

        //Start all the background tasks
        public static void TasksBackgroundStart()
        {
            try
            {
                TaskStartLoop(vTaskLoop_UpdateSettings, vTask_UpdateSettings);
            }
            catch { }
        }

        //Stop all the background tasks
        public static async Task TasksBackgroundStop()
        {
            try
            {
                await TaskStopLoop(vTask_UpdateSettings, 5000);
            }
            catch { }
        }
    }
}