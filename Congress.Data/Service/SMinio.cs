using Congress.Core.Interface;
using Congress.Data.Data;
using Microsoft.AspNetCore.Http;
using Minio;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Congress.Data.Service
{
    public class SMinio : IMinio
    {
        Connection con = new Connection();
        public async Task<string> UploadFile(string bucketName, IFormFile file)
        {
            string _rtn = "";
            try
            {
                MinioClient minio = new MinioClient(con.minioHost, con.minioAccessKey, con.minioSecretKey);
            BucketControl:
                bool found = await minio.BucketExistsAsync(bucketName);
                if (found)
                {
                    string fileName = Guid.NewGuid().ToString();
                    string[] splittedName = file.FileName.Split('.');
                    fileName += "." + splittedName[1].ToString();
                    using (Stream stream = file.OpenReadStream())
                    {
                        await minio.PutObjectAsync(bucketName, fileName, stream, stream.Length);
                        _rtn = String.Format(@"http://{0}/{1}/{2}", con.remoteMinioHost, bucketName, fileName);
                    }
                }
                else
                {
                    await minio.MakeBucketAsync(bucketName);
                    goto BucketControl;

                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return _rtn;
        }
    }
}
