using System;
using System.Net;

namespace Congress.Api.Models
{
    public class BaseResult<T>
    {
        public T data { get; set; }
        public HttpStatusCode statusCode { get; set; }
        public string errMessage { get; set; }

        public BaseResult()
        {
            data = (T)Activator.CreateInstance(typeof(T));
        }
    }
}
