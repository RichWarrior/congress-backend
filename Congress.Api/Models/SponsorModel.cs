using Congress.Core.Entity;
using System.Collections.Generic;

namespace Congress.Api.Models
{
    public class SponsorModel
    {
        public Sponsor  sponsor{ get; set; }
        public List<Sponsor> sponsors{ get; set; }
    }
}
