using System.Collections.Generic;
using System.Linq;
using Congress.Core.Entity;
using Congress.Core.Interface;

namespace Congress.Data.Service
{
    public class SCategory : ICategory
    {
        IDbContext dbContext;
        public SCategory(IDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public bool BulkUpdateCategory(List<Category> category)
        {
            return dbContext.BulkUpdate<Category>(category);
        }

        public bool CategoryUpdate(Category category)
        {
            return dbContext.Update(category);
        }

        public List<Category> GetCategories()
        {
            string sql = "SELECT * FROM category WHERE statusId = 2 ORDER BY parentCategoryId ASC";
            return dbContext.GetByQueryAll<Category>(sql, new { }).ToList();
        }

        public List<Category> GetMainCategory()
        {
            string sql = "SELECT * FROM category WHERE parentCategoryId = 0 AND statusId = 2";
            return dbContext.GetByQueryAll<Category>(sql, new { }).ToList();
        }

        public List<Category> GetSubCategories(int mainCategoryId)
        {
            string sql = "SELECT * FROM category WHERE parentCategoryId = @mainCategoryId AND statusId = 2";
            return dbContext.GetByQueryAll<Category>(sql, new { mainCategoryId = mainCategoryId }).ToList();
        }

        public int Insert(Category category)
        {
            return dbContext.Insert(category);
        }
    }
}
