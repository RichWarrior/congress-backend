using Dapper.Contrib.Extensions;
using System;

namespace Congress.Core.Entity
{
    public class BaseEntity
    {
        [ExplicitKey]
        public int id { get; set; }
        public int creatorId { get; set; }
        public DateTime creationDate { get; set; }
        public int statusId { get; set; }
    }
}
