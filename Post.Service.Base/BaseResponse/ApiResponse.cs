using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace Post.Service.Base.BaseResponse
{
    public class HttpStatusCodeException : Exception
    {
        public int StatusCode { get; set; }
        public string ContentType { get; set; } = @"text/plain";
        public string Code { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public HttpStatusCodeException(int statusCode)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            this.StatusCode = statusCode;
        }


        public HttpStatusCodeException(int statusCode, string message, string Code = "0") : base(message)
        {
            this.ContentType = @"application/json";
            this.StatusCode = statusCode;
            this.Code = Code;
        }

        public HttpStatusCodeException(int statusCode, Exception inner, string Code = "0") : this(statusCode, inner.ToString(), Code) { }

        public HttpStatusCodeException(int statusCode, JObject errorObject, string Code = "0") : this(statusCode, errorObject.ToString(), Code)
        {
            this.ContentType = @"application/json";
        }
    }

    public class CommonApiResponse
    {

        public object Content { get; set; }
        public int StatusCode { get; set; }
        public int ErrorCode { get; set; }
        public string Errormessage { get; set; }
    }

    public class NotFoundAPIResponse
    {
        /// <summary>
        /// (Conditional) An error code to find help for the exception.
        /// </summary>
        [JsonProperty("code")]

        public int code { get; set; }

        /// <summary>
        /// A more descriptive message regarding the exception.
        /// </summary>
        [JsonProperty("message")]
        public string message { get; set; }

        /// <summary>
        /// The HTTP status code for the exception.
        /// </summary>
        [JsonProperty("status")]
        public int status { get; set; }

    }
}
