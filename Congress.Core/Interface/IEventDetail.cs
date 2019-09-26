using Congress.Core.Entity;
using System.Collections.Generic;

namespace Congress.Core.Interface
{
    public interface IEventDetail
    {
        /// <summary>
        /// Konuşmacı Ekleme İşlemleri
        /// </summary>
        /// <param name="eventDetail"></param>
        /// <returns></returns>
        int InsertEventDetail(EventDetail eventDetail);
        /// <summary>
        /// Konuşmacı Listeleme İşlemleri
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        List<EventDetail> GetEventDetails(int eventId);
        /// <summary>
        /// Konuşmacı Bilgilerini Güncelleme
        /// </summary>
        /// <param name="eventDetail"></param>
        /// <returns></returns>
        bool UpdateEventDetail(EventDetail eventDetail);
    }
}
