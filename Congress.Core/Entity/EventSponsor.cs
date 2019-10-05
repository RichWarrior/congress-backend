using Dapper.Contrib.Extensions;

namespace Congress.Core.Entity
{
    [Table("eventsponsor")]
    public class EventSponsor : BaseEntity
    {
        public int eventId { get; set; }
        public int sponsorId { get; set; }
    }
}
