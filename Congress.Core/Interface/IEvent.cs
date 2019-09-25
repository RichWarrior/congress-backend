using Congress.Core.Entity;
using System.Collections.Generic;

namespace Congress.Core.Interface
{
    public interface IEvent
    {
        int Insert(Event _event);
        List<Event> GetEvents(int userId);
        bool UpdateEvent(Event _event);
        Event GetById(int id);
    }
}
