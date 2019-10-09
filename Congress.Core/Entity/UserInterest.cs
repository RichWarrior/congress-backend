using Dapper.Contrib.Extensions;

namespace Congress.Core.Entity
{
    [Table("userinterest")]
    public class UserInterest : BaseEntity
    {
        public int userId { get; set; }
        public int interestId { get; set; }
    }
}
