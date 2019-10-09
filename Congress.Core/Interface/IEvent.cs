using Congress.Core.Entity;
using System.Collections.Generic;

namespace Congress.Core.Interface
{
    public interface IEvent
    {
        /// <summary>
        /// Etkinlik Oluşturma
        /// </summary>
        /// <param name="_event"></param>
        /// <returns></returns>
        int Insert(Event _event);
        /// <summary>
        /// Kullanıcının Etkinliklerini Getirme
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        List<Event> GetEvents(int userId);
        /// <summary>
        /// Etkinlik Güncelleme
        /// </summary>
        /// <param name="_event"></param>
        /// <returns></returns>
        bool UpdateEvent(Event _event);
        /// <summary>
        /// Id Değerine Göre Etkinlik Getirme
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Event GetById(int id);
        /// <summary>
        /// Kullanıcının Katılım Gösterdiği Etkinlikleri Listeler
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        List<Event> GetActiveEvents(int userId);
        /// <summary>
        /// İstenilen Koşula Göre Etkinlikleri Listeler
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="_params"></param>
        /// <returns></returns>
        List<Event> GetEvents(string sql,object _params);
    }
}
