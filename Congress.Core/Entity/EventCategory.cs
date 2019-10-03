using Dapper.Contrib.Extensions;

namespace Congress.Core.Entity
{
    [Table("eventcategory")]
    public class EventCategory : BaseEntity
    {
        public int eventId { get; set; }
        public int categoryId { get; set; }

        [Write(false)]
        public string categoryName { get; set; }
    }
}
