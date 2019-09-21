using Congress.Api.Filters;
using Congress.Api.HubDispatcher;
using Congress.Api.Models;
using Congress.Core.Entity;
using Congress.Core.Enums;
using Congress.Core.Interface;
using Congress.Core.QueueModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Congress.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BusinessController : BaseController
    {
        IUser _SUser;
        INotificationDispatcher notificationDispatcher;
        public BusinessController(IMethod _SMethod,IUser _SUser,INotificationDispatcher notificationDispatcher) 
            : base(_SMethod)
        {
            this._SUser = _SUser;
            this.notificationDispatcher = notificationDispatcher;
        }

        /// <summary>
        /// Firma Oluşturmak İçin Kullanılır.
        /// </summary>
        /// <param name="user">Firma Modeli</param>
        /// <returns></returns>
        [HttpPost("newbusiness")]
        [DoctorValidation]
        public async Task<IActionResult> NewBusiness([FromBody] User user)
        {
            BaseResult<UserModel> baseResult = new BaseResult<UserModel>();
            int userId = Convert.ToInt32(HttpContext.User.Identity.Name);
            user.creatorId = userId;
            bool isSuccess = false;
            User _user = _SUser.CheckUser(user.email);
            if (String.IsNullOrEmpty(_user.email))
            {
                user.gender = (int)enumGenderType.Belirtilmemiş;
                user.userGuid = Guid.NewGuid().ToString();
                string password = "";
                for (int i = 0; i < 8; i++)
                {
                    password += user.userGuid[i].ToString();
                }
                user.password = _SMethod.StringToMd5(password);
                user.userTypeId = (int)enumUserType.business;
                user.id = _SUser.InsertUser(user);
                if (user.id > 0)
                {
                    EmailVerificationQueueModel emailVerificationQueueModel = new EmailVerificationQueueModel()
                    {
                        email = user.email,
                        userGuid = user.userGuid
                    };
                    PasswordQueueModel passwordQueueModel = new PasswordQueueModel()
                    {
                        email = user.email,
                        password = password
                    };
                    await notificationDispatcher.SendEmailVerification(emailVerificationQueueModel);
                    await notificationDispatcher.SendPassword(passwordQueueModel);
                    isSuccess = true;
                    baseResult.data.user = user;
                }
                else
                {
                    baseResult.errMessage = "Firma Oluşturulamadı!";
                }
            }
            else
            {
                baseResult.errMessage = "Bu Bilgilerde Bir Firma Zaten Mevcut";
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
        /// Firmaları Getirmek İçin Kullanılır.
        /// </summary>
        /// <returns></returns>
        [HttpPost("getbusiness")]
        [DoctorValidation]
        public IActionResult GetBusiness()
        {
            BaseResult<UserModel> baseResult = new BaseResult<UserModel>();
            baseResult.data.users = _SUser.GetBusiness();
            return Json(baseResult);
        }
        /// <summary>
        /// Firmaları Güncellemek İçin Kullanılır
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost("updatebusiness")]
        [DoctorValidation]
        public IActionResult UpdateBusiness([FromBody] User user)
        {
            BaseResult<UserModel> baseResult = new BaseResult<UserModel>();
            baseResult.data.user = user;
            bool isSuccess = false;
            isSuccess = _SUser.UpdateUser(user);
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
        /// Firma Silmek İçin Kullanılır.
        /// </summary>
        /// <param name="user">Silinecek Firma Modeli</param>
        /// <returns></returns>
        [HttpPost("deletebusiness")]
        [DoctorValidation]
        public IActionResult DeleteBusiness([FromBody] User user)
        {
            BaseResult<UserModel> baseResult = new BaseResult<UserModel>();
            baseResult.data.user = user;
            bool isSuccess = false;
            user.statusId = 1;
            isSuccess = _SUser.UpdateUser(user);
            if (isSuccess)
            {
                return Json(baseResult);
            }
            else
            {
                baseResult.errMessage = "Silme İşlemi Tamamlanamadı!";
                return new NotFoundObjectResult(baseResult);
            }
        }
    }
}