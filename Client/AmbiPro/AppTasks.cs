using System.Threading.Tasks;
using static ArnoldVinkCode.AVActions;

namespace AmbiPro
{
    public partial class AppTasks
    {
        //Application Tasks
        public static AVTaskDetails vTask_UpdateLed = new AVTaskDetails("vTask_UpdateLed");
        public static AVTaskDetails vTask_UpdateSettings = new AVTaskDetails("vTask_UpdateSettings");
        public static AVTaskDetails vTask_UpdateStatus = new AVTaskDetails("vTask_UpdateStatus");

        //Start all the background tasks
        public static void TasksBackgroundStart()
        {
            try
            {
                TaskStartLoop(vTaskLoop_UpdateSettings, vTask_UpdateSettings);
                TaskStartLoop(vTaskLoop_UpdateStatus, vTask_UpdateStatus);
            }
            catch { }
        }

        //Stop all the background tasks
        public static async Task TasksBackgroundStop()
        {
            try
            {
                await TaskStopLoop(vTask_UpdateSettings, 5000);
                await TaskStopLoop(vTask_UpdateStatus, 5000);
            }
            catch { }
        }
    }
}