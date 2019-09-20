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
    public class UserController : BaseController
    {
        IUser _SUser;
        IMinio _SMinio;
        INotificationDispatcher notificationDispatcher;
        IMenu _SMenu;
        public UserController(IMethod _SMethod,IUser _SUser,
            IMinio _SMinio,INotificationDispatcher notificationDispatcher,IMenu _SMenu) 
            : base(_SMethod)
        {
            this._SUser = _SUser;
            this._SMinio = _SMinio;
            this._SMenu = _SMenu;
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
            User _user = _SUser.CheckUser(user.email, user.identityNr);
            if(_user == null)
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
                        baseResult.data.user = _user;
                        baseResult.data.menus = _SMenu.GetUserMenu(user.loginType, _user.userTypeId);
                        string jsonUser = JsonConvert.SerializeObject(_user);
                        baseResult.data.token = GenerateToken(_user.id.ToString(), jsonUser);
                        isSuccess = true;
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
    }
}