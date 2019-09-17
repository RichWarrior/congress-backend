using Congress.Core.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Congress.Api.Controllers
{
    public class BaseController : Controller
    {
        public IMethod _SMethod;
        public BaseController(IMethod _SMethod)
        {
            this._SMethod = _SMethod;
        }

        [NonAction]
        public string GenerateToken(string userId,string user)
        {
            var someClaims = new Claim[]{
                new Claim(JwtRegisteredClaimNames.UniqueName,userId),
                new Claim("user",user)
            };

            SecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("CongressBackendApi"));
            var token = new JwtSecurityToken(
                claims: someClaims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)
            );


            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}