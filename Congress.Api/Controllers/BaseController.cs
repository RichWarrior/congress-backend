using Congress.Core.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Congress.Api.Controllers
{
    public class BaseController : Controller
    {
        IMethod _SMethod;
        public BaseController(IMethod _SMethod)
        {
            this._SMethod = _SMethod;
        }
    }
}