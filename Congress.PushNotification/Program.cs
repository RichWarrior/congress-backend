using System.Threading;

namespace Congress.PushNotification
{
    class Program
    {
        static void Main(string[] args)
        {
            ThreadPool.SetMinThreads(10, 50);
            while (true)
            {
                Thread.Sleep(2000);
            }
        }
    }
}
