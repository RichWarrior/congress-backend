using Congress.Core.Entity;
using Congress.Core.Interface;
using System.Collections.Generic;
using System.Linq;

namespace Congress.Data.Service
{
    public class SEventDetail : IEventDetail
    {
        IDbContext dbContext;
        public SEventDetail(IDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public List<EventDetail> GetEventDetails(int eventId)
        {
            string sql = @"SELECT * FROM eventdetail WHERE statusId = 2 AND eventId = @eventId ORDER BY day,startTime ASC";
            return dbContext.GetByQueryAll<EventDetail>(sql, new { eventId = eventId}).ToList();
        }

        public int InsertEventDetail(EventDetail eventDetail)
        {
            return dbContext.Insert(eventDetail);
        }

        public bool UpdateEventDetail(EventDetail eventDetail)
        {
            return dbContext.Update(eventDetail);
        }
    }
}
