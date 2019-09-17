using Congress.Core.Entity;
using Congress.Core.Interface;
using System.Collections.Generic;
using System.Linq;

namespace Congress.Data.Service
{
    public class SCity : ICity
    {
        IDbContext dbContext;
        public SCity(IDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public List<City> GetCities(int countryId)
        {
            string sql = "SELECT * FROM city WHERE countryId = @countryId AND statusId = 2";
            return dbContext.GetByQueryAll<City>(sql, new { countryId = countryId }).ToList();
        }
    }
}
