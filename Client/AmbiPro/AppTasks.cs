using System.Threading;
using System.Threading.Tasks;

namespace AmbiPro
{
    class AppTasks
    {
        //Application Tasks
        public static Task LedTask;
        public static CancellationTokenSource LedToken;
        public static bool LedRunning() { return LedToken != null && !LedToken.IsCancellationRequested;} 

        public static Task SocketServerTask;
        public static CancellationTokenSource SocketServerToken;
        public static bool SocketServerRunning() { return SocketServerToken != null && !SocketServerToken.IsCancellationRequested; }
    }
}