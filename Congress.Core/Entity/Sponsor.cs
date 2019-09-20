using Dapper.Contrib.Extensions;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Congress.Core.Entity
{
    [Table("sponsor")]
    public class Sponsor : BaseEntity
    {
        public string name { get; set; }
        public string logoPath { get; set; }

        [Write(false)]
        public List<IFormFile> logoFile { get; set; }
    }
}
