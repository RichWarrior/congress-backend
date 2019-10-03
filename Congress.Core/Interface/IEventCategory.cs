using Congress.Core.Entity;
using System.Collections.Generic;

namespace Congress.Core.Interface
{
    public interface IEventCategory
    {
        /// <summary>
        /// Etkinliğe Kategori Tanımlar
        /// </summary>
        /// <param name="eventCategories"></param>
        /// <returns></returns>
        bool InsertEventCategories(List<EventCategory> eventCategories);
        /// <summary>
        /// Etkinliğe Ait Kategorileri Getirir
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        List<EventCategory> GetEventCategories(int eventId);
        bool UpdateEventCategory(EventCategory eventCategory);
    }
}
