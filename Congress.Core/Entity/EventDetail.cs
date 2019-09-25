using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Congress.Core.Entity
{
    [Table("eventdetail")]
    public class EventDetail : BaseEntity
    {
        public int eventId { get; set; }
        public TimeSpan startTime { get; set; }
        public TimeSpan endTime { get; set; }
        public int day { get; set; }
        public string speakerName { get; set; }
        public string description { get; set; }
    }
}
