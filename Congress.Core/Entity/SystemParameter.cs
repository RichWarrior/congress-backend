using Dapper.Contrib.Extensions;

namespace Congress.Core.Entity
{
    [Table("systemparameter")]
    public class SystemParameter : BaseEntity
    {
        public string keystr { get; set; }
        public string valuestr { get; set; }
    }
}
