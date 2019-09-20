using Congress.Api.Filters;
using Congress.Api.Models;
using Congress.Core.Entity;
using Congress.Core.Enums;
using Congress.Core.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Congress.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SponsorController : BaseController
    {
        IMinio _SMinio;
        ISponsor _SSponsor;
        public SponsorController(IMethod _SMethod,IMinio _SMinio,ISponsor _SSponsor)
            : base(_SMethod)
        {
            this._SMinio = _SMinio;
            this._SSponsor = _SSponsor;
        }

        /// <summary>
        /// Sponsor Oluşturma İşlemini Yapar.
        /// </summary>
        /// <param name="sponsor"></param>
        /// <returns></returns>
        [HttpPost("newsponsor")]
        [NotAccessUser]
        public async Task<IActionResult>  NewSponsor([FromForm]Sponsor sponsor)
        {
            BaseResult<SponsorModel> baseResult = new BaseResult<SponsorModel>();
            bool isSuccess = false;
            string user = GetUserClaims("user");
            if (!String.IsNullOrEmpty(user))
            {
                User userObject = JsonConvert.DeserializeObject<User>(user);
                sponsor.statusId = userObject.userTypeId == (int)enumUserType.doctor ? 2 : 1;
                string bucketName = _SMethod.GetEnumValue(enumBucketType.Sponsors);
                IFormFile logoFile = sponsor.logoFile.FirstOrDefault();
                string logoPath = await _SMinio.UploadFile(bucketName, logoFile);
                if (!String.IsNullOrEmpty(logoPath))
                {
                    sponsor.logoPath = logoPath;
                    sponsor.id = _SSponsor.Insert(sponsor);
                    if (sponsor.id > 0)
                    {
                        baseResult.data.sponsor = sponsor;
                        isSuccess = true;
                    }
                    else
                    {
                        baseResult.errMessage = "Sponsor Oluşturulamadı!";
                        baseResult.statusCode = HttpStatusCode.NotFound;
                    }
                }
                else
                {
                    baseResult.errMessage = "Sponsor Logosu Yüklenemedi!";
                    baseResult.statusCode = HttpStatusCode.NotFound;
                }
            }            
            if (isSuccess)
            {              
                return Json(baseResult);
            }
            else
            {
                return new NotFoundObjectResult(baseResult);
            }            
        }

        /// <summary>
        /// Sponsorları Getirir.
        /// </summary>
        [HttpPost("getsponsor")]
        [NotAccessUser]
        public IActionResult GetSponsors()
        {
            BaseResult<SponsorModel> baseResult = new BaseResult<SponsorModel>();
            baseResult.data.sponsors = _SSponsor.GetSponsors();
            return Json(baseResult);
        }
        
        [HttpPost("checksponsor")]
        [DoctorValidation]
        public IActionResult CheckSponsor([FromBody]Sponsor sponsor)
        {
            BaseResult<SponsorModel> baseResult = new BaseResult<SponsorModel>();
            baseResult.data.sponsor = sponsor;
            bool isSuccess = false;
            sponsor.statusId = 2;
            isSuccess = _SSponsor.UpdateSponsor(sponsor);
            if (isSuccess)
            {               
                return Json(baseResult);
            }
            else
            {
                baseResult.errMessage = "Sponsor Onaylanamadı!";
                baseResult.statusCode = HttpStatusCode.NotFound;
                return new NotFoundObjectResult(baseResult);
            }
            
        }
    }
}