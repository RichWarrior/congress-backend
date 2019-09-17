using Congress.Core.Entity;
using System.Collections.Generic;

namespace Congress.Core.Interface
{
    public interface ICity
    {
        /// <summary>
        /// İstenilen Ülkeye Ait Şehileri Getirir.
        /// </summary>
        /// <param name="countryId">Ülke Id</param>
        /// <returns></returns>
        List<City> GetCities(int countryId);
    }
}
