using Congress.Api.Models;
using Congress.Core.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Congress.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]    
    public class JobController : BaseController
    {
        IJob _SJob;
        public JobController(IMethod _SMethod,IJob _SJob) 
            : base(_SMethod)
        {
            this._SJob = _SJob;
        }
        
        /// <summary>
        /// Meslek Dallarını Geri Döndürür.
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetJobs")]
        [AllowAnonymous]
        public IActionResult GetJobs()
        {
            BaseResult<JobModel> baseResult = new BaseResult<JobModel>();
            baseResult.data.jobs = _SJob.GetJobs();
            return Json(baseResult);
        }
    }
}