using Dapper.Contrib.Extensions;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Congress.Core.Entity
{
    [Table("eventparticipant")]
    public class EventParticipant : BaseEntity
    {
        public int eventId { get; set; }
        public int userId { get; set; }

        [Write(false)]
        public List<IFormFile> importUserFile { get; set; }
    }
}
