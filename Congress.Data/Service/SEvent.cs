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

        public List<Event> FilterEvent(int cityId, int countryId, int categoryId)
        {
            string sql = @"SELECT * FROM event e
            INNER JOIN eventcategory ec ON ec.eventId = e.id
            WHERE e.statusId = 2 AND e.endDate > NOW() AND e.countryId = @countryId AND e.cityId = @cityId AND ec.categoryId = @categoryId AND ec.statusId = 2";
            return dbContext.GetByQueryAll<Event>(sql, new
            {
                cityId = cityId,
                countryId = countryId,
                categoryId = categoryId
            }).ToList();
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

        public List<Event> GetEvents(string sql, object _params)
        {
            return dbContext.GetByQueryAll<Event>(sql, _params).ToList();
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
