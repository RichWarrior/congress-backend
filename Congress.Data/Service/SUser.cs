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
        public User CheckUser(string email, string identityNr)
        {
            string sql = "SELECT * FROM user WHERE (email = @email OR identityNr = @identityNr) AND statusId =2";
            return dbContext.GetByQuery<User>(sql, new { email = email, identityNr = identityNr });
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
    }
}
