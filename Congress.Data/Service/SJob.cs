using Congress.Core.Entity;
using Congress.Core.Interface;
using Congress.Data.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Congress.Data.Service
{
    public class SJob : IJob
    {
        IDbContext dbContext;
        public SJob(IDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public List<Job> GetJobs()
        {
            string sql = "SELECT * FROM job WHERE statusId = 2";
            return dbContext.GetByQueryAll<Job>(sql, new { }).ToList();
        }
    }
}
