using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Congress.Core.Interface
{
    public interface IMinio
    {
        Task<string> UploadFile(string bucketName, IFormFile file);
    }
}
