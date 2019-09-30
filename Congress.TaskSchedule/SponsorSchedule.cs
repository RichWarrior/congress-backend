using Congress.Core.Interface;
using Congress.Data.Data;
using Congress.Data.Service;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Congress.TaskSchedule
{
    public class SponsorSchedule
    {
        private Timer timer;
        private object lockObject;
        private ISponsor _SSponsor;

        public SponsorSchedule()
        {
            lockObject = new object();
            timer = new Timer(SponsorTimerDoWork,lockObject,TimeSpan.Zero,TimeSpan.FromSeconds(5));
            _SSponsor = new SSponsor(new DbContext());
        }

        private void SponsorTimerDoWork(object state)
        {
            if (!Monitor.TryEnter(state))
                return;
            Console.WriteLine("Timer Do Work");
            Monitor.Exit(state);
        }
    }
}
