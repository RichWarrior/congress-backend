using Congress.Core.Entity;
using System.Collections.Generic;

namespace Congress.Core.Interface
{
    public interface IEventParticipant
    {
        /// <summary>
        /// Etkinliğe Tanımalanbilecek Katılımcıları Getirir
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        List<User> EventNotInUser(int eventId);
        /// <summary>
        /// Katılımcı Ekleme İşlemlerini Yapar
        /// </summary>
        /// <param name="eventParticipants"></param>
        /// <returns></returns>
        bool BulkInsertParticipants(List<EventParticipant> eventParticipants);
        /// <summary>
        /// Katılımcıları Listeler
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        List<User> GetEventParticipants(int eventId);
        /// <summary>
        /// Katılımcı Siler
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        bool DeleteEventParticipant(int eventId,int userId);
    }
}
