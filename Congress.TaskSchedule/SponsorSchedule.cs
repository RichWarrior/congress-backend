using Congress.Core.Entity;
using Congress.Core.Interface;
using Congress.Data.Data;
using Congress.Data.Service;
using System;
using System.Collections.Generic;
using System.Linq;
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
            timer = new Timer(SponsorTimerDoWork,lockObject,TimeSpan.Zero,TimeSpan.FromDays(1));
            _SSponsor = new SSponsor(new DbContext());
        }

        private void SponsorTimerDoWork(object state)
        {
            if (!Monitor.TryEnter(state))
                return;
            List<Sponsor> sponsors = _SSponsor.GetSponsors();
            List<Sponsor> getWaitingActivation = sponsors.Where(x => x.statusId == 3).ToList();
            foreach (var item in getWaitingActivation)
            {
                item.statusId = 2;
                if (!_SSponsor.UpdateSponsor(item))
                {
                    Console.WriteLine("Sponsor Güncellenemedi");
                }                
            }
            Monitor.Exit(state);
        }
    }
}
