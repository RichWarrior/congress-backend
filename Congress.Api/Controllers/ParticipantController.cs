using Congress.Api.Filters;
using Congress.Api.Models;
using Congress.Core.Entity;
using Congress.Core.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Congress.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ParticipantController : BaseController
    {
        IUser _SUser;
        public ParticipantController(IMethod _SMethod,IUser _SUser) 
            : base(_SMethod)
        {
            this._SUser = _SUser;
        }

        /// <summary>
        /// Tüm Katılımcı Listesini Görüntülemek İçin Kullanılır
        /// </summary>
        /// <returns></returns>
        [HttpPost("getparticipant")]
        public IActionResult GetParticipant()
        {
            BaseResult<UserModel> baseResult = new BaseResult<UserModel>();
            baseResult.data.users = _SUser.GetParticipant();
            return Json(baseResult);
        }

        /// <summary>
        /// Katılımcının Aktiflik Durumunu Değiştirir.
        /// </summary>
        /// <param name="user">Aktiflik Durumu Değiştirilecek Katılımcı Modeli</param>
        /// <returns></returns>
        [HttpPost("changeparticipantstatus")]
        [DoctorValidation]
        public IActionResult ChangeStatus([FromBody] User user)
        {
            BaseResult<UserModel> baseResult = new BaseResult<UserModel>();
            bool isSuccess = false;
            user.statusId = user.statusId == 2 ? 1 : 2;
            isSuccess = _SUser.UpdateUser(user);
            baseResult.data.user = user;
            if (isSuccess)
            {
                return Json(baseResult);
            }
            else
            {
                baseResult.errMessage = "İşleminiz Tamamlanırken Bir Hata Oluştu!";
                baseResult.statusCode = HttpStatusCode.NotFound;
                return new NotFoundObjectResult(baseResult);
            }            
        }
    }
}