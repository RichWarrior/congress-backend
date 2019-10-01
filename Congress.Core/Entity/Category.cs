using Dapper.Contrib.Extensions;

namespace Congress.Core.Entity
{
    [Table("category")]
    public class Category : BaseEntity
    {
        public int parentCategoryId { get; set; }
        public string name { get; set; }
    }
}
