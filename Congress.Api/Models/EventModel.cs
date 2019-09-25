using Congress.Core.Entity;
using System.Collections.Generic;

namespace Congress.Api.Models
{
    public class EventModel
    {
        public Event cgevent{ get; set; }
        public List<Event>events{ get; set; }        
    }
}
