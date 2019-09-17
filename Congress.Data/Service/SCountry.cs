using Congress.Core.Entity;
using Congress.Core.Interface;
using System.Collections.Generic;
using System.Linq;

namespace Congress.Data.Service
{
    public class SCountry : ICountry
    {
        IDbContext dbContext;
        public SCountry(IDbContext dbContext)
        {
            this.dbContext = dbContext;
        }   
        public List<Country> GetCountries()
        {
            string sql = "SELECT * FROM country WHERE statusId = 2";
            return dbContext.GetByQueryAll<Country>(sql, new { }).ToList();
        }
    }
}
