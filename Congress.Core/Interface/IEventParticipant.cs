using Congress.Core.Entity;
using System.Collections.Generic;

namespace Congress.Core.Interface
{
    public interface IEventParticipant
    {
        List<User> EventNotInUser(int eventId);
        bool BulkInsertParticipants(List<EventParticipant> eventParticipants);
        List<User> GetEventParticipants(int eventId);
        bool DeleteEventParticipant(int eventId,int userId);
    }
}
