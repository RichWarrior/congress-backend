using System;
using System.Collections.Generic;
using System.Text;

namespace Congress.Core.Entity
{
    public class Menu : BaseEntity
    {
        public int menuTypeId { get; set; }
        public int parentMenuId { get; set; }
        public string name { get; set; }
        public string icon { get; set; }
        public string path { get; set; }        
    }
}
