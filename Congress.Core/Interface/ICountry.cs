using Congress.Core.Entity;
using System.Collections.Generic;

namespace Congress.Core.Interface
{
    public interface ICountry
    {
        /// <summary>
        /// Ülkeleri Getirmek İçin Kullanılır.
        /// </summary>
        /// <returns></returns>
        List<Country> GetCountries();
    }
}
