using System.Collections.Generic;
using System.Linq;
using Congress.Core.Entity;
using Congress.Core.Interface;

namespace Congress.Data.Service
{
    public class SEventParticipant : IEventParticipant
    {
        IDbContext dbContext;
        public SEventParticipant(IDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public bool BulkInsertParticipants(List<EventParticipant> eventParticipants)
        {
            return dbContext.BulkInsert(eventParticipants);
        }

        public List<User> EventNotInUser(int eventId)
        {
            string sql = @"SELECT u.* FROM user u
            WHERE u.statusId = 2 AND u.userTypeId = 3 AND u.id NOT IN (SELECT userId FROM eventparticipant ep WHERE ep.eventId = @eventId AND ep.statusId = 2)";
            return dbContext.GetByQueryAll<User>(sql, new { eventId = eventId }).ToList();
        }

        public List<User> GetEventParticipants(int eventId)
        {
            string sql = @"SELECT u.id,u.email,u.name,u.surname FROM user u WHERE u.id IN (SELECT ep.userId FROM eventparticipant ep WHERE ep.eventId = @eventId AND ep.statusId = 2)";
            return dbContext.GetByQueryAll<User>(sql, new { eventId = eventId }).ToList();
        }

        public bool DeleteEventParticipant(int eventId,int userId)
        {
            string sql = "UPDATE eventparticipant SET statusId = 1 WHERE eventId = @eventId AND userId = @userId";
            return dbContext.ExecuteQuery(sql, new { eventId = eventId, userId = userId });
        }
    }
}
