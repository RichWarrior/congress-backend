using Congress.Core.Entity;
using System.Collections.Generic;

namespace Congress.Api.Models
{
    public class RegionResult
    {
        public List<Country> countries{ get; set; }
        public List<City>cities { get; set; }
    }
}
