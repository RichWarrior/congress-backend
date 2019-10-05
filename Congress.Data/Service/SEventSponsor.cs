using Congress.Core.Entity;
using Congress.Core.Interface;
using System.Collections.Generic;
using System.Linq;

namespace Congress.Data.Service
{
    public class SEventSponsor : IEventSponsor
    {
        IDbContext dbContext;
        public SEventSponsor(IDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public bool BulkInsertSponsor(List<EventSponsor> eventSponsors)
        {
            return dbContext.BulkInsert(eventSponsors);
        }

        public bool DeleteEventSponsor(int eventId, int sponsorId)
        {
            string sql = @"UPDATE eventsponsor SET statusId = 1 WHERE eventId = @eventId AND sponsorId= @sponsorId";
            return dbContext.ExecuteQuery(sql, new { eventId = eventId, sponsorId = sponsorId });
        }

        public List<Sponsor> GetEventSponsors(int eventId)
        {
            string sql = @"SELECT s.* FROM sponsor s
            INNER JOIN eventsponsor es ON es.sponsorId = s.id
            WHERE es.eventId = @eventId AND es.statusId = 2";
            return dbContext.GetByQueryAll<Sponsor>(sql, new { eventId = eventId }).ToList();
        }
    }
}
