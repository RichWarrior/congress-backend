using Dapper.Contrib.Extensions;

namespace Congress.Core.Entity
{
    [Table("job")]
    public class Job : BaseEntity
    {
        /// <summary>
        /// Meslek Adı
        /// </summary>
        public string name { get; set; }
    }
}
