using Congress.Api.Filters;
using Congress.Api.Models;
using Congress.Core.Entity;
using Congress.Core.Enums;
using Congress.Core.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Congress.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EventController : BaseController
    {
        IEvent _SEvent;
        IMinio _SMinio;
        IUser _SUser;
        IEventDetail _SEventDetail;
        public EventController(IMethod _SMethod,
            IEvent _SEvent, IMinio _SMinio,
            IUser _SUser,IEventDetail _SEventDetail)
            : base(_SMethod)
        {
            this._SEvent = _SEvent;
            this._SMinio = _SMinio;
            this._SUser = _SUser;
            this._SEventDetail = _SEventDetail;
        }

        /// <summary>
        /// Etkinlik Oluşturmak İçin Kullanılır.
        /// </summary>
        /// <param name="_event"></param>
        /// <returns></returns>
        [HttpPost("newevent")]
        [BusinessValidation]
        public async Task<IActionResult> NewEvent([FromForm]Event _event)
        {
            BaseResult<EventModel> baseResult = new BaseResult<EventModel>();
            int userId = Convert.ToInt32(HttpContext.User.Identity.Name);
            User user = _SUser.GetById(userId);
            bool isSuccess = false;
            if (user.eventCount > 0)
            {
                _event.userId = Convert.ToInt32(HttpContext.User.Identity.Name);
                string bucketName = _SMethod.GetEnumValue(enumBucketType.Events);
                IFormFile logoFile = _event.logoFiles.FirstOrDefault();
                string path = await _SMinio.UploadFile(bucketName, logoFile);
                if (!String.IsNullOrEmpty(path))
                {
                    _event.logoPath = path;
                    _event.creatorId = userId;
                    _event.id = _SEvent.Insert(_event);
                    baseResult.data.cgevent = _event;
                    isSuccess = _event.id > 0 ? true : false;
                }
                else
                {
                    baseResult.errMessage = "Logo Yüklenemedi!";
                }
            }
            else
            {
                baseResult.errMessage = "Etkinlik Oluşturma Hakkınız Bulunmuyor!";
            }

            if (isSuccess)
            {
                user.eventCount--;
                _SUser.UpdateUser(user);
                return Json(baseResult);
            }
            else
            {
                baseResult.statusCode = HttpStatusCode.NotFound;
                return new NotFoundObjectResult(baseResult);
            }
        }

        /// <summary>
        /// Kullanıcıya Ait Etkinlikleri Getirir
        /// </summary>
        /// <returns></returns>
        [HttpPost("getevents")]
        [BusinessValidation]
        public IActionResult GetEvents()
        {
            BaseResult<EventModel> baseResult = new BaseResult<EventModel>();
            int userId = Convert.ToInt32(HttpContext.User.Identity.Name);
            baseResult.data.events = _SEvent.GetEvents(userId);
            return Json(baseResult);
        }

        /// <summary>
        /// Etkinlik Silme İşlemleri İçin Kullanılır.
        /// </summary>
        /// <param name="_event">Silinecek Etkinlik Modeli</param>
        /// <returns></returns>
        [HttpPost("deleteevent")]
        [BusinessValidation]
        public IActionResult DeleteEvent([FromBody]Event _event)
        {
            BaseResult<EventModel> baseResult = new BaseResult<EventModel>();
            int userId = Convert.ToInt32(HttpContext.User.Identity.Name);
            bool isSuccess = false;
            if (_event.userId != userId)
            {
                baseResult.errMessage = "Sadece Kendine Ait Bir Etkinliği Silebilirsin.";
            }
            else
            {
                _event.statusId = 1;
                isSuccess = _SEvent.UpdateEvent(_event);
            }
            baseResult.data.cgevent = _event;
            if (isSuccess)
            {
                return Json(baseResult);
            }
            else
            {
                baseResult.statusCode = HttpStatusCode.NotFound;
                return new NotFoundObjectResult(baseResult);
            }
        }

        /// <summary>
        /// İstenilen Id Değerine Göre Etkinlik Bilgilerini Döndürür
        /// </summary>
        [HttpPost("geteventbyid")]
        [BusinessValidation]
        public IActionResult GetEventById([FromBody]Event _event)
        {
            BaseResult<EventModel> baseResult = new BaseResult<EventModel>();
            int userId = Convert.ToInt32(HttpContext.User.Identity.Name);
            Event @event = _SEvent.GetById(_event.id);
            bool isSuccess = false;
            if (@event.userId != userId)
            {
                baseResult.errMessage = "Sadece Kendi Etkinliklerinizin Bilgilerini Görebilirsiniz!";
            }
            else
            {
                baseResult.data.cgevent = @event;
                isSuccess = true;
            }
            if (isSuccess)
            {
                return Json(baseResult);
            }
            else
            {
                baseResult.statusCode = HttpStatusCode.NotFound;
                return new NotFoundObjectResult(baseResult);
            }
        }

        [HttpPost("updatevent")]
        [BusinessValidation]
        public async Task<IActionResult> UpdateEvent([FromForm]Event _event)
        {
            BaseResult<EventModel> baseResult = new BaseResult<EventModel>();
            bool isSuccess = false;
            int userId = Convert.ToInt32(HttpContext.User.Identity.Name);
            if (userId != _event.userId)
            {
                baseResult.errMessage = "Sadece Kendi Etkinliklerinizi Düzenleyebilirsiniz";
            }
            else
            {
                if (_event.logoFiles!=null)
                {
                    string bucketName = _SMethod.GetEnumValue(enumBucketType.Events);
                    IFormFile formFile = _event.logoFiles.FirstOrDefault();
                    string path = await _SMinio.UploadFile(bucketName,formFile);
                    if (!String.IsNullOrEmpty(path))
                    {
                        _event.logoPath = path;
                    }
                    else
                    {
                        baseResult.errMessage = "Etkinlik Logosu Güncellenemedi!";
                    }
                }
                isSuccess = _SEvent.UpdateEvent(_event);
            }
            if (isSuccess)
            {
                return Json(baseResult);
            }
            else
            {
                baseResult.statusCode = HttpStatusCode.NotFound;
                return new NotFoundObjectResult(baseResult);
            }            
        }
    }
}