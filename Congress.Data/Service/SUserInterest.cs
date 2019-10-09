using System.Collections.Generic;
using System.Linq;
using Congress.Core.Entity;
using Congress.Core.Interface;

namespace Congress.Data.Service
{
    public class SUserInterest : IUserInterest
    {
        IDbContext dbContext;
        public SUserInterest(IDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public bool BulkInsertInterest(List<UserInterest> userInterests)
        {
            return dbContext.BulkInsert(userInterests);
        }

        public bool DeleteUserInterest(int userId, int interestId)
        {
            string sql = @"UPDATE userinterest SET statusId = 1 WHERE userId= @userId AND interestId = @interestId";
            return dbContext.ExecuteQuery(sql, new { userId = userId, interestId = interestId });
        }

        public List<Category> GetAvailableCategories(int userId, int mainCategoryId)
        {
            string sql = @"SELECT * FROM category WHERE statusId = 2 AND parentCategoryId = @mainCategoryId AND id NOT IN (SELECT interestId FROM userinterest WHERE userId = @userId AND statusId = 2)";
            return dbContext.GetByQueryAll<Category>(sql, new { userId = userId, mainCategoryId = mainCategoryId }).ToList();
        }

        public List<Category> GetUserInterest(int userId)
        {
            string sql = @"SELECT c.* FROM userinterest ui
            INNER JOIN category c ON ui.interestId = c.id
            WHERE ui.statusId = 2 AND ui.userId = @userId";
            return dbContext.GetByQueryAll<Category>(sql, new { userId = userId }).ToList();
        }
    }
}
