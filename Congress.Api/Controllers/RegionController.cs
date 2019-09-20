using Congress.Api.Models;
using Congress.Core.Entity;
using Congress.Core.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Congress.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionController : BaseController
    {
        ICountry _SCountry;
        ICity _SCity;
        public RegionController(IMethod _SMethod, ICountry _SCountry,ICity _SCity)
            : base(_SMethod)
        {
            this._SCountry = _SCountry;
            this._SCity = _SCity;
        }

        /// <summary>
        /// Ülkeleri Geri Döndürür.
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetCountries")]
        [AllowAnonymous]
        public IActionResult GetCountries()
        {
            BaseResult<RegionResult> baseResult = new BaseResult<RegionResult>();
            baseResult.data.countries = _SCountry.GetCountries();
            return Json(baseResult);
        }
        /// <summary>
        /// İstenilen Ülkeye Ait Şehirleri Getirir.
        /// </summary>
        /// <param name="model">Country Id Gereklidir.</param>
        /// <returns></returns>
        [HttpPost("GetCities")]
        [AllowAnonymous]
        public IActionResult GetCities([FromBody]City model)
        {
            BaseResult<RegionResult> baseResult = new BaseResult<RegionResult>();
            baseResult.data.cities = _SCity.GetCities(model.countryId);
            return Json(baseResult);
        }
    }
}