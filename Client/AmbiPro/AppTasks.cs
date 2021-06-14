using System.Threading.Tasks;
using static ArnoldVinkCode.AVActions;

namespace AmbiPro
{
    partial class SerialMonitor
    {
        //Application Tasks
        public static AVTaskDetails vTask_UpdateLed = new AVTaskDetails();
        public static AVTaskDetails vTask_UpdateScreenStatus = new AVTaskDetails();

        //Start all the background tasks
        public static void TasksBackgroundStart()
        {
            try
            {
                TaskStartLoop(vTaskLoop_UpdateScreenStatus, vTask_UpdateScreenStatus);
            }
            catch { }
        }

        //Stop all the background tasks
        public static async Task TasksBackgroundStop()
        {
            try
            {
                await TaskStopLoop(vTask_UpdateScreenStatus);
            }
            catch { }
        }
    }
}