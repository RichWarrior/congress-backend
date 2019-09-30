using System.Threading;

namespace Congress.TaskSchedule
{
    class Program
    {
        static void Main(string[] args)
        {
            ThreadPool.SetMinThreads(10, 50);
            SponsorSchedule sponsorSchedule = new SponsorSchedule();
            while (true)
            {
                Thread.Sleep(1250);
            }
        }
    }
}
