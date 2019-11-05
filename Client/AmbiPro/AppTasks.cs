using System.Threading;
using System.Threading.Tasks;

namespace AmbiPro
{
    class AppTasks
    {
        //Application Tasks
        public static Task LedTask;
        public static CancellationTokenSource LedToken;
    }
}