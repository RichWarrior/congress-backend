using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Congress.Core.Interface
{
    public interface IMinio
    {
        /// <summary>
        /// Dosya Upload Etme
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        Task<string> UploadFile(string bucketName, IFormFile file);
    }
}
