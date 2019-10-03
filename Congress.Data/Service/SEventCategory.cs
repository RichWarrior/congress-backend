using Congress.Core.Entity;
using Congress.Core.Interface;
using System.Collections.Generic;
using System.Linq;

namespace Congress.Data.Service
{
    public class SEventCategory : IEventCategory
    {
        IDbContext dbContext;
        public SEventCategory(IDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public List<EventCategory> GetEventCategories(int eventId)
        {
            string sql = @"SELECT eg.*,c.name 'categoryName' FROM eventcategory eg
            INNER JOIN category c ON c.id = eg.categoryId
            WHERE eg.statusId = 2 AND eg.eventId = @eventId";
            return dbContext.GetByQueryAll<EventCategory>(sql, new { eventId = eventId }).ToList();
        }

        public bool InsertEventCategories(List<EventCategory> eventCategories)
        {
            return dbContext.BulkInsert(eventCategories);
        }

        public bool UpdateEventCategory(EventCategory eventCategory)
        {
            return dbContext.Update(eventCategory);
        }
    }
}
