using System.Collections.Generic;
using System.Linq;
using Congress.Core.Entity;
using Congress.Core.Interface;

namespace Congress.Data.Service
{
    public class SUser : IUser
    {
        IDbContext dbContext;
        public SUser(IDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public bool BulkUserInsert(List<User> users)
        {
            return dbContext.BulkInsert<User>(users);
        }

        public User CheckUser(string email)
        {
            string sql = "SELECT * FROM user WHERE (email = @email OR identityNr = @identityNr) AND statusId =2";
            return dbContext.GetByQuery<User>(sql, new { email = email });
        }

        public List<User> GetAllUser()
        {
            string sql = "SELECT * FROM user WHERE userTypeId = 3 AND statusId = 2";
            return dbContext.GetByQueryAll<User>(sql, new { }).ToList();
        }

        public List<User> GetBusiness()
        {
            string sql = "SELECT * FROM user WHERE userTypeId = 2 AND statusId = 2 ORDER BY creationDate DESC";
            return dbContext.GetByQueryAll<User>(sql, new { }).ToList();
        }

        public User GetById(int id)
        {
            return dbContext.GetById<User>(id);
        }

        public List<User> GetParticipant()
        {
            string sql = "SELECT * FROM user WHERE userTypeId = 3 ORDER BY creationDate DESC";
            return dbContext.GetByQueryAll<User>(sql,new { }).ToList();
        }

        public List<Event> GetUserParticipantEvents(int userId)
        {
            string sql = @"SELECT e.* FROM eventparticipant ep
            INNER JOIN event e ON e.id = ep.eventId
            WHERE ep.statusId = 2 AND ep.userId= @userId";
            return dbContext.GetByQueryAll<Event>(sql, new { userId = userId }).ToList();
        }

        public int InsertUser(User user)
        {
            return dbContext.Insert(user);
        }

        public User Login(string email, string password)
        {
            string sql = @"SELECT * FROM user WHERE email = @email AND password = @password";
            return dbContext.GetByQuery<User>(sql, new { email = email, password = password });
        }

        public bool UpdateUser(User user)
        {
            return dbContext.Update(user);
        }
    }
}
