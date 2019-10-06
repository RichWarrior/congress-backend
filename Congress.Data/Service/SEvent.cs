using System.Collections.Generic;
using System.Linq;
using Congress.Core.Entity;
using Congress.Core.Interface;

namespace Congress.Data.Service
{
    public class SEvent : IEvent
    {
        IDbContext dbContext;
        public SEvent(IDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public List<Event> GetActiveEvents(int userId)
        {
            string sql = @"SELECT * FROM event WHERE endDate >  NOW() AND statusId = 2 AND userId = @userId";
            return dbContext.GetByQueryAll<Event>(sql, new { userId = userId }).ToList();
        }

        public Event GetById(int id)
        {
            return dbContext.GetById<Event>(id);
        }

        public List<Event> GetEvents(int userId)
        {
            string sql = "SELECT * FROM event WHERE userId = @userId AND statusId = 2 ORDER BY startDate DESC";
            return dbContext.GetByQueryAll<Event>(sql, new { userId = userId }).ToList();
        }

        public int Insert(Event _event)
        {
            return dbContext.Insert(_event);
        }

        public bool UpdateEvent(Event _event)
        {
            return dbContext.Update(_event);
        }
    }
}
