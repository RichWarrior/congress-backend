using Congress.Core.Entity;
using System.Collections.Generic;

namespace Congress.Core.Interface
{
    public interface ISponsor
    {
        /// <summary>
        /// Sponsor Ekleme İşlemlerini Yapar.
        /// </summary>
        /// <param name="sponsor">Eklenecek Sponsor Modeli</param>
        /// <returns></returns>
        int Insert(Sponsor sponsor);
        /// <summary>
        /// Sponsorları Getirir.
        /// </summary>
        /// <returns></returns>
        List<Sponsor> GetSponsors();
        /// <summary>
        /// Sponsor Güncellemek İçin Kullanılır.
        /// </summary>
        /// <param name="sponsor">Sponsor Modeli</param>
        /// <returns></returns>
        bool UpdateSponsor(Sponsor sponsor);
        /// <summary>
        /// Etkinliğe Tanımlanabilecek Sponsorları Getirir.
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        List<Sponsor> GetEventAvailableSponsor(int eventId);
    }
}
