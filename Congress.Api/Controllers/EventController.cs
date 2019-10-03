using Congress.Api.Filters;
using Congress.Api.HubDispatcher;
using Congress.Api.Models;
using Congress.Core.Entity;
using Congress.Core.Enums;
using Congress.Core.Interface;
using Congress.Core.QueueModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
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
        IEventParticipant _SEventParticipant;
        IEventCategory _SEventCategory;
        ICategory _SCategory;
        INotificationDispatcher notificationDispatcher;
        public EventController(IMethod _SMethod,
            IEvent _SEvent, IMinio _SMinio,
            IUser _SUser, IEventDetail _SEventDetail, IEventParticipant _SEventParticipant,
            IEventCategory _SEventCategory, ICategory _SCategory,
            INotificationDispatcher notificationDispatcher
            )
            : base(_SMethod)
        {
            this._SEvent = _SEvent;
            this._SMinio = _SMinio;
            this._SUser = _SUser;
            this._SEventDetail = _SEventDetail;
            this._SEventParticipant = _SEventParticipant;
            this._SEventCategory = _SEventCategory;
            this._SCategory = _SCategory;
            this.notificationDispatcher = notificationDispatcher;
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
            foreach (var item in baseResult.data.events)
            {
                item.isCompleted = item.endDate < DateTime.Now ? 2 : 1;
            }
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
        /// <summary>
        /// Etkinlik Güncellemek İçin Kullanılır
        /// </summary>
        /// <param name="_event"></param>
        /// <returns></returns>
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
                if (_event.logoFiles != null)
                {
                    string bucketName = _SMethod.GetEnumValue(enumBucketType.Events);
                    IFormFile formFile = _event.logoFiles.FirstOrDefault();
                    string path = await _SMinio.UploadFile(bucketName, formFile);
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
        /// <summary>
        /// Yeni Etkinlik Detayı Eklemek İçin Kullanılır
        /// </summary>
        /// <param name="eventDetail"></param>
        /// <returns></returns>
        [HttpPost("neweventdetail")]
        [BusinessValidation]
        public IActionResult NewEventDetail([FromBody]EventDetail eventDetail)
        {
            BaseResult<EventModel> baseResult = new BaseResult<EventModel>();
            Event _event = _SEvent.GetById(eventDetail.eventId);
            int userId = Convert.ToInt32(HttpContext.User.Identity.Name);
            bool isSuccess = false;
            if (_event.userId != userId)
            {
                baseResult.errMessage = "Kendinize Ait Olmayan Bir Etkinliğe Müdahale Edemezsiniz!";
            }
            else
            {
                eventDetail.id = _SEventDetail.InsertEventDetail(eventDetail);
                if (eventDetail.id > 0)
                {
                    baseResult.data.eventDetail = eventDetail;
                    isSuccess = true;
                }
                else
                {
                    baseResult.errMessage = "Konuşmacı Eklenemedi!";
                }
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
        /// <summary>
        /// Etkinlik Detaylarını Görüntülemek İçin Kullanılır
        /// </summary>
        /// <param name="_event"></param>
        /// <returns></returns>
        [HttpPost("geteventdetail")]
        [BusinessValidation]
        public IActionResult GetEventDetail([FromBody]Event _event)
        {
            BaseResult<EventModel> baseResult = new BaseResult<EventModel>();
            bool isSuccess = false;
            int userId = Convert.ToInt32(HttpContext.User.Identity.Name);
            if (userId != _event.userId)
            {
                baseResult.errMessage = "Kendinize Ait Olmayan Bir Etkinlikle İlgili İşlem Yapamazsınız!";
            }
            else
            {
                baseResult.data.eventDetails = _SEventDetail.GetEventDetails(_event.id);
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
        /// <summary>
        /// Etkinlik Detayı Silmek İçin Kullanılır.
        /// </summary>
        /// <param name="eventDetail"></param>
        /// <returns></returns>
        [HttpPost("deleteeventdetail")]
        [BusinessValidation]
        public IActionResult DeleteEventDetail([FromBody]EventDetail eventDetail)
        {
            BaseResult<EventModel> baseResult = new BaseResult<EventModel>();
            bool isSuccess = false;
            int userId = Convert.ToInt32(HttpContext.User.Identity.Name);
            Event _event = _SEvent.GetById(eventDetail.eventId);
            if (_event.userId != userId)
            {
                baseResult.errMessage = "Kendi Etkinlikleriniz Dışındakilere Müdahale Edemezsiniz!";
            }
            else
            {
                eventDetail.statusId = 1;
                if (_SEventDetail.UpdateEventDetail(eventDetail))
                {
                    isSuccess = true;
                }
                else
                {
                    baseResult.errMessage = "Konuşmacı Silinemedi!";
                }
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
        /// <summary>
        /// Etkinlik Detayı Güncellemek İçin Kullanılır
        /// </summary>
        /// <param name="eventDetail"></param>
        /// <returns></returns>
        [HttpPost("updateeventdetail")]
        [BusinessValidation]
        public IActionResult UpdateEventDetail([FromBody]EventDetail eventDetail)
        {
            BaseResult<EventModel> baseResult = new BaseResult<EventModel>();
            bool isSuccess = false;
            Event _event = _SEvent.GetById(eventDetail.eventId);
            int userId = Convert.ToInt32(HttpContext.User.Identity.Name);
            if (_event.userId != userId)
            {
                baseResult.errMessage = "Sadece Kendine Ait Bir Etkinliğe Müdahale Edebilirsiniz!";
            }
            else
            {
                if (_SEventDetail.UpdateEventDetail(eventDetail))
                {
                    isSuccess = true;
                    baseResult.data.eventDetail = eventDetail;
                }
                else
                {
                    baseResult.errMessage = "Konuşmacı Bilgileri Güncellenemedi!";
                }
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

        /// <summary>
        /// İlgili Bir Etkinliğe Katılmayan Kullanıcıları Getirir.
        /// </summary>
        [HttpPost("getavailableparticipant")]
        [BusinessValidation]
        public IActionResult GetAvailableParticipant([FromBody]Event _event)
        {
            BaseResult<UserModel> baseResult = new BaseResult<UserModel>();
            baseResult.data.users = _SEventParticipant.EventNotInUser(_event.id);
            return Json(baseResult);
        }
        /// <summary>
        /// Katılımcı Ekleme İşlemleri İçin Kullanılır.
        /// </summary>
        /// <param name="eventParticipants">Katılımcı Listesi</param>
        /// <returns></returns>
        [HttpPost("newparticipant")]
        [BusinessValidation]
        public IActionResult NewParticipant([FromBody]List<EventParticipant> eventParticipants)
        {
            BaseResult<EventModel> baseResult = new BaseResult<EventModel>();
            bool isSuccess = false;
            if (eventParticipants != null)
            {
                int userId = Convert.ToInt32(HttpContext.User.Identity.Name);
                Event _event = _SEvent.GetById(eventParticipants.FirstOrDefault().eventId);
                if (_event.userId == userId)
                {
                    if (_SEventParticipant.BulkInsertParticipants(eventParticipants))
                    {
                        isSuccess = true;
                    }
                    else
                    {
                        baseResult.errMessage = "Katılımcılar Eklenemedi!";
                    }
                }
                else
                {
                    baseResult.errMessage = "Kendinize Ait Olmayan Etkinliğe Katılımcı Ekleyemezsiniz";
                }
            }
            else
            {
                baseResult.errMessage = "Katılımcılar Bulunamadı!";
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
        /// <summary>
        /// Etkinlikle İlgili Katılımcıları Listelemek İçin Kullanılır.
        /// </summary>
        /// <param name="_event"></param>
        /// <returns></returns>
        [HttpPost("geteventparticipants")]
        [BusinessValidation]
        public IActionResult GetEventParticipants([FromBody]Event _event)
        {
            BaseResult<UserModel> baseResult = new BaseResult<UserModel>();
            baseResult.data.users = _SEventParticipant.GetEventParticipants(_event.id);
            return Json(baseResult);
        }
        /// <summary>
        /// Katılımcı Silme İşlemleri İçin Kullanılır.
        /// </summary>
        /// <param name="eventParticipant"></param>
        /// <returns></returns>
        [HttpPost("deleteeventparticipant")]
        [BusinessValidation]
        public IActionResult DeleteEventParticipant([FromBody]EventParticipant eventParticipant)
        {
            BaseResult<EventModel> baseResult = new BaseResult<EventModel>();
            bool isSuccess = false;
            int userId = Convert.ToInt32(HttpContext.User.Identity.Name);
            Event _event = _SEvent.GetById(eventParticipant.eventId);
            if (userId == _event.userId)
            {
                if (_SEventParticipant.DeleteEventParticipant(eventParticipant.eventId, eventParticipant.userId))
                {
                    isSuccess = true;
                }
                else
                {
                    baseResult.errMessage = "Katılımcı Silinemedi!";
                }
            }
            else
            {
                baseResult.errMessage = "Kendinize Ait Olmayan Etkinliğe Müdahale Edemezsiniz";
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
        /// <summary>
        /// Katılımcı İmport Etmek İçin Kullanılır
        /// </summary>
        /// <param name="eventParticipant"></param>
        /// <returns></returns>
        [HttpPost("importeventparticipants")]
        [BusinessValidation]
        public async Task<IActionResult> ImportEventParticipants([FromForm]EventParticipant eventParticipant)
        {
            BaseResult<UserModel> baseResult = new BaseResult<UserModel>();
            bool isSuccess = false;
            List<User> users = _SUser.GetAllUser();
            List<User> excelFileUsers = new List<User>();
            List<EventParticipant> rels = new List<EventParticipant>();
            Dictionary<string, string> hashedValue = new Dictionary<string, string>();
            int id = Convert.ToInt32(HttpContext.User.Identity.Name);
            using (MemoryStream ms = new MemoryStream())
            {
                IFormFile formFile = eventParticipant.importUserFile[0];
                await formFile.CopyToAsync(ms);
                using (ExcelPackage excelPackage = new ExcelPackage(ms))
                {
                    var workSheet = excelPackage.Workbook.Worksheets.First();
                    int rowCount = workSheet.Dimension.End.Row;
                    int userTypeId = Convert.ToInt32(_SMethod.GetEnumValue(enumUserType.user));
                    for (int row = 1; row <= rowCount; row++)
                    {
                        var emailAddress = workSheet.Cells[row, 1].Value;
                        if (emailAddress != null)
                        {
                            bool emailVerification = _SMethod.ValidateEmail(emailAddress.ToString());
                            if (emailVerification)
                            {
                                User _user = new User();
                                string guid = Guid.NewGuid().ToString();
                                string _password = "";
                                for (int i = 0; i < 8; i++)
                                {
                                    _password += guid[i];
                                }

                                _user.email = emailAddress.ToString();
                                _user.name = _user.surname = "";
                                _user.avatarPath = "";
                                _user.phoneNr = _user.taxNr = "";
                                _user.userTypeId = userTypeId;
                                _user.userGuid = Guid.NewGuid().ToString();
                                _user.identityNr = "";
                                _user.password = _SMethod.StringToMd5(_password);
                                _user.emailVerification = 1;
                                _user.profileStatus = 1;
                                _user.notificationStatus = 2;
                                _user.statusId = 2;
                                _user.creationDate = _user.birthDate = DateTime.Now;
                                _user.gender = Convert.ToInt32(_SMethod.GetEnumValue(enumGenderType.Belirtilmemiş));
                                if (excelFileUsers.FirstOrDefault(x => x.email.Equals(emailAddress)) == null)
                                {
                                    excelFileUsers.Add(_user);
                                    hashedValue.TryAdd(emailAddress.ToString(), _password);
                                }
                            }
                        }
                    }
                    List<User> usersNotIn = excelFileUsers.Where(x => !users.Any(y => y.email.Equals(x.email))).ToList();
                    foreach (var item in usersNotIn)
                    {
                        int userId = _SUser.InsertUser(item);
                        rels.Add(new EventParticipant
                        {
                            eventId = eventParticipant.eventId,
                            userId = userId,
                            creatorId = id,
                            creationDate = DateTime.Now,
                            statusId = 2
                        });
                        string _password = "";
                        hashedValue.TryGetValue(item.email, out _password);
                        if (!String.IsNullOrEmpty(_password))
                        {
                            EmailVerificationQueueModel emailVerificationQueueModel = new EmailVerificationQueueModel()
                            {
                                email = item.email,
                                userGuid = item.userGuid
                            };
                            PasswordQueueModel passwordQueue = new PasswordQueueModel()
                            {
                                email = item.email,
                                password = _password
                            };
                            await notificationDispatcher.SendPassword(passwordQueue);
                            await notificationDispatcher.SendEmailVerification(emailVerificationQueueModel);
                        }
                    }
                    List<User> usersIn = excelFileUsers.Where(x => users.Any(y => y.email.Equals(x.email))).ToList();
                    foreach (var item in usersIn)
                    {
                        rels.Add(new EventParticipant
                        {
                            eventId = eventParticipant.eventId,
                            userId = item.id,
                            creatorId = id,
                            creationDate = DateTime.Now,
                            statusId = 2
                        });
                    }
                    List<User> participants = new List<User>();
                    participants.AddRange(usersNotIn);
                    participants.AddRange(usersIn);
                    isSuccess = _SEventParticipant.BulkInsertParticipants(rels);
                    baseResult.data.users = participants;
                }
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
        /// <summary>
        /// Bu Etkinliğe Tanımlanabilecek Alt Kategorileri Getirir.
        /// </summary>
        /// <param name="eventModel"></param>
        /// <returns></returns>
        [HttpPost("getavailablecategories")]
        [BusinessValidation]
        public IActionResult GetAvailableCategories([FromBody]EventModel eventModel)
        {
            BaseResult<EventModel> baseResult = new BaseResult<EventModel>();
            baseResult.data.eventCategories = _SCategory.GetEventAvailableCategories(eventModel.cgevent.id, eventModel.category.id);
            return Json(baseResult);
        }
        /// <summary>
        /// Etkinliğe Yeni Kategoriler Tanımlamak İçin Kullanılır.
        /// </summary>
        /// <param name="eventCategories"></param>
        /// <returns></returns>
        [HttpPost("neweventcategories")]
        [BusinessValidation]
        public IActionResult NewEventCategories([FromBody]List<EventCategory> eventCategories)
        {
            BaseResult<EventModel> baseResult = new BaseResult<EventModel>();
            bool isSuccess = false;
            if (eventCategories.Count > 0)
            {
                int eventId = eventCategories.FirstOrDefault().eventId;
                int userId = Convert.ToInt32(HttpContext.User.Identity.Name);
                Event _event = _SEvent.GetById(eventId);
                if (_event.userId == userId)
                {
                    foreach (var item in eventCategories)
                    {
                        item.creatorId = userId;
                    }
                    if (_SEventCategory.InsertEventCategories(eventCategories))
                    {
                        isSuccess = true;
                    }
                    else
                    {
                        baseResult.errMessage = "Kategoriler Tanımlanamadı!";
                    }
                }
                else
                {
                    baseResult.errMessage = "Kendinize Ait Olmayan Bir Etkinliğe Müdahale Edemezsiniz!";
                }
            }
            else
            {
                baseResult.errMessage = "Hiçbir Kategori Bulunamadı!";
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
        /// <summary>
        /// Etkinlik Kategorilerini Getirir.
        /// </summary>
        /// <param name="_event"></param>
        /// <returns></returns>
        [HttpPost("geteventcategories")]
        [BusinessValidation]
        public IActionResult GetEventCategories([FromBody]Event _event)
        {
            BaseResult<EventModel> baseResult = new BaseResult<EventModel>();
            baseResult.data.eventCategoriesRel = _SEventCategory.GetEventCategories(_event.id);
            return Json(baseResult);
        }
        /// <summary>
        /// Etkinliğe Tanımlı Kategoriyi Silmek İçin Kullanılır.
        /// </summary>
        /// <param name="eventCategory"></param>
        /// <returns></returns>
        [HttpPost("deleteeventcategory")]
        [BusinessValidation]
        public IActionResult DeleteEventCategory([FromBody]EventCategory eventCategory)
        {
            BaseResult<EventModel> baseResult = new BaseResult<EventModel>();
            bool isSuccess = false;
            Event _event = _SEvent.GetById(eventCategory.eventId);
            int userId = Convert.ToInt32(HttpContext.User.Identity.Name);
            if (_event.userId == userId)
            {
                eventCategory.statusId = 1;
                if (!_SEventCategory.UpdateEventCategory(eventCategory))
                {
                    baseResult.errMessage = "Kategori Silinemedi!";
                }
                else
                {
                    isSuccess = true;
                }
            }
            else
            {
                baseResult.errMessage = "Sadece Kendinize Ait Etkinliklere Müdahale Edebilirsiniz!";
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