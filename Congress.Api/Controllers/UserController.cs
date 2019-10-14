using Congress.Api.Filters;
using Congress.Api.HubDispatcher;
using Congress.Api.Models;
using Congress.Core.Entity;
using Congress.Core.Enums;
using Congress.Core.Interface;
using Congress.Core.QueueModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Congress.Api.Controllers
{
    [Route("api/user")]
    [ApiController]
    [Authorize]
    public class UserController : BaseController
    {
        IUser _SUser;
        IMinio _SMinio;
        IMenu _SMenu;
        IUserInterest _SUserInterest;
        INotificationDispatcher notificationDispatcher;
        public UserController(IMethod _SMethod,IUser _SUser,
            IMinio _SMinio, IMenu _SMenu,IUserInterest _SUserInterest,
            INotificationDispatcher notificationDispatcher) 
            : base(_SMethod)
        {
            this._SUser = _SUser;
            this._SMinio = _SMinio;
            this._SMenu = _SMenu;
            this._SUserInterest = _SUserInterest;
            this.notificationDispatcher = notificationDispatcher;
        }

        /// <summary>
        /// Kullanıcıyı Kayıt Eder
        /// </summary>
        /// <param name="model">Kayıt Edilecek Kullanıcı Modeli</param>
        /// <returns></returns>
        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromForm]User user)
        {
            BaseResult<UserModel> baseResult = new BaseResult<UserModel>();
            bool isSuccess = false;
            User _user = _SUser.CheckUser(user.email);
            if(String.IsNullOrEmpty(_user.email))
            {
                string bucketName = _SMethod.GetEnumValue(enumBucketType.Avatars);
                IFormFile avatarFile = user.avatarFile.FirstOrDefault();
                string avatarPath = await _SMinio.UploadFile(bucketName, avatarFile);
                if (!String.IsNullOrEmpty(avatarPath))
                {
                    user.avatarPath = avatarPath;
                    int userTypeId = Convert.ToInt32(_SMethod.GetEnumValue(enumUserType.user));
                    user.userTypeId = userTypeId;
                    user.password = _SMethod.StringToMd5(user.password);
                    user.userGuid = Guid.NewGuid().ToString();
                    user.taxNr = "";
                    user.id = _SUser.InsertUser(user);
                    if (user.id > 0)
                    {
                        baseResult.errMessage = "Kayıt İşlemi Başarıyla Tamamlandı!";
                        isSuccess = true;
                        EmailVerificationQueueModel emailVerificationQueueModel = new EmailVerificationQueueModel()
                        {
                            email = user.email,
                            userGuid = user.userGuid
                        };
                        await notificationDispatcher.SendEmailVerification(emailVerificationQueueModel);
                    }
                    else
                    {
                        baseResult.errMessage = "Kayıt Olunamadı!";
                        baseResult.statusCode = HttpStatusCode.NotFound;
                    }
                }
                else
                {
                    baseResult.errMessage = "Dosya Saklama Sunucusuna Ulaşılamadı!";
                    baseResult.statusCode = HttpStatusCode.NotFound;
                }
            }
            else
            {
                baseResult.errMessage = "Bu Bilgilere Ait Bir Hesap Var!";
                baseResult.statusCode = HttpStatusCode.NotFound;
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
        /// Giriş Yapıp Token Almak İçin Kullanılır.
        /// </summary>
        /// <param name="user">E-Posta Adresi ve Şifre Yeterlidir</param>
        /// <returns></returns>
        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody]User user)
        {
            bool isSuccess = false;
            BaseResult<UserModel> baseResult = new BaseResult<UserModel>();
            string password = _SMethod.StringToMd5(user.password);
            User _user = _SUser.Login(user.email, password);
            if (_user != null)
            {
                if(_user.statusId == 2)
                {                    
                    if(_user.emailVerification == 2)
                    {
                        if (_user.loginType == (int)enumLoginType.Phone && _user.userTypeId != (int)enumUserType.user)
                        {
                            baseResult.errMessage = "Normal Kullanıcı Dışındaki Kullanıcılar Mobil Uygulamayı Kullanamazlar";
                        }
                        else
                        {
                            baseResult.data.user = _user;
                            baseResult.data.menus = _SMenu.GetUserMenu(user.loginType, _user.userTypeId);
                            string jsonUser = JsonConvert.SerializeObject(_user);
                            baseResult.data.token = GenerateToken(_user.id.ToString(), jsonUser);
                            isSuccess = true;
                        }                        
                    }
                    else
                    {
                        baseResult.errMessage = "E-Postanızı Onaylamamışsınız! E-Posta Onaylama Maili Mail Kutunuza Tekrar Gönderildi!";
                        baseResult.statusCode = HttpStatusCode.NotFound;
                        EmailVerificationQueueModel emailVerificationQueueModel = new EmailVerificationQueueModel() {
                            email = _user.email,userGuid = _user.userGuid
                        };
                        await notificationDispatcher.SendEmailVerification(emailVerificationQueueModel);
                    }
                    
                }
                else
                {
                    baseResult.errMessage = "Kullanıcı Aktif Değil! Sistem Yöneticisiyle İletişime Geçiniz";
                    baseResult.statusCode = HttpStatusCode.NotFound;
                }
            }
            else
            {
                baseResult.errMessage = "Bu Bilgilere Ait Bir Kullanıcı Bulunamadı!";
                baseResult.statusCode = HttpStatusCode.NotFound;                
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
        /// Kullanıcı Bilgilerini Güncellemek İçin Kullanılır. (Sadece Kendi Bilgilerinizi Günceller)
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost("UpdateUser")]
        public async Task<IActionResult> UpdateUser([FromForm] User user)
        {
            BaseResult<UserModel> baseResult = new BaseResult<UserModel>();
            bool isSuccess = false;
            int userId = Convert.ToInt32(HttpContext.User.Identity.Name);
            if(userId == user.id)
            {
                User _user = _SUser.GetById(user.id);
                user.profileStatus = 2;
                if(_user.password != user.password)
                {
                    user.password = _SMethod.StringToMd5(user.password);
                }
                if (user.avatarFile!=null && user.avatarFile.Count>0)
                {
                    string bucketName = _SMethod.GetEnumValue(enumBucketType.Avatars);
                    IFormFile formFile = user.avatarFile.FirstOrDefault();
                    string path = await _SMinio.UploadFile(bucketName, formFile);
                    if (!String.IsNullOrEmpty(path))
                    {
                        user.avatarPath = path;
                    }
                    else
                    {
                        baseResult.errMessage = "Profil Resminiz Güncellenemedi!";
                    }
                }
                isSuccess = _SUser.UpdateUser(user);
                baseResult.data.user = user;
                if (!isSuccess)
                {
                    baseResult.errMessage = "Bilgileriniz Güncellenemedi!";
                }
            }
            else
            {
                baseResult.errMessage = "Sadece Kendi Bilgilerinizi Değiştirebilirsiniz!";
                isSuccess = false;
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
        /// Kullanıcıya Tanımlanabilecek Kategorileri Getirir.
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        [HttpPost("getuseravailableinterest")]
        [UserValidation]
        public IActionResult GetUserAvailableInterest([FromBody]Category category)
        {
            BaseResult<UserModel> baseResult = new BaseResult<UserModel>();
            int userId = Convert.ToInt32(HttpContext.User.Identity.Name);
            baseResult.data.userAvailableCategory = _SUserInterest.GetAvailableCategories(userId,category.id);
            return Json(baseResult);
        }
        /// <summary>
        /// Kullanıcıya İlgi Alanı Tanımlamak İçin Kullanılır
        /// </summary>
        /// <param name="userInterests"></param>
        /// <returns></returns>
        [HttpPost("newuserinterest")]
        [UserValidation]
        public IActionResult NewUserInterest([FromBody]List<UserInterest> userInterests)
        {
            BaseResult<UserModel> baseResult = new BaseResult<UserModel>();
            bool isSuccess = false;
            int userId = Convert.ToInt32(HttpContext.User.Identity.Name);
            foreach (var item in userInterests)
            {
                item.userId = item.creatorId = userId;
            }
            if (_SUserInterest.BulkInsertInterest(userInterests))
            {
                isSuccess = true;
            }
            else
            {
                baseResult.errMessage = "İlgi Alanlarınız Eklenemedi!";
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
        /// Kullanıcının İlgi Alanlarını Getirir.
        /// </summary>
        /// <returns></returns>
        [HttpPost("getuserinterest")]
        [UserValidation]
        public IActionResult GetUserInterest()
        {
            BaseResult<UserModel> baseResult = new BaseResult<UserModel>();
            int userId = Convert.ToInt32(HttpContext.User.Identity.Name);
            baseResult.data.userInterest = _SUserInterest.GetUserInterest(userId);
            return Json(baseResult);
        }
        /// <summary>
        /// İlgi Alanı Silmek İçin Kullanılır.
        /// </summary>
        /// <param name="userInterest"></param>
        /// <returns></returns>
        [HttpPost("deleteuserinterest")]
        [UserValidation]
        public IActionResult DeleteUserInterest([FromBody]Category userInterest)
        {
            BaseResult<UserModel> baseResult = new BaseResult<UserModel>();
            bool isSuccess = false;
            int userId = Convert.ToInt32(HttpContext.User.Identity.Name);
            if (_SUserInterest.DeleteUserInterest(userId,userInterest.id))
            {
                isSuccess = true;
            }
            else
            {
                baseResult.errMessage = "İlgi Alanınız Silinemedi!";
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
        /// Kullanıcının Katıldığı Etkinlikleri Listeler
        /// </summary>
        [HttpPost("getuserparticipantevent")]
        [UserValidation]
        public IActionResult GetUserParticipantEvent()
        {
            BaseResult<UserModel> baseResult = new BaseResult<UserModel>();
            int userId= Convert.ToInt32(HttpContext.User.Identity.Name);
            baseResult.data.userEvents = _SUser.GetUserParticipantEvents(userId);
            foreach (var item in baseResult.data.userEvents)
            {
                if (item.endDate> DateTime.Now)
                {
                    item.isCompleted = 1;
                }
                else
                {
                    item.isCompleted = 2;
                }
            }
            return Json(baseResult);
        }
        /// <summary>
        /// Şifre Yenileme İçin Kullanılır.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost("forgotpassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([FromBody]User user)
        {
            BaseResult<UserModel> baseResult = new BaseResult<UserModel>();
            bool isSuccess = false;
            User _user = _SUser.GetByEmail(user.email);
            if (_user != null)
            {
                string guid = Guid.NewGuid().ToString();
                string password = "";
                for (int i = 0; i < 8; i++)
                {
                    password += guid[i];
                }
                string hashedValue = _SMethod.StringToMd5(password);
                _user.password = hashedValue;
                if (_SUser.UpdateUser(_user))
                {
                    baseResult.errMessage = "Şifreniz Sıfırlandı ve E-Posta Adresinize Gönderiliyor!";
                    isSuccess = true;
                    PasswordQueueModel passwordQueueModel = new PasswordQueueModel()
                    {
                        email = _user.email,
                        password = password
                    };
                    await notificationDispatcher.SendPassword(passwordQueueModel);
                }
                else
                {
                    baseResult.errMessage = "Kullanıcının Şifresi Sıfırlanamadı!";
                }
            }
            else
            {
                baseResult.errMessage = "Böyle Bir Kullanıcı Bulunamadı!";
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
        /// Kullanıcı E-Posta Onayı İçin Kullanılır
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost("activateuser")]
        [AllowAnonymous]
        public IActionResult ActivateUser([FromBody]User user)
        {
            BaseResult<UserModel> baseResult = new BaseResult<UserModel>();
            bool isSuccess = false;
            User _user = _SUser.GetByGuid(user.userGuid);
            if (_user!=null)
            {
                _user.emailVerification = 2;
                _user.userGuid = Guid.NewGuid().ToString();
                if (_SUser.UpdateUser(_user))
                {
                    isSuccess = true;
                    baseResult.data.user = user;
                }
                else
                {
                    baseResult.errMessage = "E-Posta Adresiniz Onaylanamadı!";
                }
            }
            else
            {
                baseResult.errMessage = "Bu Bilgilere Ait Bir Kullanıcı Bulunamadı!";
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