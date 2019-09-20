using Congress.Core.Entity;
using Congress.Core.Interface;
using System.Collections.Generic;
using System.Linq;

namespace Congress.Data.Service
{
    public class SMenu : IMenu
    {
        IDbContext dbContext;
        public SMenu(IDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public List<Menu> GetUserMenu(int loginType, int userType)
        {
            string sql = "SELECT * FROM menu WHERE menuTypeId = @loginType AND userTypeId = @userType AND statusId = 2 ORDER BY priority ASC";
            return dbContext.GetByQueryAll<Menu>(sql, new { loginType = loginType, userType = userType }).ToList();            
        }
    }
}
