using System.Collections.Generic;
using System.Linq;
using Congress.Core.Entity;
using Congress.Core.Interface;

namespace Congress.Data.Service
{
    public class SSponsor : ISponsor
    {
        IDbContext dbContext;
        public SSponsor(IDbContext dbContext)
        {
            this.dbContext = dbContext;
        }        
        public int Insert(Sponsor sponsor)
        {
            return dbContext.Insert(sponsor);
        }
        public List<Sponsor> GetSponsors()
        {
            string sql = @"SELECT * FROM sponsor WHERE statusId<>1";
            return dbContext.GetByQueryAll<Sponsor>(sql, new { }).ToList();
        }

        public bool UpdateSponsor(Sponsor sponsor)
        {
            return dbContext.Update(sponsor);
        }

        public List<Sponsor> GetEventAvailableSponsor(int eventId)
        {
            string sql = @"SELECT * FROM sponsor s WHERE s.id NOT IN (SELECT sponsorId FROM eventsponsor WHERE eventId = @eventId AND statusId = 2) AND s.statusId <> 1";
            return dbContext.GetByQueryAll<Sponsor>(sql, new { eventId = eventId }).ToList();
        }
    }
}
