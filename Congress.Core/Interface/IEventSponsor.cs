using Congress.Core.Entity;
using System.Collections.Generic;

namespace Congress.Core.Interface
{
    public interface IEventSponsor
    {
        bool BulkInsertSponsor(List<EventSponsor> eventSponsors);
        List<Sponsor> GetEventSponsors(int eventId);

        bool DeleteEventSponsor(int eventId, int sponsorId);
    }
}
