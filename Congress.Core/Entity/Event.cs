using Dapper.Contrib.Extensions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace Congress.Core.Entity
{
    [Table("event")]
    public class Event : BaseEntity
    {
        public int userId { get; set; }
        public string name { get; set; }

        public string description { get; set; }
        public string logoPath { get; set; }
        public int countryId { get; set; }
        public int cityId { get; set; }
        public string address { get; set; }
        public DateTime startDate{ get; set; }
        public DateTime endDate { get; set; }    
        
        [Write(false)]
        public List<IFormFile> logoFiles { get; set; }

        [Write(false)]
        public int isCompleted { get; set; }

        [Write(false)]
        public string CountryName { get; set; }
        
        [Write(false)]
        public string CityName { get; set; }

        [Write(false)]
        public string creatorName { get; set; }
    }
}
