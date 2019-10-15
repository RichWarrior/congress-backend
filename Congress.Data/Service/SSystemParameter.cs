using Congress.Core.Entity;
using Congress.Core.Interface;
using System.Collections.Generic;
using System.Linq;

namespace Congress.Data.Service
{
    public class SSystemParameter : ISystemParameter
    {
        IDbContext dbContext;
        public SSystemParameter(IDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public List<SystemParameter> GetSystemParameter()
        {
            string sql = "SELECT * FROM systemparameter WHERE statusId = 2";
            return dbContext.GetByQueryAll<SystemParameter>(sql, new { }).ToList();
        }
    }
}
