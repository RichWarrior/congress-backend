using Congress.Core.Entity;
using System.Collections.Generic;

namespace Congress.Core.Interface
{
    public interface IJob
    {
        /// <summary>
        /// Meslekleri Getirir.
        /// </summary>
        /// <returns></returns>
        List<Job> GetJobs();
    }
}
