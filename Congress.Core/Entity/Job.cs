using Dapper.Contrib.Extensions;

namespace Congress.Core.Entity
{
    [Table("job")]
    public class Job : BaseEntity
    {
        public string name { get; set; }
    }
}
