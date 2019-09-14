using Dapper.Contrib.Extensions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Congress.Core.Entity
{
    [Table("user")]
    public class User : BaseEntity
    {
        public int userTypeId { get; set; }

        public string userGuid { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public string email{ get; set; }
        public string password{ get; set; }
        public string identityNr{ get; set; }
        public int gender{ get; set; }
        public DateTime birthDate{ get; set; }
        public int jobId{ get; set; }
        public int countryId{ get; set; }
        public int cityId{ get; set; }
        public string avatarPath{ get; set; }
        public string phoneNr { get; set; }
        public string taxNr { get; set; }
        public int eventCount { get; set; }
        public int emailVerification { get; set; }
        public int profileStatus { get; set; }
        public int notificationStatus { get; set; }  
        
        [Write(false)]
        public IFormFile avatarFile { get; set; }
    }
}
