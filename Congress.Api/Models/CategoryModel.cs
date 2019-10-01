using Congress.Core.Entity;
using System.Collections.Generic;

namespace Congress.Api.Models
{
    public class CategoryModel
    {
        public Category category { get; set; }
        public List<Category>categories{ get; set; }
    }
}
