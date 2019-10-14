using Congress.Core.Entity;
using System.Collections.Generic;

namespace Congress.Api.Models
{
    public class EventModel
    {
        public Event cgevent{ get; set; }
        public List<Event>events{ get; set; }
        public EventDetail eventDetail { get; set; }
        public List<EventDetail> eventDetails { get; set; }
        public List<Category> eventCategories { get; set; }
        public Category category { get; set; }
        public List<EventCategory> eventCategoriesRel { get; set; }
        public List<Sponsor>eventSponsors{ get; set; }
        public List<User> eventParticipants { get; set; }
        public User eventCreator { get; set; }
    }
}
