using Congress.Api.Models;
using Congress.Core.Entity;
using Congress.Core.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Security.Claims;

namespace Congress.Api.Filters
{
    public class BusinessValidation : Attribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var identity = context.HttpContext.User.Identity as ClaimsIdentity;
            if (identity!=null)
            {
                string jsonUser = identity.FindFirst("user").Value;
                if (!String.IsNullOrEmpty(jsonUser))
                {
                    User user = JsonConvert.DeserializeObject<User>(jsonUser);
                    if (user.userTypeId != (int)enumUserType.business)
                    {
                        BaseResult<string> baseResult = new BaseResult<string>();
                        baseResult.errMessage = "Buraya Erişim Yetkiniz Bulunmamaktadır";
                        baseResult.statusCode = HttpStatusCode.NotFound;
                        context.Result = new NotFoundObjectResult(baseResult);
                    }
                }
            }
        }
    }
}
